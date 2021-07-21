use coreDB
go
 
alter table ln.agentImage
	add constraint fk_agent_agent foreign key(agentID)
	references ln.agent(agentID)
go
	
alter table ln.agentImage
	add constraint fk_agent_image foreign key(imageID)
	references ln.[image](imageID)
go
	
alter table ln.agent
	add constraint fk_agent_branch foreign key(branchID)
	references ln.branch(branchID)
go
 	
alter table ln.agent
	add constraint fk_agent_accountType foreign key(accountTypeID)
	references bankAccountType(accountTypeID)
go
 	
alter table ln.agentAddress
	add constraint fk_agentAddress_agent foreign key(agentID)
	references ln.agent(agentID)
go
		
alter table ln.agentAddress
	add constraint fk_agentAddress_address foreign key(addressID)
	references ln.address(addressID)
go

	
alter table ln.agentPhone
	add constraint fk_agentPhone_agent foreign key(agentID)
	references ln.agent(agentID)
go
		
alter table ln.agentPhone
	add constraint fk_agentPhone_phone foreign key(phoneID)
	references ln.phone(phoneID)
go


alter table ln.agentEmail
	add constraint fk_agentEmail_agent foreign key(agentID)
	references ln.agent(agentID)
go
		
alter table ln.agentEmail
	add constraint fk_agentEmail_email foreign key(emailID)
	references ln.email(emailID)
go



alter table ln.agentDocument add
	constraint fk_agentDocument_agent foreign key (agentID)
	references ln.agent(agentID)
go

alter table ln.agentDocument add
	constraint fk_agentDocument_document foreign key (documentID)
	references ln.document(documentID)
go
 
alter table ln.agentNextOfKin add
	constraint fk_agentNextOfKin_agent foreign key (agentID)
	references ln.agent(agentID)
go
 
alter table ln.agentNextOfKin
	add constraint fk_agentNextOfKin_image foreign key(imageID)
	references ln.[image](imageID)
go
 
alter table ln.agentNextOfKin
	add constraint fk_agentNextOfKin_phone foreign key(phoneID)
	references ln.phone(phoneID)
go
 
alter table ln.agentNextOfKin
	add constraint fk_agentNextOfKin_email foreign key(emailID)
	references ln.email(emailID)
go
 
alter table ln.agentNextOfKin
	add constraint fk_nagentNextOfKin_idNo foreign key(idNoID)
	references ln.idNo(idNoID)
go
  