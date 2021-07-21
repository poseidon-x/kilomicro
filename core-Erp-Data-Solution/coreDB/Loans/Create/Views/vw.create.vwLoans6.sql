use coreDB
go

alter view ln.vwLoans6
with encryption as 
select
	isnull(isnull(isnull(isnull(a.loanID, b.loanID), d.loanID), c.loanId),0) as loanID,
	isnull(isnull(isnull(isnull(a.clientName,  b.clientName) , d.clientName) , c.clientName),'')  as clientName,
	isnull(isnull(isnull(isnull(a.accountNumber, b.accountNumber), d.accountNumber), c.accountNumber),'') as accountNumber,
	isnull(isnull(isnull(isnull(a.clientID, b.clientID), d.clientID), c.clientId),0) as clientID,
	isnull(isnull(isnull(isnull(a.categoryName, b.categoryName), d.categoryName), c.categoryName),'') as categoryName,
	isnull(isnull(a.amountDisbursed, d.amountDisbursed),0) as amountDisbursed,
	isnull(isnull(a.processingFee,d.processingFee),0) as processingFee,
	isnull(isnull(a.processingFeeBalance,d.processingFeeBalance),0) processingFeeBalance,
	isnull(isnull(a.loanNo, d.loanNo),'') as loanNo,
	isnull(isnull(a.disbursementDate, d.disbursementDate),getdate()) as disbursementDate, 
	isnull(isnull(a.repaymentDate, d.repaymentDate),getdate()) as repaymentDate,
	isnull(isnull(a.amountPayable,0),0) as amountPayable,
	isnull(isnull(b.amountPaid,0),0) as amountPaid,
	isnull(isnull(isnull(isnull(a.staffID, b.staffID), d.staffID), c.staffID),0) as staffID,
	isnull(isnull(isnull(isnull(a.staffName,d.staffName), d.staffName), c.staffName),'') as staffName,
	isnull(isnull(d.cumPayable,0),0) as cumPayable,
	isnull(isnull(c.cumPaid,0),0) as cumPaid,
	isnull(isnull(d.repaymentDate, c.repaymentDate),getdate()) as repaymentDate2
