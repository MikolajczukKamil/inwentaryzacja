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
        SELECT NULL  AS id,
               CONCAT_WS(
                       ' AND ',
                       idsNotFound('Building', Building_id, Building_exits),
                       CONCAT('Room name=', IF(NOT Name_unique, Room_name, NULL), ' is not unique in Building id=',
                              Building_id)
                   ) AS message;
    ELSE
        INSERT INTO rooms (name, building)
        VALUES (Room_name, Building_id);

        SELECT LAST_INSERT_ID() AS id, NULL AS message;
    END IF;
END $ DELIMITER ;
