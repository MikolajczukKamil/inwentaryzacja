<?php

/** Klasa uzytkownika */
class User implements JsonSerializable
{
    /** integer id uzytkownika */
    private $id;

    /** string login uzytkownika */
    private $login;

    /** string zahashowane haslo uzytkownika */
    private $hash;

    /**
     * Zwraca id uzytkownika
     * @return integer id uzytkownika
     */
    public function getId()
    {
        return $this->id;
    }

    /**
     * Ustawia id uzytkownika
     * @param integer $id id uzytkownika
     */
    public function setId(int $id)
    {
        $this->id = $id;
    }

    /**
     * Zwraca login uzytkownika
     * @return string login uzytkownika
     */
    public function getLogin()
    {
        return $this->login;
    }

    /**
     * Ustawia login uzytkownika
     * @param string $login login uzytkownika
     */
    public function setLogin(string $login)
    {
        $this->login = $login;
    }

    /**
     * Zwraca zahashowane haslo uzytkownika
     * @return string zahashowane haslo uzytkownika
     */
    public function getHash()
    {
        return $this->hash;
    }

    /**
     * Ustawia zahashowane haslo uzytkownika
     * @param string $hash zahashowane haslo uzytkownika
     */
    public function setHash(string $hash)
    {
        $this->hash = $hash;
    }

    /**
     * Zwraca uzytkownika w postaci JSON
     * @return string uzytkownik w postaci JSON
     */
    public function jsonSerialize()
    {
        return [
            "id" => (int) $this->id,
            "login" => $this->login,
            "hash" => $this->hash
        ];
    }

    public function jsonSerializeNoHash()
    {
        return [
            "id" => (int) $this->id,
            "login" => $this->login
        ];
    }
}