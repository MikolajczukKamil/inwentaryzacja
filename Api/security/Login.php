<?php
include_once '../repository/UserRepository.php';
include_once 'Token.php';
include_once '../repository/SessionRepository.php';
include_once '../config/Database.php';
include_once '../object/Session.php';

/**
 * Klasa tworzaca sesje dla uzytkownika
 */
class Login
{
    /**
     * Funkcja nawiazujaca polaczenie z baza danych, logujaca uzytkownika
     * @param object $data dane logowania uzytkownika
     */
   static function userLogin($data)
   {
       // get database connection
       $database = new Database();
       $db = $database->getConnection();

       //check if complete data was passed
       if(!property_exists($data, 'login') || !property_exists($data, 'password'))
       {
           http_response_code(400);
           echo json_encode(array("message" => "Incomplete data. Request could not be processed"));
       }
       else {
           //sanitize input data
           $login = htmlspecialchars(strip_tags($data->login));
           $password = htmlspecialchars(strip_tags($data->password));

           $ur = new UserRepository($db);
           $user = $ur->findOneByLogin($login);

           if($user!=null) {
               if(password_verify($password,$user->getHash()))
               {
                   $sr = new SessionRepository($db);

                   $session = new Session();
                   $start_date = new DateTime('now');
                   $end_date = $start_date->add(DateInterval::createFromDateString('1 week'));
                   $session->setExpirationDate($end_date);
                   $session->setToken(Token::getToken(20));
                   $session->setUserId($user->getId());

                   try {
                       if ($sr->addNew($session)) {
                           header("Authorization: Bearer " . $session->getToken());
                           http_response_code(200);
                           echo json_encode(array("message" => "Zalogowano"));
                       } else {
                           http_response_code(500);
                           echo json_encode(["message" => "Usługa serwera przekazała niekompletne dane. Nie można utworzyć sesji."]);
                       }
                   } catch (Exception $e) {
                       echo "Podczas tworzenia sesji zgłoszono wyjątek: " . $e->getMessage();
                   }
               }
               else
               {
                   http_response_code(401);
                   echo json_encode(array("message" => "Nieprawidłowy login i/lub hasło."));
               }
           }
           else {
               http_response_code(401);
               echo json_encode(array("message" => "Nieprawidłowy login i/lub hasło."));
           }
       }
   }
}