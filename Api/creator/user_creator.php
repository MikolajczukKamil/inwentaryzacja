<?php
include_once '../config/Database.php';
include_once '../object/User.php';
include_once '../repository/UserRepository.php';

$user_login = "test";
$user_password = "password";

$user = new User();
$user->setLogin($user_login);
$user->setHash(password_hash($user_password, PASSWORD_BCRYPT));

// get database connection
$database = new Database();
$db = $database->getConnection();

$conn = $db;

$query = "INSERT
                INTO users
                SET
                    login=:login, hash=:hash";
$stmt = $conn->prepare($query);

//bind params
$login = $user->getLogin();
$hash = $user->getHash();

$stmt->bindParam(":login",$login);
$stmt->bindParam(":hash",$hash);

//execute query
if($stmt->execute())
{
    echo "User " . $user_login . " created successfully.";
    return true;
}
return false;