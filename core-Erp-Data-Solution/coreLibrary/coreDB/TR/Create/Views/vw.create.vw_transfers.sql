use coreDB
go

CREATE View tr.vw_transfers
with encryption  AS

SELECT        t.transfer_name, t.transfer_date, t.transfer_num, t.creator, t.creation_date, t.approved_by, t.approved_date, t.transfer_id, s.season_name, s.season_id, 
                         s.start_date AS season_starts, s.end_date AS season_ends, td.transfer_dtl_id, td.num_of_bags, td.commission_rate, td.price_per_bag, td.pprice, 
                         td.commission_amt, td.total_amt, da.district_acct_id, da.acct_num, bb.branch_name, bb.branch_id, b.bank_id, b.bank_name, d.district_id, d.district_name, 
                         d.phone_num, d.district_mgr, s2.sector_id, s2.sector_name
FROM            bank_branches AS bb INNER JOIN
                         tr.district_acct AS da ON bb.branch_id = da.branch_id INNER JOIN
                         banks AS b ON da.bank_id = b.bank_id INNER JOIN
                         tr.transfer AS t INNER JOIN
                         tr.transfer_dtl AS td ON t.transfer_id = td.transfer_id ON da.district_acct_id = td.district_acct_id INNER JOIN
                         tr.district AS d ON td.district_id = d.district_id AND td.district_id = d.district_id AND td.district_id = d.district_id INNER JOIN
                         tr.season AS s ON t.season_id = s.season_id AND t.season_id = s.season_id INNER JOIN
                         tr.sector AS s2 ON d.sector_id = s2.sector_id AND d.sector_id = s2.sector_id

GO
 