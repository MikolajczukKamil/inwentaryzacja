DROP TABLE IF EXISTS scans_positions;
DROP TABLE IF EXISTS scans;
DROP TABLE IF EXISTS reports_positions;
DROP TABLE IF EXISTS reports;
DROP TABLE IF EXISTS assets;
DROP TABLE IF EXISTS rooms;
DROP TABLE IF EXISTS buildings;
DROP TABLE IF EXISTS asset_types;
DROP TABLE IF EXISTS login_sessions;
DROP TABLE IF EXISTS users;

CREATE TABLE users
(
  id    INT         NOT NULL AUTO_INCREMENT,
  login VARCHAR(64) NOT NULL UNIQUE,
  hash  VARCHAR(64) NOT NULL,
  PRIMARY KEY (id)
) ENGINE = InnoDB;

CREATE TABLE login_sessions
(
  id              INT         NOT NULL AUTO_INCREMENT,
  user            INT         NOT NULL,
  token           VARCHAR(64) NOT NULL UNIQUE,
  expiration_date DATETIME    NOT NULL,
  create_date     DATETIME    NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT fk_login_user FOREIGN KEY (user)
    REFERENCES users (id)
    ON DELETE CASCADE
) ENGINE = InnoDB;

CREATE TABLE asset_types
(
  id     INT         NOT NULL AUTO_INCREMENT,
  letter CHAR(1)     NOT NULL UNIQUE,
  name   VARCHAR(64) NOT NULL UNIQUE,
  PRIMARY KEY (id)
) ENGINE = InnoDB;

CREATE TABLE buildings
(
  id   INT         NOT NULL AUTO_INCREMENT,
  name VARCHAR(64) NOT NULL UNIQUE,
  PRIMARY KEY (id)
) ENGINE = InnoDB;

CREATE TABLE rooms
(
  id       INT         NOT NULL AUTO_INCREMENT,
  name     VARCHAR(64) NOT NULL,
  building INT         NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT fk_room_building FOREIGN KEY (building)
    REFERENCES buildings (id)
    ON DELETE CASCADE
) ENGINE = InnoDB;

CREATE TABLE assets
(
  id   INT NOT NULL AUTO_INCREMENT,
  type INT NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT fk_asset_assettype FOREIGN KEY (type)
    REFERENCES asset_types (id)
    ON DELETE CASCADE
) ENGINE = InnoDB;

CREATE TABLE reports
(
  id          INT      NOT NULL AUTO_INCREMENT,
  name        VARCHAR(64),
  room        INT      NOT NULL,
  owner       INT      NOT NULL,
  create_date DATETIME NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT fk_report_room FOREIGN KEY (room)
    REFERENCES rooms (id)
    ON DELETE CASCADE,
  CONSTRAINT fk_report_user FOREIGN KEY (owner)
    REFERENCES users (id)
    ON DELETE CASCADE
) ENGINE = InnoDB;

CREATE TABLE reports_positions
(
  report_id     INT     NOT NULL,
  asset_id      INT     NOT NULL,
  previous_room INT     NULL,
  present       BOOLEAN NOT NULL DEFAULT TRUE,
  PRIMARY KEY (report_id, asset_id),
  CONSTRAINT fk_reportPosition_report FOREIGN KEY (report_id)
    REFERENCES reports (id)
    ON DELETE CASCADE,
  CONSTRAINT fk_reportPosition_asset FOREIGN KEY (asset_id)
    REFERENCES assets (id)
    ON DELETE CASCADE
) ENGINE = InnoDB;

CREATE TABLE scans
(
  id          INT      NOT NULL AUTO_INCREMENT,
  room        INT      NOT NULL,
  owner       INT      NOT NULL,
  create_date DATETIME NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT fk_scan_room FOREIGN KEY (room)
    REFERENCES rooms (id)
    ON DELETE CASCADE,
  CONSTRAINT fk_scan_user FOREIGN KEY (owner)
    REFERENCES users (id)
    ON DELETE CASCADE
) ENGINE = InnoDB;

CREATE TABLE scans_positions
(
  scan  INT NOT NULL,
  asset INT NOT NULL,
  state INT NOT NULL,
  PRIMARY KEY (scan, asset),
  CONSTRAINT fk_scansPosition_report FOREIGN KEY (scan)
    REFERENCES scans (id)
    ON DELETE CASCADE,
  CONSTRAINT fk_scans_asset FOREIGN KEY (asset)
    REFERENCES assets (id)
    ON DELETE CASCADE
) ENGINE = InnoDB;
INSERT INTO asset_types (letter, name)
VALUES ('c', 'komputer'),
       ('k', 'krzesło'),
       ('m', 'monitor'),
       ('p', 'projektor'),
       ('s', 'stół'),
       ('t', 'tablica')
;

/* b 34 - 1 */
INSERT INTO buildings (name)
VALUES ('b 34'),
       ('b 4'),
       ('b 5'),
       ('b 6'),
       ('b 7'),
       ('b 21'),
       ('b 22'),
       ('b 23'),
       ('b 32'),
       ('b 33') /* Empty */
;

/* from b 34 1-3 */
INSERT INTO rooms (name, building)
VALUES ('3/6', 1),
       ('3/40', 1),
       ('3/19', 1),
       ('1/2', 2),
       ('1/3', 3),
       ('1/4', 4),
       ('1/5', 5),
       ('1/6', 6),
       ('1/7', 7),
       ('1/8', 8),
       ('1/9', 9)
;

/* 10 * 6 */
INSERT INTO assets (type)
VALUES (1),
       (2),
       (3),
       (4),
       (5),
       (6), /* Set 1 */
       (1),
       (2),
       (3),
       (4),
       (5),
       (6), /* Set 2 */
       (1),
       (2),
       (3),
       (4),
       (5),
       (6), /* Set 3 */
       (1),
       (2),
       (3),
       (4),
       (5),
       (6), /* Set 4 */
       (1),
       (2),
       (3),
       (4),
       (5),
       (6), /* Set 5 */
       (1),
       (2),
       (3),
       (4),
       (5),
       (6), /* Set 6 */
       (1),
       (2),
       (3),
       (4),
       (5),
       (6), /* Set 7 */
       (1),
       (2),
       (3),
       (4),
       (5),
       (6), /* Set 8 */
       (1),
       (2),
       (3),
       (4),
       (5),
       (6), /* Set 9 */
       (1),
       (2),
       (3),
       (4),
       (5),
       (6) /* Set 10 */
;

/* User1 password 111 ... */
INSERT INTO users (login, hash)
VALUES ('user1', '$2y$10$GefRLqURuebo9eyOrYm83O0zFeGlvVt3UujXgDd02gyjZwH6NO6N6'),
       ('user2', '$2y$10$Ft84wWFy.ugVLQ6Fy.ObT..6xVMAje.pD5.zPYZop8pADmpWmmpDu'),
       ('user3', '$2y$10$rqC2MTTutu/gIrhDNUf9v.y58sclDNuWPZwLKVtIKxkJ8gfHF5P.y'),
       ('user4', '$2y$10$i5O.B5ZGF9lvPS5ALIXim.4dtydW4D0qjymW5e86Jv3ulJG8CRFOO'),
       ('user5', '$2y$10$ZM4NpuGcj.Wb0EAqRpA6JuFNw.oxJCExZdcR3uEiDEp7wYwBxJVEm')
;

INSERT INTO reports (name, room, owner, create_date)
VALUES ('Raport 1', 1, 1, NOW() - INTERVAL 10 DAY), /* new Room 21 report 1 */

       ('Raport 2', 2, 1, NOW() - INTERVAL 9 DAY), /* new */

       ('Raport 3 po 1', 1, 1, NOW() - INTERVAL 8 DAY), /* Room 21 report 2 */

       ('Raport 4', 3, 1, NOW() - INTERVAL 7 DAY), /* new */

       ('Raport 5 po 3', 1, 1, NOW() - INTERVAL 6 DAY), /* Room 21 report 3 */

       ('Raport 6', 4, 1, NOW() - INTERVAL 5 DAY) /* new b. 4 */
;

/* asset_id - 30 zestawów 6 elementowych */
INSERT INTO reports_positions (report_id, asset_id, previous_room, present)
VALUES
  /* Raport 1 */
  (1, 0 + 1, NULL, TRUE),
  (1, 0 + 2, NULL, TRUE),
  (1, 0 + 3, NULL, TRUE),
  (1, 0 + 4, NULL, TRUE),
  (1, 0 + 5, NULL, TRUE),
  (1, 0 + 6, NULL, TRUE),

  /* Raport 2 */
  (2, 6 + 1, NULL, TRUE),
  (2, 6 + 2, NULL, TRUE),
  (2, 6 + 3, NULL, TRUE),
  (2, 6 + 4, NULL, TRUE),
  (2, 6 + 5, NULL, TRUE),
  (2, 6 + 6, NULL, TRUE),

  /* Raport 3 po 1 */
  (3, 0 + 1, 1, TRUE), /* The same */
  (3, 0 + 2, 1, TRUE), /* The same */
  (3, 0 + 3, 1, TRUE), /* The same */
  (3, 0 + 4, 1, TRUE), /* The same */
  (3, 0 + 5, 1, FALSE), /* Deleted */
  (3, 0 + 6, 1, FALSE), /* Deleted */
  (3, 6 + 1, 2, TRUE), /* From room 2 */
  (3, 6 + 2, 2, TRUE), /* From room 2 */

  /* Raport 4 */
  (4, 12 + 1, NULL, TRUE),
  (4, 12 + 2, NULL, TRUE),
  (4, 12 + 3, NULL, TRUE),
  (4, 12 + 4, NULL, TRUE),
  (4, 12 + 5, NULL, TRUE),
  (4, 12 + 6, NULL, TRUE),

  /* Raport 5 po 3 */
  (5, 0 + 1, 1, TRUE), /* The same */
  (5, 0 + 2, 1, TRUE), /* The same */
  (5, 0 + 3, 1, FALSE), /* Deleted */
  (5, 0 + 4, 1, FALSE), /* Deleted */
  (5, 6 + 1, 1, TRUE), /* The same */
  (5, 6 + 2, 1, TRUE), /* The same */
  (5, 12 + 1, 3, TRUE), /* From room 3 */
  (5, 12 + 2, 3, TRUE), /* From room 3 */
  (5, 6 + 3, 2, FALSE), /* From room 2 but not move */
  (5, 6 + 4, 2, FALSE), /* From room 2 but not move */

  /* Raport 6 */
  (6, 18 + 1, NULL, TRUE),
  (6, 18 + 2, NULL, TRUE),
  (6, 18 + 3, NULL, TRUE),
  (6, 18 + 4, NULL, TRUE),
  (6, 18 + 5, NULL, TRUE),
  (6, 18 + 6, NULL, TRUE)
;

INSERT INTO login_sessions (user, token, expiration_date, create_date)
VALUES (1, 'fake-token', NOW() + INTERVAL 1 YEAR, NOW() - INTERVAL 1 DAY)
;

INSERT INTO scans (room, owner, create_date)
VALUES (1, 1, NOW());

INSERT INTO scans_positions (scan, asset, state)
VALUES (1, 1, 0),
       (1, 2, 0),
       (1, 3, 0),
       (1, 4, 0),
       (1, 5, 0),
       (1, 6, 0),
       (1, 7, 0),
       (1, 15, 0)
;
DROP FUNCTION IF EXISTS assetExists;

DELIMITER $
CREATE FUNCTION assetExists(_id INT) RETURNS BOOLEAN
BEGIN
  RETURN (SELECT COUNT(*) FROM assets WHERE assets.id = _id) = 1;
END $ DELIMITER ;
DROP FUNCTION IF EXISTS asset_typeExists;

DELIMITER $
CREATE FUNCTION asset_typeExists(_id INT) RETURNS BOOLEAN
BEGIN
  RETURN (SELECT COUNT(*) FROM asset_types WHERE asset_types.id = _id) = 1;
END $ DELIMITER ;
DROP FUNCTION IF EXISTS buildingExists;

DELIMITER $
CREATE FUNCTION buildingExists(_id INT) RETURNS BOOLEAN
BEGIN
  RETURN (SELECT COUNT(*) FROM buildings WHERE buildings.id = _id) = 1;
END $ DELIMITER ;
DROP FUNCTION IF EXISTS getRoomIdWithAsset;

/*  If the asset is not allocated function returns NULL */

DELIMITER $
CREATE FUNCTION getRoomIdWithAsset(Id_asset INT) RETURNS INT
BEGIN
  DECLARE Room_id INT DEFAULT NULL;
  DECLARE Deleted BOOLEAN DEFAULT TRUE;

  SELECT reports.room,
         NOT reports_positions.present
  INTO
    Room_id,
    Deleted
  FROM reports_positions
         JOIN reports ON reports_positions.report_id = reports.id
  WHERE reports_positions.asset_id = Id_asset
    AND NOT (
      reports_positions.previous_room != reports.room
      AND NOT reports_positions.present
    ) /* skip 'do nothing' positions */
  ORDER BY reports.create_date DESC,
           reports.id DESC
  LIMIT 1;

  IF Deleted THEN
    SET Room_id = NULL;
  END IF;

  RETURN Room_id;
END $ DELIMITER ;
DROP FUNCTION IF EXISTS haveDuplicates;

DELIMITER $
CREATE FUNCTION haveDuplicates(Table_name VARCHAR(32), Ids VARCHAR(1024), Have_duplicates BOOLEAN) RETURNS VARCHAR(64)
BEGIN
  RETURN IF(Have_duplicates, NULL, CONCAT(Table_name, ' zawiera dublikaty (id = ', Ids, ')'));
END $ DELIMITER ;
DROP FUNCTION IF EXISTS idsNotFound;

DELIMITER $
CREATE FUNCTION idsNotFound(Table_name VARCHAR(32), Ids VARCHAR(1024), Is_found BOOLEAN) RETURNS VARCHAR(64)
BEGIN
  RETURN IF(Is_found, NULL, CONCAT(Table_name, ' nie istnieje (id = ', Ids, ')'));
END $ DELIMITER ;
DROP FUNCTION IF EXISTS reportExists;

DELIMITER $
CREATE FUNCTION reportExists(_id INT) RETURNS BOOLEAN
BEGIN
  RETURN (SELECT COUNT(*) FROM reports WHERE reports.id = _id) = 1;
END $ DELIMITER ;
DROP FUNCTION IF EXISTS roomExists;

DELIMITER $
CREATE FUNCTION roomExists(_id INT) RETURNS BOOLEAN
BEGIN
  RETURN (SELECT COUNT(*) FROM rooms WHERE rooms.id = _id) = 1;
END $ DELIMITER ;
DROP FUNCTION IF EXISTS scanExists;

DELIMITER $
CREATE FUNCTION scanExists(_id INT) RETURNS BOOLEAN
BEGIN
  RETURN (SELECT COUNT(*) FROM scans WHERE scans.id = _id) = 1;
END $ DELIMITER ;
DROP FUNCTION IF EXISTS userExists;

DELIMITER $
CREATE FUNCTION userExists(_id INT) RETURNS BOOLEAN
BEGIN
  RETURN (SELECT COUNT(*) FROM users WHERE users.id = _id) = 1;
END $ DELIMITER ;
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
    WHERE reports.name REGEXP CONCAT(Report_name, '\s?\d*')

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
