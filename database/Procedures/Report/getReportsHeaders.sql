/* Report headers - all informations about Report except positions */

DROP PROCEDURE IF EXISTS getReportsHeaders;
DROP PROCEDURE IF EXISTS getReportHeader;
DROP PROCEDURE IF EXISTS _ReportsHeaders;

DELIMITER $ CREATE PROCEDURE _ReportsHeaders(IN user_id INT, IN report_id INT)
BEGIN
  SELECT
    reports.id,
    reports.name,
    reports.create_date,
    users.id AS owner_id,
    users.login AS owner_name,
    rooms.id AS room_id,
    rooms.name AS room_name,
    buildings.id AS building_id,
    buildings.name AS building_name
  FROM
    reports
    JOIN users ON reports.owner = users.id
    JOIN rooms ON reports.room = rooms.id
    JOIN buildings ON rooms.building = buildings.id
  WHERE
    (user_id IS NULL OR users.id = user_id)
    AND (report_id IS NULL OR reports.id = report_id)
  ORDER BY
    reports.create_date DESC,
    reports.id DESC;

END $ DELIMITER ;

/* Many */

DELIMITER $ CREATE PROCEDURE getReportsHeaders(IN user_id INT)
BEGIN
  CALL _ReportsHeaders(user_id, NULL);
END $ DELIMITER ;

/* Single */

DELIMITER $ CREATE PROCEDURE getReportHeader(IN report_id INT)
BEGIN
  CALL _ReportsHeaders(NULL, report_id);
END $ DELIMITER ;
