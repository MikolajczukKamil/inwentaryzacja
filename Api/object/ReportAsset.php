<?php

/** Klasa srodka trwalego w raporcie */
class ReportAsset implements JsonSerializable
{
    /** Asset srodek trwaly */
    private $asset;

    /** boolean czy nowy srodek trwaly  */
    private $new_asset;

    /** boolean czy srodek trwaly zostal przeniesiony */
    private $moved;

    /** Room pokoj z ktorego srodek trwaly zostal przeniesiony */
    private $moved_from_room;

    /** integer id pokoju z ktorego srodek trwaly zostal przeniesiony? */
    private $previous_room;

    /** boolean czy srodek trwaly znajduje obecnie sie w tym pokoju  */
    private $present;


    /**
     * Zwraca srodek trwaly
     * @return integer srodek trwaly
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
    public function setNewAsset(bool $new_asset)
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
    public function setMoved(bool $moved)
    {
        $this->moved = $moved;
    }

    /**
     * Zwraca id pokoju z ktorego srodek trwaly zostal przeniesiony
     * @return integer|NULL id pokoju z ktorego srodek trwaly zostal przeniesiony
     */
    public function getPreviousRoom()
    {
        return $this->previous_room;
    }

    /**
     * Ustawia pokoj z ktorego srodek trwaly zostal przeniesiony
     * @param Room $previous_room pokoj z ktorego srodek trwaly zostal przeniesiony
     */
    public function setPreviousRoom(?Room $previous_room)
    {
        $this->previous_room = $previous_room;
    }

    /**
     * Zwraca czy srodek trwaly znajduje obecnie sie w tym pokoju
     * @return boolean czy srodek trwaly znajduje obecnie sie w tym pokoju
     */
    public function getPresent()
    {
        return $this->present;
    }

    /**
     * Ustawia czy srodek trwaly znajduje obecnie sie w tym pokoju
     * @param boolean $present czy srodek trwaly znajduje obecnie sie w tym pokoju
     */
    public function setPresent(bool $present)
    {
        $this->present = $present;
    }

    /**
     * Zwraca pokoj z ktorego srodek trwaly zostal przeniesiony
     * @return Room pokoj z ktorego srodek trwaly zostal przeniesiony
     */
    public function getMovedFromRoom()
    {
        return $this->moved_from_room;
    }

    /**
     * Ustawia pokoj z ktorego srodek trwaly zostal przeniesiony
     * @param Room $moved_from_room pokoj z ktorego srodek trwaly zostal przeniesiony
     */
    public function setMovedFromRoom(Room $moved_from_room): void
    {
        $this->moved_from_room = $moved_from_room;
    }

    /**
     * Zwraca srodek trwaly w raporcie w postaci JSON
     * @return string srodek trwaly w raporcie w postaci JSON
     */
    public function jsonSerialize()
    {
        return [
            'asset' => $this->asset,
            'present' => $this->present,
            'previous_room' => $this->previous_room
        ];
    }
}