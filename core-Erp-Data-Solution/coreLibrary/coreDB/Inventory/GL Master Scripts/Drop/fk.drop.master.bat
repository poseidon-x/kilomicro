set sqlserver=".\sqlexpress"
for %%i in ("..\..\GL\Create\Foreign Keys\fk*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit