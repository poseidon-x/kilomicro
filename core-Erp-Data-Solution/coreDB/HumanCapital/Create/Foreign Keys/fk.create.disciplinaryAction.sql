--use coreDB
--go

alter table hc.staffMisconduct
	add constraint fk_staffMisconduct_staff foreign key (staffID)
	references fa.staff (staffID)
go

alter table hc.staffMisconduct
	add constraint fk_staffMisconduct_misconductType foreign key (misconductTypeID)
	references hc.misconductType (misconductTypeID)
go

alter table hc.staffMisconduct
	add constraint fk_staffMisconduct_disciplinaryAction foreign key (disciplinaryActionID)
	references hc.disciplinaryAction (disciplinaryActionID)
go

alter table hc.staffMisconduct
	add constraint fk_staffMisconduct_misconductSeverity foreign key (misconductSeverityID)
	references hc.misconductSeverity (misconductSeverityID)
go
