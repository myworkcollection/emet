-- set default for isUseSAPcode
declare @name nvarchar(100)

set @name = (SELECT
    'ALTER TABLE ' + tables.name + ' DROP CONSTRAINT ' + default_constraints.name + ' ; ' AS 'data()' 
FROM 
    sys.all_columns

        INNER JOIN
    sys.tables
        ON all_columns.object_id = tables.object_id

        INNER JOIN 
    sys.schemas
        ON tables.schema_id = schemas.schema_id

        INNER JOIN
    sys.default_constraints
        ON all_columns.default_object_id = default_constraints.object_id
WHERE 
        schemas.name = 'dbo'
    AND tables.name = 'TQuoteDetails'
    AND all_columns.name = 'isUseSAPCode'
	FOR XML PATH('')) 

exec (@name)

alter table TQuoteDetails 
ADD CONSTRAINT DF_isUseSAPCode
DEFAULT '1' FOR isUseSAPCode;
GO

declare @name2 nvarchar(100)

set @name2 = (SELECT
    'ALTER TABLE ' + tables.name + ' DROP CONSTRAINT ' + default_constraints.name + ' ; ' AS 'data()' 
FROM 
    sys.all_columns

        INNER JOIN
    sys.tables
        ON all_columns.object_id = tables.object_id

        INNER JOIN 
    sys.schemas
        ON tables.schema_id = schemas.schema_id

        INNER JOIN
    sys.default_constraints
        ON all_columns.default_object_id = default_constraints.object_id
WHERE 
        schemas.name = 'dbo'
    AND tables.name = 'TQuoteDetails_D'
    AND all_columns.name = 'isUseSAPCode'
	FOR XML PATH('')) 

exec (@name2)

alter table TQuoteDetails_D
ADD CONSTRAINT DF_isUseSAPCode_D
DEFAULT '1' FOR isUseSAPCode;
GO

--change from typefiled ntext to nvarchar
alter table TQuoteDetails 
alter column ERemarks nvarchar(300)
GO

alter table TQuoteDetails_D
alter column ERemarks nvarchar(300)
GO

--add field 

IF COL_LENGTH('dbo.TQuoteDetails', 'isMassRevision') IS NULL
BEGIN
	alter table TQuoteDetails 
	add isMassRevision bit DEFAULT 0
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'isMassRevision') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add isMassRevision bit DEFAULT 0
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'PIRNo') IS NULL
BEGIN
	alter table TQuoteDetails
	add PIRNo nvarchar(50) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'PIRNo') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add PIRNo nvarchar(50) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'OldTotMatCost') IS NULL
BEGIN
	alter table TQuoteDetails
	add OldTotMatCost nvarchar(50) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'OldTotMatCost') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add OldTotMatCost nvarchar(50) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'OldTotSubMatCost') IS NULL
BEGIN
	alter table TQuoteDetails
	add OldTotSubMatCost nvarchar(50) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'OldTotSubMatCost') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add OldTotSubMatCost nvarchar(50) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'OldTotProCost') IS NULL
BEGIN
	alter table TQuoteDetails
	add OldTotProCost nvarchar(50) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'OldTotProCost') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add OldTotProCost nvarchar(50) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'OldTotOthCost') IS NULL
BEGIN
	alter table TQuoteDetails
	add OldTotOthCost nvarchar(50) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'OldTotOthCost') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add OldTotOthCost nvarchar(50) null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails', 'MassUpdateDate') IS NULL
BEGIN
	alter table TQuoteDetails
	add MassUpdateDate datetime null;
END
GO

IF COL_LENGTH('dbo.TQuoteDetails_D', 'MassUpdateDate') IS NULL
BEGIN
	alter table TQuoteDetails_D
	add MassUpdateDate datetime null;
END
GO

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