<?php
include_once '../interfaces/IService.php';
include_once '../object/ReportHeader.php';
include_once '../config/Database.php';
include_once '../repository/ReportRepository.php';
include_once '../object/ReportAsset.php';
include_once '../repository/ReportAssetRepository.php';
include_once '../object/Report.php';

/**
 * Klasa posrednia pomiedzy otrzymaniem danych a wstawieniem ich do bazy danych
 */
class ReportService implements IService
{
    /**
     * Funkcja prosi repozytorium aby odpytalo baze, czy zawiera w sobie element o danym id.
     * Jeżeli zawiera, to repozytorium zwraca funkcji obiekt (raport), a funkcja zwraca go jako json
     * @param integer $id id szukanego raportu
     * @return void - zwraca raport w formacie JSON jezeli jest w bazie
     */
    static function findOneById($id)
    {
        // get database connection
        $database = new Database();
        $db = $database->getConnection();

        // create a repository instance
        $rr = new ReportRepository($db);

        $response = $rr->find($id);

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
     * Funkcja prosi repozytorium aby odpytalo baze, o wszystkie elementy.
     * Repozytorium zwraca funkcji wszystkie obiekty (raporty), a funkcja zwraca je jako json
     * @return mixed|void - zwraca wszystkie raporty
     */
    static function findAll()
    {
        // get database connection
        $database = new Database();
        $db = $database->getConnection();

        // create a repository instance
        $rr = new ReportRepository($db);

        $response = $rr->findAll();
        if(is_string($response))
        {
            http_response_code(200);
            echo json_encode(array("message" => $response));
        }
        else
        {
            http_response_code(200);
            echo json_encode($response);
        }
    }

    /**
     * Funkcja prosi repozytorium aby dodalo nowy raport na podstawie jego danych do bazy
     * @param object $data dane dodawanego raportu
     */

    static function addNew($data)
    {
        if(
            property_exists($data, 'name') &&
            property_exists($data, 'room') &&
            property_exists($data, 'assets')
        )
        {
            $assets = $data->assets;
            foreach ($data->assets as $asset)
            {
                if(!property_exists($asset, 'id') || !property_exists($asset, 'present') || !property_exists($asset, 'previous')) {
                    http_response_code(400);
                    echo json_encode(array("message" => "Niepowodzenie. Przekazano niekompletne dane."));
                    exit();
                }
                if($asset->previous == -1)
                {
                    $asset->previous = null;
                }
            }
            $report = new ReportHeader();
            $room = new Room();
            $room->setId($data->room);

            $report->setName($data->name);
            $report->setRoom($room);
            $report->setCreateDate(new DateTime('now'));

            //init database
            $database = new Database();
            $db = $database->getConnection();

            $rr = new ReportRepository($db);
            $report_data = [
                'report' => $report,
                'assets' => $assets
            ];
            $resp = $rr->addNew($report_data);
            if($resp['id']!=null)
            {
                $id = (int)$resp['id'];
                http_response_code(201);
                echo json_encode(array("message" => "Raport został utworzony.", "id" => $id));
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
     * Jeżeli zawiera, to repozytorium usuwa z bazy danych ten element (raport).
     * @param integer $id id usuwanego raportu
     */

    static function deleteOneById($id)
    {
        // get database connection
        $database = new Database();
        $db = $database->getConnection();

        // create a repository instance
        $rr = new ReportRepository($db);

        if($rr->deleteOne($id))
        {
            http_response_code(200);
            echo json_encode(array("message" => "Raport został usunięty."));
        }
        else {
            http_response_code(503);
            echo json_encode(array("message" => "Niepowodzenie. Usługa chwilowo niedostępna."));
        }
    }

    static function getFullReportData($id)
    {
        // get database connection
        $database = new Database();
        $db = $database->getConnection();

        // create a repository instance
        $rr = new ReportRepository($db);

        $report_header = $rr->find($id);
        if($report_header!=null)
        {
            $rar = new ReportAssetRepository($db);
            $positions = $rar->getPositionsInReport($id);
            return new Report($report_header,$positions);
        }
        else {
            return null;
        }
    }
}