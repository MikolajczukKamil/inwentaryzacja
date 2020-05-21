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
        SELECT idsNotFound('Scan', Scan_id, Is_scan_exits) AS message;
        LEAVE updateScanProcedure;
    END IF;

    /*
      Parsing JSON array to table `ScanPositions`
      { asset: INT, state: INT }[]
    */
    CREATE TEMPORARY TABLE ScanPositions
    (
        asset INT,
        state INT
    );

    SET I = 0;
    SET ScanPositions_length = JSON_LENGTH(Scan_positions);

    WHILE (I < ScanPositions_length)
        DO
            INSERT INTO ScanPositions
            SELECT JSON_VALUE(Scan_positions, CONCAT(' $[', i, '].asset')) AS asset,
                   JSON_VALUE(Scan_positions, CONCAT(' $[', i, '].state')) AS state;

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
        SELECT idsNotFound('Asset', Assets_does_not_exists, Are_assets_exists) AS message;

        DROP TEMPORARY TABLE ScanPositions;
        LEAVE updateScanProcedure;
    END IF;

    DELETE
    FROM scans_positions
    WHERE scans_positions.scan = Scan_id
      AND scans_positions.asset NOT IN (
        SELECT ScanPositions.asset
        FROM ScanPositions
    );

    UPDATE scans_positions
    SET scans_positions.state = (
        SELECT ScanPositions.state
        FROM ScanPositions
        WHERE ScanPositions.asset = scans_positions.asset
    )
    WHERE scans_positions.scan = Scan_id;

    INSERT INTO scans_positions (scan, asset, state)
    SELECT Scan_id, ScanPositions.asset, ScanPositions.state
    FROM ScanPositions
    WHERE ScanPositions.asset NOT IN (
        SELECT sp.asset
        FROM scans_positions AS sp
        WHERE sp.scan = Scan_id
    );

    SELECT NULL AS message;

END @ DELIMITER ;
