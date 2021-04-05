--add field 

IF COL_LENGTH('dbo.TQuoteDetails', 'ApprovalDate') IS NULL
BEGIN
	alter table TQuoteDetails
	add ApprovalDate datetime null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'ApprovalDate') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add ApprovalDate datetime null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'ManagerRemark') IS NULL
BEGIN
	alter table TQuoteDetails
	add ManagerRemark nvarchar(450) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'ManagerRemark') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add ManagerRemark nvarchar(450) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'DIRRemark') IS NULL
BEGIN
	alter table TQuoteDetails
	add DIRRemark nvarchar(450) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'DIRRemark') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add DIRRemark nvarchar(450) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'AprRejByMng') IS NULL
BEGIN
	alter table TQuoteDetails
	add AprRejByMng nvarchar(450) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'AprRejByMng') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add AprRejByMng nvarchar(450) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'AprRejDateMng') IS NULL
BEGIN
	alter table TQuoteDetails
	add AprRejDateMng datetime null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'AprRejDateMng') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add AprRejDateMng datetime null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'VndAttchmnt') IS NULL
BEGIN
	alter table TQuoteDetails
	add VndAttchmnt nvarchar(250) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'VndAttchmnt') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add VndAttchmnt nvarchar(250) null;
END
GO