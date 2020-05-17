DROP PROCEDURE IF EXISTS addNewAsset;

DELIMITER $ CREATE PROCEDURE addNewAsset(IN type_id INT)
BEGIN
  DECLARE is_type_correct BOOLEAN;

  SELECT (SELECT COUNT(*) FROM asset_types WHERE asset_types.id = type_id) = 1
  INTO is_type_correct;

  IF NOT is_type_correct THEN
    SELECT
      NULL AS id,
      idsNotFound("AssetType", type_id, is_type_correct) AS message
    ;
  ELSE
    INSERT INTO assets (type)
    VALUES (type_id);

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getAssetInfo;

DELIMITER $ CREATE PROCEDURE getAssetInfo(IN asset_id INT)
BEGIN
  DECLARE Asset_room_id INT DEFAULT getRoomIdWithAsset(asset_id);

  SELECT
    assets.id,
    assets.type,
    asset_types.letter,
    asset_types.name AS asset_type_name,
    Asset_room_id AS room_id,
    rooms.name AS room_name,
    buildings.id AS building_id,
    buildings.name AS building_name
  FROM
    assets
    JOIN asset_types ON assets.type = asset_types.id
    LEFT JOIN rooms ON Asset_room_id = rooms.id
    LEFT JOIN buildings ON rooms.building = buildings.id
  WHERE
    assets.id = asset_id;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getAssetsInRoom;

DELIMITER $ CREATE PROCEDURE getAssetsInRoom(IN room_id INT)
BEGIN
  SELECT
    assets.id,
    assets.type,
    asset_types.name AS asset_type_name,
    asset_types.letter AS asset_type_letter
  FROM
    reports_positions
    JOIN reports ON reports_positions.report_id = reports.id
    JOIN assets ON reports_positions.asset_id = assets.id
    JOIN asset_types ON assets.type = asset_types.id
  WHERE
    reports_positions.report_id = (
      SELECT r.id
      FROM reports AS r
      WHERE r.room = room_id
      ORDER BY r.create_date DESC, r.id DESC
      LIMIT 1
    )
    AND reports_positions.present
    AND reports.room = getRoomIdWithAsset(reports_positions.asset_id)
  ORDER BY
    assets.id ASC;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addBuilding;

DELIMITER $
CREATE PROCEDURE addBuilding(IN building_name VARCHAR(64))
BEGIN
  DECLARE is_name_unique BOOLEAN;

  SELECT (SELECT COUNT(*) FROM buildings WHERE buildings.name = building_name ) = 0
  INTO is_name_unique;

  IF NOT is_name_unique THEN
    SELECT
      NULL AS id,
      CONCAT("Building name=", building_name, " is not unique") AS message
    ;
  ELSE
    INSERT INTO buildings (name)
    VALUES (building_name);

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getBuildings;

DELIMITER $ CREATE PROCEDURE getBuildings()
BEGIN
  SELECT buildings.id, buildings.name
  FROM buildings
  ORDER BY buildings.id ASC;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addNewReport;

