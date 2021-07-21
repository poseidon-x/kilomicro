use coreDB
go

alter table ln.notificationSchedule add constraint
	fk_notificationSchedule_notification foreign key (notificationID)
	references ln.[notification](notificationID)
go

alter table ln.notificationRecipient add constraint
	fk_notificationRecipient_notification foreign key (notificationID)
	references ln.[notification](notificationID)
go

alter table ln.notificationRecipient add constraint
	fk_notificationRecipient_staff foreign key (staffID)
	references fa.staff(staffID)
go
