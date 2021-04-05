IF COL_LENGTH('dbo.TQuoteDetails', 'EmpSubmitionOn') IS NULL
BEGIN
	alter table TQuoteDetails
	add EmpSubmitionOn datetime null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'EmpSubmitionOn') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add EmpSubmitionOn datetime null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'EmpSubmitionBy') IS NULL
BEGIN
	alter table TQuoteDetails
	add EmpSubmitionBy nvarchar(50) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'EmpSubmitionBy') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add EmpSubmitionBy nvarchar(50) null;
END
GO