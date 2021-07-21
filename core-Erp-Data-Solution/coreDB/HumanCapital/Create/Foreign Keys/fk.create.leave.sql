use coreDB
go

alter table hc.staffLeave 
	add constraint fk_staffLeave_staff foreign key (staffID)
	references fa.staff(staffID)
go

alter table hc.staffLeave 
	add constraint fk_staffLeave_leaveType foreign key (leaveTypeID)
	references hc.leaveType(leaveTypeID)
go

alter table hc.publicHoliday 
	add constraint fk_publicHoliday_holidayType foreign key (holidayTypeID)
	references hc.holidayType(holidayTypeID)
go

alter table hc.publicHoliday 
	add constraint fk_publicHoliday_year foreign key (yearID)
	references hc.[year](yearID)
go

alter table hc.staffLeaveBalance
	add constraint fk_staffLeaveBalance_staff foreign key (staffID)
	references fa.staff(staffID)
go

alter table hc.staffLeaveBalance
	add constraint fk_staffLeaveBalance_year foreign key (yearID)
	references hc.[year](yearID)
go