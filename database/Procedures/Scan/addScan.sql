DROP PROCEDURE IF EXISTS addScan;

DELIMITER $
CREATE PROCEDURE addScan(IN Room_id INT, IN Owner_id INT)
BEGIN
  DECLARE Room_exits BOOLEAN DEFAULT roomExists(Room_id);
  DECLARE Owner_exits BOOLEAN DEFAULT userExists(Owner_id);

  IF NOT Room_exits OR NOT Owner_exits THEN
    SELECT NULL AS id,
           CONCAT_WS(
               ', ',
               idsNotFound('Pomieszczenie', Room_id, Room_exits),
               idsNotFound('Użytkownik', Owner_id, Owner_exits)
             )  AS message;
  ELSE

    INSERT INTO scans (room, owner, create_date)
    VALUES (Room_id, Owner_id, NOW());

    SELECT LAST_INSERT_ID() AS id, NULL AS message;

  END IF;

END $ DELIMITER ;
