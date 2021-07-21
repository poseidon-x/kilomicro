use coreDB
go

create table dbo.authToken
(
	authTokenID bigint identity(1,1) not null primary key,
	userName nvarchar(30) not null,
	token nvarchar(128) not null unique,
	grantedDate datetime not null,
	expiryDate datetime not null
)
go

alter table dbo.authToken add
	constraint fk_authToken_users foreign key (userName)
	references dbo.users(user_name)
go

create index ix_authToken_getToken on dbo.authToken
(
	userName asc,
	expiryDate desc
)
go

alter table dbo.authToken add
	clientHostName nvarchar(50) not null default('')
go

drop index  ix_authToken_getToken on dbo.authToken
go

create index ix_authToken_getToken on dbo.authToken
(
	userName asc,
	expiryDate desc,
	clientHostName asc
)
go
