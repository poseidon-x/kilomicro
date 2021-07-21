set sqlserver=".\sqlexpress"
for %%i in ("..\..\GL\Drop\Functions\fn*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit