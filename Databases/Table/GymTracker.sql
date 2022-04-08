﻿CREATE TABLE [Gym].[dbo].[GymTracker1] (
	[ID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[DateCreated] DATETIME NOT NULL,
	[BodyPart] NVARCHAR(50) NULL,
	[Exercise] NVARCHAR(50) NULL,
	[Sets] NVARCHAR(50) NULL,
	[Reps] NVARCHAR(50) NULL,
	[Weights] NVARCHAR(50) NULL
)