IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'TMngEffDateChgLog')
BEGIN
CREATE TABLE [dbo].[TMngEffDateChgLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RequestNumber] [nvarchar](100) NOT NULL,
	[QuoteNo] [nvarchar](50) NULL,
	[Material] [nvarchar](50) NULL,
	[VendorCode1] [nvarchar](50) NULL,
	[EffectiveDate] [datetime] NULL,
	[DueOn] [datetime] NULL,
	[NewEffectiveDate] [datetime] NULL,
	[NewDueOn] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_TMngEffDateChgLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO