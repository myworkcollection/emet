IF COL_LENGTH('dbo.TQuoteDetails', 'PckReqrmnt') IS NOT NULL
BEGIN
	alter table TQuoteDetails 
	alter column PckReqrmnt nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS null
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'PckReqrmnt') IS NOT NULL
BEGIN
	alter table TQuoteDetails_D
	alter column PckReqrmnt nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS null
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'OthReqrmnt') IS NOT NULL
BEGIN
	alter table TQuoteDetails 
	alter column OthReqrmnt nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS null
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'OthReqrmnt') IS NOT NULL
BEGIN
	alter table TQuoteDetails_D
	alter column OthReqrmnt nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS null
END
GO