use coreDB
go

update msg.messagingConfig set
	httpMessagingUrl = 'https://api.smsgh.com/v3/messages/send?From=$$SENDER$$&To=$$PHONE_NUMBER$$&Content=$$MESSAGE_BODY$$&ClientId=eqdcvyxo&ClientSecret=ulmfmzzm&RegisteredDelivery=true'
go
