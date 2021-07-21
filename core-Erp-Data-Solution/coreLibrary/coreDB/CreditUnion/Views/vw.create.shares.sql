use coredb
go

alter view cu.vwMembers
with encryption 
as
SELECT     c.clientID, c.clientName, c.accountNumber, cl.sex, c.DOB, c.officeEmail, c.homePhone, c.mobilePhone, 
	c.workPhone, c.cityTown, c.addressLine1, c.addressLine2, c.directions, c.personalEmail, cm.joinedDate, cm.sharesBalance, 
	cc.chapterName, cc.dateFormed, 
    cc.docRegistrationNumber, cc.postalAddress, cc.town, cc.emailAddress, cc.telePhoneNumber, cc.pricePerShare,
        (SELECT     SUM(principalBalance + interestBalance) AS Expr1
        FROM        ln.saving AS s
        WHERE     (clientID = c.clientID)) AS savingsBalance,
        (SELECT     SUM(principalBalance + interestBalance) AS Expr1
        FROM        ln.deposit AS s
        WHERE     (clientID = c.clientID)) AS depositBalance,
        (SELECT     SUM(totalBalance) AS Expr1
        FROM        ln.vwLoansByStaff AS s
        WHERE     (clientID = c.clientID)) AS loansbalance,
        (SELECT     COUNT(*) AS Expr1
        FROM        ln.vwLoansByStaff AS s
        WHERE     (clientID = c.clientID) AND (totalBalance > 0)) AS loanCount,
		'Active' as accountStatus,
		(select max(idNoTypeName) from ln.idno i inner join ln.idnotype it on i.idNoTypeID=it.idnotypeID where i.idnoID=cl.idnoID) as idType,
		(select max(i.idno) from ln.idno i inner join ln.idnotype it on i.idNoTypeID=it.idnotypeID where i.idnoID=cl.idnoID) as idNumber
FROM        cu.creditUnionChapter AS cc INNER JOIN
        cu.creditUnionMember AS cm ON cc.creditUnionChapterID = cm.creditUnionChapterID INNER JOIN
        ln.vwClients AS c ON cm.clientID = c.clientID INNER JOIN
        ln.client AS cl ON cl.clientID = c.clientID
go
