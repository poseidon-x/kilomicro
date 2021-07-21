use coreDB
go

create table ln.invoiceLoan
(
	invoiceLoanID int identity(1,1) primary key,
	clientID int not null,
	invoiceAmount float not null default(0),
	invoiceDescription nvarchar(1000) not null default(''),
	withHoldingTax float not null default(0),
	ceilRate float not null default(0),
	proposedAmount float not null default(0),
	amountDisbursed float not null default(0),
	approvedBy nvarchar(50) not null default(''),
	approvalDate datetime,
	invoiceDate datetime not null default(getdate()),
	approved bit not null default(0),
	disbursed bit not null default(0),
	repaymentDate datetime,
	checkNo nvarchar(50),
	bankID int,
	interestAmount float, 
	processingFee float,
	checkAmount float
)
go

alter table ln.invoiceLoan add
	amountApproved float not null default(0)
go

alter table ln.invoiceLoan add
	supplierID int
go

alter table ln.invoiceLoan add
	invoiceNo nvarchar(50)
go
 
alter table ln.invoiceLoan add
	rate float not null default(6)
go
 
alter table ln.invoiceLoan add
	addFee bit not null default(0)
go
 
create table ln.invoiceLoanMaster
(
	invoiceLoanMasterID int identity (1,1) not null primary key,
	clientID int not null,
	ceilRate float not null default(0),
	approved bit not null default(0),
	disbursed bit not null default(0),
	approvedBy nvarchar(50) not null default(''),
	approvalDate datetime,
	supplierID int
)
go

alter table ln.invoiceLoan add
	invoiceLoanMasterID int null
go

create table ln.invoiceLoanConfig
(
	invoiceLoanConfigID int identity (1,1) not null primary key,
	clientID int not null,
	supplierID int,
	ceilRate float not null default(0),
	standardInterestrate float not null,
	standardProcessingFeerate float not null,
	constraint uk_invoiceLoanConfig unique (supplierID, clientID)
)
go

alter table ln.invoiceLoanMaster add
	invoiceDate datetime not null default(getdate())
go

alter table ln.invoiceLoan add
	disbursementType nvarchar(3) null,
	poNumber nvarchar(30) null,
	amountOrdered float null
go

alter table ln.invoiceLoanConfig add
	allowPODisbursement bit not null default(0)
go

alter table ln.invoiceLoanConfig add
	maximumExposure float not null default(0)
go