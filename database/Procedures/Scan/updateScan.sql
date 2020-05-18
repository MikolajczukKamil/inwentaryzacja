DROP PROCEDURE IF EXISTS updateScan;

DELIMITER @
CREATE PROCEDURE updateScan(IN scanning_id INT, IN scanning_positions JSON)
updateScanProcedure:
BEGIN
    DECLARE I INT;
    DECLARE ScanPositions_length INT;
    DECLARE Is_scanning_correct BOOLEAN DEFAULT scanningExists(scanning_id);
    DECLARE Are_assets_exists BOOLEAN;
    DECLARE Assets_does_not_exists VARCHAR(1024);

    IF NOT Is_scanning_correct THEN
        SELECT NULL                                                      AS id,
               idsNotFound('Scan', scanning_id, Is_scanning_correct) AS message;
        LEAVE updateScanProcedure;
    END IF;

    /*
      Parsing JSON array to table `ScanPositions`
      int[]
    */
    CREATE TEMPORARY TABLE ScanPositions
    (
        asset INT
    );

    SET I = 0;
    SET ScanPositions_length = JSON_LENGTH(scanning_positions);

    WHILE (I < ScanPositions_length)
        DO
            INSERT INTO ScanPositions
            SELECT JSON_VALUE(scanning_positions, CONCAT(' $[', i, ']')) AS asset;

            SET I = I + 1;
        END WHILE;

    SELECT COUNT(*) = 0,
           GROUP_CONCAT(DISTINCT ScanPositions.asset ORDER BY ScanPositions.asset SEPARATOR ', ')
    INTO
        Are_assets_exists,
        Assets_does_not_exists
    FROM ScanPositions
    WHERE NOT assetExists(ScanPositions.asset);

    If NOT Are_assets_exists THEN
        SELECT NULL                                                            AS id,
               idsNotFound('Asset', Assets_does_not_exists, Are_assets_exists) AS message;

        DROP TEMPORARY TABLE ScanPositions;
        LEAVE updateScanProcedure;
    END IF;

    INSERT INTO scans_positions (scanning, asset)
    SELECT scanning_id, ScanPositions.asset
    FROM ScanPositions
    WHERE ScanPositions.asset NOT IN (
        SELECT sp.asset
        FROM scans_positions AS sp
        WHERE sp.scanning = scanning_id
    );

    DELETE
    FROM scans_positions
    WHERE scans_positions.scanning = scanning_id
      AND scans_positions.asset NOT IN (
        SELECT ScanPositions.asset
        FROM ScanPositions
    );

END @ DELIMITER ;