from
(
select
	l.loanID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	lt.loanTypeName categoryName,
	isnull(l.amountDisbursed,0) as amountDisbursed,
	isnull(l.processingFee,0) as processingFee,
	isnull(l.processingFeeBalance,0) processingFeeBalance,
	l.loanNo,
	isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) as disbursementDate, 
	isnull(MAX(rs.repaymentDate),   getdate()) as repaymentDate,
	isnull(SUM(amountPayable),0)as amountPayable,
	isnull(s.staffID,0) as staffID,
	isnull(s.surname + ', ' + s.otherNames,'') as staffName 
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
	left outer join 
	(	
		SELECT 
			loanId,
			rs.interestPayment
			-
					case when rs.interestPayment> 
						(ISNULL((select SUM(interestWritenOff) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId), 0) 
							* rs.interestPayment/
							(
								CASE WHEN (SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId)> 0 then 
									(SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId) 
								ELSE 1 END
							)
						)
					then 
						(ISNULL((select SUM(interestWritenOff) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId), 0) 
							* rs.interestPayment/
							(
								CASE WHEN (SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId)> 0 then 
									(SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId) 
								ELSE 1 END
							)
						)
					else  rs.interestPayment end
			+ rs.principalPayment amountPayable ,
			rs.repaymentDate as repaymentDate
		from ln.repaymentSchedule rs
		WHERE repaymentDate <= getdate() 
	) rs on l.loanID = rs.loanID 
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	left outer join fa.staff s on l.staffID = s.staffID
where l.disbursementDate is not null  
group by 
	l.loanID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
	c.accountNumber,
	c.clientID,
	lt.loanTypeName ,
	isnull(l.amountDisbursed,0),
	isnull(l.processingFee,0),
	isnull(l.processingFeeBalance,0),
	l.loanNo,
	isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) ,  
	isnull(s.staffID,0),
	isnull(s.surname + ', ' + s.otherNames,'')  
) a full outer join
(
	select
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName categoryName,
		isnull(l.amountDisbursed,0) as amountDisbursed,
		isnull(l.processingFee,0) as processingFee,
		isnull(l.processingFeeBalance,0) processingFeeBalance,
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) as disbursementDate, 
		isnull( max(lr.repaymentDate), getdate()) as repaymentDate, 
		isnull(sum(lr.amountPaid-lr.feePaid),0) as amountPaid,
		isnull(s.staffID,0) as staffID,
		isnull(s.surname + ', ' + s.otherNames,'') as staffName 
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID 
		left outer join ln.loanRepayment lr on l.loanID=lr.loanID 
		left outer join fa.staff s on l.staffID = s.staffID 
		inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	where l.disbursementDate is not null 
		and repaymentDate <= getdate()
	group by 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName ,
		isnull(l.amountDisbursed,0),
		isnull(l.processingFee,0),
		isnull(l.processingFeeBalance,0),
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()),  
		isnull(s.staffID,0),
		isnull(s.surname + ', ' + s.otherNames,'') 
	having	isnull(sum(lr.amountPaid-lr.feePaid),0) >0
) b on a.loanID=b.loanID
full outer join
(
	select 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName categoryName,
		isnull(l.amountDisbursed,0) as amountDisbursed,
		isnull(l.processingFee,0) as processingFee,
		isnull(l.processingFeeBalance,0) processingFeeBalance,
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) as disbursementDate, 
		getdate() as repaymentDate, 
		isnull(s.staffID,0) as staffID,
		isnull(s.surname + ', ' + s.otherNames,'') as staffName,
		isnull(sum(amountPayable),0) as  cumPayable,
		dateadd(MM, l.loanTenure, l.disbursementDate) as expiryDate
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
		left outer join  
		(	
			SELECT 
				loanId,
				rs.interestPayment-
					case when rs.interestPayment> 
						(ISNULL((select SUM(interestWritenOff) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId), 0) 
							* rs.interestPayment/
							(
								CASE WHEN (SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId)> 0 then 
									(SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId) 
								ELSE 1 END
							)
						)
					then 
						(ISNULL((select SUM(interestWritenOff) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId), 0) 
							* rs.interestPayment/
							(
								CASE WHEN (SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId)> 0 then 
									(SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId) 
								ELSE 1 END
							)
						)
					else  rs.interestPayment end
				+ rs.principalPayment amountPayable ,
				rs.repaymentDate as repaymentDate
			from ln.repaymentSchedule rs
			WHERE repaymentDate <= getdate() 
		) rs on l.loanID = rs.loanID 
		left outer join fa.staff s on l.staffID = s.staffID
		inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	where disbursementDate is not null
	group by 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName ,
		isnull(l.amountDisbursed,0),
		isnull(l.processingFee,0),
		isnull(l.processingFeeBalance,0),
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()),  
		isnull(s.staffID,0),
		isnull(s.surname + ', ' + s.otherNames,''),
		dateadd(MM, l.loanTenure, l.disbursementDate) 
) d on a.loanID=d.loanID or b.loanID=d.loanID
full outer join
(
	select 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName categoryName,
		isnull(l.amountDisbursed,0) as amountDisbursed,
		isnull(l.processingFee,0) as processingFee,
		isnull(l.processingFeeBalance,0) processingFeeBalance,
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) as disbursementDate, 
		getDate() as repaymentDate, 
		isnull(s.staffID,0) as staffID,
		isnull(s.surname + ', ' + s.otherNames,'') as staffName,
		sum(rs.amountPaid-rs.feepaid) as  cumPaid,
		dateadd(MM, l.loanTenure, l.disbursementDate) as expiryDate
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
		left outer join ln.loanRepayment rs on l.loanID = rs.loanID 
		left outer join fa.staff s on l.staffID = s.staffID
		inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	where disbursementDate is not null and repaymentDate <= getdate() 
	group by 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName ,
		isnull(l.amountDisbursed,0),
		isnull(l.processingFee,0),
		isnull(l.processingFeeBalance,0),
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()),  
		isnull(s.staffID,0),
		isnull(s.surname + ', ' + s.otherNames,''),
		dateadd(MM, l.loanTenure, l.disbursementDate)
	having	sum(rs.amountPaid-rs.feepaid) >0
) c on a.loanID=c.loanID or d.loanID=c.loanID or c.loanId=b.loanId
where (
		(
			isnull(isnull(a.amountPayable,0),0)>0
			or isnull(isnull(b.amountPaid,0),0)>0
			or isnull(isnull(d.cumPayable,0),0) >0
			or isnull(isnull(c.cumPaid,0),0)>0)
		)
	
