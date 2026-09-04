SET IDENTITY_INSERT MappingImport ON;
INSERT INTO [dbo].[MappingImport]
           ([Id],[TableName]
           ,[NestedTable]
           ,[AllowImport]
		   ,[CreatedBy])
     VALUES
			 (1,'SysUserGroup', NULL, 1,'System')
		   , (2,'Contract', NULL, 1,'System')
		   , (3,'EmployeeProfile', NULL, 1,'System')
		   , (4,'Worklist', NULL, 1,'System')
		   , (5,'ContractAddress', 'Contract', 1,'System')
		   , (6,'ContractAsset', 'Contract', 1,'System')
		   , (7,'ContractAssetVehicle', 'Contract', 1,'System')
		   , (8,'ContractOverdue', 'Contract', 1,'System')
		   , (9,'ContractPayment', 'Contract', 1,'System')
		   , (10,'ContractPerson', 'Contract', 1,'System')
		   , (11,'ContractPhone', 'Contract', 1,'System')
SET IDENTITY_INSERT MappingImport OFF;

INSERT INTO [dbo].[MappingImportDetail]
(
    MappingId,
    FieldName,
    DataType,
    IsRequired,
    MappingField,
    IsActive,
    CreatedBy,
    CreatedDate
)
VALUES
-- MappingId = 1 (SysUserGroup)
(1, N'UserGroupId',        N'guid',     1, N'UserGroupId',        1, N'System', NULL),
(1, N'UserGroupCode',      N'string',   1, N'UserGroupCode',      1, N'System', NULL),
(1, N'UserGroupName',      N'string',   1, N'UserGroupName',      1, N'System', NULL),
(1, N'Description',        N'string',   0, N'Description',        1, N'System', NULL),
(1, N'IsActive',           N'bool',     1, N'IsActive',           1, N'System', NULL),

-- MappingId = 2 (Contract)
(2, N'ContractNo',                 N'string',   1, N'ContractNo',                 1, N'System', NULL),
(2, N'LoanType',                   N'string',   0, N'LoanType',                   1, N'System', NULL),
(2, N'ContractStartDate',          N'datetime', 0, N'ContractStartDate',          1, N'System', NULL),
(2, N'ContractEndDate',            N'datetime', 0, N'ContractEndDate',            1, N'System', NULL),
(2, N'InterestRate',               N'decimal',  0, N'InterestRate',               1, N'System', NULL),
(2, N'InterestRateType',           N'string',   0, N'InterestRateType',           1, N'System', NULL),
(2, N'PaymentDueDate',             N'int',      0, N'PaymentDueDate',             1, N'System', NULL),
(2, N'DueDate',                    N'datetime', 0, N'DueDate',                    1, N'System', NULL),
(2, N'LoanAmount',                 N'decimal',  0, N'LoanAmount',                 1, N'System', NULL),
(2, N'Term',                       N'int',      0, N'Term',                       1, N'System', NULL),
(2, N'InstallAmount',              N'decimal',  0, N'InstallAmount',              1, N'System', NULL),
(2, N'ContractMOB',                N'int',      0, N'ContractMOB',                1, N'System', NULL),
(2, N'Bucket',                     N'int',      0, N'Bucket',                     1, N'System', NULL),
(2, N'DayPastDue',                 N'int',      0, N'DayPastDue',                 1, N'System', NULL),
(2, N'OverdueAmount',              N'decimal',  0, N'OverdueAmount',              1, N'System', NULL),
(2, N'OutstandingBalance',         N'decimal',  0, N'OutstandingBalance',         1, N'System', NULL),
(2, N'PaymentReceivedTerm',        N'int',      0, N'PaymentReceivedTerm',        1, N'System', NULL),
(2, N'PaymentReceivedAmount',      N'decimal',  0, N'PaymentReceivedAmount',      1, N'System', NULL),
(2, N'LastPaymentDate',            N'datetime', 0, N'LastPaymentDate',            1, N'System', NULL),
(2, N'ContractStatus',             N'string',   0, N'ContractStatus',             1, N'System', NULL),
(2, N'RiskLevel',                  N'string',   0, N'RiskLevel',                  1, N'System', NULL),

