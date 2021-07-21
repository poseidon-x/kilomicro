use coreDB
go

create table ln.SpecialDayType
(
	specialDayTypeId smallint not null primary key,
	specialDayTypeName nvarchar(60) not null,
	specialDayTypeCode nvarchar(10) not null
)

create table ln.SpecialDay
(
	specialDayId smallint not null identity primary key,
	specialDayName nvarchar(60) not null,
	specialDayTypeId smallint not null,
	specialDayValue nvarchar(15) not null
)
GO

insert into ln.SpecialDayType(specialDayTypeId,specialDayTypeName,specialDayTypeCode) 
	values(1,'Week End','WE'),
		  (2,'Fixed Public Holiday','PH'),
		  (3,'Non Fixed Public Holiday','WE')	
go

insert into ln.SpecialDay(specialDayName,specialDayTypeId,specialDayValue) 
	values('Saturday',1,'6'),
		  ('Sunday',1,'0'),
		  ('New Year Day',2,'01-Jan'),
		  ('Independence Day',2,'06-Mar'),  
		  ('Christmas Day',2,'25-Dec'),
		  ('Boxing (Proclamation) Day',2,'26-Dec'),
		  ('Republic Day',2,'01-Jul')	
go



