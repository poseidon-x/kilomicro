use coreDB
go

drop VIEW tr.vw_transfers3
go

CREATE VIEW tr.vw_transfers3
	AS 
	SELECT        t.transfer_id, t.transfer_name, t.transfer_date, t.transfer_num, d.district_name, isnull(d.district_mgr,'') as district_mgr, 
                         isnull(d.phone_num,'') as phone_num, s2.season_name, s.sector_name, s2.season_id, s.sector_id, d.district_id, 
                         ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 1)), '') AS bank1, ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 2)), '') AS bank2, 
                         ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 3)), '') AS bank3, ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 4)), '') AS bank4, 
                         ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 5)), '') AS bank5, ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 6)), '') AS bank6, 
                         ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 7)), '') AS bank7, ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 8)), '') AS bank8, 
                         ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 9)), '') AS bank9, ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 10)), '') AS bank10, 
                         ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 1), d.district_id), 0) AS qty1, ISNULL(tr.transfer_qty(t.transfer_id, 
                         tr.bank_walk(t.transfer_id, 2), d.district_id), 0) AS qty2, ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 3), 
                         d.district_id), 0) AS qty3, ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 4), d.district_id), 0) AS qty4, 
                         ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 5), d.district_id), 0) AS qty5, ISNULL(tr.transfer_qty(t.transfer_id, 
                         tr.bank_walk(t.transfer_id, 6), d.district_id), 0) AS qty6, ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 7), 
                         d.district_id), 0) AS qty7, ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 8), d.district_id), 0) AS qty81, 
                         ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 9), d.district_id), 0) AS qty9, ISNULL(tr.transfer_qty(t.transfer_id, 
                         tr.bank_walk(t.transfer_id, 10), d.district_id), 0) AS qty10, ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 1), 
                         d.district_id, 1), 0) AS amt1_1, ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 1), d.district_id, 2), 0) AS amt1_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 1), d.district_id, 3), 0) AS amt1_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 2), d.district_id, 1), 0) AS amt2_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 2), d.district_id, 2), 0) AS amt2_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 2), d.district_id, 3), 0) AS amt2_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 3), d.district_id, 1), 0) AS amt3_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 3), d.district_id, 2), 0) AS amt3_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 3), d.district_id, 3), 0) AS amt3_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 4), d.district_id, 1), 0) AS amt4_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 4), d.district_id, 2), 0) AS amt4_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 4), d.district_id, 3), 0) AS amt4_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 5), d.district_id, 1), 0) AS amt5_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 5), d.district_id, 2), 0) AS amt5_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 5), d.district_id, 3), 0) AS amt5_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 6), d.district_id, 1), 0) AS amt6_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 6), d.district_id, 2), 0) AS amt6_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 6), d.district_id, 3), 0) AS amt6_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 7), d.district_id, 1), 0) AS amt7_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 7), d.district_id, 2), 0) AS amt7_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 7), d.district_id, 3), 0) AS amt7_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 8), d.district_id, 1), 0) AS amt8_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 8), d.district_id, 2), 0) AS amt8_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 8), d.district_id, 3), 0) AS amt8_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 9), d.district_id, 1), 0) AS amt9_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 9), d.district_id, 2), 0) AS amt9_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 9), d.district_id, 3), 0) AS amt9_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 10), d.district_id, 1), 0) AS amt10_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 10), d.district_id, 2), 0) AS amt10_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 10), d.district_id, 3), 0) AS amt10_3
FROM            tr.transfer t INNER JOIN
                         tr.season s2 ON t.season_id = s2.season_id INNER JOIN
                         tr.transfer_dtl td ON t.transfer_id = td.transfer_id INNER JOIN
                         tr.district d ON td.district_id = d.district_id INNER JOIN
                         tr.sector s ON d.sector_id = s.sector_id
GROUP BY t.transfer_id, t.transfer_name, t.transfer_date, t.transfer_num, d.district_name, d.district_mgr, 
                         d.phone_num, s2.season_name, s.sector_name, s2.season_id, s.sector_id, d.district_id
