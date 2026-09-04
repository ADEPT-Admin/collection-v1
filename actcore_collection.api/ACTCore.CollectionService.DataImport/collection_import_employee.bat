@echo off
echo Start Time: %date% %time%

cd /d "D:\Adaptivate\ACT_Collection\actcore_collection.api\ACTCore.CollectionService.DataImport\bin\Release\prod-package"

ACTCore.CollectionService.DataImport.exe EmployeeProfile "D:\DATA_TEST\COLLECTION_EMPLOYEE\EmployeeProfile"

echo End Time: %date% %time%
pause