go
 
 
 create function ln.GetvwLoans62
 (
	@startDate Datetime,
	@endDate datetime,
	@showAll bit =0,
	@expiry int=1
)
returns @tble table
(
	loanID int,
	clientName nvarchar(200),
	accountNumber nvarchar(200),
	clientID int,
	categoryName nvarchar(200),
	amountDisbursed float,
	processingFee float,
	processingFeeBalance float,
	loanNo nvarchar(200),
	disbursementDate datetime, 
	repaymentDate datetime,
	amountPayable float,
	amountPaid float,
	staffID int,
	staffName nvarchar(200),
	cumPayable float,
	cumPaid float,
	repaymentDate2 datetime
)
with encryption 
as
begin
insert into @tble
select
	isnull(isnull(isnull(isnull(a.loanID, b.loanID), d.loanID), c.loanId),0) as loanID,
	isnull(isnull(isnull(isnull(a.clientName,  b.clientName) , d.clientName) , c.clientName),'')  as clientName,
	isnull(isnull(isnull(isnull(a.accountNumber, b.accountNumber), d.accountNumber), c.accountNumber),'') as accountNumber,
	isnull(isnull(isnull(isnull(a.clientID, b.clientID), d.clientID), c.clientId),0) as clientID,
	isnull(isnull(isnull(isnull(a.categoryName, b.categoryName), d.categoryName), c.categoryName),'') as categoryName,
	isnull(isnull(a.amountDisbursed, d.amountDisbursed),0) as amountDisbursed,
	isnull(isnull(a.processingFee,d.processingFee),0) as processingFee,
	isnull(isnull(a.processingFeeBalance,d.processingFeeBalance),0) processingFeeBalance,
	isnull(isnull(a.loanNo, d.loanNo),'') as loanNo,
	isnull(isnull(a.disbursementDate, d.disbursementDate),getdate()) as disbursementDate, 
	isnull(isnull(a.repaymentDate, d.repaymentDate),getdate()) as repaymentDate,
	isnull(isnull(a.amountPayable,0),0) as amountPayable,
	isnull(isnull(b.amountPaid,0),0) as amountPaid,
	isnull(isnull(isnull(isnull(a.staffID, b.staffID), d.staffID), c.staffID),0) as staffID,
	isnull(isnull(isnull(isnull(a.staffName,d.staffName), d.staffName), c.staffName),'') as staffName,
	isnull(isnull(d.cumPayable,0),0) as cumPayable,
	isnull(isnull(c.cumPaid,0),0) as cumPaid,
	isnull(isnull(d.repaymentDate, c.repaymentDate),getdate()) as repaymentDate2
