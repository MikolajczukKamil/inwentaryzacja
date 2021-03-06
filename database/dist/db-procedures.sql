DROP PROCEDURE IF EXISTS addNewAsset;

DELIMITER $
CREATE PROCEDURE addNewAsset(IN Type_id INT)
BEGIN
  DECLARE is_type_exits BOOLEAN DEFAULT asset_typeExists(Type_id);

  IF NOT is_type_exits THEN
    SELECT NULL                                              AS id,
           idsNotFound('Typ środka', Type_id, is_type_exits) AS message;
  ELSE
    INSERT INTO assets (type)
    VALUES (Type_id);

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getAssetInfo;

DELIMITER $
CREATE PROCEDURE getAssetInfo(IN Asset_id INT)
BEGIN
  DECLARE is_asset_exits BOOLEAN DEFAULT assetExists(Asset_id);
  DECLARE Asset_room_id INT DEFAULT getRoomIdWithAsset(Asset_id);

  IF NOT is_asset_exits THEN
    SELECT idsNotFound('Środki', Asset_id, is_asset_exits) AS message,
           NULL                                            AS id,
           NULL                                            AS type,
           NULL                                            AS letter,
           NULL                                            AS asset_type_name,
           NULL                                            AS room_id,
           NULL                                            AS room_name,
           NULL                                            AS building_id,
           NULL                                            AS building_name;
  ELSE

    SELECT NULL             AS message,
           assets.id,
           assets.type,
           asset_types.letter,
           asset_types.name AS asset_type_name,
           Asset_room_id    AS room_id,
           rooms.name       AS room_name,
           buildings.id     AS building_id,
           buildings.name   AS building_name
    FROM assets
           JOIN asset_types ON assets.type = asset_types.id
           LEFT JOIN rooms ON Asset_room_id = rooms.id
           LEFT JOIN buildings ON rooms.building = buildings.id
    WHERE assets.id = Asset_id;

  END IF;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getAssetsInRoom;

DELIMITER $
CREATE PROCEDURE getAssetsInRoom(IN Room_id INT)
BEGIN
  DECLARE Room_exits BOOLEAN DEFAULT roomExists(Room_id);

  IF NOT Room_exits THEN
    SELECT idsNotFound('Pomieszczenie', Room_id, Room_exits) AS message,
           NULL                                              AS id,
           NULL                                              AS type,
           NULL                                              AS asset_type_name,
           NULL                                              AS asset_type_letter;
  ELSE

    SELECT NULL               AS message,
           assets.id,
           assets.type,
           asset_types.name   AS asset_type_name,
           asset_types.letter AS asset_type_letter
    FROM reports_positions
           JOIN reports ON reports_positions.report_id = reports.id
           JOIN assets ON reports_positions.asset_id = assets.id
           JOIN asset_types ON assets.type = asset_types.id
    WHERE reports_positions.report_id = (
      SELECT r.id
      FROM reports AS r
      WHERE r.room = Room_id
      ORDER BY r.create_date DESC, r.id DESC
      LIMIT 1
    )
      AND reports_positions.present
      AND reports.room = getRoomIdWithAsset(reports_positions.asset_id)
    ORDER BY assets.id ASC;

  END IF;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addBuilding;

DELIMITER $
CREATE PROCEDURE addBuilding(IN Building_name VARCHAR(64))
BEGIN
  DECLARE Name_unique BOOLEAN;

  SELECT (SELECT COUNT(*) FROM buildings WHERE buildings.name = Building_name) = 0
  INTO Name_unique;

  IF NOT Name_unique THEN
    SELECT NULL                                                     AS id,
           CONCAT('Budynek nazwa=', Building_name, ' już istnieje') AS message;
  ELSE
    INSERT INTO buildings (name)
    VALUES (Building_name);

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getBuildings;

DELIMITER $
CREATE PROCEDURE getBuildings()
BEGIN
  SELECT buildings.id, buildings.name
  FROM buildings
  ORDER BY buildings.id ASC;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addNewReport;

DELIMITER @
CREATE PROCEDURE addNewReport(IN Report_name VARCHAR(64), IN Report_room INT, IN Report_owner INT,
                              IN Report_positions JSON)
