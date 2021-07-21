use coreDB
go

alter View vw_accounts
with encryption  AS

SELECT     TOP (100) PERCENT acc.acc_num, acc.acc_name, acc.acc_name +' - ' + acc.acc_num as fullname, cur.major_name, cur.major_symbol, cur.minor_name, cur.minor_symbol, cat.cat_name, 
                      dbo.acc_head_walk(acc.acct_id, dbo.acc_head_level(hd.acct_head_id, 0) - 1, hd.acct_head_id, 0) AS head_name1, dbo.acc_head_walk(acc.acct_id, 
                      dbo.acc_head_level(hd.acct_head_id, 0) - 2, hd.acct_head_id, 0) AS head_name2, dbo.acc_head_walk(acc.acct_id, 
                      dbo.acc_head_level(hd.acct_head_id, 0) - 3, hd.acct_head_id, 0) AS head_name3, dbo.acc_head_walk(acc.acct_id, 
                      dbo.acc_head_level(hd.acct_head_id, 0) - 4, hd.acct_head_id, 0) AS head_name4, 
                     '' AS head_nam5, 
					  '' AS head_name6,
					   dbo.acc_head_walk(acc.acct_id, dbo.acc_head_level(hd.acct_head_id, 0) - 7, hd.acct_head_id, 0) 
                      AS head_name7, cat.cat_code, acc.acct_id, hd.acct_head_id, cat.acct_cat_id,
                      cur.currency_id, isnull(acc.companyId,0) as CompanyId, isnull(com.comp_name,'') as Comp_Name
FROM         dbo.accts AS acc INNER JOIN
                      dbo.acct_heads AS hd ON acc.acct_head_id = hd.acct_head_id INNER JOIN
                      dbo.acct_cats AS cat ON hd.acct_cat_id = cat.acct_cat_id INNER JOIN
                      dbo.currencies AS cur ON acc.currency_id = cur.currency_id LEFT OUTER JOIN
					  dbo.comp_prof AS com ON com.companyId = acc.companyId
ORDER BY cat.cat_code, acc.acc_num, acc.acc_name


GO
 