-- MappingId = 5 (Address)
(5, N'PersonRefId',       N'string',   1, N'PersonRefId',       1, N'System', NULL),
(5, N'AddressType',       N'string',   0, N'AddressType',       1, N'System', NULL),
(5, N'Address',           N'string',   0, N'Address',           1, N'System', NULL),
(5, N'Province',		  N'string',   0, N'Province',          1, N'System', NULL),
(5, N'District',          N'string',   0, N'District',          1, N'System', NULL),
(5, N'SubDistrict',       N'string',   0, N'SubDistrict',       1, N'System', NULL),
(5, N'AddressRemark',     N'string',   0, N'AddressRemark',     1, N'System', NULL),

-- MappingId = 6 (Asset - General)
(6, N'ContractNo',        N'string',   1, N'ContractNo',        1, N'System', NULL),
(6, N'AssetGroup',        N'string',   0, N'AssetGroup',        1, N'System', NULL),
(6, N'AssetType',         N'string',   0, N'AssetType',         1, N'System', NULL),
(6, N'AssetDescription',  N'string',   0, N'AssetDescription',  1, N'System', NULL),
(6, N'AssetPrice',        N'decimal',  0, N'AssetPrice',        1, N'System', NULL),

-- MappingId = 7 (Asset - Vehicle)
(7, N'ContractNo',        N'string',   1, N'ContractNo',        1, N'System', NULL),
(7, N'AssetGroup',        N'string',   0, N'AssetGroup',        1, N'System', NULL),
(7, N'AssetType',         N'string',   0, N'AssetType',         1, N'System', NULL),
(7, N'Brand',             N'string',   0, N'Brand',             1, N'System', NULL),
(7, N'Model',             N'string',   0, N'Model',             1, N'System', NULL),
(7, N'Series',            N'string',   0, N'Series',            1, N'System', NULL),
(7, N'Year',              N'int',      0, N'Year',              1, N'System', NULL),
(7, N'Color',             N'string',   0, N'Color',             1, N'System', NULL),
(7, N'EngineNo',          N'string',   0, N'EngineNo',          1, N'System', NULL),
(7, N'ChassisNo',         N'string',   0, N'ChassisNo',         1, N'System', NULL),
(7, N'PlateNo',           N'string',   0, N'PlateNo',           1, N'System', NULL),
(7, N'RegisterProvince',  N'string',   0, N'RegisterProvince',  1, N'System', NULL),
(7, N'FuelType',          N'string',   0, N'FuelType',          1, N'System', NULL),

-- MappingId = 8 (Overdue)
(8, N'ContractNo',        N'string',   1, N'ContractNo',        1, N'System', NULL),
(8, N'Order',             N'int',      0, N'Order',             1, N'System', NULL),
(8, N'OverdueType',       N'string',   0, N'OverdueType',       1, N'System', NULL),
(8, N'OverdueAmount',     N'decimal',  0, N'OverdueAmount',     1, N'System', NULL),

-- MappingId = 9 (Payment)
(9, N'ContractNo',         N'string',   1, N'ContractNo',         1, N'System', NULL),
(9, N'PaymentDate',        N'datetime', 0, N'PaymentDate',        1, N'System', NULL),
(9, N'PaymentAmount',      N'decimal',  0, N'PaymentAmount',      1, N'System', NULL),
(9, N'InstallAmount',      N'decimal',  0, N'InstallAmount',      1, N'System', NULL),
(9, N'PenaltyAmount',      N'decimal',  0, N'PenaltyAmount',      1, N'System', NULL),
(9, N'OtherFeesAmount',    N'decimal',  0, N'OtherFeesAmount',    1, N'System', NULL),
(9, N'PaymentChannel',     N'string',   0, N'PaymentChannel',     1, N'System', NULL),
(9, N'ReferenceNo',        N'string',   0, N'ReferenceNo',        1, N'System', NULL),
-- MappingId = 10 (Person)
(10, N'PersonRefId',     N'string',   1, N'PersonRefId',      1, N'System', NULL),
(10, N'ContractNo',      N'string',   1, N'ContractNo',      1, N'System', NULL),
(10, N'PersonType',      N'string',   0, N'PersonType',      1, N'System', NULL),
(10, N'PrefixId',        N'int',      0, N'PrefixId',        1, N'System', NULL),
(10, N'FirstName',       N'string',   0, N'FirstName',       1, N'System', NULL),
(10, N'LastName',        N'string',   0, N'LastName',        1, N'System', NULL),
(10, N'IdCard',          N'string',   0, N'IdCard',          1, N'System', NULL),
(10, N'DateOfBirth',     N'datetime', 0, N'DateOfBirth',     1, N'System', NULL),
(10, N'Age',             N'int',      0, N'Age',             1, N'System', NULL),
(10, N'Gender',          N'string',   0, N'Gender',          1, N'System', NULL),
(10, N'MaritalStatus',   N'string',   0, N'MaritalStatus',   1, N'System', NULL),
(10, N'Occupation',      N'string',   0, N'Occupation',      1, N'System', NULL),
(10, N'Income',          N'decimal',  0, N'Income',          1, N'System', NULL),
(10, N'WorkPlace',       N'string',   0, N'WorkPlace',       1, N'System', NULL),
(10, N'Email',           N'string',   0, N'Email',           1, N'System', NULL),
(10, N'Relationship',    N'string',   0, N'Relationship',    1, N'System', NULL),

