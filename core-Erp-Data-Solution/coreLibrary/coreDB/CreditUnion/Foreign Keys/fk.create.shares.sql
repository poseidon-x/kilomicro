use coreDB
go

alter table cu.creditUnionShareTransaction add
	constraint fk_creditUnionShareTransaction_creditUnionMember foreign key (creditUnionMemberID)
	references cu.creditUnionMember (creditUnionMemberID)
go

alter table cu.creditUnionMember add
	constraint fk_creditUnionMember_creditUnionChapter foreign key (creditUnionChapterID)
	references cu.creditUnionChapter (creditUnionChapterID)
go
