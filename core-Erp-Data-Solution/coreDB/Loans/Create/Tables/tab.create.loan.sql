use coreDB
go

create table ln.loan
(
	loanID int identity(1,1) primary key,
	clientID int not null,
	loanNo nvarchar(50) not null unique,
	amountRequested float not null,
	amountApproved float not null default(0),
	amountDisbursed float not null default(0),
	applicationDate datetime not null,
	finalApprovalDate datetime,
	loanStatusID int not null,
	interestRate float not null default(0),
	interestTypeID int,
	gracePeriod int,
	repaymentModeID int not null,
	affordabilityRatio float not null default(0),
	applicationFee float not null default(0),
	processingFee float not null default(0),
	loanTypeID int not null
)
go

create table ln.loanStatus
(
	loanStatusID int not null primary key,
	loanStatusName nvarchar(100) not null unique
)
go

create table ln.repaymentMode
(
	repaymentModeID int not null primary key,
	repaymentModeName nvarchar(100) not null unique
)
go
	
create table ln.interestType
(
	interestTypeID int not null primary key,
	interestTypeName nvarchar(100) not null unique
)
go
	
create table ln.loanGurantor
(
	loanGurantorID int identity(1,1) primary key,
	loanID int not null,
	surName nvarchar(50) not null,
	otherNames nvarchar(50) not null,
	DOB datetime,
	idNoID int,
	addressID int,
	phoneID int,
	emailID int,
	imageID int,
	creditOfficerNotes ntext,
	creditCommitteeNotes ntext,
	loanUpdateNotes ntext
)
go	

create table ln.loanCollateral
(
	loanCollateralID int identity(1,1) primary key,
	loanID int not null, 
	collateralTypeID int not null,
	fairValue float not null default(0)
)
go


create table ln.collateralImage
(
	collateralImageID int identity(1,1) primary key,
	loanCollateralID int not null, 
	imageID int not null
)
go

create table ln.collateralType
(
	collateralTypeID int not null primary key,
	collateralTypeName nvarchar(100) not null unique
)
go

create table ln.loanTranch
(
	loanTranchID int identity(1,1) primary key,
	loanID int not null,  
	amountDisbursed float not null default(0),
	disbursementDate datetime not null,
	modeOfPaymentID int not null
)
go

create table ln.repaymentSchedule
(
	repaymentScheduleID int identity(1,1) primary key,
	loanID int not null,  
	repaymentDate datetime not null,
	interestPayment float not null default(0),
	principalPayment float not null default(0),
	balanceBF float not null default(0),
	balanceCD float not null default(0) ,
	interestBalance float not null,
	principalBalance float not null  
)
go

create table ln.loanPenalty
(
	loanPenaltyID int identity(1,1) primary key,
	loanID int not null,  
	penaltyDate datetime not null,
	penaltyFee float not null default(0)
)
go

create table ln.loanRepayment
(
	loanRepaymentID int identity(1,1) primary key,
	loanID int not null,
	modeOfPaymentID int not null,
	repaymentTypeID int not null,  
	repaymentDate datetime not null,
	amountPaid float not null default(0),
	interestPaid float not null default(0),
	principalPaid float not null default(0),
	feePaid float not null default(0),
	penaltyPaid float not null default(0)
)
go

create table ln.repaymentType
(
	repaymentTypeID int not null primary key,
	repaymentTypeName nvarchar(100) not null unique
)
go
	
create table ln.loanType
(
	loanTypeID int not null primary key,
	loanTypeName nvarchar(100) not null unique
)
go
	
create table ln.modeOfPayment
(
	modeOfPaymentID int not null primary key,
	modeOfPaymentName nvarchar(100) not null unique
)
go
	

alter table ln.loan add
	disbursementDate datetime,
	loanTenure float not null,
	originalLoantenure float not null default(0),
	originalAmountDisbursed float not null default(0),
	originalInterestRate int not null default(0)
	go

