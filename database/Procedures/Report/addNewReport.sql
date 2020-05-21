DROP PROCEDURE IF EXISTS addNewReport;

DELIMITER @
CREATE PROCEDURE addNewReport(IN Report_name VARCHAR(64), IN Report_room INT, IN Report_owner INT,
                              IN Report_positions JSON)
addNewReportProcedure:
BEGIN
    DECLARE I INT;
    DECLARE Room_exits BOOLEAN DEFAULT roomExists(Report_room);
    DECLARE Owner_exits BOOLEAN DEFAULT userExists(Report_owner);
    DECLARE ReportPositions_length INT;
    DECLARE Are_assets_exists BOOLEAN;
    DECLARE Assets_does_not_exists VARCHAR(1024);
    DECLARE Are_rooms_exists BOOLEAN;
    DECLARE Rooms_does_not_exists VARCHAR(1024);
    DECLARE Are_assets_duplicated BOOLEAN;
    DECLARE Assets_duplicated VARCHAR(1024);
    DECLARE New_report_id INT;

    IF NOT Room_exits OR NOT Owner_exits THEN
        SELECT NULL  AS id,
               CONCAT_WS(
                       ' AND ',
                       idsNotFound('Room', Report_room, Room_exits),
                       idsNotFound('User', Report_owner, Owner_exits)
                   ) AS message;
        LEAVE addNewReportProcedure;
    END IF;

    /*
      Parsing JSON array to table `Positions`
      { "id": INT, "previous": INT|NULL, "present": BOOLEAN }[]
    */
    CREATE TEMPORARY TABLE ReportPositions
    (
        id       INT,
        previous INT,
        present  BOOLEAN
    );

    SET I = 0;
    SET ReportPositions_length = JSON_LENGTH(Report_positions);

    WHILE (I < ReportPositions_length)
        DO
            INSERT INTO ReportPositions
            SELECT JSON_VALUE(Report_positions, CONCAT(' $[', i, '].id'))                      AS id,
                   NULLIF(JSON_VALUE(Report_positions, CONCAT('$[', i, '].previous')), 'null') AS previous,
                   JSON_VALUE(Report_positions, CONCAT('$[', i, '].present'))                  AS present;

            SET I = I + 1;
        END WHILE;

    /* Check Report_positions.asset_id and Report_positions.previus correct */

    SELECT COUNT(*) = 0,
           GROUP_CONCAT(DISTINCT ReportPositions.id ORDER BY ReportPositions.id SEPARATOR ', ')
    INTO
        Are_assets_exists,
        Assets_does_not_exists
    FROM ReportPositions
    WHERE NOT assetExists(ReportPositions.id);

    SELECT COUNT(*) = 0,
           GROUP_CONCAT(DISTINCT ReportPositions.previous ORDER BY ReportPositions.previous SEPARATOR ', ')
    INTO
        Are_rooms_exists,
        Rooms_does_not_exists
    FROM ReportPositions
    WHERE ReportPositions.previous IS NOT NULL
      AND NOT roomExists(ReportPositions.previous);

    SELECT COUNT(*) = 0,
           GROUP_CONCAT(DISTINCT duplicates.id ORDER BY duplicates.id SEPARATOR ', ')
    INTO
        Are_assets_duplicated,
        Assets_duplicated
    FROM (
             SELECT ReportPositions.id
             FROM ReportPositions
             GROUP BY ReportPositions.id
             HAVING COUNT(ReportPositions.id) > 1
         ) AS duplicates;

    If NOT Are_assets_exists OR NOT Are_rooms_exists OR NOT Are_assets_duplicated THEN
        SELECT NULL  AS id,
               CONCAT_WS(
                       ' AND ',
                       idsNotFound('Asset', Assets_does_not_exists, Are_assets_exists),
                       idsNotFound('Room', Rooms_does_not_exists, Are_rooms_exists),
                       haveDuplicates('Asset', Assets_duplicated, Are_assets_duplicated)
                   ) AS message;

        DROP TEMPORARY TABLE ReportPositions;
        LEAVE addNewReportProcedure;
    END IF;

    /* Adding new report */

    INSERT INTO reports (name, room, owner, create_date)
    VALUES (Report_name, Report_room, Report_owner, NOW());

    SET New_report_id = LAST_INSERT_ID();

    INSERT INTO reports_positions (report_id, asset_id, previous_room, present)
    SELECT New_report_id,
           ReportPositions.id,
           ReportPositions.previous,
           ReportPositions.present
    FROM ReportPositions;

    DROP TEMPORARY TABLE ReportPositions;

    SELECT New_report_id AS id, NULL AS message;

END @ DELIMITER ;