addNewReportProcedure:
BEGIN
    DECLARE I INT;
    DECLARE Room_exits BOOLEAN DEFAULT roomExists(Report_room);
    DECLARE Owner_exits BOOLEAN DEFAULT userExists(Report_owner);
    DECLARE ReportPositions_length INT;
    DECLARE Are_assets_exists BOOLEAN;
    DECLARE Assets_does_not_exists VARCHAR(1024);
    DECLARE Are_rooms_exists BOOLEAN;
    DECLARE Rooms_does_not_exists VARCHAR(1024);
    DECLARE Are_assets_duplicated BOOLEAN;
    DECLARE Assets_duplicated VARCHAR(1024);
    DECLARE New_report_id INT;
    DECLARE Previous_reports_with_name INT;

    IF NOT Room_exits OR NOT Owner_exits THEN
        SELECT NULL  AS id,
               CONCAT_WS(
                       ', ',
                       idsNotFound('Pomieszczenie', Report_room, Room_exits),
                       idsNotFound('Użytkownik', Report_owner, Owner_exits)
                   ) AS message;
        LEAVE addNewReportProcedure;
    END IF;

    /*
      Parsing JSON array to table `Positions`
      { "id": INT, "previous": INT|NULL, "present": BOOLEAN }[]
    */
    CREATE TEMPORARY TABLE ReportPositions
    (
        id       INT,
        previous INT,
        present  BOOLEAN
    );

    SET I = 0;
    SET ReportPositions_length = JSON_LENGTH(Report_positions);

    WHILE (I < ReportPositions_length)
        DO
            INSERT INTO ReportPositions
            SELECT JSON_VALUE(Report_positions, CONCAT(' $[', i, '].id'))                      AS id,
                   NULLIF(JSON_VALUE(Report_positions, CONCAT('$[', i, '].previous')), 'null') AS previous,
                   JSON_VALUE(Report_positions, CONCAT('$[', i, '].present'))                  AS present;

            SET I = I + 1;
        END WHILE;

    /* Check Report_positions.asset_id and Report_positions.previus correct */

    SELECT COUNT(*) = 0,
           GROUP_CONCAT(DISTINCT ReportPositions.id ORDER BY ReportPositions.id SEPARATOR ', ')
    INTO
        Are_assets_exists,
        Assets_does_not_exists
    FROM ReportPositions
    WHERE NOT assetExists(ReportPositions.id);

    SELECT COUNT(*) = 0,
           GROUP_CONCAT(DISTINCT ReportPositions.previous ORDER BY ReportPositions.previous SEPARATOR ', ')
    INTO
        Are_rooms_exists,
        Rooms_does_not_exists
    FROM ReportPositions
    WHERE ReportPositions.previous IS NOT NULL
      AND NOT roomExists(ReportPositions.previous);

    SELECT COUNT(*) = 0,
           GROUP_CONCAT(DISTINCT duplicates.id ORDER BY duplicates.id SEPARATOR ', ')
    INTO
        Are_assets_duplicated,
        Assets_duplicated
    FROM (
             SELECT ReportPositions.id
             FROM ReportPositions
             GROUP BY ReportPositions.id
             HAVING COUNT(ReportPositions.id) > 1
         ) AS duplicates;

    If NOT Are_assets_exists OR NOT Are_rooms_exists OR NOT Are_assets_duplicated THEN
        SELECT NULL  AS id,
               CONCAT_WS(
                       ', ',
                       idsNotFound('Środek', Assets_does_not_exists, Are_assets_exists),
                       idsNotFound('Pomieszzenie', Rooms_does_not_exists, Are_rooms_exists),
                       haveDuplicates('Środek', Assets_duplicated, Are_assets_duplicated)
                   ) AS message;

        DROP TEMPORARY TABLE ReportPositions;
        LEAVE addNewReportProcedure;
    END IF;

    /* Adding new report */

    SELECT COUNT(*) + 1
    INTO Previous_reports_with_name
    FROM reports
    WHERE reports.name REGEXP CONCAT(Report_name, '\s?\d*');

    INSERT INTO reports (name, room, owner, create_date)
    VALUES (CONCAT(Report_name, ' ', Previous_reports_with_name), Report_room, Report_owner, NOW());

    SET New_report_id = LAST_INSERT_ID();

    INSERT INTO reports_positions (report_id, asset_id, previous_room, present)
    SELECT New_report_id,
           ReportPositions.id,
           ReportPositions.previous,
           ReportPositions.present
    FROM ReportPositions;

    DROP TEMPORARY TABLE ReportPositions;

    SELECT New_report_id AS id, NULL AS message;

END @ DELIMITER ;
DROP PROCEDURE IF EXISTS getPositionsInReport;

