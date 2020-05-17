DROP PROCEDURE IF EXISTS addNewAsset;

DELIMITER $ CREATE PROCEDURE addNewAsset(IN type_id INT)
BEGIN
  DECLARE is_type_correct BOOLEAN;

  SELECT (SELECT COUNT(*) FROM asset_types WHERE asset_types.id = type_id) = 1
  INTO is_type_correct;

  IF NOT is_type_correct THEN
    SELECT
      NULL AS id,
      idsNotFound("AssetType", type_id, is_type_correct) AS message
    ;
  ELSE
    INSERT INTO assets (type)
    VALUES (type_id);

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
