<?php

/**
 * Klasa bazy danych
 */

class Database
{
    private $host = "localhost.inwentaryzacja.com";
    private $db_name = "inwentaryzacja_db";
    private $username = "root";
    private $password = "";
    public $conn;
/**
 * Funkcja nawiazujaca polaczenie pomiedzy userem a baza danych 
 * @return PDO - obiekt klasy PDO - zwraca polaczenie z baza danych
 */

    public function getConnection()
    {
        $this->conn = null;

        try {
            $this->conn = new PDO("mysql:host=" . $this->host . ";dbname=" . $this->db_name, $this->username, $this->password);
            $this->conn->exec("set names utf8");
        } catch (PDOException $exception){
            echo "Connection error: " . $exception->getMessage();
        }

        return $this->conn;
    }
}