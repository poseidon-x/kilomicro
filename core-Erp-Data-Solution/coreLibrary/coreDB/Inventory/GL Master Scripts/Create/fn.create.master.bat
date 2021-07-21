set sqlserver="."
for %%i in ("..\..\GL\Create\Functions\fn*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit