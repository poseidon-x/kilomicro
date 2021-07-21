use coreDB
go

alter table regions add
	constraint fk_country_region foreign key (country_id)
	references countries(country_id) on delete cascade on update cascade 
go
	