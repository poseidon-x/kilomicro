set sqlserver="."
for %%i in ("..\..\GL\Create\Triggers\trg*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit