use coreDBTest
go

/****** Object:  Table [ln].[controllerRemarks]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[controllerRemarks](
	[controllerRemarksId] [nvarchar](400) NOT NULL DEFAULT ('Exact Match'),
	[description] [nvarchar](225) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[controllerRemarksId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[controllerRepaymentType]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[controllerRepaymentType](
	[controllerRepaymentTypeId] [nvarchar](30) NOT NULL DEFAULT ('P + I'),
	[description] [nvarchar](225) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[controllerRepaymentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO




SET ANSI_NULLS, QUOTED_IDENTIFIER ON
GO
create view [vw_controllerFile_outstandingLoan]
with encryption 
as
Select *
From
(
Select c.clientID, ln.loanID, cfd.staffID, cfd.fileDetailID, cfd.fileID, cfd.managementUnit, cfd.oldID,
		cfd.balBF,cfd.employeeName ,cfd.monthlyDeduction, cfd.origAmt, cfd.repaymentScheduleID, cfd.authorized, cfd.duplicate, cfd.refunded, cfd.notFound,
		cfd.overage, cfd.remarks, ln.loanNo, ln.amountDisbursed, ln.disbursementDate, 
		ROW_NUMBER() Over(Partition by ln.clientID, cfd.fileID Order by ln.disbursementDate) RecordNumber,
		Count(*) Over(Partition by ln.clientID, cfd.fileID) as LoanCount 
From ln.staffCategory sc
Inner Join ln.controllerFileDetail cfd on sc.employeeNumber=cfd.staffID
Inner Join ln.client c on c.clientID=sc.clientID
Inner JOin ln.loan ln on ln.clientID=c.clientID
where ln.disbursementDate is not null and ln.balance > 10
) Sub

where loanCount > 1
GO



/****** Object:  View [dbo].[loanNo_by_staffID]    Script Date: 25-May-19 1:00:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[loanNo_by_staffID]
AS
SELECT        TOP (100) PERCENT ln.staffCategory.employeeNumber, ln.staffCategory.clientID, ln.loan.loanNo, ln.client.clientID AS Expr1, ln.controllerFileDetail.fileID, ln.loan.balance, ln.loan.loanID
FROM            ln.client INNER JOIN
                         ln.staffCategory ON ln.client.clientID = ln.staffCategory.clientID INNER JOIN
                         ln.loan ON ln.client.clientID = ln.loan.clientID CROSS JOIN
                         ln.controllerFileDetail
ORDER BY ln.controllerFileDetail.fileID


GO
ALTER TABLE [ln].[saving] ADD [accountLocked] bit not null default (0);

ALTER TABLE [ln].[saving] ADD [depositReservation] float not null default (0);

ALTER TABLE [ln].[savingAdditional] ADD [balanceBD] [float] NOT NULL DEFAULT (0);

ALTER TABLE ln.saving ADD [withdrawalReservation] [float] NOT NULL DEFAULT (0),
	[lockedBy] [nvarchar](30) NULL,
	[lockDate] [datetime] NULL,
	[annualInterestRate] [float] NOT NULL DEFAULT (0),
	[reservedAmount] [float] NOT NULL DEFAULT (0),
	[lienBalance] [float] NOT NULL DEFAULT (0);


/****** Object:  StoredProcedure [ln].[sp_attempt_deposit]    Script Date: 27-May-19 9:57:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


USE coreDBTest
GO

/****** Object:  Table [ln].[borrowingDisbursement]    Script Date: 29-May-19 12:01:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[borrowingDisbursement](
	[borrowingDisbursementId] [int] IDENTITY(1,1) NOT NULL,
	[borrowingId] [int] NULL,
	[dateDisbursed] [datetime] NOT NULL,
	[amountDisbursed] [float] NOT NULL,
	[modeOfPaymentId] [int] NULL,
	[bankId] [int] NULL,
	[chequeNumber] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[borrowingDisbursementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ln].[borrowingDisbursement]  WITH CHECK ADD  CONSTRAINT [fk_borrowingDisbursement_bank] FOREIGN KEY([bankId])
REFERENCES [dbo].[banks] ([bank_id])
GO

ALTER TABLE [ln].[borrowingDisbursement] CHECK CONSTRAINT [fk_borrowingDisbursement_bank]
GO

ALTER TABLE [ln].[borrowingDisbursement]  WITH CHECK ADD  CONSTRAINT [fk_borrowingDisbursement_borrowing] FOREIGN KEY([borrowingId])
REFERENCES [ln].[borrowing] ([borrowingId])
GO

ALTER TABLE [ln].[borrowingDisbursement] CHECK CONSTRAINT [fk_borrowingDisbursement_borrowing]
GO

ALTER TABLE [ln].[borrowingDisbursement]  WITH CHECK ADD  CONSTRAINT [fk_borrowingDisbursement_modeOfPayment] FOREIGN KEY([modeOfPaymentId])
REFERENCES [ln].[modeOfPayment] ([modeOfPaymentID])
GO

ALTER TABLE [ln].[borrowingDisbursement] CHECK CONSTRAINT [fk_borrowingDisbursement_modeOfPayment]
GO


USE coreDBTest
GO

/****** Object:  Table [ln].[borrowingDocument]    Script Date: 29-May-19 12:02:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[borrowingDocument](
	[borrowingDocumentId] [int] IDENTITY(1,1) NOT NULL,
	[documentId] [int] NOT NULL,
	[borrowingId] [int] NOT NULL,
	[version] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[borrowingDocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ln].[borrowingDocument]  WITH CHECK ADD  CONSTRAINT [fk_borrowingDocument_borrowing] FOREIGN KEY([borrowingId])
REFERENCES [ln].[borrowing] ([borrowingId])
GO

ALTER TABLE [ln].[borrowingDocument] CHECK CONSTRAINT [fk_borrowingDocument_borrowing]
GO

ALTER TABLE [ln].[borrowingDocument]  WITH CHECK ADD  CONSTRAINT [fk_borrowingDocument_document] FOREIGN KEY([documentId])
REFERENCES [ln].[document] ([documentID])
GO

ALTER TABLE [ln].[borrowingDocument] CHECK CONSTRAINT [fk_borrowingDocument_document]
GO


USE coreDBTest
GO

/****** Object:  Table [ln].[borrowingFee]    Script Date: 29-May-19 12:04:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[borrowingFee](
	[borrowingfeeId] [int] IDENTITY(1,1) NOT NULL,
	[borrowingId] [int] NOT NULL,
	[feeDate] [datetime] NOT NULL,
	[feeAmount] [float] NOT NULL,
	[feeTypeId] [int] NOT NULL,
	[created] [datetime] NOT NULL,
	[creator] [nvarchar](100) NOT NULL,
	[modified] [datetime] NULL,
	[modifier] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[borrowingfeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ln].[borrowingFee]  WITH CHECK ADD  CONSTRAINT [fk_borrowingFee_borrowing] FOREIGN KEY([borrowingId])
REFERENCES [ln].[borrowing] ([borrowingId])
GO

ALTER TABLE [ln].[borrowingFee] CHECK CONSTRAINT [fk_borrowingFee_borrowing]
GO

ALTER TABLE [ln].[borrowingFee]  WITH CHECK ADD  CONSTRAINT [fk_borrowingFee_loanFeeType] FOREIGN KEY([feeTypeId])
REFERENCES [ln].[loanFeeType] ([feeTypeID])
GO

ALTER TABLE [ln].[borrowingFee] CHECK CONSTRAINT [fk_borrowingFee_loanFeeType]
GO


USE coreDBTest
GO

/****** Object:  Table [ln].[borrowingFeeType]    Script Date: 29-May-19 12:06:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[borrowingFeeType](
	[feeTypeId] [int] IDENTITY(1,1) NOT NULL,
	[feeTypeName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[feeTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE coreDBTest
GO

/****** Object:  Table [ln].[borrowingPenalty]    Script Date: 29-May-19 12:07:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[borrowingPenalty](
	[borrowingPenaltyId] [int] IDENTITY(1,1) NOT NULL,
	[borrowingId] [int] NOT NULL,
	[penaltyFee] [float] NOT NULL,
	[penaltyDate] [datetime] NOT NULL,
	[penaltyBalance] [float] NOT NULL,
	[created] [datetime] NOT NULL,
	[creator] [nvarchar](100) NOT NULL,
	[modified] [datetime] NULL,
	[modifier] [nvarchar](100) NULL,
	[version] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[borrowingPenaltyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ln].[borrowingPenalty] ADD  DEFAULT ((0)) FOR [penaltyBalance]
GO

ALTER TABLE [ln].[borrowingPenalty]  WITH CHECK ADD  CONSTRAINT [fk_borrowingPenalty_borrowing] FOREIGN KEY([borrowingId])
REFERENCES [ln].[borrowing] ([borrowingId])
GO

ALTER TABLE [ln].[borrowingPenalty] CHECK CONSTRAINT [fk_borrowingPenalty_borrowing]
GO


USE coreDBTest
GO

/****** Object:  Table [ln].[borrowingRepayment]    Script Date: 29-May-19 12:07:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[borrowingRepayment](
	[borrowingRepaymentId] [int] IDENTITY(1,1) NOT NULL,
	[borrowingId] [int] NOT NULL,
	[modeOfPaymentId] [int] NOT NULL,
	[repaymentTypeId] [int] NOT NULL,
	[repayementDate] [datetime] NOT NULL,
	[amountPaid] [float] NOT NULL,
	[interestPaid] [float] NOT NULL,
	[principalPaid] [float] NOT NULL,
	[feePaid] [float] NOT NULL,
	[penaltyPaid] [float] NOT NULL,
	[commissionPaid] [float] NOT NULL,
	[checkNo] [nvarchar](50) NULL,
	[bankId] [int] NULL,
	[created] [datetime] NOT NULL,
	[creator] [nvarchar](100) NOT NULL,
	[modified] [datetime] NULL,
	[modifier] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[borrowingRepaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ln].[borrowingRepayment] ADD  DEFAULT ((0)) FOR [commissionPaid]
GO

ALTER TABLE [ln].[borrowingRepayment]  WITH CHECK ADD  CONSTRAINT [fk_borrowingRepayment_borrowing] FOREIGN KEY([borrowingId])
REFERENCES [ln].[borrowing] ([borrowingId])
GO

ALTER TABLE [ln].[borrowingRepayment] CHECK CONSTRAINT [fk_borrowingRepayment_borrowing]
GO

ALTER TABLE [ln].[borrowingRepayment]  WITH CHECK ADD  CONSTRAINT [fk_borrowingRepayment_modeOfPayment] FOREIGN KEY([modeOfPaymentId])
REFERENCES [ln].[modeOfPayment] ([modeOfPaymentID])
GO

ALTER TABLE [ln].[borrowingRepayment] CHECK CONSTRAINT [fk_borrowingRepayment_modeOfPayment]
GO

ALTER TABLE [ln].[borrowingRepayment]  WITH CHECK ADD  CONSTRAINT [fk_borrowingRepayment_repaymentType] FOREIGN KEY([repaymentTypeId])
REFERENCES [ln].[repaymentType] ([repaymentTypeID])
GO

ALTER TABLE [ln].[borrowingRepayment] CHECK CONSTRAINT [fk_borrowingRepayment_repaymentType]
GO


USE coreDBTest
GO

/****** Object:  Table [ln].[borrowingRepaymentSchedule]    Script Date: 29-May-19 12:08:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[borrowingRepaymentSchedule](
	[borrowingRepaymentScheduleId] [int] IDENTITY(1,1) NOT NULL,
	[borrowingId] [int] NOT NULL,
	[repaymentDate] [datetime] NOT NULL,
	[interestPayment] [float] NOT NULL,
	[principalPayment] [float] NOT NULL,
	[interestBalance] [float] NOT NULL,
	[principalBalance] [float] NOT NULL,
	[balanceBF] [float] NOT NULL,
	[balanceCD] [float] NOT NULL,
	[edited] [bit] NOT NULL,
	[originalInterestPayment] [float] NULL,
	[originalPrincipalPayment] [float] NULL,
	[created] [datetime] NOT NULL,
	[creator] [nvarchar](100) NOT NULL,
	[modified] [datetime] NULL,
	[modifier] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[borrowingRepaymentScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ln].[borrowingRepaymentSchedule] ADD  DEFAULT ((0)) FOR [edited]
GO

ALTER TABLE [ln].[borrowingRepaymentSchedule]  WITH CHECK ADD  CONSTRAINT [fk_borrowingRepaymentSchedule_borrowing] FOREIGN KEY([borrowingId])
REFERENCES [ln].[borrowing] ([borrowingId])
GO

ALTER TABLE [ln].[borrowingRepaymentSchedule] CHECK CONSTRAINT [fk_borrowingRepaymentSchedule_borrowing]
GO


USE coreDBTest
GO

/****** Object:  Table [ln].[loanAdditionalInfo]    Script Date: 29-May-19 12:10:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[loanAdditionalInfo](
	[loanAdditionalInfoId] [int] IDENTITY(1,1) NOT NULL,
	[loanId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[loanAdditionalInfoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ln].[loanAdditionalInfo]  WITH CHECK ADD  CONSTRAINT [fk_additionalInfo_loan] FOREIGN KEY([loanId])
REFERENCES [ln].[loan] ([loanID])
GO

ALTER TABLE [ln].[loanAdditionalInfo] CHECK CONSTRAINT [fk_additionalInfo_loan]
GO


USE coreDBTest
GO

/****** Object:  Table [ln].[loanDocumentPlaceHolderType]    Script Date: 29-May-19 1:13:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[loanDocumentPlaceHolderType](
	[loanDocumentPlaceHolderTypeId] [int] IDENTITY(1,1) NOT NULL,
	[placeHolderTypeCode] [nvarchar](20) NOT NULL,
	[entityTypeCode] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[loanDocumentPlaceHolderTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE coreDBTest
GO

/****** Object:  Table [ln].[loanDocumentTemplate]    Script Date: 29-May-19 1:14:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[loanDocumentTemplate](
	[loanDocumentTemplateId] [int] IDENTITY(1,1) NOT NULL,
	[templateName] [nvarchar](30) NOT NULL,
	[creator] [nvarchar](30) NULL,
	[creationDate] [datetime] NULL,
	[modifier] [nvarchar](30) NOT NULL,
	[modified] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[loanDocumentTemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[templateName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE coreDBTest
GO

/****** Object:  Table [ln].[loanDocumentTemplatePage]    Script Date: 29-May-19 1:15:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[loanDocumentTemplatePage](
	[loanDocumentTemplatePageId] [int] IDENTITY(1,1) NOT NULL,
	[loanDocumentTemplateId] [int] NOT NULL,
	[pageNumber] [int] NOT NULL,
	[content] [ntext] NOT NULL,
	[isNew] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[loanDocumentTemplatePageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [ln].[loanDocumentTemplatePage] ADD  DEFAULT ((1)) FOR [isNew]
GO

ALTER TABLE [ln].[loanDocumentTemplatePage]  WITH CHECK ADD  CONSTRAINT [fk_loanDocumentTemplatePage_loanDocumentTemplate] FOREIGN KEY([loanDocumentTemplateId])
REFERENCES [ln].[loanDocumentTemplate] ([loanDocumentTemplateId])
GO

ALTER TABLE [ln].[loanDocumentTemplatePage] CHECK CONSTRAINT [fk_loanDocumentTemplatePage_loanDocumentTemplate]
GO


USE coreDBTest
GO

/****** Object:  Table [ln].[loanDocumentTemplatePagePlaceHolder]    Script Date: 29-May-19 1:15:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[loanDocumentTemplatePagePlaceHolder](
	[loanDocumentTemplatePagePlaceHolderId] [int] IDENTITY(1,1) NOT NULL,
	[loanDocumentTemplatePageId] [int] NOT NULL,
	[placeHolderTypeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[loanDocumentTemplatePagePlaceHolderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ln].[loanDocumentTemplatePagePlaceHolder]  WITH CHECK ADD  CONSTRAINT [fk_loanDocumentTemplatePagePlaceHolder_loanDocumentPlaceHolderType] FOREIGN KEY([placeHolderTypeId])
REFERENCES [ln].[loanDocumentPlaceHolderType] ([loanDocumentPlaceHolderTypeId])
GO

ALTER TABLE [ln].[loanDocumentTemplatePagePlaceHolder] CHECK CONSTRAINT [fk_loanDocumentTemplatePagePlaceHolder_loanDocumentPlaceHolderType]
GO

ALTER TABLE [ln].[loanDocumentTemplatePagePlaceHolder]  WITH CHECK ADD  CONSTRAINT [fk_loanDocumentTemplatePagePlaceHolder_loanDocumentTemplatePage] FOREIGN KEY([loanDocumentTemplatePageId])
REFERENCES [ln].[loanDocumentTemplatePage] ([loanDocumentTemplatePageId])
GO

ALTER TABLE [ln].[loanDocumentTemplatePagePlaceHolder] CHECK CONSTRAINT [fk_loanDocumentTemplatePagePlaceHolder_loanDocumentTemplatePage]
GO


USE coreDBTest
GO

/****** Object:  Table [ln].[metaDataType]    Script Date: 29-May-19 1:17:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[metaDataType](
	[metaDataTypeId] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[dataType] [nvarchar](50) NOT NULL,
	[nameCode] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[metaDataTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE coreDBTest
GO

/****** Object:  Table [ln].[loanMetaData]    Script Date: 29-May-19 1:16:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[loanMetaData](
	[loanMetaDataId] [int] IDENTITY(1,1) NOT NULL,
	[loanAdditionalInfoId] [int] NOT NULL,
	[metaDataTypeId] [int] NOT NULL,
	[content] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[loanMetaDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ln].[loanMetaData]  WITH CHECK ADD  CONSTRAINT [fk_loanMetaData_additionalInfo] FOREIGN KEY([loanAdditionalInfoId])
REFERENCES [ln].[loanAdditionalInfo] ([loanAdditionalInfoId])
GO

ALTER TABLE [ln].[loanMetaData] CHECK CONSTRAINT [fk_loanMetaData_additionalInfo]
GO

ALTER TABLE [ln].[loanMetaData]  WITH CHECK ADD  CONSTRAINT [fk_loanMetaData_metaDataType] FOREIGN KEY([metaDataTypeId])
REFERENCES [ln].[metaDataType] ([metaDataTypeId])
GO

ALTER TABLE [ln].[loanMetaData] CHECK CONSTRAINT [fk_loanMetaData_metaDataType]
GO


/*

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [hc].[overtimeConfig](
	[overtimeConfigID] [int] IDENTITY(1,1) NOT NULL,
	[levelID] [int] NOT NULL,
	[saturdayRate] [float] NOT NULL,
	[sundayRate] [float] NOT NULL,
	[holidayRate] [float] NOT NULL,
	[weekdayRate] [float] NOT NULL,
	[overtimeTaxRate] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[overtimeConfigID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

*/