DELIMITER $
CREATE PROCEDURE addNewReport(IN report_name VARCHAR(64), IN report_room INT, IN report_owner INT, IN report_positions JSON)
addNewReportProcedure:BEGIN
  /* DECLARE */
    DECLARE I INT;
    DECLARE Is_room_correct BOOLEAN DEFAULT roomExists(room_id);
    DECLARE Is_owner_correct BOOLEAN DEFAULT userExists(owner_id);
    DECLARE Positions_length INT;
    DECLARE Are_assets_exists BOOLEAN;
    DECLARE Assets_does_not_exists VARCHAR(1024);
    DECLARE Are_rooms_exists BOOLEAN;
    DECLARE Rooms_does_not_exists VARCHAR(1024);
    DECLARE Are_assets_duplicated BOOLEAN;
    DECLARE Assets_duplicated VARCHAR(1024);
    DECLARE New_report_id INT;
  /* END DECLARE */

  IF NOT Is_room_correct OR NOT Is_owner_correct THEN
    SELECT
      NULL AS id,
      CONCAT_WS(
        " AND ",
        idsNotFound("Room", report_room, Is_room_correct),
        idsNotFound("User", report_owner, Is_owner_correct)
      ) AS message
    ;
    LEAVE addNewReportProcedure;
  END IF;

  /*
    Parsing JSON array to table `Positions`
    { "id": INT, "previous": INT|NULL, "present": BOOLEAN }[]
  */
  CREATE TEMPORARY TABLE Positions(id INT, previous INT, present BOOLEAN);

  SET I = 0;
  SET Positions_length = JSON_LENGTH(report_positions);

  WHILE (I < Positions_length) DO
    INSERT INTO Positions
    SELECT
      JSON_VALUE(report_positions, CONCAT('$[',i,'].id')) AS id,
      NULLIF(JSON_VALUE(report_positions, CONCAT('$[',i,'].previous')), 'null') AS previous,
      JSON_VALUE(report_positions, CONCAT('$[',i,'].present')) AS present
    ;

    SET I = I + 1;
  END WHILE;     

  /* Check report_positions.asset_id and report_positions.previus correct */

    SELECT
      COUNT(*) = 0,
      GROUP_CONCAT(DISTINCT Positions.id ORDER BY Positions.id SEPARATOR ', ')
    INTO
      Are_assets_exists,
      Assets_does_not_exists
    FROM Positions
    WHERE NOT assetExists(assets);

    SELECT
      COUNT(*) = 0,
      GROUP_CONCAT(DISTINCT Positions.previous ORDER BY Positions.previous SEPARATOR ', ')
    INTO
      Are_rooms_exists,
      Rooms_does_not_exists
    FROM Positions
    WHERE
      Positions.previous IS NOT NULL
      AND NOT roomExists(Positions.previous)
    ;

    SELECT
      COUNT(*) = 0,
      GROUP_CONCAT(DISTINCT duplicates.id ORDER BY duplicates.id SEPARATOR ', ')
    INTO
      Are_assets_duplicated,
      Assets_duplicated
    FROM
      (
        SELECT Positions.id
        FROM Positions
        GROUP BY Positions.id
        HAVING COUNT(Positions.id) > 1
      ) AS duplicates
    ;

  /* END Check report_positions.asset_id and report_positions.previus correct */

  If NOT Are_assets_exists OR NOT Are_rooms_exists OR NOT Are_assets_duplicated THEN
    SELECT
      NULL AS id,
      CONCAT_WS(
        " AND ",
        idsNotFound("Asset", Assets_does_not_exists, Are_assets_exists),
        idsNotFound("Room", Rooms_does_not_exists, Are_rooms_exists),
        haveDublicates("Asset", Assets_duplicated, Are_assets_duplicated)
      ) AS message
    ;

    DROP TEMPORARY TABLE Positions;
    LEAVE addNewReportProcedure;
  END IF;

  /* Adding new report */

    INSERT INTO reports (name, room, owner, create_date)
    VALUES (report_name, report_room, report_owner, NOW());

    SET New_report_id = LAST_INSERT_ID();

    INSERT INTO reports_positions (report_id, asset_id, previous_room, present)
    SELECT
      New_report_id, Positions.id, Positions.previous, Positions.present
    FROM Positions;

    DROP TEMPORARY TABLE Positions;

    SELECT
      New_report_id AS id,
      NULL AS message
    ;

  /* END Adding new report */

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getPositionsInReport;

DELIMITER $ CREATE PROCEDURE getPositionsInReport(IN id_report INT)
BEGIN
  SELECT
    reports_positions.asset_id,
    reports_positions.present,
    assets.type AS type_id,
    asset_types.letter AS type_letter,
    asset_types.name AS type_name,
    rooms.id AS previous_id,
    rooms.name AS previous_name,
    buildings.id AS previous_building_id,
    buildings.name AS previous_building_name
  FROM
    reports_positions
    JOIN assets ON reports_positions.asset_id = assets.id
    JOIN asset_types ON assets.type = asset_types.id
    LEFT JOIN rooms ON reports_positions.previous_room = rooms.id
    LEFT JOIN buildings ON rooms.building = buildings.id
  WHERE
    reports_positions.report_id = id_report;

END $ DELIMITER ;
/* Report headers - all informations about Report except positions */

DROP PROCEDURE IF EXISTS getReportsHeaders;
DROP PROCEDURE IF EXISTS getReportHeader;
DROP PROCEDURE IF EXISTS _ReportsHeaders;

DELIMITER $ CREATE PROCEDURE _ReportsHeaders(IN user_id INT, IN report_id INT)
BEGIN
  SELECT
    reports.id,
    reports.name,
    reports.create_date,
    users.id AS owner_id,
    users.login AS owner_name,
    rooms.id AS room_id,
    rooms.name AS room_name,
    buildings.id AS building_id,
    buildings.name AS building_name
  FROM
    reports
    JOIN users ON reports.owner = users.id
    JOIN rooms ON reports.room = rooms.id
    JOIN buildings ON rooms.building = buildings.id
  WHERE
    (user_id IS NULL OR users.id = user_id)
    AND (report_id IS NULL OR reports.id = report_id)
  ORDER BY
    reports.create_date DESC,
    reports.id DESC;

END $ DELIMITER ;

/* Many */

DELIMITER $ CREATE PROCEDURE getReportsHeaders(IN user_id INT)
BEGIN
  CALL _ReportsHeaders(user_id, NULL);
END $ DELIMITER ;

/* Single */

DELIMITER $ CREATE PROCEDURE getReportHeader(IN report_id INT)
BEGIN
  CALL _ReportsHeaders(NULL, report_id);
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addRoom;

