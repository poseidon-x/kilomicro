use coreDB
go

alter table pref.roleMenuItem add
	constraint fk_roleMenuItem_roleMenu
	foreign key (roleMenuID)
	references pref.roleMenu (roleMenuID)
go
