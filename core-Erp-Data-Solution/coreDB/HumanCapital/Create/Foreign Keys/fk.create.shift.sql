--use coreDB
--go

alter table hc.shiftAllowance
	add constraint fk_shiftAllowance_shift foreign key (shiftID)
	references hc.[shift] (shiftID)
go

alter table hc.shiftAllowance
	add constraint fk_shiftAllowance_allowanceType foreign key (allowanceTypeID)
	references hc.[allowanceType] (allowanceTypeID)
go

alter table hc.staffShift
	add constraint fk_staffShift_shift foreign key (shiftID)
	references hc.[shift] (shiftID)
go

alter table hc.staffShift
	add constraint fk_staffShift_year foreign key (yearID)
	references hc.[year] (yearID)
go

alter table hc.staffShift
	add constraint fk_staffShift_staff foreign key (staffID)
	references [fa].[staff] (staffID)
go

alter table hc.staffAttendance
	add constraint fk_staffAttendance_year foreign key (yearID)
	references hc.[year] (yearID)
go

alter table hc.staffAttendance
	add constraint fk_staffAttendance_month foreign key (monthID)
	references hc.[month] (monthID)
go

alter table hc.staffAttendance
	add constraint fk_staffAttendance_staff foreign key (staffID)
	references [fa].[staff] (staffID)
go

alter table hc.staffDaysWorked
	add constraint fk_staffDaysWorked_staff foreign key (staffID)
	references [fa].[staff] (staffID)
go

alter table hc.staffDaysWorked
	add constraint fk_staffDaysWorked_payCalendar foreign key (payCalendarID)
	references hc.payCalendar (payCalendarID)
go
