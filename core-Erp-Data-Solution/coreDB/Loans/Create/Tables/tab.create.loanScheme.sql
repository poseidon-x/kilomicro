USE [coreDB]
GO

/****** Object:  Table [ln].[loanScheme]    Script Date: 10/21/2014 12:40:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[loanScheme](
	[loanSchemeId] [int] IDENTITY(1,1) NOT NULL,
	[loanTypeId] [int] NOT NULL,
	[employerId] [int] NOT NULL,
	[loanSchemeName] [nvarchar](100) NOT NULL,
	[tenure] [float] NOT NULL,
	[rate] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[loanSchemeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

