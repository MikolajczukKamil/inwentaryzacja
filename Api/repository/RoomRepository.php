<?php
include_once '../object/Room.php';

/** Klasa do obslugi tabeli pokoi */
class RoomRepository
{
    /** PDO wartosc polaczenia z baza */
    private $conn;


    /**
     * konstrukor
     * @param PDO $db polaczenie z baza
     */
    public function __construct($db)
    {
        $this->conn = $db;
    }

    /**
     * Dodaje nowy pokoj
     * @param Room $room pokoj do dodania
     * @return array czy udalo sie dodac pokoj
     */
    function addNew($room)
    {
        $query = "CALL addRoom(:name,:building)";
        $stmt = $this->conn->prepare($query);

        //bind param
        $name = $room->getName();
        $building = $room->getBuilding()->getId();

        $stmt->bindParam(":name",$name);
        $stmt->bindParam(":building",$building);

        //execute query
        if($stmt->execute())
        {
            $row = $stmt->fetch(PDO::FETCH_ASSOC);
            return [
                'message' => $row['message'],
                'id' => $row['id']
            ];
        }
        return [
            'message' => null,
            'id' => null
        ];
    }
}