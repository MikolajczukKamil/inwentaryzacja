DROP PROCEDURE IF EXISTS getBuildings;

DELIMITER $ CREATE PROCEDURE getBuildings()
BEGIN
  SELECT buildings.id, buildings.name
  FROM buildings
  ORDER BY buildings.id ASC;

END $ DELIMITER ;
