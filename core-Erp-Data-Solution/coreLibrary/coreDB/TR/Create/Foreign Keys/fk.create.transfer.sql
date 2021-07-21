use coreDB
go

alter table tr.transfer add
	constraint fk_transfer_seasons foreign key (season_id)
	references tr.season(season_id) on update cascade 
go
