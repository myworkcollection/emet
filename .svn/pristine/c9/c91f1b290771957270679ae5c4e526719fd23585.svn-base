--add field 

IF COL_LENGTH('dbo.TQuoteDetails', 'SMNPicDept') IS NULL
BEGIN
	alter table TQuoteDetails
	add SMNPicDept nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'SMNPicDept') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add SMNPicDept nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
END
GO
