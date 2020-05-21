/* Report headers - all informations about Report except positions */

DROP PROCEDURE IF EXISTS getReportsHeaders;
DROP PROCEDURE IF EXISTS getReportHeader;
DROP PROCEDURE IF EXISTS _ReportsHeaders;

DELIMITER $
CREATE PROCEDURE _ReportsHeaders(IN User_id INT, IN Report_id INT)
BEGIN
    SELECT NULL           AS message,
           reports.id,
           reports.name,
           reports.create_date,
           users.id       AS owner_id,
           users.login    AS owner_name,
           rooms.id       AS room_id,
           rooms.name     AS room_name,
           buildings.id   AS building_id,
           buildings.name AS building_name
    FROM reports
             JOIN users ON reports.owner = users.id
             JOIN rooms ON reports.room = rooms.id
             JOIN buildings ON rooms.building = buildings.id
    WHERE (User_id IS NULL OR users.id = User_id)
      AND (Report_id IS NULL OR reports.id = Report_id)
    ORDER BY reports.create_date DESC,
             reports.id DESC;

END $ DELIMITER ;

/* Many */

DELIMITER $
CREATE PROCEDURE getReportsHeaders(IN User_id INT)
BEGIN
    DECLARE is_user_exits BOOLEAN DEFAULT userExists(User_id);

    IF NOT is_user_exits THEN
        SELECT idsNotFound('User', User_id, is_user_exits) AS message,
               NULL                                        AS id,
               NULL                                        AS name,
               NULL                                        AS create_date,
               NULL                                        AS owner_id,
               NULL                                        AS owner_name,
               NULL                                        AS room_id,
               NULL                                        AS room_name,
               NULL                                        AS building_id,
               NULL                                        AS building_name;
    ELSE
        CALL _ReportsHeaders(User_id, NULL);
    END IF;

END $ DELIMITER ;

/* Single */

DELIMITER $
CREATE PROCEDURE getReportHeader(IN Report_id INT)
BEGIN
    DECLARE is_report_exits BOOLEAN DEFAULT reportExists(Report_id);

    IF NOT is_report_exits THEN
        SELECT idsNotFound('Report', Report_id, is_report_exits) AS message,
               NULL                                              AS id,
               NULL                                              AS name,
               NULL                                              AS create_date,
               NULL                                              AS owner_id,
               NULL                                              AS owner_name,
               NULL                                              AS room_id,
               NULL                                              AS room_name,
               NULL                                              AS building_id,
               NULL                                              AS building_name;
    ELSE
        CALL _ReportsHeaders(NULL, Report_id);
    END IF;
END $ DELIMITER ;
