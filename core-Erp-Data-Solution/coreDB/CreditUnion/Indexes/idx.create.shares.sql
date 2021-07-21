use coreDB
go

create index ix_creditUnionShareTransaction_member on cu.creditUnionShareTransaction
(
	creditUnionMemberID  asc
)
go

create index ix_creditUnionShareTransaction_transactionDate on cu.creditUnionShareTransaction
(
	transactionDate  asc
)
go

create index ix_creditUnionMember_client on cu.creditUnionMember
(
	clientID  asc
)
go
