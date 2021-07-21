use coreDB
go

alter Proc gen_insert
(
@p_tableName varchar(255),
@p_schemaName varchar(255)
)
with encryption
as
/**************************************************************/
--Description : Stored procedure to generate insert statements
--Original Source : http://blogs.consultantsguild.com/index.php/mclerget/2005/02/07/dynamic_sql_insert_generator_unleashed_1
--Modified By : Namwar Rizvi
--Date : 22-May-2007

/**************************************************************/
Declare @tmp table
(
SQLText varchar(8000)
)
Declare @tmp2 table
(
Id int identity,
SQLText varchar(8000)
)
set nocount on
declare @vsSQL varchar(8000),
@vsCols varchar(8000),
@vsTableName varchar(40)
declare csrTables cursor for
select sys.schemas.NAME + '.' + sys.objects.name from sys.objects
	INNER JOIN sys.schemas ON sys.objects.schema_id = sys.schemas.schema_id
	where sys.schemas.NAME=@p_schemaName and sys.objects.name=@p_tableName
order by sys.objects.name

open csrTables
fetch next from csrTables into @vsTableName
while (@@fetch_status = 0)

begin
select @vsSQL = '',@vsCols = ''
select @vsSQL = @vsSQL +
CASE when sc.type in (39,47,61,111) then
'''''''''+' + 'isnull(rtrim(replace('+ sc.name + ','''''''','''''''''''')),'''')' + '+'''''',''+'
else
'isnull(convert(varchar,' + sc.name + '),''null'')+'',''+'
end
from syscolumns sc where sc.id = object_id(@vsTableName)
order by ColID

select @vsCols = @vsCols + sc.name + ','from syscolumns sc
where sc.id = object_id(@vsTableName) order by ColID

select @vsSQL = substring(@vsSQL,1,datalength(@vsSQL)-1)
select @vsCols = substring(@vsCols,1,datalength(@vsCols)-1)


insert @tmp
exec ('select ' + @vsSQL + ' from ' + @vsTableName)

update @tmp
set sqltext = 'insert ' + @vsTableName + '(' + @vsCols + ') values(' + substring(sqltext,1,datalength(sqltext)-1) + ')'

insert @tmp2 select 'use coreDB'
insert @tmp2 values ('GO')

--insert @tmp2 select 'DELETE from ' + @vsTableName
--insert @tmp2 values ('GO')

if (select count(id) from syscolumns where id = object_id(@vsTableName) and ((status & 128) = 128) ) = 1
begin
insert @tmp2 select 'set identity_insert ' + @vsTableName + ' on'
end


insert @tmp2 select * from @tmp

if (select count(id) from syscolumns where id = object_id(@vsTableName) and ((status & 128) = 128) ) = 1

begin
insert @tmp2
select 'set identity_insert ' + @vsTableName + ' off'
end

insert @tmp2 values ('GO')
insert @tmp2

select 'update statistics ' + @vsTableName
insert @tmp2 values ('GO')


delete @tmp
fetch next from csrTables into @vsTableName
end

close csrTables
deallocate csrTables

update @tmp2
set sqltext = substring(sqltext,1,charindex(',)',sqltext)-1) + ',NULL)'
where not(charindex(',)',sqltext) = 0)

--update @tmp2
--set sqltext = replace(sqltext, ',''''',',null')
--where not (charindex(',''''',sqltext) = 0)

update @tmp2
set sqltext = replace(sqltext, '(''''',',null')
where not (charindex('(''''',sqltext) = 0)

set nocount off
select sqltext from @tmp2 order by id
go