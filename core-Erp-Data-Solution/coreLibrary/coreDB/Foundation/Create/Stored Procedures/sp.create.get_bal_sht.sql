use coreDB
go


alter PROCEDURE get_bal_sht
(
	@start_date datetime,
	@end_date datetime,
	@no_tx bit,
	@cost_center_id int = null
)
with encryption
AS
BEGIN
declare 
	@tbl TABLE 
(
	acct_id int,
	acc_num nvarchar(50),
	acc_name nvarchar(250),
	head_name1 nvarchar(100),
	head_name2 nvarchar(100),
	head_name3 nvarchar(100),
	head_name4 nvarchar(100),
	head_name5 nvarchar(100),
	head_name6 nvarchar(100),
	head_name7 nvarchar(100),
	cat_code int,
	cat_name nvarchar(100),
	major_name nvarchar(50),
	major_symbol nvarchar(3),
	minor_name nvarchar(50),
	minor_symbol nvarchar(3),
	currency_id int,
	cur_rate float,
	loc_end_bal float,
	frgn_end_bal float,
	loc_beg_bal float,
	frgn_beg_bal float,
	bud_bal float,
	debit float,
	credit float
)
	declare @fin_year_start datetime
	
	select @fin_year_start = dbo.fin_year_start(@end_date)
	
	insert into @tbl
	select
		acct_id,
		acc_num,
		acc_name,
		isnull(head_name1,''),
		isnull(head_name2,''),
		isnull(head_name3,''),
		isnull(head_name4,''),
		isnull(head_nam5,''),
		isnull(head_name6,''),
		isnull(head_name7,''),
		isnull(cat_code,''),
		isnull(cat_name,''),
		isnull(major_name,''),
		isnull(major_symbol,''),
		isnull(minor_name,''),
		isnull(minor_symbol,''),
		isnull(currency_id,0),
		isnull((select current_buy_rate from currencies c where c.currency_id = a.currency_id),0),
		isnull(dbo.acc_bal(acct_id, cat_code, @start_date, @end_date, @cost_center_id),0) as loc_end_bal,
		0 as frgn_end_bal,
		isnull(dbo.acc_bal(acct_id, cat_code, dbo.fin_year_start(dateadd(ss,-1, @start_date)), dateadd(ss,-1, @start_date), @cost_center_id  ),0) as loc_beg_bal,
		0 as frgn_beg_bal,
		isnull(gl.bud_bal(acct_id, @end_date, @cost_center_id), 0) as bud_bal,
		isnull(dbo.acc_debit(acct_id, cat_code, @start_date, @end_date, @cost_center_id),0) as debit,
		isnull(dbo.acc_credit(acct_id, cat_code, @start_date, @end_date, @cost_center_id),0) as credit
	from vw_accounts a
	where cat_code <=3  
		and ( acct_id not in (select acct_id from def_accts where code in ('CE','RE')) )
	union all
	select
		acct_id,
		acc_num,
		acc_name,
		isnull(head_name1,''),
		isnull(head_name2,''),
		isnull(head_name3,''),
		isnull(head_name4,''),
		isnull(head_nam5,''),
		isnull(head_name6,''),
		isnull(head_name7,''),
		isnull(cat_code,''),
		isnull(cat_name,''),
		isnull(major_name,''),
		isnull(major_symbol,''),
		isnull(minor_name,''),
		isnull(minor_symbol,''),
		isnull(currency_id,0),
		isnull((select current_buy_rate from currencies c where c.currency_id = a.currency_id),0),
		isnull(dbo.cur_earn(@end_date, @cost_center_id),0) as loc_end_bal,
		0 as frgn_end_bal,
		0 as loc_beg_bal,
		0 as frgn_beg_bal,
		isnull(gl.bud_bal(acct_id, @end_date, @cost_center_id), 0) as bud_bal,
		isnull(0, 0) as debit,
		isnull(0, 0) as credit
	from vw_accounts a
	where acct_id in (select acct_id from def_accts where code='CE')
	union all
	select
		acct_id,
		acc_num,
		acc_name,
		isnull(head_name1,''),
		isnull(head_name2,''),
		isnull(head_name3,''),
		isnull(head_name4,''),
		isnull(head_nam5,''),
		isnull(head_name6,''),
		isnull(head_name7,''),
		isnull(cat_code,''),
		isnull(cat_name,''),
		isnull(major_name,''),
		isnull(major_symbol,''),
		isnull(minor_name,''),
		isnull(minor_symbol,''),
		isnull(currency_id,0),
		isnull((select current_buy_rate from currencies c where c.currency_id = a.currency_id),0),
		isnull(dbo.ret_earn(@end_date, @cost_center_id),0) as loc_end_bal,
		0 as frgn_end_bal,
		0 as loc_beg_bal,
		0 as frgn_beg_bal,
		isnull(gl.bud_bal(acct_id, @end_date, @cost_center_id), 0) as bud_bal,
		isnull(0, 0) as debit,
		isnull(0,0) as credit
	from vw_accounts a
	where acct_id in (select acct_id from def_accts where code='RE')
	
	select * from @tbl 
	where  @no_tx=1 or abs(loc_end_bal)>0.009 or abs(loc_beg_bal)>0.009 or abs(bud_bal)>0.009
		or abs(debit) > 0.009 or abs(credit) > 0.009
