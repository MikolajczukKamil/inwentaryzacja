DROP PROCEDURE IF EXISTS getPositionsInReport;

DELIMITER $ CREATE PROCEDURE getPositionsInReport(IN id_report INT)
BEGIN
  SELECT
    reports_positions.asset_id,
    reports_positions.present,
    assets.type AS type_id,
    asset_types.letter AS type_letter,
    asset_types.name AS type_name,
    rooms.id AS previous_id,
    rooms.name AS previous_name,
    buildings.id AS previous_building_id,
    buildings.name AS previous_building_name
  FROM
    reports_positions
    JOIN assets ON reports_positions.asset_id = assets.id
    JOIN asset_types ON assets.type = asset_types.id
    LEFT JOIN rooms ON reports_positions.previous_room = rooms.id
    LEFT JOIN buildings ON rooms.building = buildings.id
  WHERE
    reports_positions.report_id = id_report;

END $ DELIMITER ;
