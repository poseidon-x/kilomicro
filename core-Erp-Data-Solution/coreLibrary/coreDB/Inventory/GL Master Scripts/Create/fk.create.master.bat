set sqlserver="."
for %%i in ("..\..\GL\Drop\Foreign Keys\fk*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit