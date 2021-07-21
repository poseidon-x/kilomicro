use coreDB
GO
set identity_insert pref.roleMenu on
insert pref.roleMenu(roleMenuID,roleName,description,iconFile) values(1,'Cashier','Click this if you are a cashier','cashier.jpg')
insert pref.roleMenu(roleMenuID,roleName,description,iconFile) values(2,'Credit','Click this of your are a credit officer or manager','credit.jpg')
insert pref.roleMenu(roleMenuID,roleName,description,iconFile) values(3,'Accounting','This is for accounts officers, controllers, auditors and other finance department staff','finance.jpg')
insert pref.roleMenu(roleMenuID,roleName,description,iconFile) values(4,'HR & Payroll','Click here if you are in the human capital/resource management department','hr.jpg')
insert pref.roleMenu(roleMenuID,roleName,description,iconFile) values(5,'Management','This link for senior staff and management','management.jpg')
insert pref.roleMenu(roleMenuID,roleName,description,iconFile) values(6,'Front Desk','This link is for CSO''s, Receptionists and Representatives','receptionist.jpg')
insert pref.roleMenu(roleMenuID,roleName,description,iconFile) values(7,'IT Operations','The link for IT and allied staff','it.jpg')
insert pref.roleMenu(roleMenuID,roleName,description,iconFile) values(8,'Loan Administration','Link for loan administrators and operations staff','loan.jpg')
insert pref.roleMenu(roleMenuID,roleName,description,iconFile) values(9,'Investments Administration','Link for investments portfolio managers','investment.jpg')
set identity_insert pref.roleMenu off
GO
update statistics pref.roleMenu
GO
