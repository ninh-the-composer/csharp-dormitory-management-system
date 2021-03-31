/* Account */

INSERT INTO Account(Username, Password, IsManager, IsActive)
VALUES ('admin', '123', 'True', 'True');

INSERT INTO Account(Username, Password, IsManager, IsActive)
VALUES ('user', '123', 'False', 'True');

INSERT INTO Account(Username, Password, IsManager, IsActive)
VALUES ('user2', '123', 'True', 'False');

/* Price Type: Electricity, Water, Room, Fine */
INSERT INTO PriceType(Name) VALUES('Electricity');
INSERT INTO PriceType(Name) VALUES('Water');
INSERT INTO PriceType(Name) VALUES('Room');
INSERT INTO PriceType(Name) VALUES('Fine');

/* Price: Price of specified type, has standard price and standard usage */
INSERT INTO Price(PriceType, StandardPrice, StandardUsage) VALUES (1, 3000, 80);

INSERT INTO Price(PriceType, StandardPrice, StandardUsage) VALUES (2, 3000, 150);

INSERT INTO Price(PriceType, StandardPrice) VALUES (3, 800000);

INSERT INTO Price(PriceType, StandardPrice) VALUES (4, 50000);

/* Dom: Contains (A, B, C, D, E, F) */
INSERT INTO Dom(Name) VALUES('A');
INSERT INTO Dom(Name) VALUES('B');
INSERT INTO Dom(Name) VALUES('C');
INSERT INTO Dom(Name) VALUES('D');
INSERT INTO Dom(Name) VALUES('E');
INSERT INTO Dom(Name) VALUES('F');
INSERT INTO Dom(Name) VALUES('G');
INSERT INTO Dom(Name) VALUES('H');


/* 
	Floor: 1 Dom has 4 floor, has 8 Doms (A, B, C, D, E, F, G, H)
	DomID: 1-> 8
	FloorID: 1-> 4
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
	Room: 1 floor has 14 rooms, has 4 floors (1, 2, 3, 4) 
	FloorID: 1 -> 32
	RoomNumber: 1 -> 14
*/
DECLARE @FloorID INT, @RoomNumber INT
SET @FloorID = 1

WHILE (@FloorID <= 32)	 
	BEGIN
		SET @RoomNumber = 1
		WHILE (@RoomNumber <= 14)
			BEGIN
				INSERT INTO Room(FloorId, RoomNumber) VALUES (@FloorID, @RoomNumber);
				SET @RoomNumber = @RoomNumber + 1
			END
		SET @FloorID = @FloorID + 1;
	END

/*
	Bed: 1 room has 8 beds
	RoomId: 1 * 8 (Dom) * 4 (Floor) * 14(room) = 448
	Bed: 1 * 8 (Dom) * 4 (Floor) * 14(room) * 8(bed) = 3584
*/
DECLARE @RoomID INT, @BedNumber INT
SET @RoomID = 1

WHILE (@RoomID <= 448)	 
	BEGIN
		SET @BedNumber = 1
		WHILE (@BedNumber <= 8)
			BEGIN
				INSERT INTO Bed(RoomId, BedNumber, IsAvailable) VALUES (@RoomID, @BedNumber, 'False');
				SET @BedNumber = @BedNumber + 1
			END
		SET @RoomID = @RoomID + 1;
	END

select count(*) from bed