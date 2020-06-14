<?php
include_once '../interfaces/IRepository.php';
include_once '../object/ReportHeader.php';
include_once '../config/Database.php';
include_once '../security/BearerToken.php';
include_once '../object/Building.php';

/** Klasa do obslugi tabeli raportow */
class ReportRepository implements IRepository
{
    /** PDO wartosc polaczenia z baza */
    private $conn;

    /** string nazwa tabeli */
    private $table_name = "reports";


    /**
     * konstrukor
     * @param PDO $db polaczenie z baza
     */
    public function __construct($db)
    {
        $this->conn = $db;
    }

    /**
     * Zwraca raport o podanym id
     * @param integer $id id raportu
     * @return ReportHeader|string znaleziony raport, lub błąd zwrócony przez bazę danych
     */
    function find($id)
    {
        $query = "CALL getReportHeader(?)";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(1,$id);

        //execute query
        $stmt->execute();

        //fetch row
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        if($row['message'] != null){
            return $row['message'];
        }

        return self::createReport($row);
    }

    /**
     * Zwraca tablice z wszystkimi raportami
     * @return array|string tablica z wszystkimi raportami, lub błąd zwrócony przez bazę danych
     */
    function findAll()
    {
        $query = "CALL getLoginSession(?)";
        $stmt = $this->conn->prepare($query);

        $token = BearerToken::getBearerToken();
        $stmt->bindParam(1,$token);

        $stmt->execute();
        $row = $stmt->fetch(PDO::FETCH_ASSOC);

        $user_id = $row['user_id'];

        $query = "CALL getReportsHeaders(?)";
        $stmt = $this->conn->prepare($query);

        $stmt->bindParam(1,$user_id);

        //execute query
        $stmt->execute();
        $report_array = array();
        while($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
            if($row['message']!=null)
            {
                return $row['message'];
            }
            $report_array [] = self::createReport($row);
        }
        return $report_array;
    }

    /**
     * Usuwa raport o podanym id
     * @param integer $id id raportu do usuniecia
     * @return bool czy udalo sie usunac raport
     */
    function deleteOne($id)
    {
        $query = "DELETE
                FROM " . $this->table_name . "
                WHERE id = ?";
        //prepare_query
        $stmt = $this->conn->prepare($query);

        //sanitize data
        $id = htmlspecialchars(strip_tags($id));

        //bind parameter
        $stmt->bindParam(1,$id);

        if($stmt->execute() && $stmt->rowCount()>0)
        {
            return true;
        }
        return false;
    }

    /**
     * Dodaj nowy raport
     * @param array $report_data raport do dodania
     * @return array czy udalo sie dodac raport
     */
    function addNew($report_data)
    {
        /** @var ReportHeader $report */
        $report = $report_data['report'];
        $assets = $report_data['assets'];
        //sanitize data
        $report->setName(htmlspecialchars(strip_tags($report->getName())));
        $report->getRoom()->setId(htmlspecialchars(strip_tags($report->getRoom()->getId())));
        $this->setOwnerForReport($report);

        $query = "CALL addNewReport(:name,:room,:owner,:positions)";
        $stmt = $this->conn->prepare($query);

        $name = $report->getName();
        $room_id = $report->getRoom()->getId();
        $owner_id = $report->getOwner()->getId();
        $assets = json_encode($assets);
        $stmt->bindParam(':name', $name);
        $stmt->bindParam(':room', $room_id);
        $stmt->bindParam(':owner', $owner_id);
        $stmt->bindParam(':positions', $assets);

        if($stmt->execute())
        {
            $row = $stmt->fetch(PDO::FETCH_ASSOC);
            return [
                'message' => $row['message'],
                'id' => $row['id']
            ];
        }
        return ["message" => null, "id" => null];

    }

    /**
     * Ustawia wlasciciela podanego raportu na obecnie zalogowanego uzytkownika
     * @param ReportHeader $report raport
     */
    private function setOwnerForReport(ReportHeader $report)
    {
        $query = "CALL getLoginSession(?)";
        $stmt = $this->conn->prepare($query);

        $token = BearerToken::getBearerToken();
        $stmt->bindParam(1,$token);

        $stmt->execute();
        $row = $stmt->fetch(PDO::FETCH_ASSOC);

        $owner = new User();
        $owner->setId($row['user_id']);
        $report->setOwner($owner);
    }

    /**
     * Tworzy i zwraca metadane raportu (naglowek) na podstawie przekazanego wyniku kwerendy
     * @param array $row wynik kwerendy fetch
     * @return ReportHeader utworzony naglowek raportu
     */
    private static function createReport($row)
    {
        $report = new ReportHeader();
        $report->setId($row["id"]);
        $report->setName($row["name"]);
        try {
            $report->setCreateDate(new DateTime($row["create_date"]));
        } catch (Exception $e) {
            echo 'Exception thrown while setting CreateDate in ReportRepository on line 134: ' . $e->getMessage();
        }
        $owner = new User();
        $owner->setId($row['owner_id']);
        $owner->setLogin($row['owner_name']);
        $report->setOwner($owner);

        $room = new Room();
        $building = new Building();
        $building->setName($row['building_name']);
        $building->setId($row['building_id']);

        $room->setName($row['room_name']);
        $room->setId($row['room_id']);
        $room->setBuilding($building);

        $report->setRoom($room);
        return $report;
    }

    /**
     * Zwraca id ostatniego naglowka raportu w tabeli
     * @return integer id ostatniego naglowka raportu w tabeli
     */
    public function getLastReportID()
    {
        $query = "SELECT MAX(id) AS id FROM reports";
        $stmt = $this->conn->prepare($query);
        $stmt->execute();
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        return $row['id'];
    }
}