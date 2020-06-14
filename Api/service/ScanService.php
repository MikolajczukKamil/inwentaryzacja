<?php
include_once '../repository/ScanRepository.php';
include_once '../repository/UserRepository.php';

/**
 * Klasa zarzadzajaca skanami
 */
class ScanService
{
    /**
     * Funkcja prosi repozytorium aby odpytalo baze, o wszystkie skany.
     * Repozytorium zwraca funkcji obiekt (skany),a funkcja zwraca go jako json.
     */
    public static function getScans()
    {
        $database = new Database();
        $db = $database->getConnection();

        $ur = new UserRepository($db);
        $user = $ur->findCurrentUser();

        $sr = new ScanRepository($db);
        $response = $sr->getScans($user->getId());

        if(!is_string($response))
        {
            http_response_code(200);
            echo json_encode($response);
        }
        else
        {
            http_response_code(200);
            echo json_encode(array("message" => $response));
        }
    }

    /**
     * Zwraca liste srodkow trwalych w skanie w formacie JSON
     * @param integer $id id skanu
     */
    public static function getScanPositions($id)
    {
        $database = new Database();
        $db = $database->getConnection();

        $sr = new ScanRepository($db);
        $response = $sr->getScanPositions($id);

        if(!is_string($response))
        {
            http_response_code(200);
            echo json_encode($response);
        }
        else
        {
            http_response_code(404);
            echo json_encode(array("message" => $response));
        }
    }

    /**
     * Funkcja prosi repozytorium aby dodalo nowy skan do bazy
     * @param object $data dane nowego elementu (skanu)
     */
    static function addNew($data)
    {
        if(property_exists($data,'room'))
        {
            //init database
            $database = new Database();
            $db = $database->getConnection();

            $sr = new ScanRepository($db);

            $ur = new UserRepository($db);
            $user = $ur->findCurrentUser();

            $resp = $sr->addScan($data->room, $user->getId());

            if($resp['id']!=null)
            {
                $id = (int)$resp['id'];
                http_response_code(201);
                echo json_encode(array("message" => "Zapis został utworzony.", "id" => $id));
            }
            else if($resp['message']!=null)
            {
                http_response_code(409);
                echo json_encode(array("message" => $resp['message'], "id"=> null));
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
     * Jeżeli zawiera, to repozytorium usuwa z bazy danych ten element (skan).
     * @param integer $id id skanu
     */
    public static function deleteOne($id)
    {
        //init database
        $database = new Database();
        $db = $database->getConnection();

        $sr = new ScanRepository($db);
        if($sr->deleteScan($id))
        {
            http_response_code(200);
            echo json_encode(array("message" => "Skan został usunięty."));
        }
        else
        {
            http_response_code(503);
            echo json_encode(array("message" => "Niepowodzenie. Usługa chwilowo niedostępna."));
        }
    }
    /**
     * Funkcja prosi repozytorium aby odpytalo baze, aby zaktualizowac dany skan.
     * @param object $data dane skanu do zaktualizowania
     */
    public static function updateScan($data)
    {
        if(property_exists($data,'id') && property_exists($data,'positions')) {
            $positions = $data->positions;
            foreach ($positions as $position) {
                if (!property_exists($position,'asset') || !property_exists($position,'state')) {
                    http_response_code(400);
                    echo json_encode(array("message" => "Niepowodzenie. Przekazano niekompletne dane."));
                    exit();
                }
            }
        }
        else
        {
            http_response_code(400);
            echo json_encode(array("message" => "Niepowodzenie. Przekazano niekompletne dane."));
            exit();
        }

        //init database
        $database = new Database();
        $db = $database->getConnection();

        $sr = new ScanRepository($db);
        if($sr->updateScan($data->id, $data->positions))
        {
            http_response_code(200);
            echo json_encode(array("message" => "Skan został zaktualizowany."));
        }
        else
        {
            http_response_code(503);
            echo json_encode(array("message" => "Niepowodzenie. Usługa chwilowo niedostępna."));
        }
    }
}