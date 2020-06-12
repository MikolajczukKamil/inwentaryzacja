DROP PROCEDURE IF EXISTS getRooms;

DELIMITER $
CREATE PROCEDURE getRooms(IN Id_building INT)
BEGIN
  DECLARE Building_exits BOOLEAN DEFAULT buildingExists(Id_building);

  IF NOT Building_exits THEN
    SELECT idsNotFound('Budynek', Id_building, Building_exits) AS message,
           NULL                                                AS id,
           NULL                                                AS name,
           NULL                                                AS building_id,
           NULL                                                AS building_name;
  ELSE

    SELECT NULL           AS message,
           rooms.id,
           rooms.name,
           buildings.id   AS building_id,
           buildings.name AS building_name
    FROM rooms
           JOIN buildings ON rooms.building = buildings.id
    WHERE rooms.building = Id_building
    ORDER BY rooms.id ASC;

  END IF;

END $ DELIMITER ;
