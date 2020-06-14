<?php

/** Klasa srodka trwalego w pokoju */
class RoomAsset implements JsonSerializable
{
    /** Asset srodek trwaly */
    private $asset;

    /** boolean czy nowy srodek trwaly  */
    private $new_asset;

    /** boolean czy srodek trwaly zostal przeniesiony */
    private $moved;

    /** Room pokoj z ktorego srodek trwaly zostal przeniesiony */
    private $moved_from_room;

    /**
     * Zwraca srodek trwaly
     * @return Asset srodek trwaly
     */
    public function getAsset()
    {
        return $this->asset;
    }

    /**
     * Ustawia srodek trwaly
     * @param Asset $asset srodek trwaly
     */
    public function setAsset(Asset $asset): void
    {
        $this->asset = $asset;
    }

    /**
     * Zwraca czy srodek trwaly jest nowy
     * @return boolean czy nowy srodek trwaly
     */
    public function getNewAsset()
    {
        return $this->new_asset;
    }

    /**
     * Ustawia czy srodek trwaly jest nowy
     * @param boolean $new_asset czy nowy srodek trwaly
     */
    public function setNewAsset(bool $new_asset): void
    {
        $this->new_asset = $new_asset;
    }

    /**
     * Zwraca czy srodek trwaly zostal przeniesiony
     * @return boolean czy srodek trwaly zostal przeniesiony
     */
    public function getMoved()
    {
        return $this->moved;
    }

    /**
     * Ustawia czy srodek trwaly zostal przeniesiony
     * @param boolean $moved czy srodek trwaly zostal przeniesiony
     */
    public function setMoved(bool $moved): void
    {
        $this->moved = $moved;
    }

    /**
     * Zwraca pokoj z ktorego srodek trwaly zostal przeniesiony
     * @return Room|NULL pokoj z ktorego srodek trwaly zostal przeniesiony
     */
    public function getMovedFromRoom()
    {
        return $this->moved_from_room;
    }

    /**
     * Ustawia pokoj z ktorego srodek trwaly zostal przeniesiony
     * @param Room|NULL $moved_from_room pokoj z ktorego srodek trwaly zostal przeniesiony
     */
    public function setMovedFromRoom(?Room $moved_from_room): void
    {
        $this->moved_from_room = $moved_from_room;
    }

    /**
     * Zwraca srodek trwaly w pokoju w postaci JSON
     * @return string srodek trwaly w pokoju w postaci JSON
     */
    public function jsonSerialize()
    {
        $json = [];
        $json ['asset'] = $this->asset;
        $json ['new_asset'] = $this->new_asset;
        $json ['moved'] = $this->moved;
        $json ['moved_from_room'] = $this->moved_from_room;

        return $json;
    }
}