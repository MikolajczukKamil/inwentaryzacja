DROP PROCEDURE IF EXISTS getAssetInfo;

DELIMITER $
CREATE PROCEDURE getAssetInfo(IN Asset_id INT)
BEGIN
    DECLARE is_asset_exits BOOLEAN DEFAULT assetExists(Asset_id);
    DECLARE Asset_room_id INT DEFAULT getRoomIdWithAsset(Asset_id);

    IF NOT is_asset_exits THEN
        SELECT idsNotFound('Asset', Asset_id, is_asset_exits) AS message,
               NULL                                             AS id,
               NULL                                             AS type,
               NULL                                             AS letter,
               NULL                                             AS asset_type_name,
               NULL                                             AS room_id,
               NULL                                             AS room_name,
               NULL                                             AS building_id,
               NULL                                             AS building_name;
    ELSE

        SELECT NULL             AS message,
               assets.id,
               assets.type,
               asset_types.letter,
               asset_types.name AS asset_type_name,
               Asset_room_id    AS room_id,
               rooms.name       AS room_name,
               buildings.id     AS building_id,
               buildings.name   AS building_name
        FROM assets
                 JOIN asset_types ON assets.type = asset_types.id
                 LEFT JOIN rooms ON Asset_room_id = rooms.id
                 LEFT JOIN buildings ON rooms.building = buildings.id
        WHERE assets.id = Asset_id;

    END IF;

END $ DELIMITER ;
