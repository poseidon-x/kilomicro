use coreDB
go

alter table cities add
	constraint fk_district_cities foreign key (district_id)
	references districts(district_id) on delete cascade on update cascade 
go
	