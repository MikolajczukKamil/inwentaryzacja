<?php
include_once '../config/Database.php';
include_once '../repository/ReportAssetRepository.php';

/**
 * Klasa odpowiadajaca za obsluge rzeczy zwiazanych z raportami srodkow trwalych
 * 
 */
class ReportAssetService
{
    /**
     * Funkcja prosi repozytorium aby odpytalo baze, czy zawiera w sobie element o danym id.
     * (srodki trwale w pokoju na podstawie ostatniego raportu)
     * Jeżeli zawiera, to repozytorium zwraca funkcji obiekt (srodki trwale),a funkcja zwraca go jako json.
     * Jeżeli nie, to zwracany jest błąd (string)
     * @param integer $room_id id pokoju ktory jest sprawdzany
     */

    public static function getAssetsInRoom($room_id)
    {
        // get database connection
        $database = new Database();
        $db = $database->getConnection();

        // create a repository instance
        $rar = new ReportAssetRepository($db);

        $response = $rar->getAssetsInRoom($room_id);
        if(is_string($response))
        {
            http_response_code(404);
            echo json_encode(array("message" => $response));
        }
        else if(count($response)>0)
        {
            //everything went OK, assets were found
            http_response_code(200);
            echo json_encode($response);
        }
        else {
            http_response_code(200); // last report for room was not found
            echo json_encode([]);
        }
    }
    /**
     * Funkcja prosi repozytorium aby odpytalo baze, czy zawiera w sobie element o danym id.
     * (Sprawdza pozycje (srodki trwale) raportu na podstawie jego id)
     * Jeżeli zawiera, to repozytorium zwraca funkcji obiekt (srodki trwale z raportu), a funkcja zwraca go jako json
     * @param integer $report_id id raportu
     */
    public static function getPositionsInReport($report_id)
    {
        // get database connection
        $database = new Database();
        $db = $database->getConnection();

        // create a repository instance
        $rar = new ReportAssetRepository($db);

        $response = $rar->getPositionsInReport($report_id);
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
}