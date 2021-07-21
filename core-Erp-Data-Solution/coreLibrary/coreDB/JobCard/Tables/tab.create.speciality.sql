use coreDB
go

create table jc.speciality
(
	specialityId tinyInt identity (1,1) primary key,
	specialityName nvarchar(40) not null,
	specialityCategoryId tinyint not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null
)


create table jc.specialityCategory
(
	specialityCategoryId tinyInt identity (1,1) primary key,
	specialityCategoryName nvarchar(40) not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null,
)