from
(
select
	l.loanID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	lt.loanTypeName categoryName,
	isnull(l.amountDisbursed,0) as amountDisbursed,
	isnull(l.processingFee,0) as processingFee,
	isnull(l.processingFeeBalance,0) processingFeeBalance,
	l.loanNo,
	isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) as disbursementDate, 
	isnull(MAX(rs.repaymentDate),   getdate()) as repaymentDate,
	isnull(SUM(amountPayable),0)as amountPayable,
	isnull(s.staffID,0) as staffID,
	isnull(s.surname + ', ' + s.otherNames,'') as staffName 
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
	left outer join 
	(	
		SELECT 
			loanId,
			rs.interestPayment
			-
					case when rs.interestPayment> 
						(ISNULL((select SUM(interestWritenOff) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId), 0) 
							* rs.interestPayment/
							(
								CASE WHEN (SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId)> 0 then 
									(SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId) 
								ELSE 1 END
							)
						)
					then 
						(ISNULL((select SUM(interestWritenOff) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId), 0) 
							* rs.interestPayment/
							(
								CASE WHEN (SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId)> 0 then 
									(SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId) 
								ELSE 1 END
							)
						)
					else  rs.interestPayment end
			+ rs.principalPayment amountPayable ,
			rs.repaymentDate as repaymentDate
		from ln.repaymentSchedule rs
		WHERE repaymentDate between @startDate and @endDate 
	) rs on l.loanID = rs.loanID 
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	left outer join fa.staff s on l.staffID = s.staffID
where l.disbursementDate is not null  
group by 
	l.loanID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
	c.accountNumber,
	c.clientID,
	lt.loanTypeName ,
	isnull(l.amountDisbursed,0),
	isnull(l.processingFee,0),
	isnull(l.processingFeeBalance,0),
	l.loanNo,
	isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) ,  
	isnull(s.staffID,0),
	isnull(s.surname + ', ' + s.otherNames,'')  
) a full outer join
(
	select
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName categoryName,
		isnull(l.amountDisbursed,0) as amountDisbursed,
		isnull(l.processingFee,0) as processingFee,
		isnull(l.processingFeeBalance,0) processingFeeBalance,
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) as disbursementDate, 
		isnull( max(lr.repaymentDate), getdate()) as repaymentDate, 
		isnull(sum(lr.amountPaid-lr.feePaid),0) as amountPaid,
		isnull(s.staffID,0) as staffID,
		isnull(s.surname + ', ' + s.otherNames,'') as staffName 
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID 
		left outer join ln.loanRepayment lr on l.loanID=lr.loanID 
		left outer join fa.staff s on l.staffID = s.staffID 
		inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	where l.disbursementDate is not null 
		and repaymentDate between @startDate and @endDate
	group by 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName ,
		isnull(l.amountDisbursed,0),
		isnull(l.processingFee,0),
		isnull(l.processingFeeBalance,0),
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()),  
		isnull(s.staffID,0),
		isnull(s.surname + ', ' + s.otherNames,'') 
	having	isnull(sum(lr.amountPaid-lr.feePaid),0) >0
) b on a.loanID=b.loanID
full outer join
(
	select 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName categoryName,
		isnull(l.amountDisbursed,0) as amountDisbursed,
		isnull(l.processingFee,0) as processingFee,
		isnull(l.processingFeeBalance,0) processingFeeBalance,
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) as disbursementDate, 
		@endDate as repaymentDate, 
		isnull(s.staffID,0) as staffID,
		isnull(s.surname + ', ' + s.otherNames,'') as staffName,
		isnull(sum(amountPayable),0) as  cumPayable,
		dateadd(MM, l.loanTenure, l.disbursementDate) as expiryDate
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
		left outer join  
		(	
			SELECT 
				loanId,
				rs.interestPayment-
					case when rs.interestPayment> 
						(ISNULL((select SUM(interestWritenOff) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId), 0) 
							* rs.interestPayment/
							(
								CASE WHEN (SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId)> 0 then 
									(SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId) 
								ELSE 1 END
							)
						)
					then 
						(ISNULL((select SUM(interestWritenOff) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId), 0) 
							* rs.interestPayment/
							(
								CASE WHEN (SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId)> 0 then 
									(SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId) 
								ELSE 1 END
							)
						)
					else  rs.interestPayment end
				+ rs.principalPayment amountPayable ,
				rs.repaymentDate as repaymentDate
			from ln.repaymentSchedule rs
			WHERE repaymentDate <= @endDate 
		) rs on l.loanID = rs.loanID 
		left outer join fa.staff s on l.staffID = s.staffID
		inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	where disbursementDate is not null
	group by 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName ,
		isnull(l.amountDisbursed,0),
		isnull(l.processingFee,0),
		isnull(l.processingFeeBalance,0),
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()),  
		isnull(s.staffID,0),
		isnull(s.surname + ', ' + s.otherNames,''),
		dateadd(MM, l.loanTenure, l.disbursementDate) 
) d on a.loanID=d.loanID or b.loanID=d.loanID
full outer join
(
	select 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName categoryName,
		isnull(l.amountDisbursed,0) as amountDisbursed,
		isnull(l.processingFee,0) as processingFee,
		isnull(l.processingFeeBalance,0) processingFeeBalance,
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) as disbursementDate, 
		@endDate as repaymentDate, 
		isnull(s.staffID,0) as staffID,
		isnull(s.surname + ', ' + s.otherNames,'') as staffName,
		sum(rs.amountPaid-rs.feepaid) as  cumPaid,
		dateadd(MM, l.loanTenure, l.disbursementDate) as expiryDate
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
		left outer join ln.loanRepayment rs on l.loanID = rs.loanID 
		left outer join fa.staff s on l.staffID = s.staffID
		inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	where disbursementDate is not null and repaymentDate<=@endDate 
	group by 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName ,
		isnull(l.amountDisbursed,0),
		isnull(l.processingFee,0),
		isnull(l.processingFeeBalance,0),
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()),  
		isnull(s.staffID,0),
		isnull(s.surname + ', ' + s.otherNames,''),
		dateadd(MM, l.loanTenure, l.disbursementDate)
	having	sum(rs.amountPaid-rs.feepaid) >0
) c on a.loanID=c.loanID or d.loanID=c.loanID or c.loanId=b.loanId
where (
		(
			isnull(isnull(a.amountPayable,0),0)>0
			or isnull(isnull(b.amountPaid,0),0)>0
			or isnull(isnull(d.cumPayable,0),0) >0
			or isnull(isnull(c.cumPaid,0),0)>0)
		)
	and (
			(@showAll = 1 )
			or(( @showAll=0 and isnull(isnull(d.cumPayable,0),0) > isnull(isnull(c.cumPaid,0),0)+10))
			or (( @showAll is null and isnull(isnull(d.cumPayable,0),0) <= isnull(isnull(c.cumPaid,0),0) + 10)
		)
	)
		and
		(
			@expiry = 1
			or (@expiry = 2 and isnull(d.expiryDate, c.expiryDate) <= @endDate)
			or (@expiry = 3 and isnull(d.expiryDate, c.expiryDate) > @endDate)
		)
	return	 
end
go
	
 alter proc ln.GetvwLoans6
 (
	@startDate Datetime,
	@endDate datetime,
	@showAll bit =0,
	@expiry int=1
)
with encryption as 
select
	isnull(isnull(isnull(isnull(a.loanID, b.loanID), d.loanID), c.loanId),0) as loanID,
	isnull(isnull(isnull(isnull(a.clientName,  b.clientName) , d.clientName) , c.clientName),'')  as clientName,
	isnull(isnull(isnull(isnull(a.accountNumber, b.accountNumber), d.accountNumber), c.accountNumber),'') as accountNumber,
	isnull(isnull(isnull(isnull(a.clientID, b.clientID), d.clientID), c.clientId),0) as clientID,
	isnull(isnull(isnull(isnull(a.categoryName, b.categoryName), d.categoryName), c.categoryName),'') as categoryName,
	isnull(isnull(a.amountDisbursed, d.amountDisbursed),0) as amountDisbursed,
	isnull(isnull(a.processingFee,d.processingFee),0) as processingFee,
	isnull(isnull(a.processingFeeBalance,d.processingFeeBalance),0) processingFeeBalance,
	isnull(isnull(a.loanNo, d.loanNo),'') as loanNo,
	isnull(isnull(a.disbursementDate, d.disbursementDate),getdate()) as disbursementDate, 
	isnull(isnull(a.repaymentDate, d.repaymentDate),getdate()) as repaymentDate,
	isnull(isnull(a.amountPayable,0),0) as amountPayable,
	isnull(isnull(b.amountPaid,0),0) as amountPaid,
	isnull(isnull(isnull(isnull(a.staffID, b.staffID), d.staffID), c.staffID),0) as staffID,
	isnull(isnull(isnull(isnull(a.staffName,d.staffName), d.staffName), c.staffName),'') as staffName,
	isnull(isnull(d.cumPayable,0),0) as cumPayable,
	isnull(isnull(c.cumPaid,0),0) as cumPaid,
	isnull(isnull(d.repaymentDate, c.repaymentDate),getdate()) as repaymentDate2
