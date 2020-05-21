DROP PROCEDURE IF EXISTS getUser;

DELIMITER $
CREATE PROCEDURE getUser(IN User_id INT)
BEGIN
    DECLARE is_user_exits BOOLEAN DEFAULT userExists(User_id);

    IF NOT is_user_exits THEN
        SELECT idsNotFound('User', User_id, is_user_exits) AS message,
               NULL                                        AS id,
               NULL                                        AS login,
               NULL                                        AS hash;
    ELSE

        SELECT NULL as message,
               users.id,
               users.login,
               users.hash
        FROM users
        WHERE users.id = User_id;


    END IF;

END $ DELIMITER ;
