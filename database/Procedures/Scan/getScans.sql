/* Report headers - all informations about Report except positions */

DROP PROCEDURE IF EXISTS getScans;

DELIMITER $
CREATE PROCEDURE getScans(IN user_id INT)
BEGIN
    SELECT scans.id,
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
    WHERE scans.owner = user_id
    ORDER BY reports.create_date DESC,
             reports.id DESC;

END $ DELIMITER ;
