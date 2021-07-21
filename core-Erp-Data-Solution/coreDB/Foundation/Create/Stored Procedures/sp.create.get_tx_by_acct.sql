use coreDB
go


alter PROCEDURE get_tx_by_acct
(
	@acct_id int,
	@start_date datetime,
	@end_date datetime,
	@no_tx bit,
	@frgn bit,
	@sourceID int,
	@batchNo nvarchar(30),
	@currency_id int,
	@refNo nvarchar(30),
	@cost_center_id int = null
)
with encryption
AS
BEGIN
declare @sd datetime, @ed datetime
	select @sd=
		cast(''+cast(datepart(yyyy,@start_Date) as nvarchar(4))+'-'+cast(datepart(mm,@start_Date) as nvarchar(2))
		+'-'+cast(datepart(dd,@start_Date) as nvarchar(2)) as datetime)
	select @ed=
		cast(''+cast(datepart(yyyy,@end_date) as nvarchar(4))+'-'+cast(datepart(mm,@end_date) as nvarchar(2))
		+'-'+cast(datepart(dd,@end_date) as nvarchar(2)) + ' 23:59:59' as datetime)
declare 
	@tbl TABLE 
(
	jnl_id int primary key,
	acct_id int,
	acc_num nvarchar(50),
	acc_name nvarchar(250),
	head_name1 nvarchar(250),
	head_name2 nvarchar(250),
	head_name3 nvarchar(250),
	head_name4 nvarchar(250),
	head_name5 nvarchar(250),
	head_name6 nvarchar(250),
	head_name7 nvarchar(250),
	cat_code int,
	cat_name nvarchar(250),
	major_name nvarchar(250),
	major_symbol nvarchar(30),
	minor_name nvarchar(250),
	minor_symbol nvarchar(30),
	currency_id int,
	loc_end_bal float,
	frgn_end_bal float,
	loc_beg_bal float,
	frgn_beg_bal float,
	tx_date datetime,
	description nvarchar(400),
	ref_no nvarchar(50),
	dbt_amt float,
	crdt_amt float,
	frgn_crdt_amt float,
	frgn_dbt_amt float,
	cost_center nvarchar(250),
	currency nvarchar(50),
	debit float,
	credit float,
	end_bal float,
	beg_bal float,
	batch_no nvarchar(30),
	creator nvarchar(30),
	creation_date datetime
)
	declare @tbl2 table
	(
		acct_id int,
		beg_bal float,
		end_bal float
	)
	insert into @tbl2
	select
		acct_id,
		isnull(dbo.acc_bal(acct_id, cat_code, dateadd(yy,-50,@start_date), dateadd(ss,-1, @start_date), @cost_center_id),0),
		isnull(dbo.acc_bal(acct_id, cat_code,  dateadd(yy,-50,@start_date), @end_date, @cost_center_id),0)
	from vw_accounts
	where
		(
			(@acct_id is null or acct_id = @acct_id) 
		)

	declare @fin_year_start datetime
	
	select @fin_year_start = dbo.fin_year_start(@end_date)
	 
	 --insert into @tbl
	select
		jnl_id,
		j.acct_id,
		acc_num,
		acc_name,
		isnull(head_name1,'') head_name1,
		isnull(head_name2,'') head_name2,
		isnull(head_name3,'') head_name3,
		isnull(head_name4,'') head_name4,
		isnull(a.head_nam5,'') head_name5,
		isnull(head_name6,'') head_name6,
		isnull(head_name7,'') head_name7,
		isnull(cast(cat_code as int),'') cat_code,
		isnull(a.cat_name,'') cat_name,
		isnull(c.major_name,'') major_name,
		isnull(c.major_symbol,'') major_symbol,
		isnull(c.minor_name,'') minor_name,
		isnull(c.minor_symbol,'') minor_symbol,
		isnull(c.currency_id,0) currency_id,
		isnull(end_bal,0) as loc_end_bal,
		isnull(end_bal,0) as frgn_end_bal,
		isnull(beg_bal,0) as loc_beg_bal,
		isnull(beg_bal,0) as frgn_beg_bal,
		j.tx_date,
		isnull(j.description,'') description,
		isnull(j.ref_no,0) ref_no,
		isnull(j.dbt_amt,0) dbt_amt,
		isnull(j.crdt_amt,0) crdt_amt,
		isnull(j.frgn_crdt_amt,0) frgn_crdt_amt,
		isnull(j.frgn_dbt_amt,0) frgn_dbt_amt,
		isnull(ou_name,'') as cost_center,
		case when p.currency_id is not null then c.major_name else 'LOCAL' end as currency,
		j.dbt_amt  as debit,
		crdt_amt  as credit,
		case when @currency_id is null then 
			end_bal else 
			end_bal end as end_bal,
		case when @currency_id is null then 
			beg_bal else 
			beg_bal end as 
			beg_bal,
		isnull(b.batch_no,'') batch_no,
		j.creator,
		j.creation_date
	from vw_accounts a 
		inner join @tbl2 t2 on a.acct_id=t2.acct_id
		inner join jnl j on a.acct_id = j.acct_id
		inner join jnl_batch b on j.jnl_batch_id = b.jnl_batch_id
		left outer join currencies c on j.currency_id = c.currency_id
		left outer join comp_prof p on c.currency_id = p.currency_id
		left join vw_gl_ou u on j.cost_center_id = u.ou_id
	where
		(
			(@acct_id is null or j.acct_id = @acct_id)
			and
			(@frgn is null or @frgn=b.multi_currency )
			and (@batchNo is null or b.batch_no=@batchNo)
			and (@refNo is null or j.ref_no=@refNo)
			and (@currency_id is null or j.currency_id = @currency_id)
			and 
			(
			 (cat_code <4 )
			 or
			 (acct_period is null or tx_date >= @fin_year_start)
			)
			and (tx_date <= @ed)
			and (tx_date >= @sd)
			and (@cost_center_id is null or len(ltrim(rtrim(@cost_center_id)))=0 or @cost_center_id=cost_center_id)
		)
	--order by j.tx_date, j.jnl_id
	/*
	select
	jnl_id,
	acct_id ,
	acc_num ,
	acc_name ,
	head_name1 ,
	head_name2 ,
	head_name3,
	head_name4,
	head_name5 ,
	head_name6 ,
	head_name7 ,
	cat_code ,
	cat_name ,
	major_name ,
	major_symbol ,
	minor_name ,
	minor_symbol ,
	currency_id ,
	loc_end_bal ,
	frgn_end_bal ,
	loc_beg_bal ,
	frgn_beg_bal ,
	tx_date ,
	[description],
	ref_no,
	dbt_amt,
	crdt_amt,
	frgn_crdt_amt,
	frgn_dbt_amt,
	cost_center,
	currency,
	debit,
	credit,
	end_bal,
	beg_bal,
	batch_no,
	creator,
	creation_date 
	 from @tbl  */
END
GO