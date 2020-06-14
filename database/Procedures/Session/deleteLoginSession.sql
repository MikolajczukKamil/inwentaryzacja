DROP PROCEDURE IF EXISTS deleteLoginSession;

DELIMITER $
CREATE PROCEDURE deleteLoginSession(IN User_token VARCHAR(64))
BEGIN
  DELETE
  FROM login_sessions
  WHERE login_sessions.token = User_token;

END $ DELIMITER ;
