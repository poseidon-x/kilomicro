use coreDB
go

create table ar.creditMemoReason
(
	creditMemoReasonID int identity (1,1) not null primary key,
	description nvarchar(250) not null constraint uk_creditMemoReason unique
)
go
