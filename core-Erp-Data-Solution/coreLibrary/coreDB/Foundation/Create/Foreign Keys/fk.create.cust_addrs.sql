use coreDB
go

alter table cust_addr add
	constraint fk_cust_addr_type foreign key (addr_type)
	references addr_types(addr_type) on delete cascade on update cascade 
go
