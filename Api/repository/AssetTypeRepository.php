<?php
include_once '../interfaces/IRepository.php';
include_once '../object/AssetType.php';
include_once '../config/Database.php';

/**
 * Klasa do obslugi tabeli typow srodkow trwalych
 */
class AssetTypeRepository implements IRepository
{
    /** PDO wartosc polaczenia z baza */
    private $conn;

    /** string nazwa tabeli */
    private $table_name = "asset_types";


    /**
     * konstrukor
     * @param PDO $db polaczenie z baza
     */
    public function __construct($db)
    {
        $this->conn = $db;
    }

    /**
     * Znajuje i zwraca typ o podanym id
     * @param integer $id id typu
     * @return AssetType typu
     */
    function find($id)
    {
        $query = "SELECT 
                a_t.id, a_t.letter, a_t.name 
          FROM
            " . $this->table_name . " a_t
            WHERE
                a_t.id = ?
            LIMIT
                0,1";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(1,$id);

        //execute query
        $stmt->execute();

        //fetch row
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        if(!$row) return null;

        $asset_type = new AssetType();
        $asset_type->setId($id);
        $asset_type->setName($row["name"]);
        $asset_type->setLetter($row["letter"]);
        return $asset_type;
    }

    /**
     * Zwraca tablice z wszystkimi typami i ich liczebnoscia
     * @return array tablica z wszystkimi typami
     */
    function findAll()
    {
        $query = "SELECT
                a_t.id, a_t.letter, a_t.name
            FROM
                " . $this->table_name . " a_t
                ORDER BY a_t.id";
        $stmt = $this->conn->prepare($query);

        //execute query
        $stmt->execute();
        $asset_type_array = array();
        while($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
            $asset_type = new AssetType();
            $asset_type->setId($row["id"]);
            $asset_type->setName($row["name"]);
            $asset_type->setLetter($row["letter"]);
            $asset_type_array [] = $asset_type;
        }
        return array("count" => $stmt->rowCount(), "asset_types" => $asset_type_array);
    }

    /**
     * Usuwa typ o podanym id
     * @param integer $id id typu
     * @return bool czy udalo sie usunac typ
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
     * Dodaje nowy typ do tabeli
     * @param AssetType $asset_type typ do dodania
     * @return bool czy udalo sie dodac typ
     */
    function addNew($asset_type)
    {
        $query = "INSERT
                INTO " . $this->table_name . "
                SET
                    letter=:letter, name=:name";
        $stmt = $this->conn->prepare($query);

        //sanitize data
        $asset_type->setName(htmlspecialchars(strip_tags($asset_type->getName())));
        $asset_type->setLetter(htmlspecialchars(strip_tags($asset_type->getLetter())));

        //bind param
        $name = $asset_type->getName();
        $letter = $asset_type->getLetter();

        $stmt->bindParam(":name",$name);
        $stmt->bindParam(":letter",$letter);

        //execute query
        if($stmt->execute())
        {
            return true;
        }
        return false;
    }
}