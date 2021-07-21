use coreDB
go

create table ln.loanAdditionalInfo
(
	loanAdditionalInfoId int identity (1,1) primary key,
	loanId int not null
)

create table ln.loanMetaData
(
	loanMetaDataId int identity (1,1) primary key,
	loanAdditionalInfoId  int not null,
	metaDataTypeId int not null,
	content nvarchar(100) not null
)

create table ln.metaDataType
(
	metaDataTypeId int identity (1,1) primary key, 
	name nvarchar(50) not null, 
	nameCode nvarchar(50) not null,
	dataType nvarchar(50) not null
)
go