alter table ln.loan add 
	tenureTypeID int not null
go
		
create table ln.tenureType
(
	tenureTypeID int not null primary key,
	tenureTypeName nvarchar(100) not null unique
)
go
 
alter table ln.loan add 
	commission float not null default(0)
go
	
alter table ln.loan add 
	balance float not null default(0)
go
	

alter table ln.loanRepayment add 
	commission_paid float not null default(0)
go

alter table ln.loanPenalty
	add penaltyBalance float not null default(0)
go

alter table ln.repaymentSchedule
	add proposedInterestWriteOff float not null default(0),
	interestWritenOff float not null default(0)
go
	
alter table ln.loan add 
	applicationFeeBalance float not null default(0),
	processingFeeBalance float not null default(0),
	commissionBalance float not null default(0)
go

create table ln.categoryCheckList
(
	categoryCheckListID int identity(1,1) primary key,
	categoryID int not null,
	[description] nvarchar(1000) not null 
)
go

create table ln.loanCheckList
(
	loanCheckListID int identity(1,1) primary key,
	categoryCheckListID int not null,
	loanID int not null,
	passed bit not null default(0)
)
go
	
alter table ln.loan add
	creditOfficerNotes ntext not null default('')
go
	
alter table ln.loan add
	approvalComments ntext not null default('')
go
create table ln.loanFinancial
(
	loanFinancialID int identity(1,1) primary key,
	loanID int not null,
	financialTypeID int not null,
	revenue float not null default(0),
	expenses float not null default(0)
)
go
			
create table ln.financialType
(
	financialTypeID int not null primary key,
	financialTypeName nvarchar(100) not null unique
)
go

alter table ln.loanRepayment add
	bankID int,
	bankName nvarchar(500),
	checkNo nvarchar(50)
go
 
alter table ln.loanType
	add vaultAccountID int not null default(0),
	bankAccountID int not null default(0),
	writeOffAccountID int not null default(0),
	unearnedInterestAccountID int not null default(0),
	interestIncomeAccountID int not null default(0),
	unpaidCommissionAccountID int not null default(0),
	commissionAndFeesAccountID int not null default(0),
	accountsReceivableAccountID int not null default(0),
	unearnedExtraChargesAccountID int not null default(0)
go

alter table ln.loanType add
	defaultInterestRate float,
	defaultPenaltyRate float ,
	defaultGracePeriod int ,
	defaultRepaymentModeID int, 
	defaultApplicationFeeRate float ,
	defaultProcessingFeeRate float ,
	defaultCommissionRate float 
go

alter table ln.loan add
	lastPenaltyDate datetime
go

alter table ln.loanPenalty add
	proposedAmount float
go

create table ln.loanCheck
(
	loanCheckID int identity(1,1) primary key,
	loanID int not null,
	checkNumber nvarchar(50) not null,
	bankID int,
	checkAmount float not null,
	checkDate datetime not null,
	cashed bit not null default(0),
	cashDate datetime
)
go

alter table ln.loanTranch add
	checkNumber nvarchar(50),
	bankID int
go

alter table ln.loanType drop column
	defaultInterestRate
go

alter table ln.loanType drop column
	defaultInterestRate
go

alter table ln.loanType drop column
	defaultPenaltyRate
go

alter table ln.loanType drop column
	defaultGracePeriod
go

alter table ln.loanType drop column
	defaultRepaymentModeID
go

alter table ln.loanType drop column
	defaultApplicationFeeRate
go

alter table ln.loanType drop column
	defaultProcessingFeeRate
go

alter table ln.loanType drop column
	defaultCommissionRate
go

create table ln.tenor 
(
	tenorid int identity(1,1) primary key,
	tenor int not null,
	defaultInterestRate float,
	defaultPenaltyRate float ,
	defaultGracePeriod int ,
	defaultRepaymentModeID int, 
	defaultApplicationFeeRate float ,
	defaultProcessingFeeRate float ,
	defaultCommissionRate float 
)
go

