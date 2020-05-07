/*
  ALTER DATABASE mmaz DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci;

  USE mmaz;
*/

/* Tables */

  DROP TABLE IF EXISTS login_sessions;
  DROP TABLE IF EXISTS reports_assets;
  DROP TABLE IF EXISTS reports;
  DROP TABLE IF EXISTS assets;
  DROP TABLE IF EXISTS rooms;
  DROP TABLE IF EXISTS buildings;
  DROP TABLE IF EXISTS asset_types;
  DROP TABLE IF EXISTS users;

  CREATE TABLE users (
    id INT NOT NULL AUTO_INCREMENT,
    login VARCHAR(20) NOT NULL UNIQUE,
    hash VARCHAR(64) NOT NULL,
    PRIMARY KEY(id)
  );

  CREATE TABLE asset_types (
    id INT NOT NULL AUTO_INCREMENT,
    letter CHAR(1) NOT NULL UNIQUE,
    name VARCHAR(20) NOT NULL UNIQUE,
    PRIMARY KEY(id)
  );

  CREATE TABLE buildings (
    id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE,
    PRIMARY KEY(id)
  );

  CREATE TABLE rooms (
    id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(30) NOT NULL,
    building INT NOT NULL,
    PRIMARY KEY(id),
    CONSTRAINT fk_room_building FOREIGN KEY(building)
      REFERENCES buildings(id)
      ON DELETE CASCADE
  );

  CREATE TABLE assets (
    id INT NOT NULL AUTO_INCREMENT,
    type INT NOT NULL,
    PRIMARY KEY(id),
    CONSTRAINT fk_asset_assettype FOREIGN KEY(type)
      REFERENCES asset_types(id)
      ON DELETE CASCADE
  );

  CREATE TABLE reports (
    id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(50),
    room INT NOT NULL,
    create_date DATETIME NOT NULL,
    owner INT NOT NULL,
    PRIMARY KEY(id),
    CONSTRAINT fk_report_room FOREIGN KEY(room)
      REFERENCES rooms(id)
      ON DELETE CASCADE,
    CONSTRAINT fk_report_user FOREIGN KEY(owner)
      REFERENCES users(id)
      ON DELETE CASCADE
  );

  CREATE TABLE reports_assets (
    report_id INT NOT NULL,
    asset_id INT NOT NULL,
    previous_room INT NULL,
    present BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY(report_id, asset_id),
    CONSTRAINT fk_reportasset_report FOREIGN KEY(report_id)
      REFERENCES reports(id)
      ON DELETE CASCADE,
    CONSTRAINT fk_reportasset_asset FOREIGN KEY(asset_id)
      REFERENCES assets(id)
      ON DELETE CASCADE,
    CONSTRAINT fk_reportasset_room FOREIGN KEY(previous_room)
      REFERENCES rooms(id)
  );

  CREATE TABLE login_sessions (
    id INT NOT NULL AUTO_INCREMENT,
    user_id INT NOT NULL,
    token VARCHAR(64) NOT NULL UNIQUE,
    expiration_date DATETIME NOT NULL,
    create_date DATETIME NOT NULL,
    PRIMARY KEY(id),
    CONSTRAINT fk_login_user FOREIGN KEY(user_id)
      REFERENCES users(id)
      ON DELETE CASCADE
  );

/* Functions */

  DROP FUNCTION IF EXISTS getRoomIdWithAsset;

  /*  If the asset is not allocated function returns -1 */

  DELIMITER $$
  CREATE FUNCTION getRoomIdWithAsset(asset_id INT) RETURNS INT
  BEGIN
    DECLARE room_id INT DEFAULT -1;
    DECLARE present BOOLEAN DEFAULT FALSE;

    SELECT
      reports.room, reports_assets.present
    INTO 
      room_id, present
    FROM
      reports
    JOIN
      reports_assets ON reports.id = reports_assets.report_id
    WHERE
      reports_assets.asset_id = asset_id
    ORDER BY
      reports.create_date DESC,
      reports.id DESC
    LIMIT
      1
    ;

    IF NOT present THEN
      SET room_id = -1;
    END IF;

    RETURN room_id;
  END $$ DELIMITER ;

/* Procedures */

  /* Pobranie nagłówków raportów */
    DROP PROCEDURE IF EXISTS getReportsHeaders;

    DELIMITER $$
    CREATE PROCEDURE getReportsHeaders()
    BEGIN
      SELECT
        reports.id, reports.name, reports.create_date, reports.owner AS owner_id,
        users.login AS owner_name,
        rooms.name AS room_name,
        buildings.name AS building_name
      FROM
        reports
      JOIN
        users ON reports.owner = users.id
      JOIN
        rooms ON reports.room = rooms.id
      JOIN
        buildings ON rooms.building = buildings.id
      ORDER BY
        reports.create_date DESC,
        reports.id DESC
      ;
    END $$ DELIMITER ;

  /* Pobranie zawartości raportu - assety w danym raporcie */
    DROP PROCEDURE IF EXISTS getAssetsInReport;

    DELIMITER $$
    CREATE PROCEDURE getAssetsInReport(IN ReportId INT)
    BEGIN
      SELECT
        reports_assets.asset_id, reports_assets.previous_room, reports_assets.present,
        assets.type AS asset_type,
        asset_types.name AS asset_type_name
      FROM
        reports_assets
      JOIN
        assets ON reports_assets.asset_id = assets.id
      JOIN
        asset_types ON assets.type = asset_types.id
      WHERE
        reports_assets.report_id = ReportId
      ;
    END $$ DELIMITER ;

  /* Pobranie listy przedmiotów z danej sali - assety w danej sali */
    DROP PROCEDURE IF EXISTS getAssetsInRoom;

    DELIMITER $$
    CREATE PROCEDURE getAssetsInRoom(IN RoomId INT)
    BEGIN
      SELECT
        assets.id, assets.type,
        asset_types.name AS asset_type_name,
        IFNULL(reports_assets.previous_room, 0) = 0 AS new_asset,
        IFNULL(reports_assets.previous_room, reports.room) != reports.room AS moved,
        reports_assets.previous_room AS moved_from_id,
        rooms.name AS moved_from_name
      FROM
        reports_assets
      JOIN
        reports ON reports_assets.report_id = reports.id
      JOIN
        assets ON reports_assets.asset_id = assets.id
      JOIN
        asset_types ON assets.type = asset_types.id
      LEFT JOIN
        rooms ON reports_assets.previous_room = rooms.id
      WHERE
        reports_assets.report_id = (
          SELECT
            reports.id
          FROM
            reports
          WHERE
            reports.room = RoomId
          ORDER BY
            reports.create_date DESC,
            reports.id DESC
          LIMIT 1
        ) AND 
        reports_assets.present AND
        reports.room = getRoomIdWithAsset(reports_assets.asset_id)
      ;
    END $$ DELIMITER ;

  /* Pobranie informacji o przedmiocie nie należącym do spodziewanej sali */
    DROP PROCEDURE IF EXISTS getAssetInfo;

    DELIMITER $$
    CREATE PROCEDURE getAssetInfo(IN AssetId INT)
    BEGIN
      SELECT
        assets.id, assets.type,
        asset_types.name AS asset_type_name,
        RIdWA.room_id AS room_id,
        rooms.name AS room_name,
        buildings.name AS building_name
      FROM
        (SELECT getRoomIdWithAsset(AssetId) AS room_id) AS RIdWA
      JOIN
        assets ON assets.id = AssetId
      JOIN
        asset_types ON assets.type = asset_types.id
      JOIN
        rooms ON RIdWA.room_id = rooms.id
      JOIN
        buildings ON rooms.building = buildings.id
      ;
    END $$ DELIMITER ;

  /* Pobranie danych uzytkownika */

    DROP PROCEDURE IF EXISTS getUser;

    DELIMITER $$
    CREATE PROCEDURE getUser(IN UserId INT)
    BEGIN
      SELECT
        users.id, users.login, users.hash
      FROM
        users
      WHERE
        users.id = UserId
      ;
    END $$ DELIMITER ;

  /* Pobranie sesji logowania */

    DROP PROCEDURE IF EXISTS getLoginSession;

    DELIMITER $$
    CREATE PROCEDURE getLoginSession(IN user_token VARCHAR(64))
    BEGIN
      SELECT
        login_sessions.id, login_sessions.user_id, login_sessions.expiration_date, login_sessions.token,
        login_sessions.expiration_date <= NOW() AS expired
      FROM
        login_sessions
      WHERE
        login_sessions.token = user_token
      ;
    END $$ DELIMITER ;
  
  /* Utworzenie sesji logowania */

    DROP PROCEDURE IF EXISTS addLoginSession;

    DELIMITER $$
    CREATE PROCEDURE addLoginSession(IN user_id INT, IN expiration_date DATETIME, IN user_token VARCHAR(64))
    BEGIN
      INSERT INTO
        login_sessions (user_id, token, expiration_date, create_date)
      VALUES 
        (user_id, user_token, expiration_date, NOW())
      ;
    END $$ DELIMITER ;

  /* Usunięcie sesji logowania */

    DROP PROCEDURE IF EXISTS deleteLoginSession;

    DELIMITER $$
    CREATE PROCEDURE deleteLoginSession(IN user_token VARCHAR(64))
    BEGIN
      DELETE
      FROM
        login_sessions
      WHERE 
        login_sessions.token = user_token
      ;
    END $$ DELIMITER ;
  /* Utworzenie nowego raportu */

    DROP PROCEDURE IF EXISTS addNewReport;

    DELIMITER $$
    CREATE PROCEDURE addNewReport(IN report_name VARCHAR(64), IN report_room INT, IN report_owner INT, IN report_positions VARCHAR(1024))
    BEGIN
      DECLARE new_report_id INT;
      DECLARE position_id INT;
      DECLARE position_previous INT;
      DECLARE position_present BOOLEAN;
      DECLARE i INT DEFAULT 0;

      INSERT INTO
        reports (name, room, create_date, owner)
      VALUES
        (report_name, report_room, NOW(), report_owner);

      SET new_report_id = LAST_INSERT_ID();

      WHILE i < JSON_LENGTH(report_positions)
      DO
        /* [ { "id": 25, "previous": 1, "present": 1 } ] */
        SET position_id = JSON_VALUE(report_positions, CONCAT('$[', i ,'].id'));
        SET position_previous = JSON_VALUE(report_positions, CONCAT('$[', i ,'].previous'));
        SET position_present = JSON_VALUE(report_positions, CONCAT('$[', i ,'].present'));

        INSERT INTO
          reports_assets (report_id, asset_id, previous_room, present)
        VALUES
          (new_report_id, position_id, previous_room, position_present)
        ;

        SET i = i + 1;
      END WHILE;
      
    END $$ DELIMITER ;

/* Fake data */

  INSERT INTO
    asset_types (letter, name)
  VALUES
    ('c', 'komputer'),
    ('k', 'krzesło'),
    ('m', 'monitor'),
    ('p', 'projektor'),
    ('s', 'stół'),
    ('t', 'tablica')
  ;

  /* b 34 - 1 */
  INSERT INTO
    buildings (name)
  VALUES
    ('b 34'),
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
  INSERT INTO
    rooms (name, building)
  VALUES
    ('3/6', 1),
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
  INSERT INTO
    assets (type)
  VALUES
    (1), (2), (3), (4), (5), (6),
    (1), (2), (3), (4), (5), (6),
    (1), (2), (3), (4), (5), (6),
    (1), (2), (3), (4), (5), (6),
    (1), (2), (3), (4), (5), (6),
    (1), (2), (3), (4), (5), (6),
    (1), (2), (3), (4), (5), (6),
    (1), (2), (3), (4), (5), (6),
    (1), (2), (3), (4), (5), (6),
    (1), (2), (3), (4), (5), (6)
  ;
  
  /* User1 password 111 ... */
  INSERT INTO
    users (login, hash)
  VALUES
    ('user1', '$2y$10$GefRLqURuebo9eyOrYm83O0zFeGlvVt3UujXgDd02gyjZwH6NO6N6'),
    ('user2', '$2y$10$Ft84wWFy.ugVLQ6Fy.ObT..6xVMAje.pD5.zPYZop8pADmpWmmpDu'),
    ('user3', '$2y$10$rqC2MTTutu/gIrhDNUf9v.y58sclDNuWPZwLKVtIKxkJ8gfHF5P.y'),
    ('user4', '$2y$10$i5O.B5ZGF9lvPS5ALIXim.4dtydW4D0qjymW5e86Jv3ulJG8CRFOO'),
    ('user5', '$2y$10$ZM4NpuGcj.Wb0EAqRpA6JuFNw.oxJCExZdcR3uEiDEp7wYwBxJVEm')
  ;
  
  INSERT INTO
    reports (name, room, create_date, owner)
  VALUES
    ('Raport 1', 1, NOW() - INTERVAL 10 DAY, 1), /* new Room 21 report 1 */

    ('Raport 2', 2, NOW() - INTERVAL 9 DAY, 1), /* new */
    
    ('Raport 3 po 1', 1, NOW() - INTERVAL 8 DAY, 1), /* Room 21 report 2 */

    ('Raport 4', 3, NOW() - INTERVAL 7 DAY, 1), /* new */
    
    ('Raport 5 po 3', 1, NOW() - INTERVAL 6 DAY, 1), /* Room 21 report 3 */

    ('Raport 6', 4, NOW() - INTERVAL 5 DAY, 1) /* new b. 4 */
  ;

  /* asset_id - 30 zestawów 6 elementowych */
  INSERT INTO
    reports_assets (report_id, asset_id, previous_room, present)
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
    (3, 6 + 2, 2, TRUE),  /* From room 2 */

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

    /* Raport 6 */
    (6, 18 + 1, NULL, TRUE),
    (6, 18 + 2, NULL, TRUE),
    (6, 18 + 3, NULL, TRUE),
    (6, 18 + 4, NULL, TRUE),
    (6, 18 + 5, NULL, TRUE),
    (6, 18 + 6, NULL, TRUE)
  ;

  INSERT INTO
    login_sessions (user_id, token, expiration_date, create_date)
  VALUES
    (1, 'fake-token', NOW() + INTERVAL 1 DAY, NOW() - INTERVAL 1 DAY)
  ;
