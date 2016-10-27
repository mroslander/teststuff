@echo off
sqlcmd.exe -S ".\SQLEXPRESS" -Q "drop database EFTrackingStore"
echo EFTrackingStore successfully removed
pause