set sqlserver=".\sqlexpress"
for %%i in ("..\..\GL\Drop\Triggers\trg*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit