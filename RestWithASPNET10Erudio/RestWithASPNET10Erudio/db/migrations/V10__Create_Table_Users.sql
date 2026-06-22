CREATE TABLE users (
  id BIGINT NOT NULL AUTO_INCREMENT,
  user_name VARCHAR(50) NOT NULL,
  password VARCHAR(130) NOT NULL,
  full_name VARCHAR(120) NOT NULL,
  refresh_token VARCHAR(500) NULL,
  refresh_token_expiration_time DATETIME NULL,
  CONSTRAINT UQ_users_user_name UNIQUE (user_name),
  PRIMARY KEY (id)
);