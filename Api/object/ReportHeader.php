<?php
include_once '../object/Room.php';
include_once '../object/User.php';

/** Klasa metadanych raportu */
class ReportHeader implements JsonSerializable
{
    /** integer id raportu  */
    private $id;

    /** string nazwa raportu */
    private $name;

    /** DateTime utworzenia raportu  */
    private $create_date;

    /** User wlasciciel raportu */
    private $owner;

    /** Room pokoj dla ktorego zostal utworzony raport */
    private $room;


    /**
     * Zwraca id raportu
     * @return integer id raportu
     */
    public function getId()
    {
        return (int) $this->id;
    }

    /**
     * Ustawia id raportu
     * @param integer $id id raportu
     */
    public function setId(int $id)
    {
        $this->id = $id;
    }

    /**
     * Zwraca nazwe raportu
     * @return string nazwa raportu
     */
    public function getName()
    {
        return $this->name;
    }

    /**
     * Ustawia nazwe raportu
     * @param string $name nazwa raportu
     */
    public function setName(string $name)
    {
        $this->name = $name;
    }

    /**
     * Zwraca date utworzenia raportu
     * @return DateTime data utworzenia raportu
     */
    public function getCreateDate()
    {
        return $this->create_date;
    }

    /**
     * Ustawia date utworzenia raportu
     * @param DateTime $create_date data utworzenia raportu
     */
    public function setCreateDate(DateTime $create_date)
    {
        $this->create_date = $create_date;
    }

    /**
     * Zwraca wlasciciela raportu
     * @return User wlasciciel raportu
     */
    public function getOwner()
    {
        return $this->owner;
    }

    /**
     * Ustawia wlasciciela raportu
     * @param User $owner wlasciciel raportu
     */
    public function setOwner(User $owner): void
    {
        $this->owner = $owner;
    }

    /**
     * Zwraca pokoj raportu
     * @return Room pokoj raportu
     */
    public function getRoom()
    {
        return $this->room;
    }

    /**
     * Ustawia pokoj raportu
     * @param Room $room pokoj raportu
     */
    public function setRoom(Room $room): void
    {
        $this->room = $room;
    }

    /**
     * Zwraca metadane raportu w postaci JSON
     * @return string metadane raportu w postaci JSON
     */
    public function jsonSerialize()
    {
        return [
            "id" => (int)$this->id,
            "name" => $this->name,
            "create_date" => $this->create_date->format('Y-m-d H:i:s'),
            "owner" => $this->owner->jsonSerializeNoHash(),
            "room" => $this->room
        ];
    }
}