USE coreDBTest
GO

/****** Object:  Table [hc].[overTimeConfig]    Script Date: 29-May-19 1:25:36 PM ******/
DROP TABLE [hc].[overtimeConfig]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [hc].[overTimeConfig](
	[overTimeConfigID] [int] IDENTITY(1,1) NOT NULL,
	[levelID] [int] NOT NULL,
	[saturdayHoursRate] [float] NOT NULL,
	[sundayHoursRate] [float] NOT NULL,
	[holidayHoursRate] [float] NOT NULL,
	[weekdayAfterWorkHoursRate] [float] NOT NULL,
	[overTime5PerTax] [float] NOT NULL,
	[overTime10PerTax] [float] NOT NULL,
	[maxOvertimeHours] [float] NOT NULL,
	[maxOvertimePercentOfBasic] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[overTimeConfigID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [hc].[overTimeConfig] ADD  DEFAULT ((0)) FOR [saturdayHoursRate]
GO

ALTER TABLE [hc].[overTimeConfig] ADD  DEFAULT ((0)) FOR [sundayHoursRate]
GO

ALTER TABLE [hc].[overTimeConfig] ADD  DEFAULT ((0)) FOR [holidayHoursRate]
GO

ALTER TABLE [hc].[overTimeConfig] ADD  DEFAULT ((0)) FOR [weekdayAfterWorkHoursRate]
GO

ALTER TABLE [hc].[overTimeConfig] ADD  DEFAULT ((5)) FOR [overTime5PerTax]
GO

ALTER TABLE [hc].[overTimeConfig] ADD  DEFAULT ((10)) FOR [overTime10PerTax]
GO

ALTER TABLE [hc].[overTimeConfig] ADD  DEFAULT ((0)) FOR [maxOvertimeHours]
GO

ALTER TABLE [hc].[overTimeConfig] ADD  DEFAULT ((0)) FOR [maxOvertimePercentOfBasic]
GO

ALTER TABLE [hc].[overTimeConfig]  WITH CHECK ADD  CONSTRAINT [fk_overTimeConfig_level] FOREIGN KEY([levelID])
REFERENCES [hc].[level] ([levelID])
GO

ALTER TABLE [hc].[overTimeConfig] CHECK CONSTRAINT [fk_overTimeConfig_level]
GO