alter table ln.loanCollateral add
	legalOwner nvarchar(100),
	collateralDescription ntext
go

alter table ln.loanFinancial add
	otherCosts float not null default(0),
	frequencyID int 
go

alter table ln.loanCheckList add
	comments ntext
go

alter table ln.loanCheckList add
	[description] nvarchar(500)
go

create table ln.genericCheckList
(
	genericCheckListID int identity(1,1) primary key,
	[description] nvarchar(1000) not null 
)
go

alter table ln.loanCheckList alter column
	categoryCheckListID int null
go

alter table ln.loan add
	addFeesToPrincipal bit not null default(0)
go

alter table ln.loan add
	invoiceNo nvarchar(50)
go

alter table ln.loan alter column 
	loanTenure float not null  
go


alter table ln.repaymentSchedule add 
	edited bit not null default(0)  
go

alter table ln.loan add 
	edited bit not null default(0)  
go

alter table ln.loan add 
	staffID int  null 
go

alter table ln.loan add 
	agentID int  null 
go

alter table ln.loanCheckList add
	creationDate datetime 
go

alter table ln.loanType add
	tagPrefix nvarchar(4) null
go

create table ln.loanFee
(
	loanFeeID int identity(1,1) not null primary key,
	loanID int not null,
	feeDate datetime not null,
	feeAmount float not null check(feeAmount>0),
	feeTypeID int not null default(1)
)
go

create table ln.loanFeeType
(
	feeTypeID int not null primary key,
	feeTypeName nvarchar(250) not null unique
)
go
 
create table ln.loanIterestWriteOff
(
	loanIterestWriteOffID int identity(1,1) not null primary key,
	loanID int not null,
	writeOffDate datetime not null,
	writeOffAmount float not null check(writeOffAmount>0)
)
go

alter table ln.loanIterestWriteOff add
  creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
go

alter table ln.loanFee add
  creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
go

create table ln.loanProduct
(
	loanProductID int identity(1,1) primary key,
	loanProductName nvarchar(50) not null unique,
	loanTenure float not null check(loanTenure > 0),
	rate float not null,
	minAge float not null default (18),
	maxAge float not null default (59)
)
go

create table ln.loanPurpose
(
	loanPurposeID int identity(1,1) primary key,
	loanPurposeName nvarchar(50) not null unique,
	accepted bit not null default(0)
)
go

create table ln.loanPurposeDetail
(
	loanPurposeDetailID int identity(1,1) primary key,
	loanPurposeID int not null,
	loanPurposeDetailName nvarchar(50) not null,
	accepted bit not null default(1)
)
go

create table ln.loanProductHistory
(
	loanProductHistoryID int identity(1,1) primary key,
	loanProductID int,
	loanProductName nvarchar(50),
	loanTenure float not null default(0),
	rate float not null,
	archiveDate datetime,
	minAge float not null default (18),
	maxAge float not null default (59)
)
go

 
create table ln.prLoanDetail
(
	loanDetailID int not null identity(1,1) primary key,
	loanID int not null,
	authorityNoteNumber nvarchar(50),
	loanAdvanceNumber nvarchar(50),
	grossSalary float not null default(0),
	netSalary float not null default(0),
	socialSecWelfare float not null default(0),
	tax float not null default(0),
	totalDeductions float not null default(0),
	loanDeductionsNotOnPr float not null default(0),
	amd float not null default(0),
	amdPreAuditVerified bit not null default(0),
	amdPreAuditVerifiedBy nvarchar(50) null,
	loanProductID int null,
	loanPurposeID int null,
	loanPurposeDetailID int null,
	loanPurposeDescription nvarchar(250) null
)
go

create table ln.prAllowanceType
(
	allowanceTypeID int identity(1,1) primary key not null,
	allowanceTypeName nvarchar(100) not null unique,
	isPermanent bit not null default(0)
)
go

