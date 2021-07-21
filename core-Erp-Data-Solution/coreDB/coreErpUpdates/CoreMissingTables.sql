/****** Object:  Table [ln].[bogReportsConfig]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[bogReportsConfig](
	[bogReportsConfigId] [int] NOT NULL,
	[paidUpCapital] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[bogReportsConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[cashierCashupCoin]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[cashierCashupCoin](
	[cashierCashupNoteId] [int] IDENTITY(1,1) NOT NULL,
	[cashierCashupId] [int] NOT NULL,
	[currencyNoteId] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[total] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierCashupNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[cashierCashupNote]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[cashierCashupNote](
	[cashierCashupNoteId] [int] IDENTITY(1,1) NOT NULL,
	[cashierCashupId] [int] NOT NULL,
	[currencyNoteId] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[total] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierCashupNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[cashierFundCoin]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[cashierFundCoin](
	[cashierFundNoteId] [int] IDENTITY(1,1) NOT NULL,
	[cashierFundId] [int] NOT NULL,
	[currencyNoteId] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[total] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierFundNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[cashierFundNote]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[cashierFundNote](
	[cashierFundNoteId] [int] IDENTITY(1,1) NOT NULL,
	[cashierFundId] [int] NOT NULL,
	[currencyNoteId] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[total] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierFundNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[cashierFundsTransfer]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[cashierFundsTransfer](
	[cashierFundsTransferId] [int] IDENTITY(1,1) NOT NULL,
	[sendingCashierTillId] [int] NOT NULL,
	[receivingCashierTillId] [int] NOT NULL,
	[transferAmount] [float] NOT NULL,
	[transferDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierFundsTransferId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[cashierRemainingCoin]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[cashierRemainingCoin](
	[cashierRemainingCoinId] [int] IDENTITY(1,1) NOT NULL,
	[cashierTillId] [int] NOT NULL,
	[currencyNoteId] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[total] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierRemainingCoinId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[cashierRemainingNote]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[cashierRemainingNote](
	[cashierRemainingCoinId] [int] IDENTITY(1,1) NOT NULL,
	[cashierTillId] [int] NOT NULL,
	[currencyNoteId] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[total] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierRemainingCoinId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[cashierTransferType]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[cashierTransferType](
	[cashierTransferTypeId] [int] IDENTITY(1,1) NOT NULL,
	[transferTypeName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierTransferTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[clientConfig]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[clientConfig](
	[clientConfigId] [int] IDENTITY(1,1) NOT NULL,
	[admissionFeeEnabled] [bit] NOT NULL,
	[admissionFeeAmount] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[clientConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[clientMandateType]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[clientMandateType](
	[clientMandateTypeId] [int] IDENTITY(1,1) NOT NULL,
	[mandate] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[clientMandateTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[clientServiceCharge]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[clientServiceCharge](
	[clientServiceChargeId] [int] IDENTITY(1,1) NOT NULL,
	[clientId] [int] NOT NULL,
	[chargeTypeId] [int] NOT NULL,
	[chargeDate] [datetime] NOT NULL,
	[chargeAmount] [float] NOT NULL,
	[posted] [bit] NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NULL,
	[modified] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[clientServiceChargeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[depositCertificateConfig]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[depositCertificateConfig](
	[depositCertificateConfigId] [tinyint] NOT NULL,
	[earlyRedemptionText] [ntext] NOT NULL,
	[trustText] [ntext] NOT NULL,
	[authorityText] [ntext] NOT NULL,
	[riskDisclosureText] [ntext] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[depositCertificateConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [ln].[depositDisInvestmentConfig]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[depositDisInvestmentConfig](
	[depositDisInvestmentConfigId] [int] IDENTITY(1,1) NOT NULL,
	[minTenure] [float] NOT NULL,
	[maxTenure] [float] NOT NULL,
	[penaltyRate] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[depositDisInvestmentConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[depositPeriodInDays]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[depositPeriodInDays](
	[depositPeriodInDaysId] [int] NOT NULL,
	[periodInDays] [int] NOT NULL,
	[period] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[depositPeriodInDaysId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[depositTypePlanRate]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[depositTypePlanRate](
	[depositTypePlanRateId] [int] IDENTITY(1,1) NOT NULL,
	[depositTypeId] [int] NOT NULL,
	[minimumAmount] [float] NOT NULL,
	[maximumAmount] [float] NOT NULL,
	[interestRate] [float] NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NULL,
	[modified] [datetime] NULL,
	[useTreasuryBillRate] [bit] NOT NULL,
	[useIncrement] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[depositTypePlanRateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[lienReason]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[lienReason](
	[lienReasonId] [int] NOT NULL,
	[lienReasonName] [nvarchar](60) NOT NULL,
	[detailDescription] [ntext] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[lienReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [ln].[lienReleaseReason]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[lienReleaseReason](
	[lienReleaseReasonId] [int] IDENTITY(1,1) NOT NULL,
	[reason] [nvarchar](150) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[lienReleaseReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[provisionBatch]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[provisionBatch](
	[provisionBatchId] [int] IDENTITY(1,1) NOT NULL,
	[provisionYear] [int] NOT NULL,
	[provisionMonth] [int] NOT NULL,
	[provisionDate] [datetime] NOT NULL,
	[edited] [bit] NOT NULL,
	[posted] [bit] NOT NULL,
	[reversed] [bit] NOT NULL,
	[provisionJournalBatchId] [int] NULL,
	[reversalJournalBatchId] [int] NULL,
	[postedValueDate] [datetime] NULL,
	[reversalValueDate] [datetime] NULL,
	[initiatedBy] [nvarchar](30) NOT NULL,
	[initiationDate] [datetime] NOT NULL,
	[postedBy] [nvarchar](30) NULL,
	[reversedBy] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[provisionBatchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[reservationType]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[reservationType](
	[reservationTypeId] [tinyint] NOT NULL,
	[reservationTypeName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[reservationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[savingBalance]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[savingBalance](
	[savingBalanceId] [int] IDENTITY(1,1) NOT NULL,
	[balanceDate] [datetime] NOT NULL,
	[beginningOfDayBalance] [float] NOT NULL,
	[endOfDayBalance] [float] NULL,
	[totalCredit] [float] NULL,
	[totalDebit] [float] NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NOT NULL,
	[modified] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[savingBalanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[savingLienRelease]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[savingLienRelease](
	[savingLienReleaseId] [int] IDENTITY(1,1) NOT NULL,
	[savingLienId] [int] NOT NULL,
	[releaseDate] [datetime] NOT NULL,
	[releaseAmount] [float] NOT NULL,
	[releaseReasonId] [int] NOT NULL,
	[releaseBy] [nvarchar](30) NOT NULL,
	[releaseDetail] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[savingLienReleaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [ln].[savingPlanInterval]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[savingPlanInterval](
	[savingPlanIntervalId] [int] IDENTITY(1,1) NOT NULL,
	[planDays] [int] NOT NULL,
	[planIntervalName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[savingPlanIntervalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[savingReservationTransc]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[savingReservationTransc](
	[savingReservationTranscId] [int] IDENTITY(1,1) NOT NULL,
	[savingId] [int] NOT NULL,
	[reservationAmount] [float] NOT NULL,
	[reservationDate] [datetime] NOT NULL,
	[reservedBy] [nvarchar](30) NOT NULL,
	[naration] [nvarchar](255) NOT NULL,
	[transactionId] [nvarchar](30) NOT NULL,
	[committed] [bit] NOT NULL,
	[committedDate] [datetime] NULL,
	[reservationTypeId] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[savingReservationTranscId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[transactionType]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[transactionType](
	[transactionTypeId] [int] NOT NULL,
	[transactionTypeName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[transactionTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[loanPenaltyDisable]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[loanPenaltyDisable](
	[loanPenaltyDisableId] [int] IDENTITY(1,1) NOT NULL,
	[loanId] [int] NOT NULL,
	[disabledDate] [datetime] NOT NULL,
	[disabledBy] [nvarchar](30) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[loanPenaltyEnable]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[loanPenaltyEnable](
	[loanPenaltyEnableId] [int] IDENTITY(1,1) NOT NULL,
	[loanId] [int] NOT NULL,
	[enabledDate] [datetime] NOT NULL,
	[enabledBy] [nvarchar](30) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[depositNextOfKin]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[depositNextOfKin](
	[depositNextOfKinId] [int] IDENTITY(1,1) NOT NULL,
	[depositId] [int] NOT NULL,
	[otherNames] [nvarchar](100) NOT NULL,
	[surName] [nvarchar](50) NOT NULL,
	[dateOfBirth] [datetime] NULL,
	[relationshipTypeId] [int] NOT NULL,
	[idTypeId] [int] NOT NULL,
	[idNumber] [nvarchar](20) NOT NULL,
	[phoneNumber] [nvarchar](20) NULL,
	[percentageAllocated] [float] NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NULL,
	[modified] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[depositNextOfKinId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[employeeDepartment]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[employeeDepartment](
	[employeeDepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[dapartmentName] [nvarchar](100) NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NULL,
	[modified] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[employeeDepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[privateCompanyStaffAddress]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[privateCompanyStaffAddress](
	[privateCompanyStaffAddressId] [int] IDENTITY(1,1) NOT NULL,
	[privateCompanyStaffId] [int] NOT NULL,
	[addressTypeId] [int] NOT NULL,
	[cityId] [int] NOT NULL,
	[addressLine] [nvarchar](250) NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NULL,
	[modified] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[privateCompanyStaffAddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[privateCompanyStaffVerification]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[privateCompanyStaffVerification](
	[privateCompanyStaffVerificationId] [int] IDENTITY(1,1) NOT NULL,
	[privateCompanyStaffId] [int] NOT NULL,
	[contactPersonName] [int] NOT NULL,
	[contactPersonPosition] [nvarchar](150) NOT NULL,
	[departmentId] [int] NOT NULL,
	[email] [nvarchar](100) NOT NULL,
	[phone] [nvarchar](15) NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NULL,
	[modified] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[privateCompanyStaffVerificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[relationshipType]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[relationshipType](
	[relationshipTypeId] [int] IDENTITY(1,1) NOT NULL,
	[relationshipTypeName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[relationshipTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[relationshipTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[treasuryBillRate]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[treasuryBillRate](
	[treasuryBillRateId] [int] NOT NULL,
	[treasuryBillRate] [float] NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[treasuryBillRateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[cashierTransactionReceipt]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[cashierTransactionReceipt](
	[cashierTransactionReceiptId] [int] IDENTITY(1,1) NOT NULL,
	[receiptDate] [datetime] NOT NULL,
	[transactionId] [int] NOT NULL,
	[transactionTypeId] [int] NOT NULL,
	[totalReceiptAmount] [float] NOT NULL,
	[cashierTillId] [int] NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NULL,
	[modified] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierTransactionReceiptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[privateCompanyStaff]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[privateCompanyStaff](
	[privateCompanyStaffId] [int] IDENTITY(1,1) NOT NULL,
	[employerId] [int] NOT NULL,
	[employeeNumber] [nvarchar](50) NULL,
	[clientId] [int] NOT NULL,
	[employeeContractTypeId] [int] NOT NULL,
	[employmentStartDate] [date] NULL,
	[socialSecurityNumber] [nvarchar](20) NULL,
	[position] [nvarchar](100) NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NULL,
	[modified] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[privateCompanyStaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[depositUpgrade]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[depositUpgrade](
	[depositUpgradeId] [int] IDENTITY(1,1) NOT NULL,
	[upgradeDate] [datetime] NOT NULL,
	[upgradeDepositTypeID] [int] NOT NULL,
	[previousDepositTypeID] [int] NOT NULL,
	[previousDepositId] [int] NOT NULL,
	[newDepositId] [int] NOT NULL,
	[balanceCD] [float] NOT NULL,
	[topUpAmount] [float] NOT NULL,
	[creator] [nvarchar](100) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](100) NULL,
	[modified] [datetime] NULL,
	[annualInterestRate] [float] NOT NULL,
	[depositPeriodInDays] [int] NOT NULL,
	[maturityDate] [datetime] NOT NULL,
	[topupPaymentModeId] [int] NOT NULL,
	[topupBankId] [int] NULL,
	[topupCheckNo] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[depositUpgradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[currencyNote]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[currencyNote](
	[currencyNoteId] [int] IDENTITY(1,1) NOT NULL,
	[currencyId] [int] NOT NULL,
	[value] [float] NOT NULL,
	[noteName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[currencyNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[depositRateUpgrade]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[depositRateUpgrade](
	[depositRateUpgradeId] [int] IDENTITY(1,1) NOT NULL,
	[depositId] [int] NOT NULL,
	[currentPrincipalBalance] [float] NOT NULL,
	[currentRate] [float] NOT NULL,
	[proposedRate] [float] NOT NULL,
	[approved] [bit] NOT NULL,
	[approvalDate] [datetime] NULL,
	[approvedBy] [nvarchar](30) NULL,
	[rejected] [bit] NULL,
	[rejectedDate] [datetime] NULL,
	[rejectedBy] [nvarchar](30) NULL,
	[created] [datetime] NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[isTreasuryBillUpgrade] [bit] NOT NULL,
	[depositTypePlanRateId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[depositRateUpgradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[savingLien]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[savingLien](
	[savingLienId] [int] IDENTITY(1,1) NOT NULL,
	[savingId] [int] NOT NULL,
	[lienDate] [datetime] NOT NULL,
	[lienAmount] [float] NOT NULL,
	[lienReasonId] [int] NOT NULL,
	[lienHoldingAccountId] [int] NOT NULL,
	[lienPlaceBy] [nvarchar](30) NOT NULL,
	[released] [bit] NOT NULL,
	[releaseDate] [datetime] NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NULL,
	[modified] [datetime] NULL,
	[lienNumber] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[savingLienId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[cashierCashup]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[cashierCashup](
	[cashierCashupId] [int] IDENTITY(1,1) NOT NULL,
	[cashierTillId] [int] NOT NULL,
	[transferTypeId] [int] NOT NULL,
	[bankAccountId] [int] NULL,
	[cashInVaultId] [int] NULL,
	[transferAmount] [float] NOT NULL,
	[cashupDate] [datetime] NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NULL,
	[modified] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierCashupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[cashierFund]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ln].[cashierFund](
	[cashierFundId] [int] IDENTITY(1,1) NOT NULL,
	[cashierTillId] [int] NOT NULL,
	[transferTypeId] [int] NOT NULL,
	[bankAccountId] [int] NULL,
	[cashInVaultId] [int] NULL,
	[transferAmount] [float] NOT NULL,
	[fundDate] [datetime] NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NULL,
	[modified] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierFundId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [hc].[staffOvertime]    Script Date: 5/24/2019 4:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hc].[staffOvertime](
	[staffOvertimeID] [int] IDENTITY(1,1) NOT NULL,
	[staffID] [int] NOT NULL,
	[payCalendarID] [int] NOT NULL,
	[saturdayHours] [float] NOT NULL,
	[sundayHours] [float] NOT NULL,
	[holidayHours] [float] NOT NULL,
	[weekDayHours] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[staffOvertimeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

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


/****** Object:  View [ln].[vwActiveClients]    Script Date: 24-May-19 6:39:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create View [ln].[vwActiveClients] as
Select accountNumber, CONCAT(otherNames,',',' ',surName) as 'Client Name', b.branchName
From ln.client a
Inner Join ln.branch b ON a.branchID = b.branchID
where clientTypeID <> -1;
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

/****** Object:  View [ln].[vwFlaggedClients]    Script Date: 24-May-19 6:50:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create View [ln].[vwFlaggedClients] as
Select accountNumber, CONCAT(otherNames,',',' ',surName) as 'Client Name', b.branchName
From ln.client a
Inner Join ln.branch b ON a.branchID = b.branchID
where clientTypeID = -1;
GO

/****** Object:  Table [ln].[cashierTillConfig]    Script Date: 24-May-19 6:53:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[cashierTillConfig](
	[cashierTillConfigId] [int] IDENTITY(1,1) NOT NULL,
	[opendate] [datetime] NOT NULL,
	[open] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierTillConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [ln].[cashierTransactionReceiptCurrency]    Script Date: 24-May-19 7:08:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[cashierTransactionReceiptCurrency](
	[cashierTransacReceiptCurrencyId] [int] IDENTITY(1,1) NOT NULL,
	[cashierTransactionReceiptId] [int] NOT NULL,
	[currencyNoteId] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[total] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierTransacReceiptCurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ln].[cashierTransactionReceiptCurrency]  WITH CHECK ADD  CONSTRAINT [fk_cashierTransactionReceiptCurrency_cashierTransactionReceipt] FOREIGN KEY([cashierTransactionReceiptId])
REFERENCES [ln].[cashierTransactionReceipt] ([cashierTransactionReceiptId])
GO

ALTER TABLE [ln].[cashierTransactionReceiptCurrency] CHECK CONSTRAINT [fk_cashierTransactionReceiptCurrency_cashierTransactionReceipt]
GO

ALTER TABLE [ln].[cashierTransactionReceiptCurrency]  WITH CHECK ADD  CONSTRAINT [fk_cashierTransactionReceiptCurrency_currencyNote] FOREIGN KEY([currencyNoteId])
REFERENCES [ln].[currencyNote] ([currencyNoteId])
GO

ALTER TABLE [ln].[cashierTransactionReceiptCurrency] CHECK CONSTRAINT [fk_cashierTransactionReceiptCurrency_currencyNote]
GO

/****** Object:  Table [ln].[cashierTransactionWithdrawal]    Script Date: 24-May-19 7:09:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[cashierTransactionWithdrawal](
	[cashierTransactionWithdrawalId] [int] IDENTITY(1,1) NOT NULL,
	[receiptDate] [datetime] NOT NULL,
	[transactionId] [int] NOT NULL,
	[transactionTypeId] [int] NOT NULL,
	[totalReceiptAmount] [float] NOT NULL,
	[cashierTillId] [int] NOT NULL,
	[creator] [nvarchar](30) NOT NULL,
	[created] [datetime] NOT NULL,
	[modifier] [nvarchar](30) NULL,
	[modified] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierTransactionWithdrawalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ln].[cashierTransactionWithdrawal]  WITH CHECK ADD  CONSTRAINT [fk_cashierTransactionWithdrawal_cashierTill] FOREIGN KEY([cashierTillId])
REFERENCES [ln].[cashiersTill] ([cashiersTillID])
GO

ALTER TABLE [ln].[cashierTransactionWithdrawal] CHECK CONSTRAINT [fk_cashierTransactionWithdrawal_cashierTill]
GO

ALTER TABLE [ln].[cashierTransactionWithdrawal]  WITH CHECK ADD  CONSTRAINT [fk_cashierTransactionWithdrawal_creator] FOREIGN KEY([creator])
REFERENCES [ln].[cashiersTill] ([userName])
GO

ALTER TABLE [ln].[cashierTransactionWithdrawal] CHECK CONSTRAINT [fk_cashierTransactionWithdrawal_creator]
GO

ALTER TABLE [ln].[cashierTransactionWithdrawal]  WITH CHECK ADD  CONSTRAINT [fk_cashierTransactionWithdrawal_modifier] FOREIGN KEY([modifier])
REFERENCES [ln].[cashiersTill] ([userName])
GO

ALTER TABLE [ln].[cashierTransactionWithdrawal] CHECK CONSTRAINT [fk_cashierTransactionWithdrawal_modifier]
GO

ALTER TABLE [ln].[cashierTransactionWithdrawal]  WITH CHECK ADD  CONSTRAINT [fk_cashierTransactionWithdrawal_transactionType] FOREIGN KEY([transactionTypeId])
REFERENCES [ln].[transactionType] ([transactionTypeId])
GO

ALTER TABLE [ln].[cashierTransactionWithdrawal] CHECK CONSTRAINT [fk_cashierTransactionWithdrawal_transactionType]
GO


/****** Object:  Table [ln].[cashierTransactionWithdrawalCurrency]    Script Date: 24-May-19 7:11:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [ln].[cashierTransactionWithdrawalCurrency](
	[cashierTransactionWithdrawalCurrencyId] [int] IDENTITY(1,1) NOT NULL,
	[cashierTransactionWithdrawalId] [int] NOT NULL,
	[currencyNoteId] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[total] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cashierTransactionWithdrawalCurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ln].[cashierTransactionWithdrawalCurrency]  WITH CHECK ADD  CONSTRAINT [fk_cashierTransactionWithdrawalCurrency_cashierTransactionWithdrawal] FOREIGN KEY([cashierTransactionWithdrawalId])
REFERENCES [ln].[cashierTransactionWithdrawal] ([cashierTransactionWithdrawalId])
GO

ALTER TABLE [ln].[cashierTransactionWithdrawalCurrency] CHECK CONSTRAINT [fk_cashierTransactionWithdrawalCurrency_cashierTransactionWithdrawal]
GO

ALTER TABLE [ln].[cashierTransactionWithdrawalCurrency]  WITH CHECK ADD  CONSTRAINT [fk_cashierTransactionWithdrawalCurrency_currencyNote] FOREIGN KEY([currencyNoteId])
REFERENCES [ln].[currencyNote] ([currencyNoteId])
GO

ALTER TABLE [ln].[cashierTransactionWithdrawalCurrency] CHECK CONSTRAINT [fk_cashierTransactionWithdrawalCurrency_currencyNote]
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



CREATE proc [ln].[sp_attempt_deposit]
	@savingId int,
	@savingAmount float,
	@reservedBy nvarchar(30),
	@savingDate datetime,
	@bankId int null,
	@checkNo nvarchar(30) null,
	@modeOfPaymentId int,
	@naration nvarchar(100),
	@transactionId nvarchar(60),
	@savingAdditionalId nvarchar(15) out
as
	declare @isAccountLocked bit = 0
	declare @DepositReservation float
	declare @isFundsReserved bit = 1

--begin try
begin
	set transaction isolation level repeatable read
	begin transaction
		
		--Check for reservation
		select @isFundsReserved = [committed] from ln.savingReservationTransc 
												where transactionId = @transactionId 
												and savingID = @savingId 
												and reservedBy = @reservedBy
												and reservationTypeId = 2
												and (DATEDIFF(MINUTE, reservationDate, GETDATE())<2)

		select @isAccountLocked = accountLocked from ln.saving where savingID = @savingId
		--Lock Account and withdraw funds if not locked
		if(@isFundsReserved = 0)
			begin

			declare @principalBal float
			declare @avaialablePrincipalBal float
			declare @amountInvested float

			select @principalBal = principalBalance from ln.saving where savingID = @savingId
			select @avaialablePrincipalBal = availablePrincipalBalance from ln.saving where savingID = @savingId
			select @amountInvested = amountInvested from ln.saving where savingID = @savingId

			--Proceed to deposit funds if lock succeed
			if(@savingAmount > 0)
			begin
					declare @interestBalance float = 0
					select @principalBal = principalBalance from ln.saving where savingID = @savingId	
					select @interestBalance = interestBalance from ln.saving where savingID = @savingId	
								
					--Reserve fund
					update ln.saving set principalBalance += @savingAmount,availablePrincipalBalance += @savingAmount,amountInvested += @savingAmount,
					depositReservation -= @savingAmount
					where savingID = @savingId 

					insert into ln.savingAdditional(savingId,savingDate,savingAmount,principalBalance,interestBalance,creation_date,creator,
							bankID,checkNo,modeOfPaymentID,fxRate,localAmount,lastPrincipalFxGainLoss,posted,naration,closed,balanceBD)
					values(@savingId,@savingDate,@savingAmount,(@principalBal+@savingAmount),@interestBalance,GETDATE(),@reservedBy,@bankId,
						@checkNo,@modeOfPaymentId,0,@savingAmount,0,0,@naration,0,(@principalBal+@interestBalance))

					 select @savingAdditionalId = cast(SCOPE_IDENTITY() as nvarchar(15))	
					 
					 update ln.savingReservationTransc set [committed] = 1, committedDate = GETDATE()
					 where transactionId = @transactionId and savingID = @savingId  and reservedBy = @reservedBy
					 and reservationTypeId = 2 and (DATEDIFF(MINUTE, reservationDate, GETDATE())<2)					 							
			end
			else 
			raiserror ('Deposit amount cannot be zero or less',16,16)
		end
		
	commit transaction
	select @savingAdditionalId
	end
--end try
--begin catch
	--rollback
	--select 'An Error occured' 
--end catch
GO


/****** Object:  StoredProcedure [ln].[sp_attempt_reservation]    Script Date: 27-May-19 11:36:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [ln].[sp_attempt_reservation]
	@savingId int, 
	@amount float,
	@reservationTypeId int,
	@reservedBy nvarchar(30),
	@naration nvarchar(255),
	@transactionId nvarchar(60) out
as
	declare @currentBalance float
	declare @reservedAmount float
	declare @isAccountLocked bit = 0
	declare @lockedDateTime datetime = getDate()

/*begin try*/
	set transaction isolation level repeatable read
	begin transaction

		
		select @isAccountLocked = accountLocked from ln.saving where savingID = @savingId
		--Lock Account and reserve funds if not locked
		if(@isAccountLocked = 0)
			begin

			--Lock Account
			update ln.saving set accountLocked = 1,lockedBy=@reservedBy,lockDate=@lockedDateTime where savingID = @savingId
				and accountLocked=0
			
			--Check if lock succeed
			declare @lockSucceeded bit = 0
			select @lockSucceeded = accountLocked from ln.saving where savingID = @savingId and lockedBy = @reservedBy
			and DATEDIFF(MINUTE, lockDate, @lockedDateTime)<1

			--Proceed to reserve funds if lock succeed
			if(@lockSucceeded = 1)
			begin
				select @currentBalance = sum(interestBalance+principalBalance) from ln.saving where savingID = @savingId
				if(@reservationTypeId = 1) 
				select @reservedAmount = withdrawalReservation from ln.saving where savingID = @savingId
				select @transactionId = NEWID()
				print @transactionId

				--Check if client has enough balance
				if((@reservationTypeId = 1 and @reservedAmount+@amount <= @currentBalance) or @reservationTypeId = 2)
				begin
					select 'Enough balance'
					
					--For savings withdrawal
					if(@reservationTypeId = 1)
					begin
						--Reserve Withdrawal fund
						update ln.saving set withdrawalReservation += @amount where savingID = @savingId and accountLocked = 1 
							and lockedBy = @reservedBy and lockDate = @lockedDateTime
					end
					--For savings deposit
					else if(@reservationTypeId = 2)
					begin
						--Reserve Deposit fund
						update ln.saving set depositReservation += @amount where savingID = @savingId and accountLocked = 1 
							and lockedBy = @reservedBy and lockDate = @lockedDateTime
					end

					insert into ln.savingReservationTransc(savingId,reservationAmount,reservationTypeId,reservationDate,reservedBy,naration,transactionId,[committed])
					values(@savingId,@amount,@reservationTypeId,@lockedDateTime,@reservedBy,@naration,@transactionId,0)

					--Release lock
					update ln.saving set accountLocked = 0,lockedBy=null,lockDate=null where savingID = @savingId and accountLocked = 1 
						and lockedBy = @reservedBy and DATEDIFF(MINUTE, lockDate, @lockedDateTime)<1
				end
				--If balance is less than amount to reserve, release lock and raise error
				else
					
					begin
						update ln.saving set accountLocked = 0, lockedBy=null,lockDate=null where savingID = @savingId and accountLocked = 1 
						and lockedBy = @reservedBy and DATEDIFF(MINUTE, lockDate, @lockedDateTime)<1

					raiserror ('Not enough balance',16,16)
				end
			end
		end
		
	commit transaction
/*end try
begin catch
	rollback
	raiserror ('An Error occured',16,16)
end catch*/
GO



