use coreDB
go

alter table sup_addr add
	constraint fk_sup_addr_type foreign key (addr_type)
	references addr_types(addr_type) on delete cascade on update cascade 
go
