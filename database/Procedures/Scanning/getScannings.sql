/* Report headers - all informations about Report except positions */

DROP PROCEDURE IF EXISTS getScannings;

DELIMITER $
CREATE PROCEDURE getScannings(IN user_id INT)
BEGIN
    SELECT scannings.id,
           scannings.create_date,
           users.id       AS owner_id,
           users.login    AS owner_name,
           rooms.id       AS room_id,
           rooms.name     AS room_name,
           buildings.id   AS building_id,
           buildings.name AS building_name
    FROM scannings
             JOIN users ON scannings.owner = users.id
             JOIN rooms ON scannings.room = rooms.id
             JOIN buildings ON rooms.building = buildings.id
    WHERE scannings.owner = user_id
    ORDER BY reports.create_date DESC,
             reports.id DESC;

END $ DELIMITER ;
