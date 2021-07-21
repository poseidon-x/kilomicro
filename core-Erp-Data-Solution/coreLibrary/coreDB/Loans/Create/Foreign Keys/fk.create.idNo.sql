use coreDB
go

alter table ln.idNo
	add constraint fk_idNo_idNoType foreign key(idNoTypeID)
	references ln.idNoType(idNoTypeID)
go