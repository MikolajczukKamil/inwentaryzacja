<?php
include_once '../interfaces/IRepository.php';
include_once '../object/Building.php';
include_once '../object/Room.php';

/** Klasa do obslugi tabeli budynkow */
class BuildingRepository implements IRepository
{
    /** PDO wartosc polaczenia z baza */
    private $conn;

    /** string nazwa tabeli */
    private $table_name = "buildings";


    /**
     * konstrukor
     * @param PDO $db polaczenie z baza
     */
    public function __construct($db)
    {
        $this->conn = $db;
    }

    /**
     * Zwraca tablice z wszystkimi pokojami w podanym budynku i ich liczebnoscia
     * @param integer $building_id id budynku
     * @return array|string tablica z wszystkimi pokojami w podanym budynku, lub wiadomość o błędzie zwrócona przez bazę
     */
    function findAllRooms($building_id)
    {
        $query = "CALL getRooms(?)";
        $stmt = $this->conn->prepare($query);

        //sanitize
        $building_id = htmlspecialchars(strip_tags($building_id));

        $stmt->bindParam(1,$building_id);

        //execute query
        $stmt->execute();
        $room_array = array();
        while($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
            if($row['message'] != null)
            {
                return $row['message'];
            }
            $room = new Room();
            $room->setId($row["id"]);
            $room->setName($row["name"]);

            $building = new Building();
            $building->setId($row["building_id"]);
            $building->setName($row['building_name']);

            $room->setBuilding($building);
            $room_array [] = $room;
        }
        return $room_array;
    }

    /**
     * Zwraca budynek o podanym id
     * @param integer $id id budynku
     * @return Building|null znaleziony budynek
     */
    function find($id)
    {
        $query = "SELECT 
                b.id, b.name 
          FROM
            " . $this->table_name . " b
            WHERE
                b.id = ?
            LIMIT
                0,1";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(1,$id);

        //execute query
        $stmt->execute();

        //fetch row
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        if(!$row) return null;

        $building = new Building();
        $building->setId($id);
        $building->setName($row["name"]);

        return $building;
    }

    /**
     * Zwraca tablice z wszystkimi budynkami i ich liczebnoscia
     * @return array tablica z wszystkimi budynkami
     */
    function findAll()
    {
        $query = "CALL getBuildings()";
        $stmt = $this->conn->prepare($query);

        //execute query
        $stmt->execute();

        $building_array = array();
        while($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
            $building = new Building();
            $building->setId($row['id']);
            $building->setName($row['name']);
            $building_array [] = $building;
        }
        return array("count" => $stmt->rowCount(), "buildings" => $building_array);
    }

    /**
     * Usuwa budynek o podanym id
     * @param integer $id id budynku do usuniecia
     * @return bool czy udalo sie usunac budynek
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
     * Dodaje nowy budynek
     * @param Building $building budynek do dodania
     * @return array wiadomosc czy udalo sie dodac budynek i id dodanego budynku
     */
    function addNew($building)
    {
        $query = "CALL addBuilding(:name)";
        $stmt = $this->conn->prepare($query);

        //sanitize data
        $building->setName(htmlspecialchars(strip_tags($building->getName())));

        //bind param
        $name = $building->getName();

        $stmt->bindParam(":name",$name);

        //execute query
        if($stmt->execute())
        {
            $row = $stmt->fetch(PDO::FETCH_ASSOC);
            return [
                "id" => $row['id'],
                "message" => $row['message']
            ];
        }
        return ["id" => null, "message" => null];
    }

    /**
     * Zwraca id ostatniego budynku w tabeli
     * @return integer id ostatniego budynku w tabeli
     */
    public function getLastBuildingID()
    {
        $query = "SELECT MAX(id) AS id FROM buildings";
        $stmt = $this->conn->prepare($query);
        $stmt->execute();
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        return $row['id'];
    }
}