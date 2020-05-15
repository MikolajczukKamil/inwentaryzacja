DROP PROCEDURE IF EXISTS addNewReport;

DELIMITER $
CREATE PROCEDURE addNewReport(IN report_name VARCHAR(64), IN report_room INT, IN report_owner INT, IN report_positions JSON)
addNewReportProcedure:BEGIN
  /* DECLARE */
    DECLARE I INT;
    DECLARE Is_room_correct BOOLEAN;
    DECLARE Is_owner_correct BOOLEAN;
    DECLARE Positions_length INT;
    DECLARE Are_assets_exists BOOLEAN;
    DECLARE Assets_does_not_exists VARCHAR(1024);
    DECLARE Are_rooms_exists BOOLEAN;
    DECLARE Rooms_does_not_exists VARCHAR(1024);
    DECLARE Are_assets_duplicated BOOLEAN;
    DECLARE Assets_duplicated VARCHAR(1024);
    DECLARE New_report_id INT;
  /* END DECLARE */

  /* Check report_room and report_owner correct */

    SELECT (SELECT COUNT(*) FROM rooms WHERE rooms.id = report_room) = 1
    INTO Is_room_correct;

    SELECT (SELECT COUNT(*) FROM users WHERE users.id = report_owner) = 1
    INTO Is_owner_correct;

  /* END Check report_room and report_owner correct */

  IF NOT Is_room_correct OR NOT Is_owner_correct THEN
    SELECT
      NULL AS id,
      CONCAT_WS(
        " AND ",
        idsNotFound("Room", IF(NOT Is_room_correct, report_room, NULL)),
        idsNotFound("User", IF(NOT Is_owner_correct, report_owner, NULL))
      ) AS message
    ;
    LEAVE addNewReportProcedure;
  END IF;

  /*
    Parsing JSON array to table `Positions`
    { "id": INT, "previous": INT|NULL, "present": BOOLEAN }[]
  */
  CREATE TEMPORARY TABLE Positions(id INT, previous INT, present BOOLEAN);

  SET I = 0;
  SET Positions_length = JSON_LENGTH(report_positions);

  WHILE (I < Positions_length) DO
    INSERT INTO Positions
    SELECT
      JSON_VALUE(report_positions, CONCAT('$[',i,'].id')) AS id,
      NULLIF(JSON_VALUE(report_positions, CONCAT('$[',i,'].previous')), 'null') AS previous,
      JSON_VALUE(report_positions, CONCAT('$[',i,'].present')) AS present
    ;

    SET I = I + 1;
  END WHILE;     

  /* Check report_positions.asset_id and report_positions.previus correct */

    SELECT
      COUNT(*) = 0,
      GROUP_CONCAT(DISTINCT Positions.id ORDER BY Positions.id SEPARATOR ', ')
    INTO
      Are_assets_exists,
      Assets_does_not_exists
    FROM Positions
    LEFT JOIN
      assets ON Positions.id = assets.id
    WHERE
      assets.id IS NULL
    ;

    SELECT
      COUNT(*) = 0,
      GROUP_CONCAT(DISTINCT Positions.previous ORDER BY Positions.previous SEPARATOR ', ')
    INTO
      Are_rooms_exists,
      Rooms_does_not_exists
    FROM Positions
    LEFT JOIN
      rooms ON Positions.previous = rooms.id
    WHERE
      Positions.previous IS NOT NULL AND
      rooms.id IS NULL
    ;

    SELECT
      COUNT(*) = 0,
      GROUP_CONCAT(DISTINCT duplicates.id ORDER BY duplicates.id SEPARATOR ', ')
    INTO
      Are_assets_duplicated,
      Assets_duplicated
    FROM
      (
        SELECT Positions.id
        FROM Positions
        GROUP BY Positions.id
        HAVING COUNT(Positions.id) > 1
      ) AS duplicates
    ;

  /* END Check report_positions.asset_id and report_positions.previus correct */

  If NOT Are_assets_exists OR NOT Are_rooms_exists OR NOT Are_assets_duplicated THEN
    SELECT
      NULL AS id,
      CONCAT_WS(
        " AND ",
        idsNotFound("Asset", Assets_does_not_exists),
        idsNotFound("Room", Rooms_does_not_exists),
        CONCAT("Asset id=", Assets_duplicated, " have duplicates")
      ) AS message
    ;

    DROP TEMPORARY TABLE Positions;
    LEAVE addNewReportProcedure;
  END IF;

  /* Adding new report */

    INSERT INTO
      reports (name, room, create_date, owner)
    VALUES
      (report_name, report_room, NOW(), report_owner)
    ;

    SET New_report_id = LAST_INSERT_ID();

    INSERT INTO
      reports_assets (report_id, asset_id, previous_room, present)
    SELECT
      New_report_id, Positions.id, Positions.previous, Positions.present
    FROM
      Positions
    ;

    DROP TEMPORARY TABLE Positions;

    SELECT
      New_report_id AS id,
      NULL AS message
    ;

    /* END Adding new report */

END $ DELIMITER ;
