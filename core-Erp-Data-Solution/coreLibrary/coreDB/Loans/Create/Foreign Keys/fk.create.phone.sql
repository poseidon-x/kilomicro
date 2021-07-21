use coreDB
go


alter table ln.phone
	add constraint fk_phone_phoneType foreign key(phoneTypeID)
	references ln.phoneType(phoneTypeID)
go