use coreDB
go

alter table cust_phons add
	constraint fk_cust_phone_type foreign key (phon_type)
	references phon_types(phon_type) on delete cascade on update cascade 
go
