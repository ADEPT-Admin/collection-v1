@echo off
echo Start Time: %date% %time%
set IMPORT_EXE_PATH=D:\Adaptivate\ACT_Collection\actcore_collection.api\ACTCore.CollectionService.MappingConsole\bin\Debug\net9.0\ACTCore.CollectionService.DataImport.exe

REM call "%IMPORT_EXE_PATH%" Contract "D:\DATA_TEST\COLLECTION_CONTRACT\Contract"

ACTCore.CollectionService.DataImport EmployeeProfile "D:\DATA_TEST\COLLECTION_EMPLOYEE\EmployeeProfile"

echo End Time: %date% %time%
pause