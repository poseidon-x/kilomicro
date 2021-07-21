use coreDB
go

IF EXISTS (SELECT * FROM    sys.objects 
	INNER JOIN sys.schemas ON sys.objects.schema_id = sys.schemas.schema_id
	where sys.schemas.NAME='dbo' and sys.objects.name='get_v')
	BEGIN
		DROP  view gl.vw_v
	END
GO
 