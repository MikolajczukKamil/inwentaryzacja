-- Pobranie nagłówków raportów

SELECT
  reports.id, reports.name, reports.create_date, reports.owner AS owner_id,
  users.login AS owner_name,
  rooms.name AS room_name,
  buildings.name AS building_name
FROM
  reports
JOIN
  users ON reports.owner = users.id
JOIN
  rooms ON reports.room = rooms.id
JOIN
  buildings ON rooms.building = buildings.id
ORDER BY
  reports.create_date DESC,
  reports.id DESC
;

-- Pobranie zawartości raportu
-- Parametr: @ReportId

SELECT
  reports_assets.asset_id, reports_assets.previous_room, reports_assets.present,
  assets.name AS asset_name, assets.asset_type,
  asset_types.name AS asset_type_name
FROM
  reports_assets
JOIN
  assets ON reports_assets.asset_id = assets.id
JOIN
  asset_types ON assets.asset_type = asset_types.id
WHERE
  reports_assets.report_id = @ReportId
;

-- Pobranie listy przedmiotów z danej sali
-- Parametr: @RoomId

SELECT
  assets.id, assets.name, assets.asset_type,
  asset_types.name AS asset_type_name,
  IFNULL(reports_assets.previous_room, 0) = 0 AS new_asset,
  IFNULL(reports_assets.previous_room, reports.room) != reports.room AS moved,
  reports_assets.previous_room AS moved_from_id,
  rooms.name AS moved_from_name
FROM
  reports_assets
JOIN
  reports ON reports_assets.report_id = reports.id
JOIN
  assets ON reports_assets.asset_id = assets.id
JOIN
  asset_types ON assets.asset_type = asset_types.id
LEFT JOIN
  rooms ON reports_assets.previous_room = rooms.id
WHERE
  reports_assets.report_id = (
    SELECT
      reports.id
    FROM
      reports
    WHERE
      reports.room = @RoomId
    ORDER BY
      reports.create_date DESC,
      reports.id DESC
    LIMIT 1
  ) AND 
  reports_assets.present AND
  reports.room = getRoomIdWithAsset(reports_assets.asset_id)
;
