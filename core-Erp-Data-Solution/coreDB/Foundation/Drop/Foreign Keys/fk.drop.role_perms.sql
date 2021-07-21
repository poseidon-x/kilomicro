use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_role_perms_perm')
	BEGIN
		alter table role_perms DROP  constraint fk_role_perms_perm
	END
GO
  
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_role_perms_role')
	BEGIN
		alter table role_perms DROP  constraint fk_role_perms_role
	END
GO
  
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_role_perms_module')
	BEGIN
		alter table role_perms DROP  constraint fk_role_perms_module
	END
GO
  