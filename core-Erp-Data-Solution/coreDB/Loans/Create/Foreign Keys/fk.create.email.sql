use coreDB
go


alter table ln.email
	add constraint fk_email_emailType foreign key(emailTypeID)
	references ln.emailType(emailTypeID)
go