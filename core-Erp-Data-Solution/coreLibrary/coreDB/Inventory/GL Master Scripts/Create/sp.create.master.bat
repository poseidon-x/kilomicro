set sqlserver="."
for %%i in ("..\..\GL\Create\Stored Procedures\sp*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit