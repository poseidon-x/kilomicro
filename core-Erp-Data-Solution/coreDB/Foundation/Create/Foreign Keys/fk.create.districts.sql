use coreDB
go

alter table districts add
	constraint fk_region_district foreign key (region_id)
	references regions(region_id) on delete cascade on update cascade 
go
	