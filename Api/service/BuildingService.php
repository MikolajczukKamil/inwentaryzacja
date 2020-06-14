<?php
include_once '../interfaces/IService.php';
include_once '../object/Building.php';
include_once '../repository/BuildingRepository.php';
include_once '../config/Database.php';

/**
 * Klasa zarzadzajaca ustawieniami budynkow w bazie danych
 * 
 */
class BuildingService implements IService
{
    /**
     * Funkcja prosi repozytorium aby odpytalo baze, czy zawiera w sobie element o danym id.
     * Jeżeli zawiera, to repozytorium zwraca funkcji obiekt (pokoje z budynku o danym id), a funkcja zwraca go jako json
     * @param integer $building_id id budynku
     */

    public static function findAllRooms($building_id)
    {
        // get database connection
        $database = new Database();
        $db = $database->getConnection();

        // create a repository instance
        $rr = new BuildingRepository($db);

        $response = $rr->findAllRooms($building_id);
        if(is_string($response))
        {
            http_response_code(404);
            echo json_encode(array("message" => $response));
        }
        else
        {
            http_response_code(200);
            echo json_encode($response);
        }
    }

    /**
     * Funkcja prosi repozytorium aby odpytalo baze, czy zawiera w sobie element o danym id.
     * Jeżeli zawiera, to repozytorium zwraca funkcji obiekt (budynek), a funkcja zwraca go jako json
     * @param integer $id id szukanego budynku
     * @return mixed|void - zwraca obiekt (budynek) z bazy
     */
    static function findOneById($id)
    {
        // get database connection
        $database = new Database();
        $db = $database->getConnection();

        // create a repository instance
        $br = new BuildingRepository($db);

        $building = $br->find($id);

        if($building!=null)
        {
            //everything went OK, asset was found
            http_response_code(200);
            echo json_encode($building);
        }
        else {
            http_response_code(404); // building was not found
            echo json_encode(["message" => "Budynek nie istnieje"]);
        }
    }

    /**
     * Funkcja prosi repozytorium aby odpytalo baze, o wszystkie elementy.
     * Repozytorium zwraca funkcji wszystkie obiekty (budynki), a funkcja zwraca je jako json
     * @return mixed|void - zwraca obiekty (budynki) z bazy
     */
    static function findAll()
    {
        // get database connection
        $database = new Database();
        $db = $database->getConnection();

        // create a repository instance
        $br = new BuildingRepository($db);

        $buildings = $br->findAll();

        if($buildings['count']>0)
        {
            http_response_code(200);
            echo json_encode($buildings["buildings"]);
        }
        else
        {
            http_response_code(404);
            echo json_encode(array("message" => "Nie znaleziono żadnych budynków"));
        }
    }

    /**
     * Funkcja prosi repozytorium aby dodalo nowy budynek do bazy
     * @param object $data dane dodawanego obiektu (budynku)
     */
    static function addNew($data)
    {
        if(property_exists($data, 'name'))
        {
            $building = new Building();
            $building->setName($data->name);

            //init database
            $database = new Database();
            $db = $database->getConnection();

            $br = new BuildingRepository($db);
            $resp = $br->addNew($building);
            if($resp['id'] != null)
            {
                $id = (int)$resp['id'];
                http_response_code(201);
                echo json_encode(array("message" => "Budynek został utworzony", "id" => $id));
            }
            else if ($resp['message']!=null)
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

    /**
     * Funkcja prosi repozytorium aby odpytalo baze, czy zawiera w sobie element o danym id.
     * Jeżeli zawiera, to repozytorium usuwa z bazy danych ten element (budynek).
     * @param integer $id id usuwanego obiektu
     */
    static function deleteOneById($id)
    {
        // get database connection
        $database = new Database();
        $db = $database->getConnection();

        // create a repository instance
        $br = new BuildingRepository($db);

        if($br->deleteOne($id))
        {
            http_response_code(200);
            echo json_encode(array("message" => "Budynek został usunięty."));
        }
        else {
            http_response_code(503);
            echo json_encode(array("message" => "Niepowodzenie. Usługa chwilowo niedostępna."));
        }
    }
}