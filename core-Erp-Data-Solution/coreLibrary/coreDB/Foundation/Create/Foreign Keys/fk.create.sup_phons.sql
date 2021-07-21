use coreDB
go

alter table sup_phons add
	constraint fk_sup_phone_type foreign key (phon_type)
	references phon_types(phon_type) on delete cascade on update cascade 
go
