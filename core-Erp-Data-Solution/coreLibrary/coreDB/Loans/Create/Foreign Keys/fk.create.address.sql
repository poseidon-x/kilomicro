use coreDB
go


alter table ln.[addressImage]
	add constraint fk_addressImage_address foreign key(addressID)
	references ln.[address](addressID)
go

alter table ln.[addressImage]
	add constraint fk_addressImage_image foreign key(imageID)
	references ln.[image](imageID)
go

alter table ln.[address] add
	constraint fk_address_ownershipType foreign key(ownerShipTypeID)
	references ln.ownerShipType(ownerShipTypeID)
go
