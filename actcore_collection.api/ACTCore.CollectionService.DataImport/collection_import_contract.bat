@echo off
echo Start Time: %date% %time%

cd /d "D:\Adaptivate\ACT_Collection\actcore_collection.api\ACTCore.CollectionService.DataImport\bin\Release\prod-package"

ACTCore.CollectionService.DataImport.exe Contract "D:\DATA_TEST\COLLECTION_CONTRACT\Contract"
ACTCore.CollectionService.DataImport.exe ContractAddress "D:\DATA_TEST\COLLECTION_CONTRACT\ContractAddress"
ACTCore.CollectionService.DataImport.exe ContractAsset "D:\DATA_TEST\COLLECTION_CONTRACT\ContractAsset"
ACTCore.CollectionService.DataImport.exe ContractAssetVehicle "D:\DATA_TEST\COLLECTION_CONTRACT\ContractAssetVehicle"
ACTCore.CollectionService.DataImport.exe ContractOverdue "D:\DATA_TEST\COLLECTION_CONTRACT\ContractOverdue"
ACTCore.CollectionService.DataImport.exe ContractPayment "D:\DATA_TEST\COLLECTION_CONTRACT\ContractPayment"
ACTCore.CollectionService.DataImport.exe ContractPerson "D:\DATA_TEST\COLLECTION_CONTRACT\ContractPerson"
ACTCore.CollectionService.DataImport.exe ContractPhone "D:\DATA_TEST\COLLECTION_CONTRACT\ContractPhone"

echo End Time: %date% %time%
pause