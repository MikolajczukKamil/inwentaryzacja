DROP FUNCTION IF EXISTS haveDublicates;

DELIMITER $ CREATE FUNCTION haveDublicates(table_name VARCHAR(32), ids VARCHAR(1024), have_dublicates BOOLEAN) RETURNS VARCHAR(64)
BEGIN
  RETURN IF(have_dublicates, NULL, CONCAT(table_name, " id=", ids, " have duplicates"));
END $ DELIMITER ;
