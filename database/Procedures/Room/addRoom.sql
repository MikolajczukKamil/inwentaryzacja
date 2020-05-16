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
        idsNotFound("Building", IF(NOT is_building_correct, building_id, NULL)),
        CONCAT("Room name=", IF(NOT is_name_unique, room_name, NULL), " is not unique in Building id=", building_id)
      ) AS message
    ;
  ELSE
    INSERT INTO rooms (name, building)
    VALUES (room_name, building_id);

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
