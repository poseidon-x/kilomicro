USE [coreDB]
GO

alter view vwCompProf
with encryption as
SELECT [comp_prof_id]
      ,[comp_name]
      ,isnull([addr_line_1],'') as [addr_line_1]
      ,isnull([addr_line_2],'') as [addr_line_2]
      ,isnull([phon_num] ,'') as [phon_num]
      ,isnull([logo],'') as [logo],
	  isnull(ssnitNo, '') ssnitNo
  FROM [dbo].[comp_prof]
GO