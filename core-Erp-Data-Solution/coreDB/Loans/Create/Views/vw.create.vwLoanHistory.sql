 use coreDB
 go

 alter view ln.vwLoanHistory
 with encryption as
SELECT        l.clientID, l.loanID, l.loanNo, ISNULL(YEAR(l.disbursementDate), 0) AS year0, ISNULL(MONTH(l.disbursementDate), 0) AS month0, ISNULL(l.disbursementDate, 
                         GETDATE()) AS disbursementDate, l.amountDisbursed, l.amountApproved, l.amountRequested, CASE WHEN
                             (SELECT        isnull(SUM(fairValue), 1)
                               FROM            ln.loanCollateral lc
                               WHERE        lc.loanID = l.loanID) = 0 THEN 0 ELSE isnull
                             ((SELECT        SUM(rs.principalPayment + rs.interestPayment)
                                 FROM            ln.repaymentSchedule rs
                                 WHERE        rs.loanID = l.loanID) /
                             (SELECT         top 1 isnull(SUM(fairValue), 1)
                               FROM            ln.loanCollateral lc
                               WHERE        lc.loanID = l.loanID), 0) * 100 END AS riskRatio, 
							   CASE WHEN
                             ((SELECT        top 1 isnull(SUM((revenue - expenses - otherCosts) / frequencyID), 1)
                                 FROM            ln.loanFinancial lc
                                 WHERE        lc.loanID = l.loanID) * loantenure) = 0 THEN 0 ELSE isnull(isnull
                             ((SELECT        top 1 SUM(rs.principalPayment + rs.interestPayment)
                                 FROM            ln.repaymentSchedule rs
                                 WHERE        rs.loanID = l.loanID), 1) / l.repaymentModeID /
                             ((SELECT         top 1 isnull(SUM((revenue - expenses - otherCosts) / frequencyID), 1)
                                 FROM            ln.loanFinancial lc
                                 WHERE        lc.loanID = l.loanID) * loantenure), 0) * 100 END AS affordabilityRatio, l.approvalComments, l.creditOfficerNotes, l.interestRate, l.loanTenure, 
                         ISNULL
                             ((SELECT        MIN(repaymentDate) AS Expr1
                                 FROM            ln.repaymentSchedule AS rs
                                 WHERE        (loanID = l.loanID)), GETDATE()) AS startDate, ISNULL
                             ((SELECT        MAX(repaymentDate) AS Expr1
                                 FROM            ln.repaymentSchedule AS rs
                                 WHERE        (loanID = l.loanID)), GETDATE()) AS lastPaymentDate, s.surName + ', ' + s.otherNames AS staffName, ISNULL(s.staffID, 0) AS staffID, ISNULL
                             ((SELECT        AVG(DATEDIFF(dd, repaymentDate, GETDATE())) AS Expr1
                                 FROM            ln.repaymentSchedule AS rs
                                 WHERE        (loanID = l.loanID) AND (repaymentDate <= GETDATE()) AND (principalBalance > 0)), 0) AS daysDelta,
					   isnull((select sum(amountPaid) from ln.loanRepayment lr
						       where lr.loanID = l.loanID),0) as amountPaid,
					   isnull((select sum(principalBalance+interestBalance) from ln.repaymentSchedule lr
						       where lr.loanID = l.loanID),0) as totalBalance
FROM            ln.loan AS l LEFT OUTER JOIN
                         fa.staff AS s ON l.staffID = s.staffID
go