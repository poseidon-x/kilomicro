use coreDB
go

CREATE Trigger on_insert_client ON ln.client 
with encryption 

FOR INSERT AS 

GO
