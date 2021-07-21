use coreDB
go


alter view ln.vwDeposit
with encryption
as
	select
		clientID,
		clientName,
		accountNumber,
		isnull(max(loanID),0) as depositID,
		isnull(max(loanNo),'') depositNo ,
		isnull(min([date]), getdate()) as [date],
		max([Description]) as [Description],
		isnull(sum(Dr), 0) as Dr,
		isnull(sum(Cr),0) as Cr,
		isnull(sum(dr-cr), 0) as balance,
		isnull(max(sortCode),0) as sortCode,
		isnull(min(firstDepositDate), getdate()) as firstDepositDate,
		isnull(max(maturityDate),getdate()) as maturityDate,
		'Fixed Deposit' as productType
	from ln.vwDepositStatement
	group by		
		clientID,
		clientName,
		accountNumber 
	union all
	select
		clientID,
		clientName,
		accountNumber,
		isnull(max(loanID),0) as depositID,
		isnull(max(loanNo),'') depositNo ,
		isnull(min([date]), getdate()) as [date],
		max([Description]) as [Description],
		isnull(sum(Dr), 0) as Dr,
		isnull(sum(Cr),0) as Cr,
		isnull(sum(dr-cr), 0) as balance,
		isnull(max(sortCode),0) as sortCode,
		isnull(min(firstSavingDate),getdate()) as firstDepositDate,
		isnull(max(maturityDate),getdate()) as maturityDate,
		'Savings Account' as productType
	from ln.vwSavingStatement
	group by		
		clientID,
		clientName,
		accountNumber 
go

create procedure ln.getDeposit
(
	@endDate datetime
)
with encryption
as
	select
		clientID,
		clientName,
		accountNumber,
		isnull(max(loanID),0) as depositID,
		isnull(max(loanNo),'') depositNo ,
		isnull(min([date]), getdate()) as [date],
		max([Description]) as [Description],
		isnull(sum(Dr), 0) as Dr,
		isnull(sum(Cr),0) as Cr,
		isnull(sum(dr-cr), 0) as balance,
		isnull(max(sortCode),0) as sortCode,
		isnull(min(firstDepositDate), getdate()) as firstDepositDate,
		isnull(max(maturityDate),getdate()) as maturityDate,
		'Fixed Deposit' as productType
	from ln.vwDepositStatement
	where [date] <= @endDate
	group by		
		clientID,
		clientName,
		accountNumber 
	union all
	select
		clientID,
		clientName,
		accountNumber,
		isnull(max(loanID),0) as depositID,
		isnull(max(loanNo),'') depositNo ,
		isnull(min([date]), getdate()) as [date],
		max([Description]) as [Description],
		isnull(sum(Dr), 0) as Dr,
		isnull(sum(Cr),0) as Cr,
		isnull(sum(dr-cr), 0) as balance,
		isnull(max(sortCode),0) as sortCode,
		isnull(min(firstSavingDate),getdate()) as firstDepositDate,
		isnull(max(maturityDate),getdate()) as maturityDate,
		'Savings Account' as productType
	from ln.vwSavingStatement
	where [date] <= @endDate
	group by		
		clientID,
		clientName,
		accountNumber 