-- MappingId = 11 (Phone)
(11, N'PersonRefId',     N'string',   1, N'PersonRefId',      1, N'System', NULL),
(11, N'PhoneType',       N'string',   0, N'PhoneType',       1, N'System', NULL),
(11, N'PhoneNo',         N'string',   0, N'PhoneNo',         1, N'System', NULL),
(11, N'PhoneRemark',     N'string',   0, N'PhoneRemark',     1, N'System', NULL),

-- MappingId = 3 (Employee)
(3, N'EmployeeId',           N'string',   1, N'EmployeeId',           1, N'System', NULL),
(3, N'UserName',             N'string',   0, N'UserName',             1, N'System', NULL),
(3, N'PrefixId',             N'int',      0, N'PrefixId',             1, N'System', NULL),
(3, N'FirstName',            N'string',   0, N'FirstName',            1, N'System', NULL),
(3, N'LastName',             N'string',   0, N'LastName',             1, N'System', NULL),
(3, N'FirstNameEn',          N'string',   0, N'FirstNameEn',          1, N'System', NULL),
(3, N'LastNameEn',           N'string',   0, N'LastNameEn',           1, N'System', NULL),
(3, N'NickName',             N'string',   0, N'NickName',             1, N'System', NULL),
(3, N'Gender',               N'string',   0, N'Gender',               1, N'System', NULL),
(3, N'DateOfBirth',          N'datetime', 0, N'DateOfBirth',          1, N'System', NULL),
(3, N'NationalID',           N'string',   0, N'NationalId',           1, N'System', NULL),
(3, N'Nationality',          N'string',   0, N'Nationality',          1, N'System', NULL),
(3, N'Religion',             N'string',   0, N'Religion',             1, N'System', NULL),
(3, N'Email',                N'string',   0, N'Email',                1, N'System', NULL),
(3, N'PhoneNo',              N'string',   0, N'PhoneNo',              1, N'System', NULL),
(3, N'Branch',               N'string',   0, N'Branch',               1, N'System', NULL),
(3, N'SupervisorId',         N'string',   0, N'SupervisorId',         1, N'System', NULL),
(3, N'WorkStatus',           N'string',   0, N'WorkStatus',           1, N'System', NULL),
(3, N'DepartmentId',         N'int',      0, N'DepartmentId',         1, N'System', NULL),
(3, N'PositionId',           N'int',      0, N'PositionId',           1, N'System', NULL),
(3, N'TeamId',               N'int',      0, N'TeamId',               1, N'System', NULL),
(3, N'StartWorkingDate',     N'datetime', 0, N'StartWorkingDate',     1, N'System', NULL),
(3, N'ProbationDate',		 N'datetime', 0, N'ProbationDate',        1, N'System', NULL),
(3, N'WorkEffectiveDate',    N'datetime', 0, N'WorkEffectiveDate',    1, N'System', NULL),
(3, N'TerminationDate',      N'datetime', 0, N'TerminationDate',      1, N'System', NULL),
(3, N'IsActive',             N'bool',     1, N'IsActive',             1, N'System', NULL),
(3, N'LastSyncDate',         N'datetime', 0, N'LastSyncDate',         1, N'System', NULL);