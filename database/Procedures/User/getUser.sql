DROP PROCEDURE IF EXISTS getUser;

DELIMITER $
CREATE PROCEDURE getUser(IN User_id INT)
BEGIN
    SELECT NULL as message,
           users.id,
           users.login,
           users.hash
    FROM users
    WHERE users.id = User_id;

END $ DELIMITER ;
