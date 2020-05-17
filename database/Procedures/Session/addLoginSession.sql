DROP PROCEDURE IF EXISTS addLoginSession;

DELIMITER $
CREATE PROCEDURE addLoginSession(IN user_id INT, IN date_expiration DATETIME, IN user_token VARCHAR(64))
BEGIN
  DECLARE is_type_correct BOOLEAN;

  SELECT (SELECT COUNT(*) FROM users WHERE users.id = user_id) = 1
  INTO is_type_correct;

  IF NOT is_type_correct THEN
    SELECT
      NULL AS id,
      idsNotFound("User", user_id, is_type_correct) AS message
    ;
  ELSE
    INSERT INTO login_sessions (user, token, expiration_date, create_date)
    VALUES (user_id, user_token, date_expiration, NOW());

    SELECT LAST_INSERT_ID() AS id, NULL AS message;
  END IF;
END $ DELIMITER ;
