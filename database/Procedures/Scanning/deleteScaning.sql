DROP PROCEDURE IF EXISTS deleteScanning;

DELIMITER $
CREATE PROCEDURE deleteScanning(IN scanning_id INT)
BEGIN
  DELETE FROM scannings_positions
  WHERE scannings_positions.scanning = scanning_id;

  DELETE FROM scannings
  WHERE scannings.id = scanning_id;

END $ DELIMITER ;