DELIMITER $
CREATE PROCEDURE addRoom(IN room_name VARCHAR(64), IN building_id INT)
BEGIN
  DECLARE is_building_correct BOOLEAN;
  DECLARE is_name_unique BOOLEAN;

  SELECT (SELECT COUNT(*) FROM buildings WHERE buildings.id = building_id) = 1
  INTO is_building_correct;

  SELECT (SELECT COUNT(*) FROM rooms WHERE rooms.building = building_id AND rooms.name = room_name) = 0
  INTO is_name_unique;

  IF NOT is_building_correct OR NOT is_name_unique THEN
    SELECT
      NULL AS id,
        CONCAT_WS(
        " AND ",
        idsNotFound("Building", building_id, is_building_correct),
        CONCAT("Room name=", IF(NOT is_name_unique, room_name, NULL), " is not unique in Building id=", building_id)
      ) AS message
    ;
  ELSE
    INSERT INTO rooms (name, building)
    VALUES (room_name, building_id);

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getRooms;

DELIMITER $
CREATE PROCEDURE getRooms(IN id_building INT)
BEGIN
  SELECT
    rooms.id, rooms.name,
    buildings.id AS building_id,
    buildings.name AS building_name
  FROM rooms
  JOIN buildings ON rooms.building = buildings.id
  WHERE rooms.building = id_building
  ORDER BY rooms.id ASC;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addScanning;

DELIMITER $
CREATE PROCEDURE addScanning(IN room_id INT, IN owner_id INT)
addScanningProcedure:BEGIN
  DECLARE Is_room_correct BOOLEAN DEFAULT roomExists(room_id);
  DECLARE Is_owner_correct BOOLEAN DEFAULT userExists(owner_id);

  IF NOT Is_room_correct OR NOT Is_owner_correct THEN
    SELECT
      NULL AS id,
      CONCAT_WS(
        " AND ",
        idsNotFound("Room", report_room, Is_room_correct),
        idsNotFound("User", report_owner, Is_owner_correct)
      ) AS message
    ;
    LEAVE addScanningProcedure;
  END IF;



END $ DELIMITER ;
DROP PROCEDURE IF EXISTS deleteScanning;

DELIMITER $
CREATE PROCEDURE deleteScanning(IN scanning_id INT)
BEGIN
  DELETE FROM scannings_positions
  WHERE scannings_positions.scanning = scanning_id;

  DELETE FROM scannings
  WHERE scannings.id = scanning_id;

END $ DELIMITER ;
/* Report headers - all informations about Report except positions */

DROP PROCEDURE IF EXISTS getScannings;

DELIMITER $ CREATE PROCEDURE getScannings(IN user_id INT)
BEGIN
  SELECT
    scannings.id,
    scannings.create_date,
    users.id AS owner_id,
    users.login AS owner_name,
    rooms.id AS room_id,
    rooms.name AS room_name,
    buildings.id AS building_id,
    buildings.name AS building_name
  FROM
    scannings
    JOIN users ON scannings.owner = users.id
    JOIN rooms ON scannings.room = rooms.id
    JOIN buildings ON rooms.building = buildings.id
  WHERE
    scannings.owner = user_id
  ORDER BY
    reports.create_date DESC,
    reports.id DESC;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS updateScanning;

DELIMITER $
CREATE PROCEDURE updateScanning(IN scanning_id INT, IN scanning_positions JSON)
BEGIN

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addLoginSession;

DELIMITER $
CREATE PROCEDURE addLoginSession(IN user_id INT, IN date_expiration DATETIME, IN user_token VARCHAR(64))
BEGIN
  DECLARE is_type_correct BOOLEAN;

  SELECT (SELECT COUNT(*) FROM users WHERE users.id = user_id) = 1
  INTO is_type_correct;

  IF NOT is_type_correct THEN
    SELECT
      NULL AS id,
      idsNotFound("User", user_id, is_type_correct) AS message
    ;
  ELSE
    INSERT INTO login_sessions (user, token, expiration_date, create_date)
    VALUES (user_id, user_token, date_expiration, NOW());

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS deleteLoginSession;

DELIMITER $
CREATE PROCEDURE deleteLoginSession(IN user_token VARCHAR(64))
BEGIN
  DELETE FROM login_sessions
  WHERE login_sessions.token = user_token;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getLoginSession;

DELIMITER $ CREATE PROCEDURE getLoginSession(IN user_token VARCHAR(64)) BEGIN
  SELECT
    login_sessions.id,
    login_sessions.user AS user_id,
    login_sessions.expiration_date,
    login_sessions.token,
    login_sessions.expiration_date <= NOW() AS expired
  FROM login_sessions
  WHERE login_sessions.token = user_token;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getUser;

DELIMITER $ CREATE PROCEDURE getUser(IN user_id INT)
BEGIN
  SELECT
    users.id,
    users.login,
    users.hash
  FROM users
  WHERE users.id = user_id;

END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getUserByLogin;

DELIMITER $ CREATE PROCEDURE getUserByLogin(IN user_login VARCHAR(64))
BEGIN
  SELECT
    users.id,
    users.login,
    users.hash
  FROM users
  WHERE users.login = user_login;

END $ DELIMITER ;
