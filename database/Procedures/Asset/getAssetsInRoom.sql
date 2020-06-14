DROP PROCEDURE IF EXISTS getAssetsInRoom;

DELIMITER $
CREATE PROCEDURE getAssetsInRoom(IN Room_id INT)
BEGIN
  DECLARE Room_exits BOOLEAN DEFAULT roomExists(Room_id);

  IF NOT Room_exits THEN
    SELECT idsNotFound('Pomieszczenie', Room_id, Room_exits) AS message,
           NULL                                              AS id,
           NULL                                              AS type,
           NULL                                              AS asset_type_name,
           NULL                                              AS asset_type_letter;
  ELSE

    SELECT NULL               AS message,
           assets.id,
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
      WHERE r.room = Room_id
      ORDER BY r.create_date DESC, r.id DESC
      LIMIT 1
    )
      AND reports_positions.present
      AND reports.room = getRoomIdWithAsset(reports_positions.asset_id)
    ORDER BY assets.id ASC;

  END IF;

END $ DELIMITER ;
