DROP PROCEDURE IF EXISTS getRooms;

DELIMITER $
CREATE PROCEDURE getRooms(IN id_building INT)
BEGIN
    SELECT rooms.id,
           rooms.name,
           buildings.id   AS building_id,
           buildings.name AS building_name
    FROM rooms
             JOIN buildings ON rooms.building = buildings.id
    WHERE rooms.building = id_building
    ORDER BY rooms.id ASC;

END $ DELIMITER ;
