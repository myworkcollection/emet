IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'TERRORLOG')
BEGIN
    CREATE TABLE [dbo].[TERRORLOG](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ErrDescription] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ErrType] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ErrSource] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ErrUrl] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AddedBy] [nvarchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AddedOn] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO