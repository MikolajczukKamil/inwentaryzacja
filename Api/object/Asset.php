<?php
include_once 'Building.php';
include_once 'AssetType.php';
include_once 'Room.php';

/** Klasa srodka trwalego */
class Asset implements JsonSerializable
{
    /** int id srodka trwalego */
    private $id;

    /** AssetType typ srodka trwalego */
    private $assetType;

    /** Room pokoj do ktorego przypisany jest srodek trwaly */
    private $room;


    /**
     * Pobiera id srodka trwalego
     * @return integer id srodka trwalego
     */
    public function getId()
    {
        return (int) $this->id;
    }

    /**
     * Ustawia id srodka trwalego
     * @param integer $id id srodka trwalego
     */
    public function setId(int $id)
    {
        $this->id = (int) $id;
    }

    /**
     * Pobiera typ srodka trwalego
     * @return AssetType typ srodka trwalego
     */
    public function getAssetType()
    {
        return $this->assetType;
    }

    /**
     * Ustawia typ srodka trwalego
     * @param AssetType $assetType typ srodka trwalego
     */
    public function setAssetType(AssetType $assetType)
    {
        $this->assetType = $assetType;
    }

    /**
     * Pobiera pokoj do ktorego przypisany jest srodkek trwaly
     * @return Room pokoj do ktorego przypisany jest srodkek trwaly
     */
    public function getRoom()
    {
        return $this->room;
    }

    /**
     * Przypisuje pokoj do srodka trwalego
     * @param Room $room pokoj, do ktorego ma byc przypisany srodkek trwaly
     */
    public function setRoom(Room $room)
    {
        $this->room = $room;
    }

    /**
     * Zwraca srodkek trwaly w postaci JSON
     * @return array srodkek trwaly w postaci JSON
     */
    public function jsonSerialize()
    {
        $json = array();
        $json['id'] = (int) $this->id;
        $json['type'] = $this->assetType;
        if($this->room != null)
            $json['room'] = $this->room;
        else $json['room'] = null;
        return $json;
    }
}