DROP PROCEDURE IF EXISTS deleteLoginSession;

DELIMITER $
CREATE PROCEDURE deleteLoginSession(IN user_token VARCHAR(64))
BEGIN
    DELETE
    FROM login_sessions
    WHERE login_sessions.token = user_token;

END $ DELIMITER ;