create table ln.prAllowance
(
	prAllowanceID int identity(1,1) primary key,
	loanDetailID int not null,
	allowanceTypeID int not null,
	amount float not null default(0)
)
go

alter table ln.categoryCheckList add
	isMandatory bit not null default(0)
go

alter table ln.prLoanDetail add
	usedAMD float not null default(0)
go

alter table ln.loanProduct add
	procFeeRate float not null default(0)
go

alter table ln.prLoanDetail add
	basicSalary float not null default(0)
go

alter table ln.prLoanDetail add
	isReported bit not null default(0),
	isSentToController bit not null default(0)
go

create table ln.modeOfEntry
(
	modeOfEntryID int identity(1,1) not null primary key,
	modeOfEntryName nvarchar(200) not null unique
)
go

alter table ln.prLoanDetail add
	modeOfEntryID int null
go

alter table ln.prLoanDetail add
	incentivePaid bit not null default(0)
go

create table ln.incentiveStructure
(
	incentiveStructureID int identity(1,1) primary key not null,
	lowerLimit float not null,
	upperLimit float not null,
	incentiveAmount float not null
)
go

create table ln.loanIncentive
(
	loanIncentiveID int identity(1,1) not null primary key,
	loanID int not null,
	agentID int not null,
	loanAmount float not null,
	incentiveAmount float not null,
	posted bit not null default(0)
)
go

alter table ln.loanIncentive add
	incetiveDate datetime,
	postedDate datetime
go

alter table ln.loanIncentive add
	modeOfPaymentID int,
	bankID int,
	checkNo nvarchar(50)
go


alter table ln.loanIncentive add
	creation_date datetime,
	creator nvarchar(50)
go

alter table ln.loanIncentive add
	approved bit not null default(0) 
go
 
alter table ln.loanType
	add incentiveAccountID int not null default(0) 
go

alter table ln.loanType
	add holdingAccountID int not null default(0) 
go


alter table ln.loanType
	add refundAccountID int not null default(0) 
go

alter table ln.loanType
	add withHoldingAccountID int not null default(0) 
go

alter table ln.loanIncentive add
	withHoldingAmount float not null default(0),
	commissionAmount float not null default(0)
go

alter table ln.incentiveStructure add
	withHoldingRate float not null default(0),
	commissionRate float not null default(0)
go

alter table ln.loanType
	add apIncentiveAccountID int not null default(0) ,
	 apCommissionAccountID int not null default(0),
	 commissionAccountID int not null default(0)  
go

alter table ln.loanIncentive add
	paid bit not null default(0),
	paidDate datetime
go

alter table ln.loanType	add 
	 provisionExpenseAccountID int not null default(0) ,
	 provisionsAccountID int not null default(0) 
go

alter table ln.loanIncentive add
	commPaid bit not null default(0),
	commPaidDate datetime
go

alter table ln.loanIncentive add
	commPosted bit not null default(0)
go

alter table ln.loanIncentive add
	commPostedDate datetime  null 
go

alter table ln.loanIncentive add
	netCommission float not  null default(0)
go

alter table ln.loanCheck alter column
	loanID int null
go

alter table ln.loanCheck alter column
	checkDate datetime null
go

alter table ln.loanCheck add
	clientID int null
go

create table ln.insuranceSetup
(
	insuranceSetupID int identity(1,1) not null primary key,
	loanTypeID int not null,
	insurancePercent float not null check(insurancePercent between 0 and 100),
	insuranceAccountID int,
	isEnabled bit not null default(1)
)
go

create table ln.loanInsurance
(
	loanInsuranceID int identity(1,1) not null primary key,
	loanID int not null,
	amount float not null,
	insuranceDate datetime not null default(getdate()),
	paid bit not null default(0),
	paidDate datetime
)
go

alter table ln.loan add
	insuranceAmount float not null default(0)
go

alter table ln.tenor add
	loanTypeID int null
