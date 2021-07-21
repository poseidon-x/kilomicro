use coreDB
go

CREATE View tr.vw_transfers2
with encryption  AS

SELECT        t.transfer_name, t.transfer_date, t.transfer_num, t.creator, isnull(t.creation_date, GETDATE()) as creation_date, isnull(t.approved_by, '') as approved_by, isnull(t.approved_date,'1900-01-01') as approved_date, t.transfer_id, s.season_name, s.season_id, 
                         isnull(s.start_date,'1900-01-01')  AS season_starts, isnull(s.end_date,'1900-01-01') AS season_ends, td.transfer_dtl_id, td.num_of_bags, td.commission_rate, td.price_per_bag, td.pprice, 
                         td.commission_amt, td.total_amt, da.district_acct_id, da.acct_num, bb.branch_name, bb.branch_id, b.bank_id, b.bank_name, d.district_id, d.district_name, 
                         d.phone_num, d.district_mgr, s2.sector_id, s2.sector_name, 'TRANSFER' as details, num_of_bags as qty, pprice as amt, B.FULL_NAME AS BANK_FULL_NAME
FROM            bank_branches AS bb INNER JOIN
                         tr.district_acct AS da ON bb.branch_id = da.branch_id INNER JOIN
                         banks AS b ON da.bank_id = b.bank_id INNER JOIN
                         tr.transfer AS t INNER JOIN
                         tr.transfer_dtl AS td ON t.transfer_id = td.transfer_id ON da.district_acct_id = td.district_acct_id INNER JOIN
                         tr.district AS d ON td.district_id = d.district_id AND td.district_id = d.district_id AND td.district_id = d.district_id INNER JOIN
                         tr.season AS s ON t.season_id = s.season_id AND t.season_id = s.season_id INNER JOIN
                         tr.sector AS s2 ON d.sector_id = s2.sector_id AND d.sector_id = s2.sector_id
