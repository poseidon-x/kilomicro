use coreDB
go

CREATE View vw_months
with encryption  AS

	SELECT   fmoy as m1,
		((fmoy+0)  % 12)+1 as m2,
		((fmoy+1)  % 12)+1 as m3,
		((fmoy+2)  % 12)+1 as m4,
		((fmoy+3)  % 12)+1 as m5,
		((fmoy+4)  % 12)+1 as m6,
		((fmoy+5)  % 12)+1 as m7,
		((fmoy+6)  % 12)+1 as m8,
		((fmoy+7)  % 12)+1 as m9,
		((fmoy+8)  % 12)+1 as m10,
		((fmoy+9)  % 12)+1 as m11,
		((fmoy+10)  % 12)+1 as m12
FROM         dbo.comp_prof 


GO
 