DELIMITER $
CREATE PROCEDURE getPositionsInReport(IN Id_report INT)
BEGIN
  DECLARE is_report_exits BOOLEAN DEFAULT reportExists(Id_report);

  IF NOT is_report_exits THEN
    SELECT idsNotFound('Raport', Id_report, is_report_exits) AS message,
           NULL                                              AS asset_id,
           NULL                                              AS present,
           NULL                                              AS type_id,
           NULL                                              AS type_letter,
           NULL                                              AS type_name,
           NULL                                              AS previous_id,
           NULL                                              AS previous_name,
           NULL                                              AS previous_building_id,
           NULL                                              AS previous_building_name;
  ELSE

    SELECT NULL               AS message,
           reports_positions.asset_id,
           reports_positions.present,
           assets.type        AS type_id,
           asset_types.letter AS type_letter,
           asset_types.name   AS type_name,
           rooms.id           AS previous_id,
           rooms.name         AS previous_name,
           buildings.id       AS previous_building_id,
           buildings.name     AS previous_building_name
    FROM reports_positions
           JOIN assets ON reports_positions.asset_id = assets.id
           JOIN asset_types ON assets.type = asset_types.id
           LEFT JOIN rooms ON reports_positions.previous_room = rooms.id
           LEFT JOIN buildings ON rooms.building = buildings.id
    WHERE reports_positions.report_id = Id_report;

  END IF;

END $ DELIMITER ;
/* Report headers - all informations about Report except positions */

DROP PROCEDURE IF EXISTS getReportsHeaders;
DROP PROCEDURE IF EXISTS getReportHeader;
DROP PROCEDURE IF EXISTS _ReportsHeaders;

DELIMITER $
CREATE PROCEDURE _ReportsHeaders(IN User_id INT, IN Report_id INT)
BEGIN
  SELECT NULL           AS message,
         reports.id,
         reports.name,
         reports.create_date,
         users.id       AS owner_id,
         users.login    AS owner_name,
         rooms.id       AS room_id,
         rooms.name     AS room_name,
         buildings.id   AS building_id,
         buildings.name AS building_name
  FROM reports
         JOIN users ON reports.owner = users.id
         JOIN rooms ON reports.room = rooms.id
         JOIN buildings ON rooms.building = buildings.id
  WHERE (User_id IS NULL OR users.id = User_id)
    AND (Report_id IS NULL OR reports.id = Report_id)
  ORDER BY reports.create_date DESC,
           reports.id DESC;

END $ DELIMITER ;

/* Many */

DELIMITER $
CREATE PROCEDURE getReportsHeaders(IN User_id INT)
BEGIN
  DECLARE is_user_exits BOOLEAN DEFAULT userExists(User_id);

  IF NOT is_user_exits THEN
    SELECT idsNotFound('Użytkownik', User_id, is_user_exits) AS message,
           NULL                                              AS id,
           NULL                                              AS name,
           NULL                                              AS create_date,
           NULL                                              AS owner_id,
           NULL                                              AS owner_name,
           NULL                                              AS room_id,
           NULL                                              AS room_name,
           NULL                                              AS building_id,
           NULL                                              AS building_name;
  ELSE
    CALL _ReportsHeaders(User_id, NULL);
  END IF;

END $ DELIMITER ;

/* Single */

DELIMITER $
CREATE PROCEDURE getReportHeader(IN Report_id INT)
BEGIN
  DECLARE is_report_exits BOOLEAN DEFAULT reportExists(Report_id);

  IF NOT is_report_exits THEN
    SELECT idsNotFound('Raport', Report_id, is_report_exits) AS message,
           NULL                                              AS id,
           NULL                                              AS name,
           NULL                                              AS create_date,
           NULL                                              AS owner_id,
           NULL                                              AS owner_name,
           NULL                                              AS room_id,
           NULL                                              AS room_name,
           NULL                                              AS building_id,
           NULL                                              AS building_name;
  ELSE
    CALL _ReportsHeaders(NULL, Report_id);
  END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addRoom;

DELIMITER $
CREATE PROCEDURE addRoom(IN Room_name VARCHAR(64), IN Building_id INT)
BEGIN
  DECLARE Building_exits BOOLEAN;
  DECLARE Name_unique BOOLEAN;

  SELECT (SELECT COUNT(*) FROM buildings WHERE buildings.id = Building_id) = 1
  INTO Building_exits;

  SELECT (SELECT COUNT(*) FROM rooms WHERE rooms.building = Building_id AND rooms.name = Room_name) = 0
  INTO Name_unique;

  IF NOT Building_exits OR NOT Name_unique THEN
    SELECT NULL AS id,
           CONCAT_WS(
               ', ',
               idsNotFound('Budynek', Building_id, Building_exits),
               CONCAT('Pomieszczenie ', IF(NOT Name_unique, Room_name, NULL), ' już istnieje w budynku nr ',
                      Building_id)
             )  AS message;
  ELSE
    INSERT INTO rooms (name, building)
    VALUES (Room_name, Building_id);

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getRooms;

DELIMITER $
CREATE PROCEDURE getRooms(IN Id_building INT)
BEGIN
  DECLARE Building_exits BOOLEAN DEFAULT buildingExists(Id_building);

  IF NOT Building_exits THEN
    SELECT idsNotFound('Budynek', Id_building, Building_exits) AS message,
           NULL                                                AS id,
           NULL                                                AS name,
           NULL                                                AS building_id,
           NULL                                                AS building_name;
  ELSE

    SELECT NULL           AS message,
           rooms.id,
           rooms.name,
           buildings.id   AS building_id,
           buildings.name AS building_name
    FROM rooms
           JOIN buildings ON rooms.building = buildings.id
    WHERE rooms.building = Id_building
    ORDER BY rooms.id ASC;

  END IF;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addScan;

DELIMITER $
CREATE PROCEDURE addScan(IN Room_id INT, IN Owner_id INT)
BEGIN
  DECLARE Room_exits BOOLEAN DEFAULT roomExists(Room_id);
  DECLARE Owner_exits BOOLEAN DEFAULT userExists(Owner_id);

  IF NOT Room_exits OR NOT Owner_exits THEN
    SELECT NULL AS id,
           CONCAT_WS(
               ', ',
               idsNotFound('Pomieszczenie', Room_id, Room_exits),
               idsNotFound('Użytkownik', Owner_id, Owner_exits)
             )  AS message;
  ELSE

    INSERT INTO scans (room, owner, create_date)
    VALUES (Room_id, Owner_id, NOW());

    SELECT LAST_INSERT_ID() AS id, NULL AS message;

  END IF;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS deleteScan;

DELIMITER $
CREATE PROCEDURE deleteScan(IN Scan_id INT)
BEGIN
  DELETE
  FROM scans_positions
  WHERE scans_positions.scan = Scan_id;

  DELETE
  FROM scans
  WHERE scans.id = Scan_id;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getScanPositions;

DELIMITER $
CREATE PROCEDURE getScanPositions(IN Scan_id INT)
BEGIN
  DECLARE Is_scan_exits BOOLEAN DEFAULT scanExists(Scan_id);

  IF NOT Is_scan_exits THEN
    SELECT idsNotFound('Skanowanie', Scan_id, Is_scan_exits) AS message,
           NULL                                              AS state,
           NULL                                              AS id,
           NULL                                              AS type,
           NULL                                              AS letter,
           NULL                                              AS asset_type_name,
           NULL                                              AS room_id,
           NULL                                              AS room_name,
           NULL                                              AS building_id,
           NULL                                              AS building_name;
  ELSE

    SELECT NULL             AS message,
           scans_positions.state,
           assets.id,
           assets.type,
           asset_types.letter,
           asset_types.name AS asset_type_name,
           rooms.id         AS room_id,
           rooms.name       AS room_name,
           buildings.id     AS building_id,
           buildings.name   AS building_name
    FROM scans_positions
           JOIN assets ON scans_positions.asset = assets.id
           JOIN asset_types ON assets.type = asset_types.id
           LEFT JOIN rooms ON getRoomIdWithAsset(assets.id) = rooms.id
           LEFT JOIN buildings ON rooms.building = buildings.id
    WHERE scans_positions.scan = Scan_id;

  END IF;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getScans;

DELIMITER $
CREATE PROCEDURE getScans(IN User_id INT)
BEGIN
  DECLARE Is_user_exits BOOLEAN DEFAULT userExists(User_id);

  IF NOT Is_user_exits THEN
    SELECT idsNotFound('Użytkownik', User_id, Is_user_exits) AS message,
           NULL                                              AS id,
           NULL                                              AS create_date,
           NULL                                              AS owner_id,
           NULL                                              AS owner_name,
           NULL                                              AS room_id,
           NULL                                              AS room_name,
           NULL                                              AS building_id,
           NULL                                              AS building_name;
  ELSE

    SELECT NULL           AS message,
           scans.id,
           scans.create_date,
           users.id       AS owner_id,
           users.login    AS owner_name,
           rooms.id       AS room_id,
           rooms.name     AS room_name,
           buildings.id   AS building_id,
           buildings.name AS building_name
    FROM scans
           JOIN users ON scans.owner = users.id
           JOIN rooms ON scans.room = rooms.id
           JOIN buildings ON rooms.building = buildings.id
    WHERE scans.owner = User_id
    ORDER BY scans.create_date DESC,
             scans.id DESC;

  END IF;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS updateScan;

