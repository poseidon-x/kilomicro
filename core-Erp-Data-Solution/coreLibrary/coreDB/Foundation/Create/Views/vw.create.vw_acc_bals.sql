use coreDB
go

alter View vw_acc_bals
with encryption  AS
	select
		'' acc_num,
		'' acc_name,
		'' head_name1,
		'' head_name2,
		'' head_name3,
		'' head_name4,
		0 cat_code,
		'' cat_name,
		'' major_name,
		'' major_symbol,
		isnull(cast(0 as float ),0) as loc_end_bal,
		isnull(cast(0 as float ),0) as frgn_end_bal,
		isnull(cast(0 as float ),0) as loc_beg_bal,
		isnull(cast(0 as float ),0) as frgn_beg_bal,
		'' minor_name,
		'' minor_symbol,
		0 acct_id,
		'' head_name5,
		'' head_name6,
		'' head_name7,
		0 currency_id,
		isnull(cast(0 as float ),0) as cur_rate,
		isnull(cast(0 as float ),0) as bud_bal,
		isnull(cast(0 as float ),0) as credit,
		isnull(cast(0 as float ),0) as debit


GO
 