union all
select *
from
(
SELECT        t.transfer_id, t.transfer_name, t.transfer_date, t.transfer_num, d.district_name, isnull(d.district_mgr,'') as district_mgr, 
                         isnull(d.phone_num,'') as phone_num, s2.season_name, s.sector_name, s2.season_id, s.sector_id, d.district_id, 
                         ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 1)), '') AS bank1, ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 2)), '') AS bank2, 
                         ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 3)), '') AS bank3, ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 4)), '') AS bank4, 
                         ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 5)), '') AS bank5, ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 6)), '') AS bank6, 
                         ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 7)), '') AS bank7, ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 8)), '') AS bank8, 
                         ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 9)), '') AS bank9, ISNULL(tr.bank_name(tr.bank_walk(t.transfer_id, 10)), '') AS bank10, 
                         ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 1), d.district_id), 0) AS qty1, ISNULL(tr.transfer_qty(t.transfer_id, 
                         tr.bank_walk(t.transfer_id, 2), d.district_id), 0) AS qty2, ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 3), 
                         d.district_id), 0) AS qty3, ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 4), d.district_id), 0) AS qty4, 
                         ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 5), d.district_id), 0) AS qty5, ISNULL(tr.transfer_qty(t.transfer_id, 
                         tr.bank_walk(t.transfer_id, 6), d.district_id), 0) AS qty6, ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 7), 
                         d.district_id), 0) AS qty7, ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 8), d.district_id), 0) AS qty81, 
                         ISNULL(tr.transfer_qty(t.transfer_id, tr.bank_walk(t.transfer_id, 9), d.district_id), 0) AS qty9, ISNULL(tr.transfer_qty(t.transfer_id, 
                         tr.bank_walk(t.transfer_id, 10), d.district_id), 0) AS qty10, ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 1), 
                         d.district_id, 1), 0) AS amt1_1, ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 1), d.district_id, 2), 0) AS amt1_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 1), d.district_id, 3), 0) AS amt1_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 2), d.district_id, 1), 0) AS amt2_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 2), d.district_id, 2), 0) AS amt2_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 2), d.district_id, 3), 0) AS amt2_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 3), d.district_id, 1), 0) AS amt3_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 3), d.district_id, 2), 0) AS amt3_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 3), d.district_id, 3), 0) AS amt3_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 4), d.district_id, 1), 0) AS amt4_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 4), d.district_id, 2), 0) AS amt4_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 4), d.district_id, 3), 0) AS amt4_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 5), d.district_id, 1), 0) AS amt5_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 5), d.district_id, 2), 0) AS amt5_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 5), d.district_id, 3), 0) AS amt5_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 6), d.district_id, 1), 0) AS amt6_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 6), d.district_id, 2), 0) AS amt6_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 6), d.district_id, 3), 0) AS amt6_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 7), d.district_id, 1), 0) AS amt7_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 7), d.district_id, 2), 0) AS amt7_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 7), d.district_id, 3), 0) AS amt7_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 8), d.district_id, 1), 0) AS amt8_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 8), d.district_id, 2), 0) AS amt8_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 8), d.district_id, 3), 0) AS amt8_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 9), d.district_id, 1), 0) AS amt9_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 9), d.district_id, 2), 0) AS amt9_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 9), d.district_id, 3), 0) AS amt9_3, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 10), d.district_id, 1), 0) AS amt10_1, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 10), d.district_id, 2), 0) AS amt10_2, 
                         ISNULL(tr.transfer_amt(t.transfer_id, tr.bank_walk(t.transfer_id, 10), d.district_id, 3), 0) AS amt10_3
FROM            tr.transfer t INNER JOIN
                         tr.season s2 ON t.season_id = s2.season_id INNER JOIN
                         tr.transfer_dtl td ON t.transfer_id = td.transfer_id INNER JOIN
                         tr.district d ON td.district_id <> d.district_id INNER JOIN
                         tr.sector s ON d.sector_id = s.sector_id
GROUP BY t.transfer_id, t.transfer_name, t.transfer_date, t.transfer_num, d.district_name, d.district_mgr, 
                         d.phone_num, s2.season_name, s.sector_name, s2.season_id, s.sector_id, d.district_id
) t
where qty1+qty2+qty3+qty4+qty5+qty6+qty7+qty81+qty9+qty10=0
go