go

alter table ln.loan add
	creditManagerNotes ntext null
go

create table ln.checkType
(
	checkTypeID int identity (1,1) primary key,
	checkTypeName nvarchar(400) not null
)
go

alter table ln.loanCheck add
	checkTypeID int 
go

alter table ln.loanCheck add
	balance float not null default(0)
go

alter table ln.loanCheck add 
	sourceBankID int null
go

alter table ln.loan add
	loanSchemeId int
go

alter table ln.loanPenalty add
	penaltyTypeId int null
go

create table ln.penaltyType
(
	penaltyTypeId int primary key not null,
	penaltyTypeName nvarchar(100) not null unique,
	narration nvarchar(255) not null unique
)
go

insert into ln.penaltyType (penaltyTypeId, penaltyTypeName, narration) values (2, 'Bank Charges for Returned Cheques', 'Bank Charges for Returned Cheques')
insert into ln.penaltyType (penaltyTypeId, penaltyTypeName, narration) values (1, 'Additional Interest for Overdue Repayment', 'Additional Interest for Overdue Repayment')
go

alter table ln.loan add
	closed bit not null default(0)
go

create table ln.loanClosure
(
	loanClosureId int identity(1,1) primary key,
	loanId int not null,
	closureDate datetime,
	requestedClosureDate datetime,
	approved bit null,
	approvalComments nvarchar(400),
	posted bit not null,
	approvalDate datetime,
	approvedBy nvarchar(30),
	requestedBy nvarchar(30) not null,
	postingDate datetime,
	postedBy nvarchar(30),
	loanClosureReasonId int not null,
	closureComments nvarchar(400) not null,
	principalBalanceAtClosure float not null,
	interestBalanceAtClosure float not null,
	feesAndChargesAtClosure float not null
)
go

create table ln.loanClosureReason
(
	loanClosureReasonId int not null identity(1,1) primary key,
	loanClosureReasonName nvarchar(100) not null
)
go







create table ln.loanGroup
(
	loanGroupId int not null identity primary key,
	loanGroupNumber nvarchar(20) not null unique,
	loanGroupName nvarchar(100) not null unique,
    loanGroupDayId int not null,
	relationsOfficerStaffId int not null,
	leaderClientId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) null,
	modified datetime null
)
go

create table ln.loanGroupClient
(
	loanGroupClientId int not null identity primary key,
	loanGroupId int not null,
	clientId int not null unique,
	creator nvarchar(50) not null,
	created datetime not null
)
go

create table ln.loanGroupDay
(
	loanGroupDayId int not null primary key,
	[dayName] nvarchar(20) not null
)

insert into ln.loanGroupDay(loanGroupDayId,[dayName])
values	(1,'Monday'),
		(2,'Tuesday'),
		(3,'Wednesday'),
		(4,'Thursday'),
		(5,'Friday')

create table ln.config
(
	configId int not null identity primary key,
	postInterestUnIntOnDisb bit not null default(1)
)

insert into ln.config(postInterestUnIntOnDisb)
	values(0)


create table ln.loanApproval
(
	loanApprovalId int identity(1,1) primary key,
	loanId int not null,
	amountApproved float not null,
	approvalDate datetime not null,
	approvalAction char not null,
	approvedBy nvarchar(60) not null,
	approvalComment nvarchar(200) not null default(''),
	approvalStageId int not null,
	created datetime not null
)

create table ln.loanApprovalStage
(
	loanApprovalStageId int identity(1,1) primary key,
	loanTypeId int not null,
	name nvarchar(100) not null,
	isMandatory bit not null,
	ordinal char not null,
)

create table ln.loanApprovalStageOfficer
(
	loanApprovalStageOfficerId int identity(1,1) primary key,
	loanApprovalStageId int not null,
	profileType char not null,
	profileValue nvarchar(100) not null
)

alter table ln.loanPenalty add
	modeOfPaymentId int null
go





