DROP FUNCTION IF EXISTS haveDuplicates;

DELIMITER $
CREATE FUNCTION haveDuplicates(Table_name VARCHAR(32), Ids VARCHAR(1024), Have_duplicates BOOLEAN) RETURNS VARCHAR(64)
BEGIN
    RETURN IF(Have_duplicates, NULL, CONCAT(Table_name, ' zawiera dublikaty (id = ', Ids, ')'));
END $ DELIMITER ;