END
GO

alter PROCEDURE get_bal_sht_sum
(
	@start_date datetime,
	@end_date datetime,
	@no_tx bit, 
	@sum bit = 0,
	@cost_center_id int = null
)
with encryption
AS
BEGIN
declare 
	@tbl TABLE 
(
	acct_id int,
	acc_num nvarchar(50),
	acc_name nvarchar(250),
	head_name1 nvarchar(100),
	head_name2 nvarchar(100),
	head_name3 nvarchar(100),
	head_name4 nvarchar(100),
	head_name5 nvarchar(100),
	head_name6 nvarchar(100),
	head_name7 nvarchar(100),
	cat_code int,
	cat_name nvarchar(100),
	major_name nvarchar(50),
	major_symbol nvarchar(3),
	minor_name nvarchar(50),
	minor_symbol nvarchar(3),
	currency_id int,
	cur_rate float,
	loc_end_bal float,
	frgn_end_bal float,
	loc_beg_bal float,
	frgn_beg_bal float,
	bud_bal float,
	debit float,
	credit float
)
	declare @fin_year_start datetime
	
	select @fin_year_start = dbo.fin_year_start(@end_date)
	
	insert into @tbl
	select
		case when @sum=1 then 0 
			 else acct_id
			end as acct_id,		
		case when @sum=1 then '' 
			 else acc_num
			end as acc_num,
		case when @sum=1 and head_name4 is not null and head_name4<>'' then head_name4
		     when @sum=1 and head_name3 is not null and head_name3<>'' then head_name3
			 when @sum=1 and head_name2 is not null and head_name2<>'' then head_name2
			 when @sum=1 and head_name1 is not null and head_name1<>'' then head_name1
			 else acc_name
			end as acc_name,
		isnull(head_name1,''),
		isnull(case when @sum=1 and (head_name3 is null or head_name3='') then ''
				else head_name2 end,''),
		isnull(case when @sum=1 and (head_name4 is null or head_name4='') then ''
				else head_name3 end,''),
		isnull(case when @sum=1 and (head_nam5 is null or head_nam5='') then ''
				else head_name4 end,''),
		isnull(case when @sum=1 and (head_name6 is null or head_name6='') then ''
				else head_nam5 end,''),
		isnull(case when @sum=1 and (head_name7 is null or head_name7='') then ''
				else head_name6 end,''),
		isnull(head_name7,''),
		isnull(cat_code,''),
		isnull(cat_name,''),
		isnull(major_name,''),
		isnull(major_symbol,''),
		isnull(minor_name,''),
		isnull(minor_symbol,''),
		isnull(currency_id,0),
		isnull((select current_buy_rate from currencies c where c.currency_id = a.currency_id),0),
		sum(isnull(dbo.acc_bal(acct_id, cat_code, @start_date, @end_date, @cost_center_id),0)) as loc_end_bal,
		0 as frgn_end_bal,
		sum(isnull(dbo.acc_bal(acct_id, cat_code, dbo.fin_year_start(dateadd(ss,-1, @start_date)), dateadd(ss,-1, @start_date), @cost_center_id  ),0)) as loc_beg_bal,
		0 as frgn_beg_bal,
		sum(isnull(gl.bud_bal(acct_id, @end_date, @cost_center_id), 0)) as bud_bal,
		sum(isnull(dbo.acc_debit(acct_id, cat_code, @start_date, @end_date, @cost_center_id),0)) as debit,
		sum(isnull(dbo.acc_credit(acct_id, cat_code, @start_date, @end_date, @cost_center_id),0)) as credit
	from vw_accounts a
	where cat_code <=3 
		and acct_id not in (select acct_id from def_accts where code in ('CE','RE'))
	group by 
		case when @sum=1 then 0 
			 else acct_id
			end ,		
		case when @sum=1 then '' 
			 else acc_num
			end ,
		case when @sum=1 and head_name4 is not null and head_name4<>'' then head_name4
		     when @sum=1 and head_name3 is not null and head_name3<>'' then head_name3
			 when @sum=1 and head_name2 is not null and head_name2<>'' then head_name2
			 when @sum=1 and head_name1 is not null and head_name1<>'' then head_name1
			 else acc_name
			end ,
		isnull(head_name1,''),
		isnull(case when @sum=1 and (head_name3 is null or head_name3='') then ''
				else head_name2 end,''),
		isnull(case when @sum=1 and (head_name4 is null or head_name4='') then ''
				else head_name3 end,''),
		isnull(case when @sum=1 and (head_nam5 is null or head_nam5='') then ''
				else head_name4 end,''),
		isnull(case when @sum=1 and (head_name6 is null or head_name6='') then ''
				else head_nam5 end,''),
		isnull(case when @sum=1 and (head_name7 is null or head_name7='') then ''
				else head_name6 end,''),
		isnull(head_name7,''),
		isnull(cat_code,''),
		isnull(cat_name,''),
		isnull(major_name,''),
		isnull(major_symbol,''),
		isnull(minor_name,''),
		isnull(minor_symbol,''),
		currency_id
	union all
	select
		case when @sum=1 then 0 
			 else acct_id
			end as acct_id,		
		case when @sum=1 then '' 
			 else acc_num
			end as acc_num,
		case when @sum=1 and head_name4 is not null and head_name4<>'' then head_name4
		     when @sum=1 and head_name3 is not null and head_name3<>'' then head_name3
			 when @sum=1 and head_name2 is not null and head_name2<>'' then head_name2
			 when @sum=1 and head_name1 is not null and head_name1<>'' then head_name1
			 else acc_name
			end as acc_name,
		isnull(head_name1,''),
		isnull(case when @sum=1 and (head_name3 is null or head_name3='') then ''
				else head_name2 end,''),
		isnull(case when @sum=1 and (head_name4 is null or head_name4='') then ''
				else head_name3 end,''),
		isnull(case when @sum=1 and (head_nam5 is null or head_nam5='') then ''
				else head_name4 end,''),
		isnull(case when @sum=1 and (head_name6 is null or head_name6='') then ''
				else head_nam5 end,''),
		isnull(case when @sum=1 and (head_name7 is null or head_name7='') then ''
				else head_name6 end,''),
		isnull(head_name7,''),
		isnull(cat_code,''),
		isnull(cat_name,''),
		isnull(major_name,''),
		isnull(major_symbol,''),
		isnull(minor_name,''),
		isnull(minor_symbol,''),
		isnull(currency_id,0),
		isnull((select current_buy_rate from currencies c where c.currency_id = a.currency_id),0),
		sum(isnull(dbo.cur_earn(@end_date, @cost_center_id),0)) as loc_end_bal,
		0 as frgn_end_bal,
		0 as loc_beg_bal,
		0 as frgn_beg_bal,
		sum(isnull(gl.bud_bal(acct_id, @end_date, @cost_center_id), 0)) as bud_bal,
		sum(isnull(dbo.acc_debit(acct_id, cat_code, @start_date, @end_date, @cost_center_id),0)) as debit,
		sum(isnull(dbo.acc_credit(acct_id, cat_code, @start_date, @end_date, @cost_center_id),0)) as credit
	from vw_accounts a
	where acct_id in (select acct_id from def_accts where code='CE')
	group by
		case when @sum=1 then 0 
			 else acct_id
			end ,		
		case when @sum=1 then '' 
			 else acc_num
			end,
		case when @sum=1 and head_name4 is not null and head_name4<>'' then head_name4
		     when @sum=1 and head_name3 is not null and head_name3<>'' then head_name3
			 when @sum=1 and head_name2 is not null and head_name2<>'' then head_name2
			 when @sum=1 and head_name1 is not null and head_name1<>'' then head_name1
			 else acc_name
			end ,
		isnull(head_name1,''),
		isnull(case when @sum=1 and (head_name3 is null or head_name3='') then ''
				else head_name2 end,''),
		isnull(case when @sum=1 and (head_name4 is null or head_name4='') then ''
				else head_name3 end,''),
		isnull(case when @sum=1 and (head_nam5 is null or head_nam5='') then ''
				else head_name4 end,''),
		isnull(case when @sum=1 and (head_name6 is null or head_name6='') then ''
				else head_nam5 end,''),
		isnull(case when @sum=1 and (head_name7 is null or head_name7='') then ''
				else head_name6 end,''),
		isnull(head_name7,''),
		isnull(cat_code,''),
		isnull(cat_name,''),
		isnull(major_name,''),
		isnull(major_symbol,''),
		isnull(minor_name,''),
		isnull(minor_symbol,''),
		currency_id
	union all
	select
		case when @sum=1 then 0 
			 else acct_id
			end as acct_id,		
		case when @sum=1 then '' 
			 else acc_num
			end as acc_num,
		case when @sum=1 and head_name4 is not null and head_name4<>'' then head_name4
		     when @sum=1 and head_name3 is not null and head_name3<>'' then head_name3
			 when @sum=1 and head_name2 is not null and head_name2<>'' then head_name2
			 when @sum=1 and head_name1 is not null and head_name1<>'' then head_name1
			 else acc_name
			end as acc_name,
		isnull(head_name1,''),
		isnull(case when @sum=1 and (head_name3 is null or head_name3='') then ''
				else head_name2 end,''),
		isnull(case when @sum=1 and (head_name4 is null or head_name4='') then ''
				else head_name3 end,''),
		isnull(case when @sum=1 and (head_nam5 is null or head_nam5='') then ''
				else head_name4 end,''),
		isnull(case when @sum=1 and (head_name6 is null or head_name6='') then ''
				else head_nam5 end,''),
		isnull(case when @sum=1 and (head_name7 is null or head_name7='') then ''
				else head_name6 end,''),
		isnull(head_name7,''),
		isnull(cat_code,''),
		isnull(cat_name,''),
		isnull(major_name,''),
		isnull(major_symbol,''),
		isnull(minor_name,''),
		isnull(minor_symbol,''),
		isnull(currency_id,0),
		isnull((select current_buy_rate from currencies c where c.currency_id = a.currency_id),0),
		sum(isnull(dbo.ret_earn(@end_date, @cost_center_id),0)) as loc_end_bal,
		0 as frgn_end_bal,
		0 as loc_beg_bal,
		0 as frgn_beg_bal,
		sum(isnull(gl.bud_bal(acct_id, @end_date, @cost_center_id), 0)) as bud_bal,
		sum(isnull(dbo.acc_debit(acct_id, cat_code, @start_date, @end_date, @cost_center_id),0)) as debit,
		sum(isnull(dbo.acc_credit(acct_id, cat_code, @start_date, @end_date, @cost_center_id),0)) as credit
	from vw_accounts a
	where acct_id in (select acct_id from def_accts where code='RE')
	group by 	
		case when @sum=1 then 0 
			 else acct_id
			end ,		
		case when @sum=1 then '' 
			 else acc_num
			end ,
		case when @sum=1 and head_name4 is not null and head_name4<>'' then head_name4
		     when @sum=1 and head_name3 is not null and head_name3<>'' then head_name3
			 when @sum=1 and head_name2 is not null and head_name2<>'' then head_name2
			 when @sum=1 and head_name1 is not null and head_name1<>'' then head_name1
			 else acc_name
			end ,
		isnull(head_name1,''),
		isnull(case when @sum=1 and (head_name3 is null or head_name3='') then ''
				else head_name2 end,''),
		isnull(case when @sum=1 and (head_name4 is null or head_name4='') then ''
				else head_name3 end,''),
		isnull(case when @sum=1 and (head_nam5 is null or head_nam5='') then ''
				else head_name4 end,''),
		isnull(case when @sum=1 and (head_name6 is null or head_name6='') then ''
				else head_nam5 end,''),
		isnull(case when @sum=1 and (head_name7 is null or head_name7='') then ''
				else head_name6 end,''),
		isnull(head_name7,''),
		isnull(cat_code,''),
		isnull(cat_name,''),
		isnull(major_name,''),
		isnull(major_symbol,''),
		isnull(minor_name,''),
		isnull(minor_symbol,''),
		currency_id
	
	select * from @tbl 
	where  @no_tx=1 or abs(loc_end_bal)>0.009 or abs(loc_beg_bal)>0.009 or abs(bud_bal)>0.009
		or abs(debit) > 0.009 or abs(credit) > 0.009
END
GO