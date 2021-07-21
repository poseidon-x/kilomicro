use coreDB
go

create schema lic
go

create table lic.[kerberos]
(
	id bit not null default(0) primary key,
	kerbContent ntext not null
)
go
