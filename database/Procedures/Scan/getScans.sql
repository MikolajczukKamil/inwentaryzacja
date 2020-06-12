DROP PROCEDURE IF EXISTS getScans;

DELIMITER $
CREATE PROCEDURE getScans(IN User_id INT)
BEGIN
  DECLARE Is_user_exits BOOLEAN DEFAULT userExists(User_id);

  IF NOT Is_user_exits THEN
    SELECT idsNotFound('Użytkownik', User_id, Is_user_exits) AS message,
           NULL                                              AS id,
           NULL                                              AS create_date,
           NULL                                              AS owner_id,
           NULL                                              AS owner_name,
           NULL                                              AS room_id,
           NULL                                              AS room_name,
           NULL                                              AS building_id,
           NULL                                              AS building_name;
  ELSE

    SELECT NULL           AS message,
           scans.id,
           scans.create_date,
           users.id       AS owner_id,
           users.login    AS owner_name,
           rooms.id       AS room_id,
           rooms.name     AS room_name,
           buildings.id   AS building_id,
           buildings.name AS building_name
    FROM scans
           JOIN users ON scans.owner = users.id
           JOIN rooms ON scans.room = rooms.id
           JOIN buildings ON rooms.building = buildings.id
    WHERE scans.owner = User_id
    ORDER BY scans.create_date DESC,
             scans.id DESC;

  END IF;

END $ DELIMITER ;
