CREATE TABLE ln.currencyNoteType(
    currencyNoteTypeId int primary key not null,
    noteTypeName nvarchar(100) not null,
    currencyNoteId int not null references ln.currencyNote(currencyNoteId)
);

CREATE TABLE ln.cashierTillConfigDetail(
    cashierTillConfigDetailId int primary key not null,
    cashierTillConfigId int not null references ln.cashierTillConfig(cashierTillConfigId),
    errorDate datetime not null,
    errorDetail nvarchar(100) not null
);

CREATE TABLE ln.depositAuthorization(
    depositAuthorizationId int not null primary key,
    authorizationDate datetime not null,
    authorizeInterest float not null,
    authorizeInterestBalance float not null,
    authorizePrincipal float not null,
    authorizePrincipalBalance float not null,
    created datetime not null,
    creator nvarchar(100) not null,
    depositId int not null references ln.deposit(depositID),
    modified datetime null,
    modifier nvarchar(100)
);

CREATE TABLE ln.depositSystemDateChange(
    depositSystemDateChangeId int not null primary key,
    changeFrom date null,
    changeTo date null,
    created datetime null,
    creator nvarchar(100),
    systemDateID tinyint not null references ln.systemDate(systemDateID)
);

CREATE TABLE ln.accountsSystemDateChange(
    accountsSystemDateChangeId int not null primary key,
    changeFrom date null,
    changeTo date null,
    created datetime null,
    creator nvarchar(100),
    systemDateID tinyint not null references ln.systemDate(systemDateID)
);

CREATE TABLE ln.creditUnionSystemDateChange(
    creditUnionSystemDateChangeId int not null primary key,
    changeFrom date null,
    changeTo date null,
    created datetime null,
    creator nvarchar(100),
    systemDateID tinyint not null references ln.systemDate(systemDateID)
);

CREATE TABLE ln.savingSystemDateChange(
    savingSystemDateChangeId int not null primary key,
    changeFrom date null,
    changeTo date null,
    created datetime null,
    creator nvarchar(100),
    systemDateID tinyint not null references ln.systemDate(systemDateID)
);

CREATE TABLE ln.susuSystemDateChange(
    susuSystemDateChangeId int not null primary key,
    changeFrom date null,
    changeTo date null,
    created datetime null,
    creator nvarchar(100),
    systemDateID tinyint not null references ln.systemDate(systemDateID)
);

CREATE TABLE ln.investmentSystemDateChange(
    investmentSystemDateChangeId int not null primary key,
    changeFrom date null,
    changeTo date null,
    created datetime null,
    creator nvarchar(100),
    systemDateID tinyint not null references ln.systemDate(systemDateID)
);

CREATE TABLE ln.loanSystemDateChange(
    loanSystemDateChangeId int not null primary key,
    changeFrom date null,
    changeTo date null,
    created datetime null,
    creator nvarchar(100),
    systemDateID tinyint not null references ln.systemDate(systemDateID)
);


/****** Object:  StoredProcedure [ln].[sp_attempt_deposit]    Script Date: 28-May-19 4:50:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE proc [ln].[sp_withdraw_fund]
	@savingId int,
	@interestWithdrawal float,
	@principalWithdrawal float,
	@reservedBy nvarchar(60),
	@withdrawalDate datetime,
	@bankId int null,
	@checkNo nvarchar(30) null,
	@modeOfPaymentId int,
	@naration nvarchar(100),
	@transactionId nvarchar(30)
	
as

--begin try
begin
	set transaction isolation level repeatable read
	begin transaction
		
		
	end
--end try
--begin catch
	--rollback
	--select 'An Error occured' 
--end catch
GO

