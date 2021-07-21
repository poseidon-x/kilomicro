use coreDB
go

create table ln.borrowingType
(
	borrowingTypeId int identity primary key not null,
	borrowingTypeName nvarchar(50) not null,
	vaultAccountId int not null default(0),
	bankAccountId int not null default(0),
	writeOffAccountId int not null default(0),
	unearnedInterestAccountId int not null default(0),
	interestIncomeAccountId int not null default(0),
	unpaidCommissionAccountId int not null default(0),
	commissionAndFeesAccountId int not null default(0),
	accountsReceivableAccountId int not null default(0),
	unearnedExtraChargesAccountId int not null default(0),
	tagPrefix nvarchar(4) null,
	incentiveAccountId int not null default(0),
	holdingAccountId int not null default(0),
	refundAccountId  int not null default(0), 
	withHoldingAccountId  int not null default(0),
	apIncentiveAccountId  int not null default(0) ,
	apCommissionAccountId  int not null default(0), 
	commissionAccountId  int not null default(0), 
	provisionExpenseAccountId  int not null default(0), 
	provisionsAccountId  int not null default(0) 
)

create table ln.borrowing 
(
	borrowingId int identity primary key not null,
	clientId int not null,
	borrowingTypeId int not null,
	borrowingTenure float not null,
	tenureTypeId int not null,
	borrowingNo nvarchar(50) not null,
	amountRequested float not null,
	amountApproved float not null default(0),
	amountDisbursed int not null default(0),
	applicationDate dateTime not null,
	aprovalDate dateTime null,
	borrowingStatusId int not null,
	interestTypeId int not null,
	interestRate float not null,
	gracePeriod int null,
	repaymentModeId int not null,
	affordabilityRatio float not null default(0),
	applicationFee float not null default(0),
	processingFee float not null default(0),
	disbursedDate dateTime null,
	appliedBy nvarchar(100) not null,
	approvedBy nvarchar(100),
	disbursedBy nvarchar(100),
	commission float not null default(0),
	balance float not null default(0),
	creditOfficerNotes ntext not null default(''),
	lastPenaltyDate datetime null,
	approvalComments ntext not null default(''),
	edited bit not null default(0),
	lastEOD datetime null,
	[version] timestamp not null,
	closed bit not null default(0),
	created dateTime not null,
	creator nvarchar(100) not null,
	modified datetime null,
	modifier nvarchar(100) null
)
go

create table ln.borrowingRepaymentSchedule
(
	borrowingRepaymentScheduleId int identity primary key not null,
	borrowingId int not null,
	repaymentDate datetime not null,
	interestPayment float not null,
	principalPayment float not null,
	interestBalance float not null,
	principalBalance float not null,
	balanceBF float not null,
	balanceCD float not null,
	edited bit not null default(0),
	originalInterestPayment float null,
	originalPrincipalPayment float null,
	created dateTime not null,
	creator nvarchar(100) not null,
	modified datetime null,
	modifier nvarchar(100) null
)

create table ln.borrowingFee(
	borrowingfeeId int identity primary key not null,
	borrowingId int not null,
	feeDate datetime not null,
	feeAmount float not null,
	feeTypeId int not null,
	created dateTime not null,
	creator nvarchar(100) not null,
	modified datetime null,
	modifier nvarchar(100) null
)

create table ln.borrowingFeeType(
	feeTypeId int identity primary key not null,
	feeTypeName nvarchar(50) not null
)

create table ln.borrowingPenalty(
	borrowingPenaltyId int identity primary key not null,
	borrowingId int not null,
	penaltyFee float not null,
	penaltyDate datetime not null,
	penaltyBalance float not null default(0),
	created dateTime not null,
	creator nvarchar(100) not null,
	modified datetime null,
	modifier nvarchar(100) null,
	[version] timestamp not null
)

create table ln.borrowingDocument(
	borrowingDocumentId int identity primary key not null,
	documentId int not null,
	borrowingId int not null,
	[version] timestamp not null
)

create table ln.borrowingRepayment(
	borrowingRepaymentId int identity primary key not null,
	borrowingId int not null,
	modeOfPaymentId int not null,
	repaymentTypeId int not null,
	repayementDate datetime not null,
	amountPaid float not null,
	interestPaid float not null,
	principalPaid float not null,
	feePaid float not null,
	penaltyPaid float not null,
	commissionPaid float not null default(0),
	checkNo nvarchar(50) null,
	bankId int null,
	created datetime not null,
	creator nvarchar(100) not null,
	modified datetime null,
	modifier nvarchar(100) null,
)

create table ln.borrowingDisbursement(
	borrowingDisbursementId int identity primary key not null,
	borrowingId int null,
	dateDisbursed dateTime not null,
	amountDisbursed float not null,
	modeOfPaymentId int null,
	bankId int null,
	chequeNumber nvarchar(50) null
)
