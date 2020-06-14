<?php

/**
 * Klasa odpowiadajaca za obsluge skanowania
 */
class Scan implements JsonSerializable
{
    private $id;
    private $room;
    private $owner;
    private $create_date;

    /**
     * Funkcja zwraca id skanu
     * @return integer id skanu
     */
    public function getId()
    {
        return $this->id;
    }

    /**
     * Funkcja ustawia id skanu
     * @param integer $id id skanu
     */
    public function setId($id): void
    {
        $this->id = $id;
    }

    /**
     * Funkcja zwraca skanowany pokoj
     * @return Room skanowany pokoj
     */
    public function getRoom()
    {
        return $this->room;
    }

    /**
     * Funkcja ustawia skanowany pokoj
     * @param Room $room skanowany pokoj
     */
    public function setRoom($room): void
    {
        $this->room = $room;
    }

    /**
     * Funkcja zwraca wlasciciela skanu (uzytkownika)
     * @return User wlasciciel skanu
     */
    public function getOwner()
    {
        return $this->owner;
    }

    /**
     * Funkcja ustawia wlasciciela skanu (uzytkownika)
     * @param User $owner wlasciciel skanu
     */
    public function setOwner($owner): void
    {
        $this->owner = $owner;
    }

    /**
     * Funkcja zwraca date skanu
     * @return DateTime data skanu
     */
    public function getCreateDate()
    {
        return $this->create_date;
    }

    /**
     * Funkcja tworzy date skanu
     * @param DateTime $create_date data skanu
     */
    public function setCreateDate($create_date): void
    {
        $this->create_date = $create_date;
    }

    /**
     * Funkcja zwraca skan w postaci JSON
     * @return string skan w postaci JSON
     */
    public function jsonSerialize()
    {
        return [
            'id' => $this->id,
            'room' => $this->room,
            'owner' => $this->owner,
            'create_date' => $this->create_date
        ];
    }
}