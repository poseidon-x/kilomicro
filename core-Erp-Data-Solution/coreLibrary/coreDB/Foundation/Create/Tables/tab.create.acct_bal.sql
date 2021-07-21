
CREATE TABLE dbo.acct_bals
(
	acct_bal_id	int	identity(1,1) not null constraint pk_acct_bals primary key,
	acct_id	int		not null,
	acct_period	int		not null,
	buy_rate	float		not null,
	sell_rate	float		not null,
	loc_bal	float		not null,
	frgn_bal	float		not null,
	currency_id	int		not null,
	creation_date	datetime	null,
	creator	nvarchar(50)		not null,
	modification_date	datetime	null,
	last_modifier	nvarchar(50)	null
)
 