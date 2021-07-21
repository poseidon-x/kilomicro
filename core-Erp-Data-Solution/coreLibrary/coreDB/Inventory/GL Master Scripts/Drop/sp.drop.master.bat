set sqlserver=".\sqlexpress"
for %%i in ("..\..\GL\Drop\Stored Procedures\sp*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit