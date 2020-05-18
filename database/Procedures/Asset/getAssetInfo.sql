DROP PROCEDURE IF EXISTS getAssetInfo;

DELIMITER $
CREATE PROCEDURE getAssetInfo(IN asset_id INT)
BEGIN
    DECLARE Asset_room_id INT DEFAULT getRoomIdWithAsset(asset_id);

    SELECT assets.id,
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
    WHERE assets.id = asset_id;

END $ DELIMITER ;
