<?php
include_once '../config/Database.php';
include_once '../object/Session.php';

/** Klasa do obslugi tabeli sesji */
class SessionRepository
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
     * Usun sesje o podanym tokenie
     * @param string $token token
     * @return bool czy udalo sie usunac sesje
     */
    public function deleteOneByToken($token)
    {
        $query = "CALL deleteLoginSession(?)";

        //prepare_query
        $stmt = $this->conn->prepare($query);

        //sanitize data
        $token = htmlspecialchars(strip_tags($token));

        //bind parameter
        $stmt->bindParam(1,$token);

        if($stmt->execute() && $stmt->rowCount()>0)
        {
            return true;
        }
        return false;
    }

    /**
     * Dodaj nowa sesje
     * @param Session $session sesja do dodania
     * @return bool czy udalo sie dodac sesje
     */
    public function addNew($session)
    {
        $query = "CALL addLoginSession(:u_id,:exp_date,:token)";
        $stmt = $this->conn->prepare($query);

        //sanitize data
        $session->setUserId(htmlspecialchars(strip_tags($session->getUserId())));
        $session->setToken(htmlspecialchars(strip_tags($session->getToken())));

        //prepare params
        $user_id = $session->getUserId();
        $token = $session->getToken();
        $exp_date = $session->getExpirationDate()->format('Y-m-d H:i:s');

        //bind params
        $stmt->bindParam(":u_id",$user_id);
        $stmt->bindParam(":token",$token);
        $stmt->bindParam(":exp_date",$exp_date);

        //execute query
        if($stmt->execute())
        {
            return true;
        }
        return false;
    }

    /**
     * Znajdz i zwroc sesje o podanym tokenie
     * @param string $token token
     * @return Session|null znaleziona sesja
     */
    public function findOneByToken($token)
    {
        $query = "CALL getLoginSession(?)";
        $stmt = $this->conn->prepare($query);
        $stmt->bindParam(1,$token);

        //execute query
        $stmt->execute();

        //fetch row
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        if(!$row) return null;

        $session = new Session();
        $session->setId($row["id"]);
        $session->setUserId($row["user_id"]);
        try {
            $session->setExpirationDate(new DateTime($row["expiration_date"]));
        } catch (Exception $e) {
            echo 'Error while setting expirationDate for Session: ' . $e->getMessage();
        }
        $session->setToken($row["token"]);
        $session->setExpired($row['expired']);

        return $session;
    }
}