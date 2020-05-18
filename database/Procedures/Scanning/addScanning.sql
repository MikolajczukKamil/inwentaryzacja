DROP PROCEDURE IF EXISTS addScanning;

DELIMITER $
CREATE PROCEDURE addScanning(IN room_id INT, IN owner_id INT)
BEGIN
    DECLARE Is_room_correct BOOLEAN DEFAULT roomExists(room_id);
    DECLARE Is_owner_correct BOOLEAN DEFAULT userExists(owner_id);

    IF NOT Is_room_correct OR NOT Is_owner_correct THEN
        SELECT NULL  AS id,
               CONCAT_WS(
                       ' AND ',
                       idsNotFound('Room', room_id, Is_room_correct),
                       idsNotFound('User', owner_id, Is_owner_correct)
                   ) AS message;
    ELSE

        INSERT INTO scannings (room, owner, create_date)
        VALUES (room_id, owner_id, NOW());

        SELECT LAST_INSERT_ID() AS id, NULL AS message;

    END IF;

END $ DELIMITER ;
