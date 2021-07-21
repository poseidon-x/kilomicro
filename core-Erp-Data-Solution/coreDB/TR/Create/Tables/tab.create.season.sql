use coreDB
go
 
CREATE TABLE tr.season
(
  season_id int identity(1,1) not null constraint pk_season primary key, 
  season_name nvarchar(50) not null constraint ck_season_name check(datalength(ltrim(rtrim(season_name))) > 0), 
  start_date datetime null,
  end_date datetime null,
  creation_date datetime null default(getdate()) constraint ck_season_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_season_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_season_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_season_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
