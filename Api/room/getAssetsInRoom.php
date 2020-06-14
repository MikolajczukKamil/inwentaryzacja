<?php
// required headers
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Headers: access");
header("Access-Control-Allow-Methods: GET");
header("Access-Control-Allow-Credentials: true");
header('Content-Type: application/json');
include_once '../service/ReportAssetService.php';
include_once '../security/Security.php';

$id = isset($_GET['id']) ? $_GET['id'] : die();

if(Security::performAuthorization())
{
    ReportAssetService::getAssetsInRoom($id);
}