DROP FUNCTION IF EXISTS haveDuplicates;

DELIMITER $
CREATE FUNCTION haveDuplicates(table_name VARCHAR(32), ids VARCHAR(1024), have_duplicates BOOLEAN) RETURNS VARCHAR(64)
BEGIN
    RETURN IF(have_duplicates, NULL, CONCAT(table_name, ' id=', ids, ' have duplicates'));
END $ DELIMITER ;
