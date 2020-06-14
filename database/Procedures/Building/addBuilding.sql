DROP PROCEDURE IF EXISTS addBuilding;

DELIMITER $
CREATE PROCEDURE addBuilding(IN Building_name VARCHAR(64))
BEGIN
  DECLARE Name_unique BOOLEAN;

  SELECT (SELECT COUNT(*) FROM buildings WHERE buildings.name = Building_name) = 0
  INTO Name_unique;

  IF NOT Name_unique THEN
    SELECT NULL                                                     AS id,
           CONCAT('Budynek nazwa=', Building_name, ' już istnieje') AS message;
  ELSE
    INSERT INTO buildings (name)
    VALUES (Building_name);

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
