CREATE TABLE IF NOT EXISTS Users (
    userID int PRIMARY KEY AUTO_INCREMENT,
    userEmail varchar(70) NOT NULL,
        UNIQUE (userEmail),
    userPass varchar(256) NOT NULL
);