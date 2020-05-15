DROP FUNCTION IF EXISTS getRoomIdWithAsset;

/*  If the asset is not allocated function returns NULL */

DELIMITER $ CREATE FUNCTION getRoomIdWithAsset(id_asset INT) RETURNS INT
BEGIN
  DECLARE Room_id INT DEFAULT NULL;
  DECLARE Deleted BOOLEAN DEFAULT TRUE;

  SELECT
    reports.room,
    NOT reports_assets.present 
  INTO
    Room_id,
    Deleted
  FROM
    reports_assets
    JOIN reports ON reports_assets.report_id = reports.id
  WHERE
    reports_assets.asset_id = id_asset
    AND NOT (
      reports_assets.previous_room != reports.room
      AND NOT reports_assets.present
    ) /* skip 'do nothing' positions */
  ORDER BY
    reports.create_date DESC,
    reports.id DESC
  LIMIT
    1;

  IF Deleted THEN
    SET Room_id = NULL;
  END IF;

  RETURN Room_id;
END $ DELIMITER ;
