﻿IF EXISTS (SELECT * FROM    sys.objects 
	INNER JOIN sys.schemas ON sys.objects.schema_id = sys.schemas.schema_id
	where sys.schemas.NAME='dbo' and sys.objects.name='acct_period')
	BEGIN
		DROP  Table dbo.acct_period
	END
GO
 