from
(
select
	l.loanID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	lt.loanTypeName categoryName,
	isnull(l.amountDisbursed,0) as amountDisbursed,
	isnull(l.processingFee,0) as processingFee,
	isnull(l.processingFeeBalance,0) processingFeeBalance,
	l.loanNo,
	isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) as disbursementDate, 
	isnull(MAX(rs.repaymentDate),   getdate()) as repaymentDate,
	isnull(SUM(amountPayable),0)as amountPayable,
	isnull(s.staffID,0) as staffID,
	isnull(s.surname + ', ' + s.otherNames,'') as staffName 
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
	left outer join 
	(	
		SELECT 
			loanId,
			rs.interestPayment
			-
					case when rs.interestPayment> 
						(ISNULL((select SUM(interestWritenOff) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId), 0) 
							* rs.interestPayment/
							(
								CASE WHEN (SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId)> 0 then 
									(SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId) 
								ELSE 1 END
							)
						)
					then 
						(ISNULL((select SUM(interestWritenOff) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId), 0) 
							* rs.interestPayment/
							(
								CASE WHEN (SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId)> 0 then 
									(SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId) 
								ELSE 1 END
							)
						)
					else  rs.interestPayment end
			+ rs.principalPayment amountPayable ,
			rs.repaymentDate as repaymentDate
		from ln.repaymentSchedule rs
		WHERE repaymentDate between @startDate and @endDate 
	) rs on l.loanID = rs.loanID 
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	left outer join fa.staff s on l.staffID = s.staffID
where l.disbursementDate is not null  
group by 
	l.loanID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
	c.accountNumber,
	c.clientID,
	lt.loanTypeName ,
	isnull(l.amountDisbursed,0),
	isnull(l.processingFee,0),
	isnull(l.processingFeeBalance,0),
	l.loanNo,
	isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) ,  
	isnull(s.staffID,0),
	isnull(s.surname + ', ' + s.otherNames,'')  
) a full outer join
(
	select
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName categoryName,
		isnull(l.amountDisbursed,0) as amountDisbursed,
		isnull(l.processingFee,0) as processingFee,
		isnull(l.processingFeeBalance,0) processingFeeBalance,
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) as disbursementDate, 
		isnull( max(lr.repaymentDate), getdate()) as repaymentDate, 
		isnull(sum(lr.amountPaid-lr.feePaid),0) as amountPaid,
		isnull(s.staffID,0) as staffID,
		isnull(s.surname + ', ' + s.otherNames,'') as staffName 
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID 
		left outer join ln.loanRepayment lr on l.loanID=lr.loanID 
		left outer join fa.staff s on l.staffID = s.staffID 
		inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	where l.disbursementDate is not null 
		and repaymentDate between @startDate and @endDate
	group by 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName ,
		isnull(l.amountDisbursed,0),
		isnull(l.processingFee,0),
		isnull(l.processingFeeBalance,0),
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()),  
		isnull(s.staffID,0),
		isnull(s.surname + ', ' + s.otherNames,'') 
	having	isnull(sum(lr.amountPaid-lr.feePaid),0) >0
) b on a.loanID=b.loanID
full outer join
(
	select 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName categoryName,
		isnull(l.amountDisbursed,0) as amountDisbursed,
		isnull(l.processingFee,0) as processingFee,
		isnull(l.processingFeeBalance,0) processingFeeBalance,
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) as disbursementDate, 
		@endDate as repaymentDate, 
		isnull(s.staffID,0) as staffID,
		isnull(s.surname + ', ' + s.otherNames,'') as staffName,
		isnull(sum(amountPayable),0) as  cumPayable,
		dateadd(MM, l.loanTenure, l.disbursementDate) as expiryDate
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
		left outer join  
		(	
			SELECT 
				loanId,
				rs.interestPayment-
					case when rs.interestPayment> 
						(ISNULL((select SUM(interestWritenOff) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId), 0) 
							* rs.interestPayment/
							(
								CASE WHEN (SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId)> 0 then 
									(SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId) 
								ELSE 1 END
							)
						)
					then 
						(ISNULL((select SUM(interestWritenOff) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId), 0) 
							* rs.interestPayment/
							(
								CASE WHEN (SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId)> 0 then 
									(SELECT SUM(rs2.interestPayment) from ln.repaymentSchedule rs2 where rs.loanId=rs2.loanId) 
								ELSE 1 END
							)
						)
					else  rs.interestPayment end
				+ rs.principalPayment amountPayable ,
				rs.repaymentDate as repaymentDate
			from ln.repaymentSchedule rs
			WHERE repaymentDate <= @endDate 
		) rs on l.loanID = rs.loanID 
		left outer join fa.staff s on l.staffID = s.staffID
		inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	where disbursementDate is not null
	group by 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName ,
		isnull(l.amountDisbursed,0),
		isnull(l.processingFee,0),
		isnull(l.processingFeeBalance,0),
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()),  
		isnull(s.staffID,0),
		isnull(s.surname + ', ' + s.otherNames,''),
		dateadd(MM, l.loanTenure, l.disbursementDate) 
) d on a.loanID=d.loanID or b.loanID=d.loanID
full outer join
(
	select 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName categoryName,
		isnull(l.amountDisbursed,0) as amountDisbursed,
		isnull(l.processingFee,0) as processingFee,
		isnull(l.processingFeeBalance,0) processingFeeBalance,
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()) as disbursementDate, 
		@endDate as repaymentDate, 
		isnull(s.staffID,0) as staffID,
		isnull(s.surname + ', ' + s.otherNames,'') as staffName,
		sum(rs.amountPaid-rs.feepaid) as  cumPaid,
		dateadd(MM, l.loanTenure, l.disbursementDate) as expiryDate
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
		left outer join ln.loanRepayment rs on l.loanID = rs.loanID 
		left outer join fa.staff s on l.staffID = s.staffID
		inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	where disbursementDate is not null and repaymentDate<=@endDate 
	group by 
		l.loanID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
		c.accountNumber,
		c.clientID,
		lt.loanTypeName ,
		isnull(l.amountDisbursed,0),
		isnull(l.processingFee,0),
		isnull(l.processingFeeBalance,0),
		l.loanNo,
		isnull(isnull(l.disbursementDate, l.applicationDate), getdate()),  
		isnull(s.staffID,0),
		isnull(s.surname + ', ' + s.otherNames,''),
		dateadd(MM, l.loanTenure, l.disbursementDate)
	having	sum(rs.amountPaid-rs.feepaid) >0
) c on a.loanID=c.loanID or d.loanID=c.loanID or c.loanId=b.loanId
where (
		(
			isnull(isnull(a.amountPayable,0),0)>0
			or isnull(isnull(b.amountPaid,0),0)>0
			or isnull(isnull(d.cumPayable,0),0) >0
			or isnull(isnull(c.cumPaid,0),0)>0)
		)
	and (
			(@showAll = 1 )
			or(( @showAll=0 and isnull(isnull(d.cumPayable,0),0) > isnull(isnull(c.cumPaid,0),0)+10))
			or (( @showAll is null and isnull(isnull(d.cumPayable,0),0) <= isnull(isnull(c.cumPaid,0),0) + 10)
		)
	)
		and
		(
			@expiry = 1
			or (@expiry = 2 and isnull(d.expiryDate, c.expiryDate) <= @endDate)
			or (@expiry = 3 and isnull(d.expiryDate, c.expiryDate) > @endDate)
		)
	
go
 