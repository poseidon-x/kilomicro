set sqlserver="."
for %%i in ("..\..\GL\Create\Data\dat*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit