DROP PROCEDURE IF EXISTS getUserByLogin;

DELIMITER $ CREATE PROCEDURE getUserByLogin(IN user_login VARCHAR(64))
BEGIN
  SELECT
    users.id,
    users.login,
    users.hash
  FROM users
  WHERE users.login = user_login;

END $ DELIMITER ;
