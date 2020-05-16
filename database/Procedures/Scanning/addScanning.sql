DROP PROCEDURE IF EXISTS addScanning;

DELIMITER $
CREATE PROCEDURE addScanning(IN room_id INT, IN owner_id INT)
addScanningProcedure:BEGIN
  DECLARE Is_room_correct BOOLEAN DEFAULT roomExists(room_id);
  DECLARE Is_owner_correct BOOLEAN DEFAULT userExists(owner_id);

  IF NOT Is_room_correct OR NOT Is_owner_correct THEN
    SELECT
      NULL AS id,
      CONCAT_WS(
        " AND ",
        idsNotFound("Room", IF(NOT Is_room_correct, report_room, NULL)),
        idsNotFound("User", IF(NOT Is_owner_correct, report_owner, NULL))
      ) AS message
    ;
    LEAVE addScanningProcedure;
  END IF;



END $ DELIMITER ;
