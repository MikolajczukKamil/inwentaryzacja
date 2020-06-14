<?php

/** Klasa typ srodka trwalego */
class AssetType implements JsonSerializable
{
    /** integer id typu */
    private $id;

    /** string litera przypisana typowi */
    private $letter;

    /** string nazwa typu */
    private $name;


    /**
     * Zwraca id typu
     * @return integer id typu
     */
    public function getId()
    {
        return (int) $this->id;
    }

    /**
     * Ustawia id typu
     * @param integer $id id typu
     */
    public function setId(int $id)
    {
        $this->id = (int) $id;
    }

    /**
     * Zwraca litere typu
     * @return string litera typu
     */
    public function getLetter()
    {
        return $this->letter;
    }

    /**
     * Ustawia litere typu
     * @param string $letter litera typu
     */
    public function setLetter(string $letter)
    {
        $this->letter = $letter;
    }

    /**
     * Zwraca nazwe typu
     * @return string nazwa typu
     */
    public function getName()
    {
        return $this->name;
    }

    /**
     * Ustawia nazwe typu
     * @param string $name nazwa typu
     */
    public function setName(string $name)
    {
        $this->name = $name;
    }

    /**
     * Zwraca typ w postaci JSON
     * @return string typ w postaci JSON
     */
    public function jsonSerialize()
    {
        return [
            "id" => (int) $this->id,
            "letter" => $this->letter,
            "name" => $this->name
        ];
    }
}