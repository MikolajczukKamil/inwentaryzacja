DROP PROCEDURE IF EXISTS deleteScan;

DELIMITER $
CREATE PROCEDURE deleteScan(IN Scan_id INT)
BEGIN
  DELETE
  FROM scans_positions
  WHERE scans_positions.scan = Scan_id;

  DELETE
  FROM scans
  WHERE scans.id = Scan_id;

END $ DELIMITER ;
