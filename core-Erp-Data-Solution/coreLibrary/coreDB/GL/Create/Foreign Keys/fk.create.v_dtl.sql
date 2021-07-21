use coreDB
go

alter table gl.v_dtl add
	constraint fk_v_dtl_accts foreign key (acct_id)
	references accts(acct_id) on update cascade 
go
	 
alter table gl.v_dtl add
	constraint fk_v_dtl_v_head foreign key (v_head_id)
	references gl.v_head(v_head_id) on update cascade 
go
	 