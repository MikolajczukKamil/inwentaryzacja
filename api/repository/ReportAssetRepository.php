<?php
include_once '../object/ReportAsset.php';
include_once '../object/Building.php';
include_once '../object/Room.php';
include_once '../object/AssetType.php';
include_once '../object/Asset.php';
include_once '../object/RoomAsset.php';

class ReportAssetRepository
{
    //database connection and table name
    /**
     * @var PDO
     */
    private $conn;

    public function __construct($db)
    {
        $this->conn = $db;
    }

    public function getAssetsInRoom($room_id)
    {
        $query = "CALL getAssetsInRoom(?)";
        $stmt = $this->conn->prepare($query);

        //sanitize
        $room_id = htmlspecialchars(strip_tags($room_id));

        $stmt->bindParam(1,$room_id);

        //execute query
        $stmt->execute();

        $room_assets = array();
        while($row = $stmt->fetch(PDO::FETCH_ASSOC)) {

            $room_assets [] = self::createRoomAsset($row);
        }
        return $room_assets;
    }

    public function getPositionsInReport($report_id)
    {
        $query = "CALL getPositionsInReport(?)";

        $stmt = $this->conn->prepare($query);

        //sanitize
        $report_id = (int) htmlspecialchars(strip_tags($report_id));
        //bind param
        $stmt->bindParam(1,$report_id);
        //execute query
        $stmt->execute();

        $report_assets = array();
        while($row = $stmt->fetch(PDO::FETCH_ASSOC)) {

            $report_assets [] = self::createReportAsset($row);
        }
        return $report_assets;
    }

    /**
     * @param array $row
     * @return ReportAsset
     */
    private static function createReportAsset(array $row)
    {
        $report_asset = new ReportAsset();

        if($row['previous_id']!=null)
        {
            $previous_building = new Building();
            $previous_building->setId($row['previous_building_id']);
            $previous_building->setName($row['previous_building_name']);

            $previous_room = new Room();
            $previous_room->setName($row['previous_name']);
            $previous_room->setId($row['previous_id']);
            $previous_room->setBuilding($previous_building);

            $report_asset->setPreviousRoom($previous_room);
        }
        $report_asset->setPresent($row['present']);

        $asset_type = new AssetType();
        $asset_type->setId($row['type_id']);
        $asset_type->setName($row['type_name']);
        $asset_type->setLetter($row['type_letter']);

        $asset = new Asset();
        $asset->setId($row['asset_id']);
        $asset->setAssetType($asset_type);

        $report_asset->setAsset($asset);
        return $report_asset;
    }

    /**
     * @param array $row
     * @return Asset
     */
    private static function createRoomAsset(array $row)
    {
        $asset_type = new AssetType();
        $asset_type->setId($row['type']);
        $asset_type->setName($row['asset_type_name']);
        $asset_type->setLetter($row['asset_type_letter']);

        $asset = new Asset();
        $asset->setId($row['id']);
        $asset->setAssetType($asset_type);

        return $asset;
    }

}