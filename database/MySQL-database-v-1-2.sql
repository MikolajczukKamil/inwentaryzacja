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
    present BOOLEAN NOT NULL DEFAULT TRUE
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
    ('t', 'tablica');

  INSERT INTO buildings (name)
  VALUES
    ('b 4'),
    ('b 5'),
    ('b 6'),
    ('b 7'),
    ('b 21'),
    ('b 22'),
    ('b 23'),
    ('b 32'),
    ('b 33'),
    ('b 34');

  INSERT INTO rooms (name, building)
  VALUES
    ('1/1', 1),
    ('1/2', 1),
    ('1/3', 2),
    ('1/4', 2),
    ('1/5', 2),
    ('1/6', 3),
    ('2/7', 3),
    ('3/8', 3),
    ('1/9', 4),
    ('2/10', 4),
    ('1/11', 5),
    ('2/12', 5),
    ('1/14', 6),
    ('1/25', 6),
    ('1/16', 7),
    ('1/26', 7),
    ('1/19', 8),
    ('1/11', 8),
    ('1/22', 9),
    ('1/13', 9),
    ('1/12', 10),
    ('1/13', 10),
    ('1/14', 10),
    ('1/15', 10),
    ('1/16', 10),
    ('1/17', 10),
    ('1/18', 10),
    ('1/19', 10),
    ('1/20', 10),
    ('2/1', 10);
  
  INSERT INTO assets (name, asset_type)
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
    ('t30', 6);
  
  INSERT INTO users (login, salt, hash)
  VALUES
    (
      'User_Jack', '1111111111111', 'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa'
    ),
    (
      'User_Bob', '2222222222222', 'bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb'
    ),
    (
      'User_Tory', '3333333333333', 'ccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc'
    ),
    (
      'User_Sasha', '4444444444444', 'ddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd'
    ),
    (
      'User_Robert', '5555555555555', 'eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee'
    ),
    (
      'User_Alica', '6666666666666', 'fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff'
    );
  
  INSERT INTO reports (name, room, create_date, owner)
  VALUES
    (
      'Raport_1/12', 21, NOW() - INTERVAL 10 DAY,
      1
    ),
    (
      'Raport_1/13', 22, NOW() - INTERVAL 9 DAY,
      1
    ),
    (
      'Raport_1/14', 23, NOW() - INTERVAL 8 DAY,
      1
    ),
    (
      'Raport_1/15', 24, NOW() - INTERVAL 7 DAY,
      1
    ),
    (
      'Raport_1/16', 25, NOW() - INTERVAL 6 DAY,
      1
    ),
    (
      'Raport_1/17', 26, NOW() - INTERVAL 5 DAY,
      1
    ),
    (
      'Raport_1/18', 27, NOW() - INTERVAL 4 DAY,
      1
    ),
    (
      'Raport_1/19', 28, NOW() - INTERVAL 3 DAY,
      1
    ),
    /* Room 21 report 2 */
    (
      'Raport_2/12', 21, NOW() - INTERVAL 1 DAY,
      1
    );

  INSERT INTO reports_assets (report_id, asset_id, previous_room, present)
  VALUES
    (1, 127, NULL, TRUE),
    (1, 128, NULL, TRUE),
    (1, 129, NULL, TRUE),
    (1, 130, NULL, TRUE),
    (1, 131, NULL, TRUE),
    (1, 132, NULL, TRUE),
    (2, 133, 29, TRUE),
    (2, 134, 29, TRUE),
    (2, 135, 29, TRUE),
    (2, 136, 29, TRUE),
    (2, 137, 29, TRUE),
    (2, 138, 29, TRUE),
    (3, 139, 28, TRUE),
    (3, 140, 28, TRUE),
    (3, 141, 28, TRUE),
    (3, 142, 28, TRUE),
    (3, 143, 28, TRUE),
    (3, 144, 28, TRUE),
    (4, 151, 27, TRUE),
    (4, 152, 27, TRUE),
    (4, 153, 27, TRUE),
    (4, 154, 27, TRUE),
    (4, 155, 27, TRUE),
    (4, 156, 27, TRUE),
    (5, 157, NULL, TRUE),
    (5, 158, 26, TRUE),
    (5, 159, 26, TRUE),
    (5, 160, 26, TRUE),
    (5, 161, 26, TRUE),
    (5, 162, 26, TRUE),
    (6, 163, 25, TRUE),
    (6, 164, NULL, TRUE),
    (6, 165, 25, TRUE),
    (6, 166, NULL, TRUE),
    (6, 167, 25, TRUE),
    (6, 168, NULL, TRUE),
    (7, 169, 24, TRUE),
    (7, 170, 24, TRUE),
    (7, 171, NULL, TRUE),
    (7, 172, 24, TRUE),
    (7, 173, NULL, TRUE),
    (7, 174, 24, TRUE),
    (8, 175, 23, TRUE),
    (8, 176, 23, TRUE),
    (8, 177, 23, TRUE),
    (8, 178, NULL, TRUE),
    (8, 179, 23, TRUE),
    (8, 180, NULL, TRUE),
    
    /* Second report in room 21 */
    (9, 127, NULL, TRUE), /* The same */
    (9, 128, NULL, TRUE), /* The same */
    (9, 129, NULL, TRUE), /* The same */
    (9, 130, NULL, TRUE), /* The same */
    (9, 131, NULL, FALSE), /* Deleted */
    (9, 132, NULL, FALSE), /* Deleted */
    (9, 133, 22, TRUE), /* From room 22 */
    (9, 133, 22, TRUE)  /* From room 22 */
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
