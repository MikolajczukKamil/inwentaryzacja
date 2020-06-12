DROP PROCEDURE IF EXISTS getScanPositions;

DELIMITER $
CREATE PROCEDURE getScanPositions(IN Scan_id INT)
BEGIN
    DECLARE Is_scan_exits BOOLEAN DEFAULT scanExists(Scan_id);

    IF NOT Is_scan_exits THEN
        SELECT idsNotFound('Skanowanie', Scan_id, Is_scan_exits) AS message,
               NULL                                           AS state,
               NULL                                           AS id,
               NULL                                           AS type,
               NULL                                           AS letter,
               NULL                                           AS asset_type_name,
               NULL                                           AS room_id,
               NULL                                           AS room_name,
               NULL                                           AS building_id,
               NULL                                           AS building_name;
    ELSE

        SELECT NULL             AS message,
               scans_positions.state,
               assets.id,
               assets.type,
               asset_types.letter,
               asset_types.name AS asset_type_name,
               rooms.id         AS room_id,
               rooms.name       AS room_name,
               buildings.id     AS building_id,
               buildings.name   AS building_name
        FROM scans_positions
                 JOIN assets ON scans_positions.asset = assets.id
                 JOIN asset_types ON assets.type = asset_types.id
                 LEFT JOIN rooms ON getRoomIdWithAsset(assets.id) = rooms.id
                 LEFT JOIN buildings ON rooms.building = buildings.id
        WHERE scans_positions.scan = Scan_id;

    END IF;

END $ DELIMITER ;
