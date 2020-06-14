<?php

/** Klasa budynek */
class Building implements JsonSerializable
{
    /** integer id budynku */
    private $id;

    /** string nazwa budynku */
    private $name;


    /**
     * Zwraca id budynku
     * @return integer id budynku
     */
    public function getId()
    {
        return $this->id;
    }

    /**
     * Ustawia id budynku
     * @param integer $id
     */
    public function setId(?int $id)
    {
        $this->id = (int) $id;
    }

    /**
     * Zwraca nazwe budynku
     * @return string nazwa budynku
     */
    public function getName()
    {
        return $this->name;
    }

    /**
     * Ustawia nazwe budynku
     * @param string $name nazwa budynku
     */
    public function setName(?string $name)
    {
        $this->name = $name;
    }


    /**
     * Zwraca budynek w postaci JSON
     * @return string budynek w postaci JSON
     */
    public function jsonSerialize()
    {
        return [
            "id" => $this->id,
            "name" => $this->name
        ];
    }
}