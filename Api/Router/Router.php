<?php
$url = trim($_SERVER['REQUEST_URI'], '/');

/**
 * Route params array
 * @var string[]
 */
$routeParams = explode('/', $url);
if($routeParams === false) $routeParams = [];

/**
 * Route params Length
 * @var int
 */
$routeParamsLength = count($routeParams);

if($routeParamsLength <= 1) {
  exit(json_encode(["message" => "Incorrect request, require /api/script-name/param1?/param2?"]));
}

array_shift($routeParams); // /api
$routeParamsLength--;

$requestedScript = array_shift($routeParams); // /api/path
$routeParamsLength--;

if($routeParamsLength >= 1) {
  $_GET['id'] = $routeParams[0];
}

if($routeParamsLength >= 2) {
  $_GET['type'] = $routeParams[1];
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
  'addNewReport' => '../report/addNewReport.php',
  'getScans' => '../scan/getScans.php',
  'addScan' => '../scan/addScan.php',
  'deleteScan' => '../scan/deleteScan.php',
  'updateScan' => '../scan/updateScan.php',
  'getScanPositions' => '../scan/getScanPositions.php',
  'pdfGenerator'=>'../generator/pdfGenerator.php'
];

foreach ($resources as $name => $path) {
  if($requestedScript === $name) {
    require($path);

    exit();
  }
}

exit(json_encode(["message" => "Incorrect request - /{$url} not found"]));
