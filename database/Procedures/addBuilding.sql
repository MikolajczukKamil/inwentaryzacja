DROP PROCEDURE IF EXISTS addBuilding;

DELIMITER $
CREATE PROCEDURE addBuilding(IN building_name VARCHAR(64))
BEGIN
  DECLARE is_name_unique BOOLEAN;

  SELECT (SELECT COUNT(*) FROM buildings WHERE buildings.name = building_name ) = 0
  INTO is_name_unique;

  IF NOT is_name_unique THEN
    SELECT
      NULL AS id,
      CONCAT("Building name=", building_name, " is not unique") AS message
    ;
  ELSE
    INSERT INTO buildings (name)
    VALUES (building_name);

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
