DROP TABLE IF EXISTS login_sessions;
DROP TABLE IF EXISTS reports_assets;
DROP TABLE IF EXISTS reports;
DROP TABLE IF EXISTS assets;
DROP TABLE IF EXISTS rooms;
DROP TABLE IF EXISTS buildings;
DROP TABLE IF EXISTS asset_types;
DROP TABLE IF EXISTS users;CREATE TABLE users (id INT NOT NULL AUTO_INCREMENT,login VARCHAR(64) NOT NULL UNIQUE,hash VARCHAR(64) NOT NULL,PRIMARY KEY(id)
);CREATE TABLE asset_types (id INT NOT NULL AUTO_INCREMENT,letter CHAR(1) NOT NULL UNIQUE,name VARCHAR(64) NOT NULL UNIQUE,PRIMARY KEY(id)
);CREATE TABLE buildings (id INT NOT NULL AUTO_INCREMENT,name VARCHAR(64) NOT NULL UNIQUE,PRIMARY KEY(id)
);CREATE TABLE rooms (id INT NOT NULL AUTO_INCREMENT,name VARCHAR(64) NOT NULL,building INT NOT NULL,PRIMARY KEY(id),CONSTRAINT fk_room_building FOREIGN KEY(building)REFERENCES buildings(id)ON DELETE CASCADE
);CREATE TABLE assets (id INT NOT NULL AUTO_INCREMENT,type INT NOT NULL,PRIMARY KEY(id),CONSTRAINT fk_asset_assettype FOREIGN KEY(type)REFERENCES asset_types(id)ON DELETE CASCADE
);CREATE TABLE reports (id INT NOT NULL AUTO_INCREMENT,name VARCHAR(64),room INT NOT NULL,create_date DATETIME NOT NULL,owner INT NOT NULL,PRIMARY KEY(id),CONSTRAINT fk_report_room FOREIGN KEY(room)REFERENCES rooms(id)ON DELETE CASCADE,CONSTRAINT fk_report_user FOREIGN KEY(owner)REFERENCES users(id)ON DELETE CASCADE
);CREATE TABLE reports_assets (report_id INT NOT NULL,asset_id INT NOT NULL,previous_room INT NULL,present BOOLEAN NOT NULL DEFAULT TRUE,PRIMARY KEY(report_id, asset_id),CONSTRAINT fk_reportasset_report FOREIGN KEY(report_id)REFERENCES reports(id)ON DELETE CASCADE,CONSTRAINT fk_reportasset_asset FOREIGN KEY(asset_id)REFERENCES assets(id)ON DELETE CASCADE
);CREATE TABLE login_sessions (id INT NOT NULL AUTO_INCREMENT,user INT NOT NULL,token VARCHAR(64) NOT NULL UNIQUE,expiration_date DATETIME NOT NULL,create_date DATETIME NOT NULL,PRIMARY KEY(id),CONSTRAINT fk_login_user FOREIGN KEY(user)REFERENCES users(id)ON DELETE CASCADE
);
INSERT INTO asset_types (letter, name)
VALUES('c', 'komputer'),('k', 'krzesło'),('m', 'monitor'),('p', 'projektor'),('s', 'stół'),('t', 'tablica')
;INSERT INTO buildings (name)
VALUES('b 34'),('b 4'),('b 5'),('b 6'),('b 7'),('b 21'),('b 22'),('b 23'),('b 32'),('b 33') 
;INSERT INTO rooms (name, building)
VALUES('3/6', 1),('3/40', 1),('3/19', 1),('1/2', 2),('1/3', 3),('1/4', 4),('1/5', 5),('1/6', 6),('1/7', 7),('1/8', 8),('1/9', 9)
;INSERT INTO assets (type)
VALUES(1), (2), (3), (4), (5), (6),(1), (2), (3), (4), (5), (6),(1), (2), (3), (4), (5), (6),(1), (2), (3), (4), (5), (6),(1), (2), (3), (4), (5), (6),(1), (2), (3), (4), (5), (6),(1), (2), (3), (4), (5), (6),(1), (2), (3), (4), (5), (6),(1), (2), (3), (4), (5), (6),(1), (2), (3), (4), (5), (6)
;INSERT INTO users (login, hash)
VALUES('user1', '$2y$10$GefRLqURuebo9eyOrYm83O0zFeGlvVt3UujXgDd02gyjZwH6NO6N6'),('user2', '$2y$10$Ft84wWFy.ugVLQ6Fy.ObT..6xVMAje.pD5.zPYZop8pADmpWmmpDu'),('user3', '$2y$10$rqC2MTTutu/gIrhDNUf9v.y58sclDNuWPZwLKVtIKxkJ8gfHF5P.y'),('user4', '$2y$10$i5O.B5ZGF9lvPS5ALIXim.4dtydW4D0qjymW5e86Jv3ulJG8CRFOO'),('user5', '$2y$10$ZM4NpuGcj.Wb0EAqRpA6JuFNw.oxJCExZdcR3uEiDEp7wYwBxJVEm')
;INSERT INTO reports (name, room, create_date, owner)
VALUES('Raport 1', 1, NOW() - INTERVAL 10 DAY, 1), ('Raport 2', 2, NOW() - INTERVAL 9 DAY, 1), ('Raport 3 po 1', 1, NOW() - INTERVAL 8 DAY, 1), ('Raport 4', 3, NOW() - INTERVAL 7 DAY, 1), ('Raport 5 po 3', 1, NOW() - INTERVAL 6 DAY, 1), ('Raport 6', 4, NOW() - INTERVAL 5 DAY, 1) 
;INSERT INTO reports_assets (report_id, asset_id, previous_room, present)
VALUES(1, 0 + 1, NULL, TRUE),(1, 0 + 2, NULL, TRUE),(1, 0 + 3, NULL, TRUE),(1, 0 + 4, NULL, TRUE),(1, 0 + 5, NULL, TRUE),(1, 0 + 6, NULL, TRUE),(2, 6 + 1, NULL, TRUE),(2, 6 + 2, NULL, TRUE),(2, 6 + 3, NULL, TRUE),(2, 6 + 4, NULL, TRUE),(2, 6 + 5, NULL, TRUE),(2, 6 + 6, NULL, TRUE),(3, 0 + 1, 1, TRUE), (3, 0 + 2, 1, TRUE), (3, 0 + 3, 1, TRUE), (3, 0 + 4, 1, TRUE), (3, 0 + 5, 1, FALSE), (3, 0 + 6, 1, FALSE), (3, 6 + 1, 2, TRUE), (3, 6 + 2, 2, TRUE),  (4, 12 + 1, NULL, TRUE),(4, 12 + 2, NULL, TRUE),(4, 12 + 3, NULL, TRUE),(4, 12 + 4, NULL, TRUE),(4, 12 + 5, NULL, TRUE),(4, 12 + 6, NULL, TRUE),(5, 0 + 1, 1, TRUE), (5, 0 + 2, 1, TRUE), (5, 0 + 3, 1, FALSE), (5, 0 + 4, 1, FALSE), (5, 6 + 1, 1, TRUE), (5, 6 + 2, 1, TRUE), (5, 12 + 1, 3, TRUE), (5, 12 + 2, 3, TRUE), (5, 6 + 3, 2, FALSE), (5, 6 + 4, 2, FALSE), (6, 18 + 1, NULL, TRUE),(6, 18 + 2, NULL, TRUE),(6, 18 + 3, NULL, TRUE),(6, 18 + 4, NULL, TRUE),(6, 18 + 5, NULL, TRUE),(6, 18 + 6, NULL, TRUE)
;INSERT INTO login_sessions (user, token, expiration_date, create_date)
VALUES(1, 'fake-token', NOW() + INTERVAL 1 DAY, NOW() - INTERVAL 1 DAY)
;
