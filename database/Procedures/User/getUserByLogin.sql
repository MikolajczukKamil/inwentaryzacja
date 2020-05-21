DROP PROCEDURE IF EXISTS getUserByLogin;

DELIMITER $
CREATE PROCEDURE getUserByLogin(IN User_login VARCHAR(64))
BEGIN
    SELECT users.id,
           users.login,
           users.hash
    FROM users
    WHERE users.login = User_login;

END $ DELIMITER ;
