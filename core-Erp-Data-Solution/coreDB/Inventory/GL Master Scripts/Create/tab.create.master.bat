set sqlserver="."
for %%i in ("..\..\GL\Create\Tables\tab*.sql") do sqlcmd -S%sqlserver% -E -i"%%i" 
exit