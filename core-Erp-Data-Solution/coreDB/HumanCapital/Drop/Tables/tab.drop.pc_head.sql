use coreDB
go

IF EXISTS (SELECT * FROM    sys.objects 
	INNER JOIN sys.schemas ON sys.objects.schema_id = sys.schemas.schema_id
	where sys.schemas.NAME='gl' and sys.objects.name='pc_head')
	BEGIN
		DROP  Table gl.pc_head
	END
GO
 