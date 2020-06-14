<?php
include_once '../repository/RoomRepository.php';
include_once '../config/Database.php';
include_once '../object/Room.php';
include_once '../object/Building.php';

/**
 * Klasa posrednia pomiedzy otrzymaniem danych a wstawieniem ich do bazy danych
 * Obsluguje wszystko zwiazane z pokojami
 */

class RoomService
{

    /**
     * Funkcja prosi repozytorium aby dodalo nowy pokoj do bazy
     * @param object $data dane dodawanego pokoju
     */

    public static function addNew($data)
    {
        if(property_exists($data, 'name') && property_exists($data, 'building'))
        {
            $room = new Room();
            $room->setName($data->name);

            $building = new Building();
            $building->setId($data->building);

            $room->setBuilding($building);

            //init database
            $database = new Database();
            $db = $database->getConnection();

            $rr = new RoomRepository($db);

            $resp = $rr->addNew($room);
            if($resp['id']!=null)
            {
                $id = (int)$resp['id'];
                http_response_code(201);
                echo json_encode(array("message" => "Sala została utworzona.", "id" => $id));
            }
            else if($resp['message']!=null)
            {
                http_response_code(409);
                echo json_encode(array("message" => $resp['message'], "id" => null));
            }
            else
            {
                http_response_code(503);
                echo json_encode(array("message" => "Niepowodzenie. Usługa chwilowo niedostępna.", "id" => null));
            }
        }
        else
        {
            http_response_code(400);
            echo json_encode(array("message" => "Niepowodzenie. Przekazano niekompletne dane."));
        }
    }


}