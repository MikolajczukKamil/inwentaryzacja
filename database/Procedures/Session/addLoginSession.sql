DROP PROCEDURE IF EXISTS addLoginSession;

DELIMITER $
CREATE PROCEDURE addLoginSession(IN User_id INT, IN Date_expiration DATETIME, IN User_token VARCHAR(64))
BEGIN
    DECLARE is_type_exits BOOLEAN;

    SELECT (SELECT COUNT(*) FROM users WHERE users.id = User_id) = 1
    INTO is_type_exits;

    IF NOT is_type_exits THEN
        SELECT NULL                                        AS id,
               idsNotFound('User', User_id, is_type_exits) AS message;
    ELSE
        INSERT INTO login_sessions (user, token, expiration_date, create_date)
        VALUES (User_id, User_token, Date_expiration, NOW());

        SELECT LAST_INSERT_ID() AS id, NULL AS message;
    END IF;
END $ DELIMITER ;
