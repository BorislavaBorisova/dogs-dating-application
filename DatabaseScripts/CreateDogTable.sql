CREATE TABLE [dbo].[Dog]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(30) NOT NULL,
	[Age] INT NOT NULL,
	[Gender] NVARCHAR(10) NOT NULL,
	[Breed] UNIQUEIDENTIFIER NOT NULL,
	[Owner] UNIQUEIDENTIFIER NOT NULL,
	[Specifics] NVARCHAR (MAX) NULL,
	[ProfilePicturePath] NVARCHAR (MAX) NULL,
	CONSTRAINT [FK_Breed] FOREIGN KEY ([Breed]) REFERENCES [Breed]([Id]),
	CONSTRAINT [FK_Owner] FOREIGN KEY ([Owner]) REFERENCES [User]([Id])
)

