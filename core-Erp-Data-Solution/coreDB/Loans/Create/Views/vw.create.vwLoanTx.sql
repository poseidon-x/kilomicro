use coreDB
go

alter view ln.vwLoanTx
with encryption as
SELECT        c1.surName + ', ' + c1.otherNames AS clientName, j.acct_id, a.acc_num, a.acc_name, 
			ISNULL(a.head_name1, N'') AS head_name1, ISNULL(a.head_name2, N'') 
                         AS head_name2, ISNULL(a.head_name3, N'') AS head_name3, ISNULL(a.head_name4, N'') AS head_name4, 
						 ISNULL(a.head_nam5, N'') AS head_name5, 
                         ISNULL(a.head_name6, N'') AS head_name6, ISNULL(a.head_name7, N'') AS head_name7, 
						 ISNULL(a.cat_code, N'') AS cat_code, ISNULL(a.cat_name, N'') 
                         AS cat_name, ISNULL(c.major_name, N'') AS major_name, ISNULL(c.major_symbol, N'') AS major_symbol, 
						 ISNULL(c.minor_name, N'') AS minor_name, 
                         ISNULL(c.minor_symbol, N'') AS minor_symbol, ISNULL(c.currency_id, 0) AS currency_id, j.tx_date, 
						 ISNULL(j.description, N'') AS description, ISNULL(j.ref_no, N'') 
                         AS ref_no, ISNULL(j.dbt_amt, 0) AS dbt_amt, ISNULL(j.crdt_amt, 0) AS crdt_amt, 
						 ISNULL(j.frgn_crdt_amt, 0) AS frgn_crdt_amt, ISNULL(j.frgn_dbt_amt, 0) 
                         AS frgn_dbt_amt, isnull(dbo.gl_ou_walk(j.cost_center_id, dbo.gl_ou_level(j.cost_center_id, 0) - 1, NULL, 0, N''),'')
						  AS cost_center, isnull(CASE WHEN p.currency_id IS NULL 
                         THEN c.major_name ELSE 'LOCAL' END,'') AS currency, 
						 isnull(CASE WHEN p.currency_id IS NULL THEN j.frgn_dbt_amt ELSE j.dbt_amt END,0) AS debit, 
                         isnull(CASE WHEN p.currency_id IS NULL THEN j.frgn_crdt_amt ELSE j.crdt_amt END,0) AS credit, 
						 ISNULL(b.batch_no, N'') AS batch_no, j.creator, isnull(j.creation_date,getdate()) as creation_date, c1.clientID, 
                         l.loanID, ISNULL(l.amountDisbursed, 0) AS amountDisbursed, lt.loanTypeID, lt.loanTypeName
FROM            dbo.vw_accounts AS a INNER JOIN
                         dbo.jnl AS j ON a.acct_id = j.acct_id INNER JOIN
                         dbo.jnl_batch AS b ON j.jnl_batch_id = b.jnl_batch_id LEFT OUTER JOIN
                         dbo.currencies AS c ON j.currency_id = c.currency_id LEFT OUTER JOIN
                         dbo.comp_prof AS p ON c.currency_id = p.currency_id INNER JOIN
                         ln.loan AS l ON l.loanNo = j.ref_no INNER JOIN
                         ln.client AS c1 ON l.clientID = c1.clientID INNER JOIN
                         ln.loanType AS lt ON l.loanTypeID = lt.loanTypeID
go
