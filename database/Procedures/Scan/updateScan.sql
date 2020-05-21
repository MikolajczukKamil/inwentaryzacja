DROP PROCEDURE IF EXISTS updateScan;

DELIMITER @
CREATE PROCEDURE updateScan(IN Scan_id INT, IN Scan_positions JSON)
updateScanProcedure:
BEGIN
    DECLARE I INT;
    DECLARE ScanPositions_length INT;
    DECLARE Is_scan_exits BOOLEAN DEFAULT scanExists(Scan_id);
    DECLARE Are_assets_exists BOOLEAN;
    DECLARE Assets_does_not_exists VARCHAR(1024);

    IF NOT Is_scan_exits THEN
        SELECT NULL                                                  AS id,
               idsNotFound('Scan', Scan_id, Is_scan_exits) AS message;
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
    SET ScanPositions_length = JSON_LENGTH(Scan_positions);

    WHILE (I < ScanPositions_length)
        DO
            INSERT INTO ScanPositions
            SELECT JSON_VALUE(Scan_positions, CONCAT(' $[', i, ']')) AS asset;

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

    INSERT INTO scans_positions (scan, asset)
    SELECT Scan_id, ScanPositions.asset
    FROM ScanPositions
    WHERE ScanPositions.asset NOT IN (
        SELECT sp.asset
        FROM scans_positions AS sp
        WHERE sp.scan = Scan_id
    );

    DELETE
    FROM scans_positions
    WHERE scans_positions.scan = Scan_id
      AND scans_positions.asset NOT IN (
        SELECT ScanPositions.asset
        FROM ScanPositions
    );

END @ DELIMITER ;
