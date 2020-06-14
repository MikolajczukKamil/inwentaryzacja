DROP PROCEDURE IF EXISTS getPositionsInReport;

DELIMITER $
CREATE PROCEDURE getPositionsInReport(IN Id_report INT)
BEGIN
  DECLARE is_report_exits BOOLEAN DEFAULT reportExists(Id_report);

  IF NOT is_report_exits THEN
    SELECT idsNotFound('Raport', Id_report, is_report_exits) AS message,
           NULL                                              AS asset_id,
           NULL                                              AS present,
           NULL                                              AS type_id,
           NULL                                              AS type_letter,
           NULL                                              AS type_name,
           NULL                                              AS previous_id,
           NULL                                              AS previous_name,
           NULL                                              AS previous_building_id,
           NULL                                              AS previous_building_name;
  ELSE

    SELECT NULL               AS message,
           reports_positions.asset_id,
           reports_positions.present,
           assets.type        AS type_id,
           asset_types.letter AS type_letter,
           asset_types.name   AS type_name,
           rooms.id           AS previous_id,
           rooms.name         AS previous_name,
           buildings.id       AS previous_building_id,
           buildings.name     AS previous_building_name
    FROM reports_positions
           JOIN assets ON reports_positions.asset_id = assets.id
           JOIN asset_types ON assets.type = asset_types.id
           LEFT JOIN rooms ON reports_positions.previous_room = rooms.id
           LEFT JOIN buildings ON rooms.building = buildings.id
    WHERE reports_positions.report_id = Id_report;

  END IF;

END $ DELIMITER ;
