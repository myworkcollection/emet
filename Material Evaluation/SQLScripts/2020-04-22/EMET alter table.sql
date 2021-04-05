IF COL_LENGTH('dbo.TQuoteDetails', 'IMRecycleRatio') IS NULL
BEGIN
	alter table TQuoteDetails 
	add IMRecycleRatio nvarchar(40) COLLATE SQL_Latin1_General_CP1_CI_AS null
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'IMRecycleRatio') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add IMRecycleRatio nvarchar(40) COLLATE SQL_Latin1_General_CP1_CI_AS null
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'NetProfDisc') IS NULL
BEGIN
	alter table TQuoteDetails
	add NetProfDisc nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS null
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'NetProfDisc') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add NetProfDisc nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS null
END
GO


IF COL_LENGTH('dbo.TMCCostDetails', 'RawMaterialCostUOM') IS NULL
BEGIN
	alter table TMCCostDetails
	add RawMaterialCostUOM nvarchar(40) COLLATE SQL_Latin1_General_CP1_CI_AS null
END
GO

IF COL_LENGTH('dbo.TMCCostDetails_D', 'RawMaterialCostUOM') IS NULL
BEGIN
	alter table TMCCostDetails_D
	add RawMaterialCostUOM nvarchar(40) COLLATE SQL_Latin1_General_CP1_CI_AS null
END
GO