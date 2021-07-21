use coreDB
go

alter table ln.specialDay
	add constraint fk_specialDay_specialDayType foreign key(specialDayTypeId)
	references ln.specialDayType(specialDayTypeId)
go