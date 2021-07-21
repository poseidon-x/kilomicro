--use coreDB
--go

alter table hc.staffPromotion add
	constraint fk_staffPromotion_staff foreign key (staffID)
	references fa.staff(staffID)
go

alter table hc.staffPromotion add
	constraint fk_staffPromotion_oldlevel foreign key (oldLevelID)
	references hc.[level](levelID)
go

alter table hc.staffPromotion add
	constraint fk_staffPromotion_newlevel foreign key (newLevelID)
	references hc.[level](levelID)
go

alter table hc.staffPromotion add
	constraint fk_staffPromotion_oldnotch foreign key (oldNotchID)
	references hc.[levelNotch](levelNotchID)
go

alter table hc.staffPromotion add
	constraint fk_staffPromotion_newnotch foreign key (newNotchID)
	references hc.[levelNotch](levelNotchID)
go

alter table hc.staffPromotion add
	constraint fk_staffPromotion_oldJobTitle foreign key (oldJobTitleID)
	references [fa].[jobTitle](jobTitleID)
go

alter table hc.staffPromotion add
	constraint fk_staffPromotion_newJobTitle foreign key (newJobTitleID)
	references [fa].[jobTitle](jobTitleID)
go

alter table hc.staffPromotion add
	constraint fk_staffPromotion_oldmanagerstaff foreign key (oldManagerStaffID)
	references [fa].staff(staffID)
go

alter table hc.staffPromotion add
	constraint fk_staffPromotion_newmanagerstaff foreign key (newManagerStaffID)
	references [fa].staff(staffID)
go
