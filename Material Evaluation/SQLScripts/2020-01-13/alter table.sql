--add field 
IF COL_LENGTH('dbo.TQuoteDetails', 'PICRejReason') IS NULL
BEGIN
	alter table TQuoteDetails
	add PICRejReason nvarchar(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'PICRejReason') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add PICRejReason nvarchar(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL;
END
GO


IF COL_LENGTH('dbo.TQuoteDetails', 'PICRejRemark') IS NULL
BEGIN
	alter table TQuoteDetails
	add PICRejRemark nvarchar(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'PICRejRemark') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add PICRejRemark nvarchar(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL;
END
GO


--add field 
IF COL_LENGTH('dbo.TQuoteDetails', 'IsReSubmit') IS NULL
BEGIN
	alter table TQuoteDetails
	add IsReSubmit bit default(0);
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'IsReSubmit') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add IsReSubmit bit default(0);
END
GO