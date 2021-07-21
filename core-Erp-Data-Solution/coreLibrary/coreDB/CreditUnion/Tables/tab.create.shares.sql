use coreDB
go

create table cu.creditUnionMember
(
	creditUnionMemberID bigint not null identity(1,1) primary key,
	clientID int not null,
	joinedDate datetime not null default(getDate()),
	sharesBalance float not null default(0),
	creditUnionChapterID int not null
)
go

create table  cu.creditUnionShareTransaction
(
	creditUnionShareTransactionID bigint not null identity(1,1) primary key,
	creditUnionMemberID bigint not null,
	transactionDate datetime not null,
	transactionType nchar(1) not null check(transactionType in ('O', 'D','C')),
	modeOfPaymentID int not null,
	checkNumber nvarchar(20) null,
	bankID int null,
	numberOfShares float not null,
	sharePrice float not null,
	posted bit not null default(0),
	postingDate datetime null,
	enteredBy nvarchar(30) not null,
	postedBy nvarchar(30) null,
	entryDate DateTime not null default(getDate())
)
go

create table cu.creditUnionChapter
(
	creditUnionChapterID int not null identity (1,1) primary key,
	chapterName nvarchar(255) not null unique,
	dateFormed datetime not null,
	town nvarchar(100) not null,
	docRegistrationNumber nvarchar(30) not null,
	[postalAddress] nvarchar(255) not null,
	emailAddress nvarchar(255) null,
	telePhoneNumber nvarchar(50) null,
	pricePerShare float not null,
	membersEquityAccountID int not null,
	vaultAccountID int not null,
	dividendsExpenseAccountID int not null
)
go
