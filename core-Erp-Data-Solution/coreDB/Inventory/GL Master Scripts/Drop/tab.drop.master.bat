set sqlserver=".\sqlexpress"
for %%i in ("..\..\GL\Drop\Tables\tab*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit