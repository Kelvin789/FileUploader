IF DB_ID('Gym') IS NULL
	CREATE DATABASE [Gym]

GO

USE [Gym]

CREATE TABLE [Gym].[dbo].[GymTracker] (
	[ID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[DateCreated] DateTime NOT NULL,
	[BodyPart] NVARCHAR(50),
	[Exercise] NVARCHAR(50),
	[Sets] NVARCHAR(50) NULL,
	[Reps] NVARCHAR(50) NULL,
	[Weights] NVARCHAR(50) NULL,
)