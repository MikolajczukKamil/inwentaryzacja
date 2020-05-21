DROP FUNCTION IF EXISTS asset_typeExists;

DELIMITER $
CREATE FUNCTION asset_typeExists(_id INT) RETURNS BOOLEAN
BEGIN
    RETURN (SELECT COUNT(*) FROM asset_types WHERE asset_types.id = _id) = 1;
END $ DELIMITER ;
