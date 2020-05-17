DROP PROCEDURE IF EXISTS updateScanning;

DELIMITER $
CREATE PROCEDURE updateScanning(IN scanning_id INT, IN scanning_positions JSON)
updateScanningProcedure:BEGIN
  DECLARE I INT;
  DECLARE ScanningPositions_length INT;
  DECLARE Is_scanning_correct BOOLEAN DEFAULT scanningExists(scanning_id);

  IF NOT Is_scanning_correct THEN
    SELECT
      NULL AS id,
      idsNotFound("Scanning", scanning_id, Is_scanning_correct) AS message
    ;
    LEAVE updateScanningProcedure;
  END IF;

  /*
    Parsing JSON array to table `ScanningPositions`
    int[]
  */
  CREATE TEMPORARY TABLE ScanningPositions(asset INT);

  SET I = 0;
  SET ScanningPositions_length = JSON_LENGTH(scanning_positions);

  WHILE (I < ScanningPositions_length) DO
    INSERT INTO ScanningPositions
    SELECT
      JSON_VALUE(scanning_positions, CONCAT('$[',i,']')) AS asset
    ;

    SET I = I + 1;
  END WHILE;

  

END $ DELIMITER ;
