--mdm
use MDM
INSERT [dbo].[TFORM] ([System], [FormName], [CreatedBy], [CreatedDate], [CreatedAt], [UpdateBy], [UpdateDate], [UpdateAt], [formdesc], [DelFlag], [TableName]) VALUES (N'eMET', N'EMET_RevisionReqWaitting', N'SQL', CURRENT_TIMESTAMP, N'SQL', NULL, NULL, NULL, N'Quote Request With SAP Code (Revision)', 0, N'')
GO

INSERT [dbo].[TFORM] ([System], [FormName], [CreatedBy], [CreatedDate], [CreatedAt], [UpdateBy], [UpdateDate], [UpdateAt], [formdesc], [DelFlag], [TableName]) VALUES (N'eMET', N'EMET_MassRevision', N'SQL', CURRENT_TIMESTAMP, N'SQL', NULL, NULL, NULL, N'Mass Revision', 0, N'')
GO

INSERT [dbo].[TFORM] ([System], [FormName], [CreatedBy], [CreatedDate], [CreatedAt], [UpdateBy], [UpdateDate], [UpdateAt], [formdesc], [DelFlag], [TableName]) VALUES (N'eMET', N'EMET_MassRevVndPending', N'SQL', CURRENT_TIMESTAMP, N'SQL', NULL, NULL, NULL, N'Quote Request With SAP Code (Mass Revision)', 0, N'')
GO

INSERT [dbo].[TFORM] ([System], [FormName], [CreatedBy], [CreatedDate], [CreatedAt], [UpdateBy], [UpdateDate], [UpdateAt], [formdesc], [DelFlag], [TableName]) VALUES (N'eMET', N'EMET_NewReqMass', N'SQL', CURRENT_TIMESTAMP, N'SQL', NULL, NULL, NULL, N'New Mass Revision Request', 0, N'')
GO

INSERT [dbo].[TFORM] ([System], [FormName], [CreatedBy], [CreatedDate], [CreatedAt], [UpdateBy], [UpdateDate], [UpdateAt], [formdesc], [DelFlag], [TableName]) VALUES (N'eMET', N'EMET_ReviewReqMass', N'SQL', CURRENT_TIMESTAMP, N'SQL', NULL, NULL, NULL, N'Draft Mass Revision Request', 0, N'')
GO

--emet
use EMET
update TQuoteDetails set isMassRevision = 0 where isMassRevision is null 
GO
