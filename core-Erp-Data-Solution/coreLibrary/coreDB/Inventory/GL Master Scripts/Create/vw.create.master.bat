set sqlserver="."
for %%i in ("..\..\GL\Create\Views\vw*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit