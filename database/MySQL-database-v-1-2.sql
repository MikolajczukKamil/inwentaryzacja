/*
  ALTER DATABASE mmaz DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci;

  USE mmaz;
*/

/* Tables */

  DROP TABLE IF EXISTS asset_types;

  DROP TABLE IF EXISTS rooms;

  DROP TABLE IF EXISTS assets;

  DROP TABLE IF EXISTS reports;

  DROP TABLE IF EXISTS reports_assets;

  DROP TABLE IF EXISTS users;

  DROP TABLE IF EXISTS buildings;

  DROP TABLE IF EXISTS login_sessions;

  CREATE TABLE asset_types(
    id TINYINT NOT NULL AUTO_INCREMENT,
    letter CHAR(1) NOT NULL UNIQUE,
    name VARCHAR(20) NOT NULL UNIQUE,
    PRIMARY KEY(id)
  );

  CREATE TABLE buildings(
    id TINYINT NOT NULL AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE,
    PRIMARY KEY(id)
  );

  CREATE TABLE rooms(
    id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(30) NOT NULL,
    building TINYINT NOT NULL,
    PRIMARY KEY(id),
    FOREIGN KEY(building) REFERENCES buildings(id)
  );

  CREATE TABLE assets(
    id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(30) NOT NULL,
    asset_type TINYINT NOT NULL REFERENCES asset_types(id),
    PRIMARY KEY(id)
  );

  CREATE TABLE users(
    id TINYINT NOT NULL AUTO_INCREMENT,
    login VARCHAR(20) NOT NULL UNIQUE,
    salt VARCHAR(30) NOT NULL,
    hash varchar(64) NOT NULL,
    PRIMARY KEY(id)
  );

  CREATE TABLE reports (
    id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(50),
    room INT NOT NULL REFERENCES rooms(id) ON DELETE CASCADE,
    create_date DATETIME NOT NULL,
    OWNER TINYINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    PRIMARY KEY(id)
  );

  /* Joining table between assets and reports */
  CREATE TABLE reports_assets (
    report_id INT NOT NULL REFERENCES reports(id) ON DELETE CASCADE,
    asset_id INT NOT NULL REFERENCES assets(id) ON DELETE CASCADE,
    previous_room INT REFERENCES rooms(id) ON DELETE CASCADE,
    present BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY(report_id, asset_id)
  );

  CREATE TABLE login_sessions (
    id INT NOT NULL AUTO_INCREMENT,
    user_id TINYINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    token varchar(64) NOT NULL,
    expiration_date DATETIME NOT NULL,
    create_date DATETIME NOT NULL,
    PRIMARY KEY(id)
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

/* Fake data */

  INSERT INTO asset_types (letter, name)
  VALUES
    ('c', 'komputer'),
    ('k', 'krzesło'),
    ('m', 'monitor'),
    ('p', 'projektor'),
    ('s', 'stół'),
    ('t', 'tablica')
  ;

  /* b 34 - 1 */
  INSERT INTO buildings (name)
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
  INSERT INTO rooms (name, building)
  VALUES
    ('3/6', 1),
    ('3/40', 1),
    ('3/19', 1),
    ('1/1', 2),
    ('1/2', 2),
    ('1/3', 3),
    ('1/4', 4),
    ('1/5', 5),
    ('1/6', 6),
    ('1/7', 7),
    ('1/8', 8),
    ('1/9', 9)
  ;
  
  INSERT INTO assets (name, asset_type) /* 30 */
  VALUES
    ('c1', 1),
    ('kr1', 2),
    ('m1', 3),
    ('p2', 4),
    ('s1', 5),
    ('t1', 6),
    ('c2', 1),
    ('kr2', 2),
    ('m2', 3),
    ('p2', 4),
    ('s2', 5),
    ('t2', 6),
    ('c3', 1),
    ('kr3', 2),
    ('m3', 3),
    ('p3', 4),
    ('s3', 5),
    ('t3', 6),
    ('c4', 1),
    ('kr4', 2),
    ('m4', 3),
    ('p4', 4),
    ('s4', 5),
    ('t4', 6),
    ('c5', 1),
    ('kr5', 2),
    ('m5', 3),
    ('p5', 4),
    ('s5', 5),
    ('t5', 6),
    ('c6', 1),
    ('kr6', 2),
    ('m6', 3),
    ('p6', 4),
    ('s6', 5),
    ('t6', 6),
    ('c7', 1),
    ('kr7', 2),
    ('m7', 3),
    ('p7', 4),
    ('s7', 5),
    ('t7', 6),
    ('c8', 1),
    ('kr8', 2),
    ('m8', 3),
    ('p8', 4),
    ('s8', 5),
    ('t8', 6),
    ('c9', 1),
    ('kr9', 2),
    ('m9', 3),
    ('p9', 4),
    ('s9', 5),
    ('t9', 6),
    ('c10', 1),
    ('k10', 2),
    ('m10', 3),
    ('p10', 4),
    ('s10', 5),
    ('t10', 6),
    ('c11', 1),
    ('kr11', 2),
    ('m11', 3),
    ('p11', 4),
    ('s11', 5),
    ('t11', 6),
    ('c12', 1),
    ('kr12', 2),
    ('m12', 3),
    ('p12', 4),
    ('s12', 5),
    ('t12', 6),
    ('c13', 1),
    ('kr13', 2),
    ('m13', 3),
    ('p13', 4),
    ('s13', 5),
    ('t13', 6),
    ('c14', 1),
    ('kr14', 2),
    ('m14', 3),
    ('p14', 4),
    ('s14', 5),
    ('t14', 6),
    ('c15', 1),
    ('kr15', 2),
    ('m15', 3),
    ('p15', 4),
    ('s15', 5),
    ('t15', 6),
    ('c16', 1),
    ('kr16', 2),
    ('m16', 3),
    ('p16', 4),
    ('s16', 5),
    ('t16', 6),
    ('c17', 1),
    ('kr17', 2),
    ('m17', 3),
    ('p17', 4),
    ('s17', 5),
    ('t17', 6),
    ('c18', 1),
    ('kr18', 2),
    ('m18', 3),
    ('p18', 4),
    ('s18', 5),
    ('t18', 6),
    ('c19', 1),
    ('kr19', 2),
    ('m19', 3),
    ('p19', 4),
    ('s19', 5),
    ('t19', 6),
    ('c20', 1),
    ('kr20', 2),
    ('m20', 3),
    ('p20', 4),
    ('s20', 5),
    ('t20', 6),
    ('c21', 1),
    ('kr21', 2),
    ('m21', 3),
    ('p21', 4),
    ('s21', 5),
    ('t21', 6),
    ('c22', 1),
    ('kr22', 2),
    ('m22', 3),
    ('p22', 4),
    ('s22', 5),
    ('t22', 6),
    ('c23', 1),
    ('kr23', 2),
    ('m23', 3),
    ('p23', 4),
    ('s23', 5),
    ('t23', 6),
    ('c24', 1),
    ('kr24', 2),
    ('m24', 3),
    ('p24', 4),
    ('s24', 5),
    ('t24', 6),
    ('c25', 1),
    ('kr25', 2),
    ('m25', 3),
    ('p25', 4),
    ('s25', 5),
    ('t25', 6),
    ('c26', 1),
    ('kr26', 2),
    ('m26', 3),
    ('p26', 4),
    ('s26', 5),
    ('t26', 6),
    ('c27', 1),
    ('kr27', 2),
    ('m27', 3),
    ('p27', 4),
    ('s27', 5),
    ('t27', 6),
    ('c28', 1),
    ('kr28', 2),
    ('m28', 3),
    ('p28', 4),
    ('s28', 5),
    ('t28', 6),
    ('c29', 1),
    ('kr29', 2),
    ('m29', 3),
    ('p29', 4),
    ('s29', 5),
    ('t29', 6),
    ('c30', 1),
    ('kr30', 2),
    ('m30', 3),
    ('p30', 4),
    ('s30', 5),
    ('t30', 6)
  ;
  
  /* User1 password 111 ... */
  INSERT INTO users (login, salt, hash)
  VALUES
    (
      'user1', '', '$2y$10$GefRLqURuebo9eyOrYm83O0zFeGlvVt3UujXgDd02gyjZwH6NO6N6'
    ),
    (
      'user2', '', '$2y$10$Ft84wWFy.ugVLQ6Fy.ObT..6xVMAje.pD5.zPYZop8pADmpWmmpDu'
    ),
    (
      'user3', '', '$2y$10$rqC2MTTutu/gIrhDNUf9v.y58sclDNuWPZwLKVtIKxkJ8gfHF5P.y'
    ),
    (
      'user4', '', '$2y$10$i5O.B5ZGF9lvPS5ALIXim.4dtydW4D0qjymW5e86Jv3ulJG8CRFOO'
    ),
    (
      'user5', '', '$2y$10$ZM4NpuGcj.Wb0EAqRpA6JuFNw.oxJCExZdcR3uEiDEp7wYwBxJVEm'
    )
  ;
  
  INSERT INTO reports (name, room, create_date, owner)
  VALUES
    ('Raport 1', 1, NOW() - INTERVAL 10 DAY, 1), /* new */

    ('Raport 2', 2, NOW() - INTERVAL 9 DAY, 1), /* new */
    
    ('Raport 1 po 1', 1, NOW() - INTERVAL 8 DAY, 1), /* Room 21 report 2 */

    ('Raport 3', 3, NOW() - INTERVAL 7 DAY, 1), /* new */
    
    ('Raport 2 po 1 po 1', 3, NOW() - INTERVAL 6 DAY, 1), /* Room 21 report 3 */

    ('Raport 4', 4, NOW() - INTERVAL 5 DAY, 1) /* new */
  ;

  /* asset_id - 30 zestawów 6 elementowych */
  INSERT INTO reports_assets (report_id, asset_id, previous_room, present)
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

    /* Raport 1 po 1 */
    (3, 0 + 1, 1, TRUE), /* The same */
    (3, 0 + 2, 1, TRUE), /* The same */
    (3, 0 + 3, 1, TRUE), /* The same */
    (3, 0 + 4, 1, TRUE), /* The same */
    (3, 0 + 5, 1, FALSE), /* Deleted */
    (3, 0 + 6, 1, FALSE), /* Deleted */
    (3, 6 + 1, 2, TRUE), /* From room 2 */
    (3, 6 + 2, 2, TRUE),  /* From room 2 */

    /* Raport 3 */
    (4, 12 + 1, NULL, TRUE),
    (4, 12 + 2, NULL, TRUE),
    (4, 12 + 3, NULL, TRUE),
    (4, 12 + 4, NULL, TRUE),
    (4, 12 + 5, NULL, TRUE),
    (4, 12 + 6, NULL, TRUE),

    /* Raport 2 po 1 po 1 */
    (5, 0 + 1, 1, TRUE), /* The same */
    (5, 0 + 2, 1, TRUE), /* The same */
    (5, 0 + 3, 1, FALSE), /* Deleted */
    (5, 0 + 4, 1, FALSE), /* Deleted */
    (5, 6 + 1, 1, TRUE), /* The same */
    (5, 6 + 2, 1, TRUE), /* The same */
    (5, 12 + 1, 3, TRUE), /* From room 3 */
    (5, 12 + 2, 3, TRUE), /* From room 3 */

    /* Raport 4 */
    (6, 18 + 1, NULL, TRUE),
    (6, 18 + 2, NULL, TRUE),
    (6, 18 + 3, NULL, TRUE),
    (6, 18 + 4, NULL, TRUE),
    (6, 18 + 5, NULL, TRUE),
    (6, 18 + 6, NULL, TRUE)
  ;

/* Examples */

    /* Assets with rooms */

    /*
    SELECT
        *,
        getRoomIdWithAsset(assets.id) AS IamInRoomId
    FROM
        assets;
    */
