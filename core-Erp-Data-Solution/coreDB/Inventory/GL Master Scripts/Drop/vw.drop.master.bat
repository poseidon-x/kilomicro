set sqlserver=".\sqlexpress"
for %%i in ("..\..\GL\Drop\Views\vw*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit