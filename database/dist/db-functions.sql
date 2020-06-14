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
