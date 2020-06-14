<?php

/** Klasa sesji */
class Session
{
    /** integer id sesji */
    private $id;

    /** integer id uzytkownika */
    private $user_id;

    /** string token sesji */
    private $token;

    /** DateTime data waznosci sesji */
    private $expiration_date;

    /** DateTime data utworzenia sesji */
    private $create_date;

    /** boolean czy sesja sie skonczyla */
    private $expired;

    /**
     * Zwraca id sesji
     * @return integer id sesji
     */
    public function getId()
    {
        return $this->id;
    }

    /**
     * Ustawia id sesji
     * @param integer $id id sesji
     */
    public function setId(int $id)
    {
        $this->id = $id;
    }

    /**
     * Zwraca id uzytkownika
     * @return integer id uzytkownika
     */
    public function getUserId()
    {
        return (int) $this->user_id;
    }

    /**
     * Ustawia id uzytkownika
     * @param integer $user_id id uzytkownika
     */
    public function setUserId(int $user_id)
    {
        $this->user_id = $user_id;
    }

    /**
     * Zwraca token
     * @return string token
     */
    public function getToken()
    {
        return $this->token;
    }

    /**
     * Ustawia token
     * @param string $token token
     */
    public function setToken(string $token)
    {
        $this->token = $token;
    }

    /**
     * Zwraca date waznosci sesji
     * @return datetime data waznosci sesji
     */
    public function getExpirationDate()
    {
        return $this->expiration_date;
    }

    /**
     * Ustawia date waznosci sesji
     * @return datetime data waznosci sesji
     */
    public function setExpirationDate(DateTime $expiration_date)
    {
        $this->expiration_date = $expiration_date;
    }

    /**
     * Zwraca date utworzenia sesji
     * @return datetime data utworzenia sesji
     */
    public function getCreateDate()
    {
        return $this->create_date;
    }

    /**
     * Ustawia date utworzenia sesji
     * @return datetime data utworzenia sesji
     */
    public function setCreateDate(DateTime $create_date)
    {
        $this->create_date = $create_date;
    }

    /**
     * Zwraca, czy sesja sie skonczyla
     * @return boolean czy sesja sie skonczyla
     */
    public function getExpired()
    {
        return $this->expired;
    }

    /**
     * Ustawia czy sesja sie skonczyla
     * @param boolean $expired czy sesja sie skonczyla
     */
    public function setExpired(bool $expired)
    {
        $this->expired = $expired;
    }




}