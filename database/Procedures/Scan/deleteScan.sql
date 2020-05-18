DROP PROCEDURE IF EXISTS deleteScan;

DELIMITER $
CREATE PROCEDURE deleteScan(IN scan_id INT)
BEGIN
    DELETE
    FROM scans_positions
    WHERE scans_positions.scan = scan_id;

    DELETE
    FROM scans
    WHERE scans.id = scan_id;

END $ DELIMITER ;
