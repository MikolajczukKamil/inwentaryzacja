DROP FUNCTION IF EXISTS idsNotFound;

DELIMITER $
CREATE FUNCTION idsNotFound(Table_name VARCHAR(32), Ids VARCHAR(1024), Is_found BOOLEAN) RETURNS VARCHAR(64)
BEGIN
    RETURN IF(Is_found, NULL, CONCAT(Table_name, ' nie istnieje (id = ', Ids, ')'));
END $ DELIMITER ;
