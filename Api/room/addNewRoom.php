<?php
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");
header("Access-Control-Allow-Methods: POST");
header("Access-Control-Max-Age: 3600");
header("Access-Control-Allow-Headers: Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With");
include_once '../service/RoomService.php';
include_once '../security/Security.php';
//get posted data
$data = json_decode(file_get_contents("php://input"));

if(Security::performAuthorization())
{
    RoomService::addNew($data);
}