use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'vw_gl_ou')
	BEGIN
		DROP  View vw_gl_ou
	END
GO
