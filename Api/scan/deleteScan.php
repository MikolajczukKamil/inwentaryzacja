<?php
//required headers
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");
include_once '../service/ScanService.php';
include_once '../security/Security.php';

$id = isset($_GET['id']) ? $_GET['id'] : die();
if(Security::performAuthorization())
{
    ScanService::deleteOne($id);
}