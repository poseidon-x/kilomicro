use coreDB
go

create view ln.vwInvestment
with encryption
as
	select
		clientID,
		clientName,
		accountNumber,
		isnull(max(loanID),0) as investmentID,
		isnull(max(loanNo),'') investmentNo ,
		isnull(min([date]), getdate()) as [date],
		max([Description]) as [Description],
		isnull(sum(Dr), 0) as Dr,
		isnull(sum(Cr),0) as Cr,
		isnull(sum(dr-cr), 0) as balance,
		isnull(max(sortCode),0) as sortCode,
		isnull(min(firstInvestmentDate),getdate()) as firstInvestmentDate,
		isnull(max(maturityDate),getdate()) as maturityDate,
		'Fixed Investment' as productType
	from ln.vwInvestmentStatement
	group by		
		clientID,
		clientName,
		accountNumber 
	union all
	select
		clientID,
		clientName,
		accountNumber,
		isnull(max(loanID),0) as investmentID,
		isnull(max(loanNo),'') investmentNo ,
		isnull(min([date]), getdate()) as [date],
		max([Description]) as [Description],
		isnull(sum(Dr), 0) as Dr,
		isnull(sum(Cr),0) as Cr,
		isnull(sum(dr-cr), 0) as balance,
		isnull(max(sortCode),0) as sortCode,
		isnull(min(firstSavingDate),getdate()) as firstInvestmentDate,
		isnull(max(maturityDate),getdate()) as maturityDate,
		'Savings Account' as productType
	from ln.vwSavingStatement
	group by		
		clientID,
		clientName,
		accountNumber 