DELIMITER @
CREATE PROCEDURE updateScan(IN Scan_id INT, IN Scan_positions JSON)
updateScanProcedure:
BEGIN
  DECLARE I INT;
  DECLARE ScanPositions_length INT;
  DECLARE Is_scan_exits BOOLEAN DEFAULT scanExists(Scan_id);
  DECLARE Are_assets_exists BOOLEAN;
  DECLARE Assets_does_not_exists VARCHAR(1024);

  IF NOT Is_scan_exits THEN
    SELECT idsNotFound('Skanowanie', Scan_id, Is_scan_exits) AS message;
    LEAVE updateScanProcedure;
  END IF;

  /*
    Parsing JSON array to table `ScanPositions`
    { asset: INT, state: INT }[]
  */
  CREATE TEMPORARY TABLE ScanPositions
  (
    asset INT,
    state INT
  );

  SET I = 0;
  SET ScanPositions_length = JSON_LENGTH(Scan_positions);

  WHILE (I < ScanPositions_length)
    DO
      INSERT INTO ScanPositions
      SELECT JSON_VALUE(Scan_positions, CONCAT(' $[', i, '].asset')) AS asset,
             JSON_VALUE(Scan_positions, CONCAT(' $[', i, '].state')) AS state;

      SET I = I + 1;
    END WHILE;

  SELECT COUNT(*) = 0,
         GROUP_CONCAT(DISTINCT ScanPositions.asset ORDER BY ScanPositions.asset SEPARATOR ', ')
  INTO
    Are_assets_exists,
    Assets_does_not_exists
  FROM ScanPositions
  WHERE NOT assetExists(ScanPositions.asset);

  If NOT Are_assets_exists THEN
    SELECT idsNotFound('Środkek', Assets_does_not_exists, Are_assets_exists) AS message;

    DROP TEMPORARY TABLE ScanPositions;
    LEAVE updateScanProcedure;
  END IF;

  DELETE
  FROM scans_positions
  WHERE scans_positions.scan = Scan_id
    AND scans_positions.asset NOT IN (
    SELECT ScanPositions.asset
    FROM ScanPositions
  );

  UPDATE scans_positions
  SET scans_positions.state = (
    SELECT ScanPositions.state
    FROM ScanPositions
    WHERE ScanPositions.asset = scans_positions.asset
  )
  WHERE scans_positions.scan = Scan_id;

  INSERT INTO scans_positions (scan, asset, state)
  SELECT Scan_id, ScanPositions.asset, ScanPositions.state
  FROM ScanPositions
  WHERE ScanPositions.asset NOT IN (
    SELECT sp.asset
    FROM scans_positions AS sp
    WHERE sp.scan = Scan_id
  );

  SELECT NULL AS message;

END @ DELIMITER ;
DROP PROCEDURE IF EXISTS addLoginSession;

DELIMITER $
CREATE PROCEDURE addLoginSession(IN User_id INT, IN Date_expiration DATETIME, IN User_token VARCHAR(64))
BEGIN
  DECLARE is_type_exits BOOLEAN;

  SELECT (SELECT COUNT(*) FROM users WHERE users.id = User_id) = 1
  INTO is_type_exits;

  IF NOT is_type_exits THEN
    SELECT NULL                                              AS id,
           idsNotFound('Użytkownik', User_id, is_type_exits) AS message;
  ELSE
    INSERT INTO login_sessions (user, token, expiration_date, create_date)
    VALUES (User_id, User_token, Date_expiration, NOW());

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS deleteLoginSession;

DELIMITER $
CREATE PROCEDURE deleteLoginSession(IN User_token VARCHAR(64))
BEGIN
  DELETE
  FROM login_sessions
  WHERE login_sessions.token = User_token;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getLoginSession;

DELIMITER $
CREATE PROCEDURE getLoginSession(IN User_token VARCHAR(64))
BEGIN
  SELECT login_sessions.id,
         login_sessions.user                     AS user_id,
         login_sessions.expiration_date,
         login_sessions.token,
         login_sessions.expiration_date <= NOW() AS expired
  FROM login_sessions
  WHERE login_sessions.token = User_token;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getUser;

DELIMITER $
CREATE PROCEDURE getUser(IN User_id INT)
BEGIN
  SELECT NULL as message,
         users.id,
         users.login,
         users.hash
  FROM users
  WHERE users.id = User_id;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getUserByLogin;

DELIMITER $
CREATE PROCEDURE getUserByLogin(IN User_login VARCHAR(64))
BEGIN
  SELECT users.id,
         users.login,
         users.hash
  FROM users
  WHERE users.login = User_login;

END $ DELIMITER ;
