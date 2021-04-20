/* 
	I. Account
	
	1. Admin (1)
	2. User (2)
*/

INSERT INTO Account(Username, Password, IsManager, IsActive)
VALUES ('admin', '123', 'True', 'True');

INSERT INTO Account(Username, Password, IsManager, IsActive)
VALUES ('HE141325', '123', 'False', 'True');

INSERT INTO Account(Username, Password, IsManager, IsActive)
VALUES ('HE141311', '123', 'False', 'True');

/* 
	II. Price Type

	1. Electricity
	2. Water
	3. Room
	4. Fine 
*/
INSERT INTO PriceType(Name) VALUES('Electricity');
INSERT INTO PriceType(Name) VALUES('Water');
INSERT INTO PriceType(Name) VALUES('Room');
INSERT INTO PriceType(Name) VALUES('Fine');

/* 
	III. Price

	1. Electricity: 3000/number, default have 100 number
	2. Water: 3000/number, default have 100 number
	3. Room: 800k/room, defaul null number
	4. Fine: 50k/fine, default null number
*/
INSERT INTO Price(TypeId, StandardPrice, StandardUsage) VALUES (1, 3000, 100);

INSERT INTO Price(TypeId, StandardPrice, StandardUsage) VALUES (2, 3000, 100);

INSERT INTO Price(TypeId, StandardPrice) VALUES (3, 800000);

INSERT INTO Price(TypeId, StandardPrice) VALUES (4, 50000);

/* 
	IV. Dom (8 totals)

	1. A
	2. B
	3. C
	4. D
	5. E
	6. F 
*/
INSERT INTO Dom(Name) VALUES('A');
INSERT INTO Dom(Name) VALUES('B');
INSERT INTO Dom(Name) VALUES('C');
INSERT INTO Dom(Name) VALUES('D');
INSERT INTO Dom(Name) VALUES('E');
INSERT INTO Dom(Name) VALUES('F');
INSERT INTO Dom(Name) VALUES('G');
INSERT INTO Dom(Name) VALUES('H');


/* 
	V. Floor (32 totals)

	1. DomID: A -> H (1 -> 8)
	2. FloorNumber: 1 -> 4
	
*/

DECLARE @DomID INT, @FloorNumber INT
SET @DomID=1

WHILE (@DomID <= 8)	 
	BEGIN
		SET @FloorNumber=1
		WHILE (@FloorNumber <= 4)
			BEGIN
				INSERT INTO Floor(DomId, Number) VALUES (@DomID, @FloorNumber)
				SET @FloorNumber = @FloorNumber + 1
			END
		SET @DomID = @DomID + 1;
	END


/* 
	VI. Room (112 totals)

	1. DomID: A -> H (1 -> 8)
	2. FloorID: 1 -> 4
	3. RoomNumber: 1 -> 14

	Note: Clone data for 2 Dom only!!
	Floor from 1 -> 8
*/
DECLARE @FloorID INT, @RoomNumber INT
SET @FloorID = 1

WHILE (@FloorID <= 8)	 
	BEGIN
		SET @RoomNumber = 1
		WHILE (@RoomNumber <= 14)
			BEGIN
				INSERT INTO Room(FloorId, RoomNumber, RoomGender, CanUse) VALUES (@FloorID, @RoomNumber, NULL, 'True');
				SET @RoomNumber = @RoomNumber + 1
			END
		SET @FloorID = @FloorID + 1;
	END

/*
	VII. Bed (896 totals)

	1. DomID: A -> H (1 -> 8)
	2. FloorID: 1 -> 4
	3. RoomId: 1 -> 14
	4. BedNumber: 1 -> 8
*/
DECLARE @RoomID INT, @BedNumber INT
SET @RoomID = 1

WHILE (@RoomID <= 112)	 
	BEGIN
		SET @BedNumber = 1
		WHILE (@BedNumber <= 8)
			BEGIN
				INSERT INTO Bed(RoomId, BedNumber, IsAvailable) VALUES (@RoomID, @BedNumber, 'False');
				SET @BedNumber = @BedNumber + 1
			END
		SET @RoomID = @RoomID + 1;
	END

/*
	VIII. Student (26)

	1. BedID: 1 -> 20

*/

DECLARE @intCharCode INT, @BedId INT
SET @intCharCode = 65
SET @BedId = 1

WHILE NOT (@intCharCode > 90)
	BEGIN
		INSERT INTO Student(StudentCode, BedId, Name, Gender, Email, Avatar, HasInDorm)
		VALUES(
		CONCAT('HE1413', @intCharCode),
		@BedId,
		CONCAT('Nguyen Van ', CHAR(@intCharCode)),
		'True',
		NULL,
		NULL,
		'True')
		SET @BedId = @BedId + 1
		SET @intCharCode = @intCharCode + 1	
	END

/*
	IX. Room Usage

	1. total 4 rooms for 26 students
	2. water usage default: 1000
	3. electric usage default: 1000

*/
DECLARE @RoomID2 INT = 1

WHILE (@RoomID2 <= 4)	 
	BEGIN
		INSERT INTO RoomUsage(RoomId, WaterUsage, ElectricUsage) VALUES (@RoomID2, 1000, 1000);
		SET @RoomID2 = @RoomID2 + 1
	END