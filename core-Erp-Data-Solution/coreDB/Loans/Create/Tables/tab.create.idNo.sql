use coreDB
go

create table ln.idNoType
(
	idNoTypeID int primary key,
	idNoTypeName nvarchar(50) not null
)
go

create table ln.idNo
(
	idNoID int identity(1,1) primary key,
	idNoTypeID int not null,
	idNo nvarchar(50) not null,
	expriryDate datetime
)
go

alter table ln.idNo add
	sortCode int not null constraint df_idNo_sortCode default(1)
go

alter table ln.idNoType add
	isNational bit not null constraint df_idNoType_isNational default(1)
go
