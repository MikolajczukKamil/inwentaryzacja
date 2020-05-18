DROP PROCEDURE IF EXISTS deleteScan;

DELIMITER $
CREATE PROCEDURE deleteScan(IN scanning_id INT)
BEGIN
    DELETE
    FROM scans_positions
    WHERE scans_positions.scanning = scanning_id;

    DELETE
    FROM scans
    WHERE scans.id = scanning_id;

END $ DELIMITER ;
