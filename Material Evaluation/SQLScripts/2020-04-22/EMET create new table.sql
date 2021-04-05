IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'TSMNBOM_RAWMATCost')
BEGIN
CREATE TABLE [dbo].[TSMNBOM_RAWMATCost](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RequestNo] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[QuoteNo] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RawMaterialCode] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RawMaterialDesc] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AmtSCur] [numeric](10, 2) NULL,
	[SellingCrcy] [nvarchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AmtVCur] [numeric](10, 2) NULL,
	[VendorCrcy] [nvarchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UOM] [nvarchar](4) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ValidFrom] [datetime] NULL,
	[ValidTo] [datetime] NULL,
	[ExchRate] [numeric](10, 5) NULL,
	[ExchValidFrom] [datetime] NULL,
	[CreatedON] [datetime] NULL,
	[CreateBy] [nvarchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,

 CONSTRAINT [PK_TRawMat] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'TSMMBOM_RAWMATCost_EffDate')
BEGIN
CREATE TABLE [dbo].[TSMMBOM_RAWMATCost_EffDate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RequestNo] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[QuoteNo] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RawMaterialCode] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RawMaterialDesc] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AmtSCur] [numeric](10, 2) NULL,
	[SellingCrcy] [nvarchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AmtVCur] [numeric](10, 2) NULL,
	[VendorCrcy] [nvarchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UOM] [nvarchar](4) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ValidFrom] [datetime] NULL,
	[ValidTo] [datetime] NULL,
	[ExchRate] [numeric](10, 5) NULL,
	[ExchValidFrom] [datetime] NULL,
	[CreatedON] [datetime] NULL,
	[CreateBy] [nvarchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,

 CONSTRAINT [PK_TRawMat] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO