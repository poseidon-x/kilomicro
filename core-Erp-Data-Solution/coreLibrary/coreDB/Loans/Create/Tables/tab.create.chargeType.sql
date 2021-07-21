USE [coreDB]
GO

drop table ln.chargeTypeTier
go

CREATE TABLE [ln].[chargeTypeTier](
	[chargeTypeTierId] [int] IDENTITY(1,1) NOT NULL,
	[chargeTypeId] [int] NOT NULL,
	percentCharge float NOT NULL constraint ck_chargeTypeTier_percentCharge check(percentCharge between 0 and 100),
	[minChargeAmount] [float] NOT NULL default (0),
	[minimumTransactionAmount] [float] NOT NULL,
	[maximumTransactionAmount] [float] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[chargeTypeTierId] ASC
	)
) 

GO
 
ALTER TABLE [ln].[chargeTypeTier]  WITH CHECK ADD  CONSTRAINT [fk_chargeTypeTier_chargeType] FOREIGN KEY([chargeTypeId])
REFERENCES [ln].[chargeType] ([chargeTypeID])
GO

ALTER TABLE [ln].[chargeTypeTier] CHECK CONSTRAINT [fk_chargeTypeTier_chargeType]
GO

ALTER TABLE [ln].[chargeTypeTier] add
	maturityPercentCharge float NOT NULL constraint ck_chargeTypeTier_maturityPercentCharge check(maturityPercentCharge between 0 and 100)
go

ALTER TABLE [ln].[chargeType] add
	accountsReceivableAccountID int   
go

