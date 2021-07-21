use coreDB
go

alter view ln.vwLoanTenor
with encryption as
select distinct
	tenor as id ,
	isnull(cast(tenor as nvarchar(10)) + ' Months','') as tenor,
	isnull(max(case when loanTypeID=2 then defaultInterestRate else 0 end),0) as ratePersonal,
	isnull(max(case when loanTypeID=4 then defaultInterestRate else 0 end),0) as rateGroup,
	isnull(max(case when loanTypeID=1 then defaultInterestRate else 0 end),0) as rateBusiness,
	isnull(max(case when loanTypeID<>2 and loanTypeID<>1 and loanTypeID<>4 then defaultInterestRate else 0 end),0) as rateOther
from ln.tenor
group by tenor ,
	isnull(cast(tenor as nvarchar(10)) + ' Months','') 
go
