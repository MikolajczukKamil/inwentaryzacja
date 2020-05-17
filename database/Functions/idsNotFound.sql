DROP FUNCTION IF EXISTS idsNotFound;

DELIMITER $ CREATE FUNCTION idsNotFound(table_name VARCHAR(32), ids VARCHAR(1024), is_found BOOLEAN) RETURNS VARCHAR(64)
BEGIN
  RETURN IF(is_found, NULL, CONCAT(table_name, " id=", ids, " does not exist"));
END $ DELIMITER ;
