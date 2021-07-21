use coreDB
go

alter view ln.vwLoanTenor2
with encryption as
select 
	tenor as id ,
	isnull(cast(tenor as nvarchar(10)) + ' Months','') as tenor,
	isnull([defaultProcessingFeeRate],0) as ratePersonal,
	isnull([defaultProcessingFeeRate],0) as rateGroup,
	isnull([defaultProcessingFeeRate],0) as rateBusiness,
	isnull([defaultProcessingFeeRate],0) as rateOther
from ln.tenor
go
