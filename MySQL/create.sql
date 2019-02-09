CREATE TABLE IF NOT EXISTS Users (
    userID int PRIMARY KEY AUTO_INCREMENT,
    userEmail varchar(70) NOT NULL,
        UNIQUE (userEmail),
    userPass varchar(256) NOT NULL
);

CREATE TABLE IF NOT EXISTS PlayerGear (
	userID int PRIMARY KEY AUTO_INCREMENT,
	pathHead varchar(256),
    pathShoulders varchar(256),
    pathRightHand varchar(256),
    pathLeftHand varchar(256)
);