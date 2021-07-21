use coreDB
go

alter table iv.locationType add 
	constraint fk_locationType_locationType foreign key (parentLocationTypeID)
	references iv.locationType (locationTypeID)
go

alter table iv.location add
	constraint fk_location_locationType foreign key (locationTypeID)
	references iv.locationType (locationTypeID)
go
