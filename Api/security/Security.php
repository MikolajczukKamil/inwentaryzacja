<?php
include_once '../repository/SessionRepository.php';
include_once '../object/Session.php';
include_once 'BearerToken.php';

/**
 * Klasa majaca za zadanie autoryzacje uzytkownika podczas logowania
 */
class Security
{

    /**
     * Funkcja autoryzujaca uzytkownika podczas logowania
     * @param string $token token reprezentujacy zalogowanego uzytkownika w sesji
     * @return bool w zaleznosci od tego czy sie powiodlo
     */
    private static function authorizeUser($token)

    {

        // get database connection
        $database = new Database();
        $db = $database->getConnection();

        $sr = new SessionRepository($db);
        $session = $sr->findOneByToken($token);

        if($session!=null)
        {
            try {
                return (self::validateTokenExpiry($session));
            } catch (Exception $e) {
                echo "Podczas walidacji ważności tokena zgłoszony został wyjątek: " . $e->getMessage();
            }
        }
        return false;
    }

    /**
     * 
     * Funkcja sprawdza czy Token przekazany przez uzytkownika nadal jest wazny
     * @param Session $session - obiekt typu session
     * @return bool w zaleznosci od tego czy jest nadal wazny, czy wygasl
     */
    private static function validateTokenExpiry($session)
    {
        return ($session->getExpirationDate()>new DateTime('now'));
    }
    /**
     * Funkcja przeprowadzajaca autoryzacje uzytkownika
     * @return bool w zaleznosci od tego czy autoryzacja powiodla sie
     */

    public static function performAuthorization()
    {
        if(!empty(BearerToken::getBearerToken())) {
            if(Security::authorizeUser(BearerToken::getBearerToken())) {
                return true;
            }
            else {
                http_response_code(503);
                echo json_encode(array("message" => "Autoryzacja nieudana. Niepoprawny, lub nieważny token.", "auth" => false));
                return false;
            }
        }
        else {
            http_response_code(400);
            echo json_encode(array("message" => "Autoryzacja nieudana. Brak tokena.", "auth" => false));
            return false;
        }
    }
}