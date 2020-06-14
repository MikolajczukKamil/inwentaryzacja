<?php
include_once '../config/Database.php';
include_once '../security/BearerToken.php';
include_once '../object/User.php';

/** Klasa do obslugi tabeli uzytkownikow */
class UserRepository
{
    /** PDO wartosc polaczenia z baza */
    private $conn;

    /**
     * konstrukor
     * @param PDO $db polaczenie z baza
     */
    public function __construct($db)
    {
        $this->conn = $db;
    }

    /**
     * Znajuje i zwraca uzytkownika o podanym id
     * @param integer $id id szukanego uzytkownika
     * @return User|null znaleziony uzytkownik
     */
    public function find($id)
    {
        $query = "CALL getUser(?)";
        $stmt = $this->conn->prepare($query);

        $stmt->bindParam(1, $id);

        $stmt->execute();

        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        if(!$row) return null;

        $user = new User();
        $user->setId($row['id']);
        $user->setLogin($row['login']);
        $user->setHash($row['hash']);

        return $user;
    }

    /**
     * Znajuje i zwraca uzytkownika o podanym loginie
     * @param string $login login szukanego uzytkownika
     * @return User|null znaleziony uzytkownik
     */
    public function findOneByLogin($login)
    {
        $query = "CALL getUserByLogin(?)";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(1,$login);

        //execute query
        $stmt->execute();

        //fetch row
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        if(!$row) return null;

        $user = new User();
        $user->setId($row["id"]);
        $user->setLogin($row["login"]);
        $user->setHash($row["hash"]);

        return $user;
    }

    /**
     * Funkcja wyszukuje i zwraca aktualnie zalogowanego uzytkownika
     * @return User|null uzytkownik jezeli jakis jest zalogowany
     */
    public function findCurrentUser()
    {
        $query = "CALL getLoginSession(?)";
        $stmt = $this->conn->prepare($query);

        $token = BearerToken::getBearerToken();
        $stmt->bindParam(1,$token);

        $stmt->execute();
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        $stmt->closeCursor();

        $user_id = $row['user_id'];
        return $this->find((int)$user_id);
    }
}