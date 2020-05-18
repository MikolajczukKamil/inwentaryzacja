DROP TABLE IF EXISTS scans_positions;
DROP TABLE IF EXISTS scans;
DROP TABLE IF EXISTS reports_positions;
DROP TABLE IF EXISTS reports;
DROP TABLE IF EXISTS assets;
DROP TABLE IF EXISTS rooms;
DROP TABLE IF EXISTS buildings;
DROP TABLE IF EXISTS asset_types;
DROP TABLE IF EXISTS login_sessions;
DROP TABLE IF EXISTS users;

CREATE TABLE users
(
    id    INT         NOT NULL AUTO_INCREMENT,
    login VARCHAR(64) NOT NULL UNIQUE,
    hash  VARCHAR(64) NOT NULL,
    PRIMARY KEY (id)
) ENGINE = InnoDB;

CREATE TABLE login_sessions
(
    id              INT         NOT NULL AUTO_INCREMENT,
    user            INT         NOT NULL,
    token           VARCHAR(64) NOT NULL UNIQUE,
    expiration_date DATETIME    NOT NULL,
    create_date     DATETIME    NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_login_user FOREIGN KEY (user)
        REFERENCES users (id)
        ON DELETE CASCADE
) ENGINE = InnoDB;

CREATE TABLE asset_types
(
    id     INT         NOT NULL AUTO_INCREMENT,
    letter CHAR(1)     NOT NULL UNIQUE,
    name   VARCHAR(64) NOT NULL UNIQUE,
    PRIMARY KEY (id)
) ENGINE = InnoDB;

CREATE TABLE buildings
(
    id   INT         NOT NULL AUTO_INCREMENT,
    name VARCHAR(64) NOT NULL UNIQUE,
    PRIMARY KEY (id)
) ENGINE = InnoDB;

CREATE TABLE rooms
(
    id       INT         NOT NULL AUTO_INCREMENT,
    name     VARCHAR(64) NOT NULL,
    building INT         NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_room_building FOREIGN KEY (building)
        REFERENCES buildings (id)
        ON DELETE CASCADE
) ENGINE = InnoDB;

CREATE TABLE assets
(
    id   INT NOT NULL AUTO_INCREMENT,
    type INT NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_asset_assettype FOREIGN KEY (type)
        REFERENCES asset_types (id)
        ON DELETE CASCADE
) ENGINE = InnoDB;

CREATE TABLE reports
(
    id          INT      NOT NULL AUTO_INCREMENT,
    name        VARCHAR(64),
    room        INT      NOT NULL,
    owner       INT      NOT NULL,
    create_date DATETIME NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_report_room FOREIGN KEY (room)
        REFERENCES rooms (id)
        ON DELETE CASCADE,
    CONSTRAINT fk_report_user FOREIGN KEY (owner)
        REFERENCES users (id)
        ON DELETE CASCADE
) ENGINE = InnoDB;

CREATE TABLE reports_positions
(
    report_id     INT     NOT NULL,
    asset_id      INT     NOT NULL,
    previous_room INT     NULL,
    present       BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY (report_id, asset_id),
    CONSTRAINT fk_reportPosition_report FOREIGN KEY (report_id)
        REFERENCES reports (id)
        ON DELETE CASCADE,
    CONSTRAINT fk_reportPosition_asset FOREIGN KEY (asset_id)
        REFERENCES assets (id)
        ON DELETE CASCADE
) ENGINE = InnoDB;

CREATE TABLE scans
(
    id          INT      NOT NULL AUTO_INCREMENT,
    room        INT      NOT NULL,
    owner       INT      NOT NULL,
    create_date DATETIME NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_scanning_room FOREIGN KEY (room)
        REFERENCES rooms (id)
        ON DELETE CASCADE,
    CONSTRAINT fk_scanning_user FOREIGN KEY (owner)
        REFERENCES users (id)
        ON DELETE CASCADE
) ENGINE = InnoDB;

CREATE TABLE scans_positions
(
    scan  INT NOT NULL,
    asset INT NOT NULL,
    state INT NOT NULL,
    PRIMARY KEY (scan, asset),
    CONSTRAINT fk_scansPosition_report FOREIGN KEY (scan)
        REFERENCES scans (id)
        ON DELETE CASCADE,
    CONSTRAINT fk_scans_asset FOREIGN KEY (asset)
        REFERENCES assets (id)
        ON DELETE CASCADE
) ENGINE = InnoDB;
