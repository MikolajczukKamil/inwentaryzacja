DROP TABLE IF EXISTS scannings_positions;
DROP TABLE IF EXISTS scannings;
DROP TABLE IF EXISTS reports_positions;
DROP TABLE IF EXISTS reports;
DROP TABLE IF EXISTS assets;
DROP TABLE IF EXISTS rooms;
DROP TABLE IF EXISTS buildings;
DROP TABLE IF EXISTS asset_types;
DROP TABLE IF EXISTS login_sessions;
DROP TABLE IF EXISTS users;CREATE TABLE users (id INT NOT NULL AUTO_INCREMENT,login VARCHAR(64) NOT NULL UNIQUE,hash VARCHAR(64) NOT NULL,PRIMARY KEY(id)
) ENGINE=InnoDB;CREATE TABLE login_sessions (id INT NOT NULL AUTO_INCREMENT,user INT NOT NULL,token VARCHAR(64) NOT NULL UNIQUE,expiration_date DATETIME NOT NULL,create_date DATETIME NOT NULL,PRIMARY KEY(id),CONSTRAINT fk_login_user FOREIGN KEY(user)REFERENCES users(id)ON DELETE CASCADE
) ENGINE=InnoDB;CREATE TABLE asset_types (id INT NOT NULL AUTO_INCREMENT,letter CHAR(1) NOT NULL UNIQUE,name VARCHAR(64) NOT NULL UNIQUE,PRIMARY KEY(id)
) ENGINE=InnoDB;CREATE TABLE buildings (id INT NOT NULL AUTO_INCREMENT,name VARCHAR(64) NOT NULL UNIQUE,PRIMARY KEY(id)
) ENGINE=InnoDB;CREATE TABLE rooms (id INT NOT NULL AUTO_INCREMENT,name VARCHAR(64) NOT NULL,building INT NOT NULL,PRIMARY KEY(id),CONSTRAINT fk_room_building FOREIGN KEY(building)REFERENCES buildings(id)ON DELETE CASCADE
) ENGINE=InnoDB;CREATE TABLE assets (id INT NOT NULL AUTO_INCREMENT,type INT NOT NULL,PRIMARY KEY(id),CONSTRAINT fk_asset_assettype FOREIGN KEY(type)REFERENCES asset_types(id)ON DELETE CASCADE
) ENGINE=InnoDB;CREATE TABLE reports (id INT NOT NULL AUTO_INCREMENT,name VARCHAR(64),room INT NOT NULL,owner INT NOT NULL,create_date DATETIME NOT NULL,PRIMARY KEY(id),CONSTRAINT fk_report_room FOREIGN KEY(room)REFERENCES rooms(id)ON DELETE CASCADE,CONSTRAINT fk_report_user FOREIGN KEY(owner)REFERENCES users(id)ON DELETE CASCADE
) ENGINE=InnoDB;CREATE TABLE reports_positions (report_id INT NOT NULL,asset_id INT NOT NULL,previous_room INT NULL,present BOOLEAN NOT NULL DEFAULT TRUE,PRIMARY KEY(report_id, asset_id),CONSTRAINT fk_reportPosition_report FOREIGN KEY(report_id)REFERENCES reports(id)ON DELETE CASCADE,CONSTRAINT fk_reportPosition_asset FOREIGN KEY(asset_id)REFERENCES assets(id)ON DELETE CASCADE
) ENGINE=InnoDB;CREATE TABLE scannings (id INT NOT NULL AUTO_INCREMENT,room INT NOT NULL,owner INT NOT NULL,create_date DATETIME NOT NULL,PRIMARY KEY(id),CONSTRAINT fk_scanning_room FOREIGN KEY(room)REFERENCES rooms(id)ON DELETE CASCADE,CONSTRAINT fk_scanning_user FOREIGN KEY(owner)REFERENCES users(id)ON DELETE CASCADE
) ENGINE=InnoDB;CREATE TABLE scannings_positions (scanning INT NOT NULL,asset INT NOT NULL,PRIMARY KEY(scanning, asset),CONSTRAINT fk_scanningPosition_report FOREIGN KEY(scanning)REFERENCES scannings(id)ON DELETE CASCADE,CONSTRAINT fk_scanningPosition_asset FOREIGN KEY(asset)REFERENCES assets(id)ON DELETE CASCADE
) ENGINE=InnoDB;
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
;INSERT INTO reports (name, room, owner, create_date)
VALUES('Raport 1', 1, 1, NOW() - INTERVAL 10 DAY), ('Raport 2', 2, 1, NOW() - INTERVAL 9 DAY), ('Raport 3 po 1', 1, 1, NOW() - INTERVAL 8 DAY), ('Raport 4', 3, 1, NOW() - INTERVAL 7 DAY), ('Raport 5 po 3', 1, 1, NOW() - INTERVAL 6 DAY), ('Raport 6', 4, 1, NOW() - INTERVAL 5 DAY) 
;INSERT INTO reports_positions (report_id, asset_id, previous_room, present)
VALUES(1, 0 + 1, NULL, TRUE),(1, 0 + 2, NULL, TRUE),(1, 0 + 3, NULL, TRUE),(1, 0 + 4, NULL, TRUE),(1, 0 + 5, NULL, TRUE),(1, 0 + 6, NULL, TRUE),(2, 6 + 1, NULL, TRUE),(2, 6 + 2, NULL, TRUE),(2, 6 + 3, NULL, TRUE),(2, 6 + 4, NULL, TRUE),(2, 6 + 5, NULL, TRUE),(2, 6 + 6, NULL, TRUE),(3, 0 + 1, 1, TRUE), (3, 0 + 2, 1, TRUE), (3, 0 + 3, 1, TRUE), (3, 0 + 4, 1, TRUE), (3, 0 + 5, 1, FALSE), (3, 0 + 6, 1, FALSE), (3, 6 + 1, 2, TRUE), (3, 6 + 2, 2, TRUE),  (4, 12 + 1, NULL, TRUE),(4, 12 + 2, NULL, TRUE),(4, 12 + 3, NULL, TRUE),(4, 12 + 4, NULL, TRUE),(4, 12 + 5, NULL, TRUE),(4, 12 + 6, NULL, TRUE),(5, 0 + 1, 1, TRUE), (5, 0 + 2, 1, TRUE), (5, 0 + 3, 1, FALSE), (5, 0 + 4, 1, FALSE), (5, 6 + 1, 1, TRUE), (5, 6 + 2, 1, TRUE), (5, 12 + 1, 3, TRUE), (5, 12 + 2, 3, TRUE), (5, 6 + 3, 2, FALSE), (5, 6 + 4, 2, FALSE), (6, 18 + 1, NULL, TRUE),(6, 18 + 2, NULL, TRUE),(6, 18 + 3, NULL, TRUE),(6, 18 + 4, NULL, TRUE),(6, 18 + 5, NULL, TRUE),(6, 18 + 6, NULL, TRUE)
;INSERT INTO login_sessions (user, token, expiration_date, create_date)
VALUES(1, 'fake-token', NOW() + INTERVAL 1 DAY, NOW() - INTERVAL 1 DAY)
;INSERT INTO scannings (room, owner, create_date)
VALUES (1, 1, NOW());INSERT INTO scannings_positions (scanning, asset)
VALUES(1, 1),(1, 2),(1, 3),(1, 4),(1, 5),(1, 6),(1, 7),(1, 15)
;
DROP FUNCTION IF EXISTS assetExists;DELIMITER $ CREATE FUNCTION assetExists(_id INT) RETURNS BOOLEAN
BEGINRETURN (SELECT COUNT(*) FROM assets WHERE assets.id = _id) = 1;
END $ DELIMITER ;
DROP FUNCTION IF EXISTS getRoomIdWithAsset;DELIMITER $ CREATE FUNCTION getRoomIdWithAsset(id_asset INT) RETURNS INT
BEGINDECLARE Room_id INT DEFAULT NULL;DECLARE Deleted BOOLEAN DEFAULT TRUE;SELECTreports.room,NOT reports_positions.present INTORoom_id,DeletedFROMreports_positionsJOIN reports ON reports_positions.report_id = reports.idWHEREreports_positions.asset_id = id_assetAND NOT (reports_positions.previous_room != reports.roomAND NOT reports_positions.present) ORDER BYreports.create_date DESC,reports.id DESCLIMIT1;IF Deleted THENSET Room_id = NULL;END IF;RETURN Room_id;
END $ DELIMITER ;
DROP FUNCTION IF EXISTS haveDublicates;DELIMITER $ CREATE FUNCTION haveDublicates(table_name VARCHAR(32), ids VARCHAR(1024), have_dublicates BOOLEAN) RETURNS VARCHAR(64)
BEGINRETURN IF(have_dublicates, NULL, CONCAT(table_name, " id=", ids, " have duplicates"));
END $ DELIMITER ;
DROP FUNCTION IF EXISTS idsNotFound;DELIMITER $ CREATE FUNCTION idsNotFound(table_name VARCHAR(32), ids VARCHAR(1024), is_found BOOLEAN) RETURNS VARCHAR(64)
BEGINRETURN IF(is_found, NULL, CONCAT(table_name, " id=", ids, " does not exist"));
END $ DELIMITER ;
DROP FUNCTION IF EXISTS roomExists;DELIMITER $ CREATE FUNCTION roomExists(_id INT) RETURNS BOOLEAN
BEGINRETURN (SELECT COUNT(*) FROM rooms WHERE rooms.id = _id) = 1;
END $ DELIMITER ;
DROP FUNCTION IF EXISTS userExists;DELIMITER $ CREATE FUNCTION userExists(_id INT) RETURNS BOOLEAN
BEGINRETURN (SELECT COUNT(*) FROM users WHERE users.id = _id) = 1;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addNewAsset;DELIMITER $ CREATE PROCEDURE addNewAsset(IN type_id INT)
BEGINDECLARE is_type_correct BOOLEAN;SELECT (SELECT COUNT(*) FROM asset_types WHERE asset_types.id = type_id) = 1INTO is_type_correct;IF NOT is_type_correct THENSELECTNULL AS id,idsNotFound("AssetType", type_id, is_type_correct) AS message;ELSEINSERT INTO assets (type)VALUES (type_id);SELECT LAST_INSERT_ID() AS id, NULL AS message;END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getAssetInfo;DELIMITER $ CREATE PROCEDURE getAssetInfo(IN asset_id INT)
BEGINDECLARE Asset_room_id INT DEFAULT getRoomIdWithAsset(asset_id);SELECTassets.id,assets.type,asset_types.letter,asset_types.name AS asset_type_name,Asset_room_id AS room_id,rooms.name AS room_name,buildings.id AS building_id,buildings.name AS building_nameFROMassetsJOIN asset_types ON assets.type = asset_types.idLEFT JOIN rooms ON Asset_room_id = rooms.idLEFT JOIN buildings ON rooms.building = buildings.idWHEREassets.id = asset_id;END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getAssetsInRoom;DELIMITER $ CREATE PROCEDURE getAssetsInRoom(IN room_id INT)
BEGINSELECTassets.id,assets.type,asset_types.name AS asset_type_name,asset_types.letter AS asset_type_letterFROMreports_positionsJOIN reports ON reports_positions.report_id = reports.idJOIN assets ON reports_positions.asset_id = assets.idJOIN asset_types ON assets.type = asset_types.idWHEREreports_positions.report_id = (SELECT r.idFROM reports AS rWHERE r.room = room_idORDER BY r.create_date DESC, r.id DESCLIMIT 1)AND reports_positions.presentAND reports.room = getRoomIdWithAsset(reports_positions.asset_id)ORDER BYassets.id ASC;END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addBuilding;DELIMITER $
CREATE PROCEDURE addBuilding(IN building_name VARCHAR(64))
BEGINDECLARE is_name_unique BOOLEAN;SELECT (SELECT COUNT(*) FROM buildings WHERE buildings.name = building_name ) = 0INTO is_name_unique;IF NOT is_name_unique THENSELECTNULL AS id,CONCAT("Building name=", building_name, " is not unique") AS message;ELSEINSERT INTO buildings (name)VALUES (building_name);SELECT LAST_INSERT_ID() AS id, NULL AS message;END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getBuildings;DELIMITER $ CREATE PROCEDURE getBuildings()
BEGINSELECT buildings.id, buildings.nameFROM buildingsORDER BY buildings.id ASC;END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addNewReport;DELIMITER $
CREATE PROCEDURE addNewReport(IN report_name VARCHAR(64), IN report_room INT, IN report_owner INT, IN report_positions JSON)
addNewReportProcedure:BEGINDECLARE I INT;DECLARE Is_room_correct BOOLEAN DEFAULT roomExists(report_room);DECLARE Is_owner_correct BOOLEAN DEFAULT userExists(report_owner);DECLARE Positions_length INT;DECLARE Are_assets_exists BOOLEAN;DECLARE Assets_does_not_exists VARCHAR(1024);DECLARE Are_rooms_exists BOOLEAN;DECLARE Rooms_does_not_exists VARCHAR(1024);DECLARE Are_assets_duplicated BOOLEAN;DECLARE Assets_duplicated VARCHAR(1024);DECLARE New_report_id INT;IF NOT Is_room_correct OR NOT Is_owner_correct THENSELECTNULL AS id,CONCAT_WS(" AND ",idsNotFound("Room", report_room, Is_room_correct),idsNotFound("User", report_owner, Is_owner_correct)) AS message;LEAVE addNewReportProcedure;END IF;CREATE TEMPORARY TABLE Positions(id INT, previous INT, present BOOLEAN);SET I = 0;SET Positions_length = JSON_LENGTH(report_positions);WHILE (I < Positions_length) DOINSERT INTO PositionsSELECTJSON_VALUE(report_positions, CONCAT('$[',i,'].id')) AS id,NULLIF(JSON_VALUE(report_positions, CONCAT('$[',i,'].previous')), 'null') AS previous,JSON_VALUE(report_positions, CONCAT('$[',i,'].present')) AS present;SET I = I + 1;END WHILE;     SELECTCOUNT(*) = 0,GROUP_CONCAT(DISTINCT Positions.id ORDER BY Positions.id SEPARATOR ', ')INTOAre_assets_exists,Assets_does_not_existsFROM PositionsWHERE NOT assetExists(assets);SELECTCOUNT(*) = 0,GROUP_CONCAT(DISTINCT Positions.previous ORDER BY Positions.previous SEPARATOR ', ')INTOAre_rooms_exists,Rooms_does_not_existsFROM PositionsWHEREPositions.previous IS NOT NULLAND NOT roomExists(Positions.previous);SELECTCOUNT(*) = 0,GROUP_CONCAT(DISTINCT duplicates.id ORDER BY duplicates.id SEPARATOR ', ')INTOAre_assets_duplicated,Assets_duplicatedFROM(SELECT Positions.idFROM PositionsGROUP BY Positions.idHAVING COUNT(Positions.id) > 1) AS duplicates;If NOT Are_assets_exists OR NOT Are_rooms_exists OR NOT Are_assets_duplicated THENSELECTNULL AS id,CONCAT_WS(" AND ",idsNotFound("Asset", Assets_does_not_exists, Are_assets_exists),idsNotFound("Room", Rooms_does_not_exists, Are_rooms_exists),haveDublicates("Asset", Assets_duplicated, Are_assets_duplicated)) AS message;DROP TEMPORARY TABLE Positions;LEAVE addNewReportProcedure;END IF;INSERT INTO reports (name, room, owner, create_date)VALUES (report_name, report_room, report_owner, NOW());SET New_report_id = LAST_INSERT_ID();INSERT INTO reports_positions (report_id, asset_id, previous_room, present)SELECTNew_report_id, Positions.id, Positions.previous, Positions.presentFROM Positions;DROP TEMPORARY TABLE Positions;SELECTNew_report_id AS id,NULL AS message;END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getPositionsInReport;DELIMITER $ CREATE PROCEDURE getPositionsInReport(IN id_report INT)
BEGINSELECTreports_positions.asset_id,reports_positions.present,assets.type AS type_id,asset_types.letter AS type_letter,asset_types.name AS type_name,rooms.id AS previous_id,rooms.name AS previous_name,buildings.id AS previous_building_id,buildings.name AS previous_building_nameFROMreports_positionsJOIN assets ON reports_positions.asset_id = assets.idJOIN asset_types ON assets.type = asset_types.idLEFT JOIN rooms ON reports_positions.previous_room = rooms.idLEFT JOIN buildings ON rooms.building = buildings.idWHEREreports_positions.report_id = id_report;END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getReportsHeaders;
DROP PROCEDURE IF EXISTS getReportHeader;
DROP PROCEDURE IF EXISTS _ReportsHeaders;DELIMITER $ CREATE PROCEDURE _ReportsHeaders(IN user_id INT, IN report_id INT)
BEGINSELECTreports.id,reports.name,reports.create_date,users.id AS owner_id,users.login AS owner_name,rooms.id AS room_id,rooms.name AS room_name,buildings.id AS building_id,buildings.name AS building_nameFROMreportsJOIN users ON reports.owner = users.idJOIN rooms ON reports.room = rooms.idJOIN buildings ON rooms.building = buildings.idWHERE(user_id IS NULL OR users.id = user_id)AND (report_id IS NULL OR reports.id = report_id)ORDER BYreports.create_date DESC,reports.id DESC;END $ DELIMITER ;DELIMITER $ CREATE PROCEDURE getReportsHeaders(IN user_id INT)
BEGINCALL _ReportsHeaders(user_id, NULL);
END $ DELIMITER ;DELIMITER $ CREATE PROCEDURE getReportHeader(IN report_id INT)
BEGINCALL _ReportsHeaders(NULL, report_id);
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addRoom;DELIMITER $
CREATE PROCEDURE addRoom(IN room_name VARCHAR(64), IN building_id INT)
BEGINDECLARE is_building_correct BOOLEAN;DECLARE is_name_unique BOOLEAN;SELECT (SELECT COUNT(*) FROM buildings WHERE buildings.id = building_id) = 1INTO is_building_correct;SELECT (SELECT COUNT(*) FROM rooms WHERE rooms.building = building_id AND rooms.name = room_name) = 0INTO is_name_unique;IF NOT is_building_correct OR NOT is_name_unique THENSELECTNULL AS id,CONCAT_WS(" AND ",idsNotFound("Building", building_id, is_building_correct),CONCAT("Room name=", IF(NOT is_name_unique, room_name, NULL), " is not unique in Building id=", building_id)) AS message;ELSEINSERT INTO rooms (name, building)VALUES (room_name, building_id);SELECT LAST_INSERT_ID() AS id, NULL AS message;END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getRooms;DELIMITER $
CREATE PROCEDURE getRooms(IN id_building INT)
BEGINSELECTrooms.id, rooms.name,buildings.id AS building_id,buildings.name AS building_nameFROM roomsJOIN buildings ON rooms.building = buildings.idWHERE rooms.building = id_buildingORDER BY rooms.id ASC;END $ DELIMITER ;
DROP PROCEDURE IF EXISTS addScanning;DELIMITER $
CREATE PROCEDURE addScanning(IN room_id INT, IN owner_id INT)
addScanningProcedure:BEGINDECLARE Is_room_correct BOOLEAN DEFAULT roomExists(room_id);DECLARE Is_owner_correct BOOLEAN DEFAULT userExists(owner_id);IF NOT Is_room_correct OR NOT Is_owner_correct THENSELECTNULL AS id,CONCAT_WS(" AND ",idsNotFound("Room", report_room, Is_room_correct),idsNotFound("User", report_owner, Is_owner_correct)) AS message;LEAVE addScanningProcedure;END IF;END $ DELIMITER ;
DROP PROCEDURE IF EXISTS deleteScanning;DELIMITER $
CREATE PROCEDURE deleteScanning(IN scanning_id INT)
BEGINDELETE FROM scannings_positionsWHERE scannings_positions.scanning = scanning_id;DELETE FROM scanningsWHERE scannings.id = scanning_id;END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getScannings;DELIMITER $ CREATE PROCEDURE getScannings(IN user_id INT)
BEGINSELECTscannings.id,scannings.create_date,users.id AS owner_id,users.login AS owner_name,rooms.id AS room_id,rooms.name AS room_name,buildings.id AS building_id,buildings.name AS building_nameFROMscanningsJOIN users ON scannings.owner = users.idJOIN rooms ON scannings.room = rooms.idJOIN buildings ON rooms.building = buildings.idWHEREscannings.owner = user_idORDER BYreports.create_date DESC,reports.id DESC;END $ DELIMITER ;
DROP PROCEDURE IF EXISTS updateScanning;DELIMITER $
CREATE PROCEDURE updateScanning(IN scanning_id INT, IN scanning_positions JSON)
BEGINEND $ DELIMITER ;
DROP PROCEDURE IF EXISTS addLoginSession;DELIMITER $
CREATE PROCEDURE addLoginSession(IN user_id INT, IN date_expiration DATETIME, IN user_token VARCHAR(64))
BEGINDECLARE is_type_correct BOOLEAN;SELECT (SELECT COUNT(*) FROM users WHERE users.id = user_id) = 1INTO is_type_correct;IF NOT is_type_correct THENSELECTNULL AS id,idsNotFound("User", user_id, is_type_correct) AS message;ELSEINSERT INTO login_sessions (user, token, expiration_date, create_date)VALUES (user_id, user_token, date_expiration, NOW());SELECT LAST_INSERT_ID() AS id, NULL AS message;END IF;
END $ DELIMITER ;
DROP PROCEDURE IF EXISTS deleteLoginSession;DELIMITER $
CREATE PROCEDURE deleteLoginSession(IN user_token VARCHAR(64))
BEGINDELETE FROM login_sessionsWHERE login_sessions.token = user_token;END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getLoginSession;DELIMITER $ CREATE PROCEDURE getLoginSession(IN user_token VARCHAR(64)) BEGINSELECTlogin_sessions.id,login_sessions.user AS user_id,login_sessions.expiration_date,login_sessions.token,login_sessions.expiration_date <= NOW() AS expiredFROM login_sessionsWHERE login_sessions.token = user_token;END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getUser;DELIMITER $ CREATE PROCEDURE getUser(IN user_id INT)
BEGINSELECTusers.id,users.login,users.hashFROM usersWHERE users.id = user_id;END $ DELIMITER ;
DROP PROCEDURE IF EXISTS getUserByLogin;DELIMITER $ CREATE PROCEDURE getUserByLogin(IN user_login VARCHAR(64))
BEGINSELECTusers.id,users.login,users.hashFROM usersWHERE users.login = user_login;END $ DELIMITER ;
