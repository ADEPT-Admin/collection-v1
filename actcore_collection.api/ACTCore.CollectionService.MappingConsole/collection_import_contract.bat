@echo off
echo Start Time: %date% %time%
set IMPORT_EXE_PATH=D:\Adaptivate\ACT_Collection\actcore_collection.api\ACTCore.CollectionService.MappingConsole\bin\Debug\net9.0\ACTCore.CollectionService.DataImport.exe

REM call "%IMPORT_EXE_PATH%" Contract "D:\DATA_TEST\COLLECTION_CONTRACT\Contract"

ACTCore.CollectionService.DataImport Contract "D:\DATA_TEST\COLLECTION_CONTRACT\Contract"
ACTCore.CollectionService.DataImport ContractAddress "D:\DATA_TEST\COLLECTION_CONTRACT\ContractAddress"
ACTCore.CollectionService.DataImport ContractAsset "D:\DATA_TEST\COLLECTION_CONTRACT\ContractAsset"
ACTCore.CollectionService.DataImport ContractAssetVehicle "D:\DATA_TEST\COLLECTION_CONTRACT\ContractAssetVehicle"
ACTCore.CollectionService.DataImport ContractOverdue "D:\DATA_TEST\COLLECTION_CONTRACT\ContractOverdue"
ACTCore.CollectionService.DataImport ContractPayment "D:\DATA_TEST\COLLECTION_CONTRACT\ContractPayment"
ACTCore.CollectionService.DataImport ContractPerson "D:\DATA_TEST\COLLECTION_CONTRACT\ContractPerson"
ACTCore.CollectionService.DataImport ContractPhone "D:\DATA_TEST\COLLECTION_CONTRACT\ContractPhone"

echo End Time: %date% %time%
pause