use coreDB
go

alter view ln.vwSavingStatement
with encryption as
select
	s.clientID,
	clientName,
	accountNumber,
	s.savingID loanID,
	s.savingNo loanNo,
	isnull([date], getdate()) as [date],
	[Description],
	isnull(Dr, 0) as Dr,
	isnull(Cr,0) as Cr,
	sortCode,
	isnull(firstSavingDate, getDate()) as firstSavingDate,
	isnull(maturityDate, getdate()) as maturityDate,
	isnull(depositAmount,0) depositAmount,
	isnull(withdrawalAmount,0) withdrawalAmount,
	isnull(interestAccruedAmount,0) interestAccruedAmount,
	isnull(chargeAmount,0) chargeAmount,
	isnull(princWithdrawalAmount, 0.0) princWithdrawalAmount,
	isnull(intWithdrawalAmount, 0.0) intWithdrawalAmount,
	isnull(a.staffName, '') as staffName,
	isnull(a.staffNo, '') as staffNo,
	isnull(savingTypeName, '') as savingTypeName
from
(			
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.savingID,
		l.savingNo,
		isnull(da.savingDate, getdate()) as [date],
		isnull(naration, 'Deposit Made') as [Description],
		da.savingAmount as Dr,
		0 as Cr,
		2 as sortCode,
		da.savingAmount as depositAmount,
		0.0 as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		0.0 as chargeAmount,
		0.0 as  princWithdrawalAmount,
		0.0 as  intWithdrawalAmount,
		st.savingTypeName
	from   ln.savingAdditional da 
		inner join ln.saving l  on da.savingID=l.savingID
		inner join ln.client c on l.clientID = c.clientID 
		inner join ln.savingType st on st.savingTypeId = l.savingTypeId
	where posted=1
	union all	
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.savingID,
		l.savingNo,
		isnull(da.rollOverDate, getdate()) as [date],
		'Savings Account Rolled Over' as [Description],
		0 as Dr,
		0 as Cr,
		2 as sortCode,
		0 as depositAmount,
		0.0 as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		0.0 as chargeAmount,
		0.0 as  princWithdrawalAmount,
		0.0 as  intWithdrawalAmount,
		st.savingTypeName
	from   ln.savingRollover da 
		inner join ln.saving l  on da.savingID=l.savingID
		inner join ln.client c on l.clientID = c.clientID  
		inner join ln.savingType st on st.savingTypeId = l.savingTypeId

	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.savingID,
		l.savingNo,
		lp.interestDate [date],
		'Interest on Savings' as [Description],
		lp.interestAmount as Dr,
		0 as Cr,
		1 as sortCode,
		0.0 as depositAmount,
		0.0 as  withdrawalAmount,
		lp.interestAmount as interestAccruedAmount,
		0.0 as chargeAmount,
		0.0 princWithdrawalAmount,
		0.0 intWithdrawalAmount,
		st.savingTypeName
	from ln.savingInterest lp inner join ln.saving l on lp.savingID=l.savingID
		inner join ln.client c on l.clientID = c.clientID 
		inner join ln.savingType st on st.savingTypeId = l.savingTypeId
	where   interestAmount>0 	 
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.savingID,
		l.savingNo,
		lp.withdrawalDate as [date],
		isnull(naration, 'Principal Withdrawal') as [Description],
		0 as Dr,
		lp.principalWithdrawal as Cr,
		4 as sortCode,
		0.0 as depositAmount,
		lp.principalWithdrawal as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		0.0 as chargeAmount,
		principalWithdrawal as  princWithdrawalAmount,
		0.0 as  intWithdrawalAmount,
		st.savingTypeName
	from ln.savingWithdrawal lp inner join ln.saving l on lp.savingID=l.savingID
		inner join ln.client c on l.clientID = c.clientID 
		inner join ln.savingType st on st.savingTypeId = l.savingTypeId
	where principalWithdrawal>0
		and posted=1
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.savingID,
		l.savingNo,
		lp.withdrawalDate as [date],
		isnull(naration, 'Interest Withdrawal') as [Description],
		0 as Dr,
		lp.interestWithdrawal as Cr,
		4 as sortCode,
		0.0 as depositAmount,
		lp.interestWithdrawal as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		0.0 as chargeAmount,
		0.0 as  princWithdrawalAmount,
		interestWithdrawal as  intWithdrawalAmount,
		st.savingTypeName
	from ln.savingWithdrawal lp inner join ln.saving l on lp.savingID=l.savingID
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.savingType st on st.savingTypeId = l.savingTypeId		 
	where interestWithdrawal>0
		and posted=1
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.savingID,
		l.savingNo,
		lp.chargeDate as [date],
		ct.chargeTypeName as [Description],
		0 as Dr,
		lp.amount as Cr,
		5 as sortCode,
		0.0 as depositAmount,
		0.0 as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		lp.amount as chargeAmount,
		0.0 as  princWithdrawalAmount,
		0.0 as  intWithdrawalAmount,
		st.savingTypeName
	from ln.savingCharge lp inner join ln.saving l on lp.savingID=l.savingID
		inner join ln.client c on l.clientID = c.clientID 
		inner join ln.chargeType ct on lp.chargeTypeID = ct.chargeTypeID
		inner join ln.savingType st on st.savingTypeId = l.savingTypeId
	where amount>0 
)  t inner join ln.saving s on t.savingID = s.savingID
	left join hc.vwStaff a on s.staffID=a.staffID