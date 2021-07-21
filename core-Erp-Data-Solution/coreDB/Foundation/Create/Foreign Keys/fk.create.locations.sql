use coreDB
go

alter table locations add
	constraint fk_city_locations foreign key (city_id)
	references cities(city_id) on delete cascade on update cascade 
go
	