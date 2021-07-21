use coreDB
go

alter table gl.v_ftr add
	constraint fk_v_ftr_accts foreign key (acct_id)
	references accts(acct_id) on update cascade 
go
	 
alter table gl.v_ftr add
	constraint fk_v_ftr_v_head foreign key (v_head_id)
	references gl.v_head(v_head_id) on update cascade 
go
	 