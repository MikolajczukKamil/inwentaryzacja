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
