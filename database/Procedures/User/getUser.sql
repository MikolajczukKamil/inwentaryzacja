DROP PROCEDURE IF EXISTS getUser;

DELIMITER $ CREATE PROCEDURE getUser(IN user_id INT)
BEGIN
  SELECT
    users.id,
    users.login,
    users.hash
  FROM users
  WHERE users.id = user_id;

END $ DELIMITER ;
