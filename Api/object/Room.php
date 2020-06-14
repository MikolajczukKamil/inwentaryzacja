<?php

/** Klasa pokoju */
class Room implements JsonSerializable
{
    /** integer id pokoju  */
    private $id;

    /** string nazwa pokoju */
    private $name;

    /** Building budynek, w ktorym znajduje sie pokoj */
    private $building;

    /**
     * Zwraca id pokoju
     * @return integer id pokoju
     */
    public function getId()
    {
        return $this->id;
    }

    /**
     * Ustawia id pokoju
     * @param integer $id id pokoju
     */
    public function setId(?int $id)
    {
        $this->id = $id;
    }

    /**
     * Zwraca nazwe pokoju
     * @return string nazwa pokoju
     */
    public function getName()
    {
        return $this->name;
    }

    /**
     * Ustawia nazwe pokoju
     * @param string $name nazwa pokoju
     */
    public function setName(?string $name)
    {
        $this->name = $name;
    }

    /**
     * Zwraca budynek, w ktorym znajduje sie pokoj
     * @return Building budynek, w ktorym znajduje sie pokoj
     */
    public function getBuilding()
    {
        return $this->building;
    }

    /**
     * Ustawia budynek, w ktorym znajduje sie pokoj
     * @param Building $building budynek, w ktorym znajduje sie pokoj
     */
    public function setBuilding(Building $building)
    {
        $this->building = $building;
    }


    /**
     * Zwraca pokoj w postaci JSON
     * @return string pokoj w postaci JSON
     */
    public function jsonSerialize()
    {
        return [
            "id" => (int) $this->id,
            "name" => $this->name,
            "building" => $this->building
        ];
    }
}