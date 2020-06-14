<?php
//required headers
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");
include_once '../service/ScanService.php';
include_once '../security/Security.php';

if(Security::performAuthorization())
{
    //get posted data
    $data = json_decode(file_get_contents("php://input"));
    ScanService::updateScan($data);
}