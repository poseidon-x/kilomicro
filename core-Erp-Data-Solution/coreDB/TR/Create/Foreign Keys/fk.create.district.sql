use coreDB
go

alter table tr.district add
	constraint fk_district_sectors foreign key (sector_id)
	references tr.sector(sector_id) on update cascade 
go
