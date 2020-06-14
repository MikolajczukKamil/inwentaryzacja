<?php
include_once '../config/Database.php';
include_once '../object/Asset.php';
include_once '../object/Room.php';
include_once '../object/AssetType.php';
include_once '../object/Building.php';

/** Klasa do obslugi tabeli srodkow trwalych */
class AssetRepository
{
    /** PDO wartosc polaczenia z baza */
    private $conn;

    /** string nazwa tabeli */
    private $table_name = "assets";


    /**
     * konstrukor
     * @param PDO $db polaczenie z baza
     */
    public function __construct($db)
    {
        $this->conn = $db;
    }

    /**
     * Znajuje i zwraca srodek trwaly o podanym id
     * @param integer $id id srodka trwalego
     * @return Asset|string znaleziony srodek trwaly lub błąd zwrócony przez bazę danych
     */
    function find($id)
    {
        $query = "CALL getAssetInfo(?)";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(1,$id);

        //execute query
        $stmt->execute();

        //fetch row
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        if($row['message']!=null)
        {
            return $row['message'];
        }

        return self::createAssetInfo($row);
    }

    /**
     * Usuwa srodek trwaly o podanym id
     * @param integer $id id srodka trwalego
     * @return bool czy udalo sie usunac srodek trwaly
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
     * Dodaje nowy srodek trwaly do tabeli
     * @param Asset $asset srodek trwaly do dodania
     * @return array czy udalo sie dodac srodek trwaly i jakie jest jego id
     */
    function addNew($asset)
    {
        $query = "CALL addNewAsset(:type_id)";
        $stmt = $this->conn->prepare($query);

        //bind params
        $type = htmlspecialchars(strip_tags($asset->getAssetType()->getId()));

        $stmt->bindParam(":type_id",$type);

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

    /**
     * Tworzy i zwraca srodek trwaly na podstawie przekazanego wyniku kwerendy
     * @param array $row wynik kwerendy fetch
     * @return Asset utworzony srodek trwaly
     */
    private static function createAssetInfo($row)
    {
        $asset = new Asset();
        $asset_type = new AssetType();


        $asset->setId($row['id']);

        $asset_type->setId($row['type']);
        $asset_type->setName($row['asset_type_name']);
        $asset_type->setLetter($row['letter']);

        if($row['room_id'])
        {
            $building = new Building();
            $room = new Room();
            $building->setName($row['building_name']);
            $building->setId($row['building_id']);

            $room->setId($row['room_id']);
            $room->setName($row['room_name']);
            $room->setBuilding($building);

            $asset->setRoom($room);
        }

        $asset->setAssetType($asset_type);
        return $asset;
    }

    /**
     * Zwraca id ostatniego srodka trwalego w tabeli
     * @return integer id ostatniego srodka trwalego w tabeli
     */
    public function getLastAssetID()
    {
        $query = "SELECT MAX(id) AS id FROM assets";
        $stmt = $this->conn->prepare($query);
        $stmt->execute();
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        return $row['id'];
    }
}