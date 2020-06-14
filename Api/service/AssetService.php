<?php
include_once '../repository/AssetRepository.php';
include_once '../config/Database.php';


/**
 * Klasa zarzadzajaca srodkami trwalymi
 * 
 */


class AssetService

{
    /**
     * Funkcja prosi repozytorium aby odpytalo baze, czy zawiera w sobie element o danym id.
     * Jeżeli zawiera, to repozytorium zwraca funkcji obiekt (srodek trwaly), a funkcja zwraca go jako json
     * @param integer $id id szukanego elementu w bazie
     */

    static function findOneById($id)
    {
        $database = new Database();
        $db = $database->getConnection();

        $ar = new AssetRepository($db);

        $response = $ar->find($id);

        if(!is_string($response))
        {
            http_response_code(200);
            echo json_encode($response);
        }
        else {
            http_response_code(404);
            echo json_encode(["message" => $response]);
        }
    }


   

    /**
     * Funkcja prosi repozytorium aby dodalo nowy srodek trwaly do bazy
     * @param object $data dane nowego elementu
     */

    static function addNew($data)
    {
        if(
            property_exists($data, 'type')
        )
        {
            $asset = new Asset();
            $asset_type = new AssetType();
            $asset_type->setId($data->type);
            $asset->setAssetType($asset_type);

            $database = new Database();
            $db = $database->getConnection();

            $ar = new AssetRepository($db);

            $resp =$ar->addNew($asset);

            if($resp['id']!=null)
            {
                $id = (int)$resp['id'];
                http_response_code(201);
                echo json_encode(array("message" => "Środek trwały został utworzony.", "id" => $id));
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

    /**
     * Funkcja prosi repozytorium aby odpytalo baze, czy zawiera w sobie element o danym id.
     * Jeżeli zawiera, to repozytorium usuwa z bazy danych ten element (srodek trwaly).
     * @param integer $id id srodka trwalego
     */
    public static function deleteOneById($id)
    {
        $database = new Database();
        $db = $database->getConnection();

        $ar = new AssetRepository($db);

        if($ar->deleteOne($id))
        {
            http_response_code(200);
            echo json_encode(array("message" => "Środek został usunięty."));
        }
        else {
            http_response_code(404);
            echo json_encode(array("message" => "Nie znaleziono środka w bazie."));
        }
    }
}