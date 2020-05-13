<?php
$url = trim($_SERVER['REQUEST_URI'], '/');

$request = explode('/', $url);

$length = count($request);

if($length <= 1) exit("Incorrect request");

$requestedScript = $request[1];

if($length >= 3) {
  $_GET['id'] = intval($request[2]);
}

$resources = [
  'getAssetInfo' => '../asset/getAssetInfo.php',
  'addNewAsset' => '../asset/addNewAsset.php',
  'loginUser' => '../login/addLoginSession.php',
  'getBuildings' => '../building/getBuildings.php',
  'addNewBuilding' => '../building/addNewBuilding.php',
  'getRooms' => '../building/getRooms.php',
  'addNewRoom' => '../room/addNewRoom.php',
  'getAssetsInRoom' => '../room/getAssetsInRoom.php',
  'getReportHeader' => '../report/getReportHeader.php',
  'getReportsHeaders' => '../report/getReportsHeaders.php',
  'getReportPositions' => '../report/getPositionsInReport.php',
  'addNewReport' => '../report/addNewReport.php'
];

foreach ($resources as $name => $path) {
  if($requestedScript === $name) {
    require($path);

    exit();
  }
}

exit(json_encode(["message" => "Incorrect request - /{$url} not found"]));
