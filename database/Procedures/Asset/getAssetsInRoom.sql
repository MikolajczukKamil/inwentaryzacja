DROP PROCEDURE IF EXISTS getAssetsInRoom;

DELIMITER $
CREATE PROCEDURE getAssetsInRoom(IN room_id INT)
BEGIN
    SELECT assets.id,
           assets.type,
           asset_types.name   AS asset_type_name,
           asset_types.letter AS asset_type_letter
    FROM reports_positions
             JOIN reports ON reports_positions.report_id = reports.id
             JOIN assets ON reports_positions.asset_id = assets.id
             JOIN asset_types ON assets.type = asset_types.id
    WHERE reports_positions.report_id = (
        SELECT r.id
        FROM reports AS r
        WHERE r.room = room_id
        ORDER BY r.create_date DESC, r.id DESC
        LIMIT 1
    )
      AND reports_positions.present
      AND reports.room = getRoomIdWithAsset(reports_positions.asset_id)
    ORDER BY assets.id ASC;

END $ DELIMITER ;
