DROP PROCEDURE IF EXISTS addNewAsset;

DELIMITER $
CREATE PROCEDURE addNewAsset(IN Type_id INT)
BEGIN
    DECLARE is_type_exits BOOLEAN DEFAULT asset_typeExists(Type_id);

    IF NOT is_type_exits THEN
        SELECT NULL                                               AS id,
               idsNotFound('AssetType', Type_id, is_type_exits) AS message;
    ELSE
        INSERT INTO assets (type)
        VALUES (Type_id);

        SELECT LAST_INSERT_ID() AS id, NULL AS message;
    END IF;
END $ DELIMITER ;
