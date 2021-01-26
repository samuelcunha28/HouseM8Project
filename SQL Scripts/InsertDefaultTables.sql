INSERT INTO dbo.[Ranks] (Id, Name)
VALUES
	(1, 'MATE'),
	(2, 'SUPER_MATE'),
	(3, 'HYPER_MATE'),
	(4, 'KING_MATE');

INSERT INTO dbo.[Roles] (Id, Name)
VALUES
	(1, 'M8'),
	(2, 'EMPLOYER'),
	(3, 'ADMIN');

INSERT INTO dbo.[PaymentType] (Id, Name)
VALUES
	(1, 'PAYPAL'),
	(2, 'MBWAY'),
	(3, 'CRYPTO'),
	(4, 'MONEY');

INSERT INTO dbo.[Categories] (Id, Name)
VALUES
	(1, 'PLUMBING'),
	(2, 'GARDENING'),
	(3, 'ELECTRICITY'),
	(4, 'FURNITURE_ASSEMBLE'),
	(5, 'INTERIOR_DESIGN'),
	(6, 'TRANSPORTATION'),
	(7, 'CLEANING');

INSERT INTO dbo.[Achievements] (Id, Name)
VALUES
	(1, 'COMPLETED_JOBS_10'),
	(2, 'TIPS_5'),
	(3, 'TIMES_FAVOURITE_5'),
	(4, 'TRAVELLED_100KM'),
	(5, 'REVIEWS_5TIMES_5STAR');