DROP PROCEDURE IF EXISTS getLoginSession;

DELIMITER $ CREATE PROCEDURE getLoginSession(IN user_token VARCHAR(64)) BEGIN
  SELECT
    login_sessions.id,
    login_sessions.user AS user_id,
    login_sessions.expiration_date,
    login_sessions.token,
    login_sessions.expiration_date <= NOW() AS expired
  FROM login_sessions
  WHERE login_sessions.token = user_token;

END $ DELIMITER ;
