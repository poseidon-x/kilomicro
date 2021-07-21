use coreDB
go


create table ln.[image]
(
	imageID int identity (1,1) primary key, 
	[description] nvarchar(100), 
	[image] image
)
go

alter table ln.[image] add
	content_type nvarchar(100)
