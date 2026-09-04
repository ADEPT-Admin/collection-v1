-- ColPermission
DECLARE @PER_VIEW_CONTRACT_DETAIL  uniqueidentifier = '3A3FA621-F0BE-48E3-B97D-622161000001';
DECLARE @PER_REASSIGN_WORKLIST  uniqueidentifier = '3A3FA621-F0BE-48E3-B97D-622161000002';
DECLARE @PER_APPROVE_REASSIGNMENT  uniqueidentifier = '3A3FA621-F0BE-48E3-B97D-622161000003';
DECLARE @PER_MANAGE_COLLECTOR_ROLE  uniqueidentifier = '3A3FA621-F0BE-48E3-B97D-622161000004';
DECLARE @PER_MANAGE_COLLECTOR_PERMISSION  uniqueidentifier = '3A3FA621-F0BE-48E3-B97D-622161000005';
DECLARE @PER_MANAGE_TEAM_ASSIGNMENT  uniqueidentifier = '3A3FA621-F0BE-48E3-B97D-622161000006';
DECLARE @PER_MANAGE_TEAM_STRUCTURE  uniqueidentifier = '3A3FA621-F0BE-48E3-B97D-622161000007';
DECLARE @PER_VIEW_TEAM_DASHBOARD  uniqueidentifier = '3A3FA621-F0BE-48E3-B97D-622161000008';
DECLARE @PER_VIEW_MANAGEMENT_DASHBOARD  uniqueidentifier = '3A3FA621-F0BE-48E3-B97D-622161000009';

INSERT INTO ColPermission(ColPermissionId, ColPermissionCode, ColPermissionName, Description, IsActive) 
	VALUES (@PER_VIEW_CONTRACT_DETAIL, 'REASSIGN_CASE', N'ดูรายละเอียดข้อมูลลูกหนี้', N'Collector สามารถเปิดดูรายละเอียดลูกหนี้', 1),
	(@PER_REASSIGN_WORKLIST, 'REASSIGN_WORKLIST', N'มอบหมายหรือย้ายงานใหม่', N'ย้ายลูกหนี้ระหว่าง Collector', 1),
	(@PER_APPROVE_REASSIGNMENT, 'APPROVE_REASSIGNMENT', N'อนุมัติการโอนงาน', N'อนุมัติการโอนงาน', 1),
	(@PER_MANAGE_COLLECTOR_ROLE, 'MANAGE_COLLECTOR_ROLE', N'จัดการ Role ของ Collector', N'เพิ่ม/แก้ไข CollectorRole', 1),
	(@PER_MANAGE_COLLECTOR_PERMISSION, 'MANAGE_COLLECTOR_PERMISSION', N'แก้ไขสิทธิ์ของ Role', N'แก้ไขตาราง Permission', 1),
	(@PER_MANAGE_TEAM_ASSIGNMENT, 'MANAGE_TEAM_ASSIGNMENT', N'จัดการการ Assign Collector', N'กำหนด Effective/Expire Date', 1),
	(@PER_MANAGE_TEAM_STRUCTURE, 'MANAGE_TEAM_STRUCTURE', N'จัดโครงสร้างทีม', N'เพิ่ม/แก้ไขทีมและ Supervisor', 1),
	(@PER_VIEW_TEAM_DASHBOARD, 'VIEW_TEAM_DASHBOARD', N'ดูผลการทำงานของทีม', N'Dashboard รายงานทีม', 1),
	(@PER_VIEW_MANAGEMENT_DASHBOARD, 'VIEW_MANAGEMENT_DASHBOARD', N'ดู Dashboard รวมองค์กร', N'ใช้ดูภาพรวมระดับ Manager', 1);


-- ColRole
DECLARE @ROLE_COL_COLL  uniqueidentifier = 'A8CF4655-99FB-43D4-9A05-37F05E000001';
DECLARE @ROLE_COL_SUPV  uniqueidentifier = 'A8CF4655-99FB-43D4-9A05-37F05E000002';
DECLARE @ROLE_COL_MGR  uniqueidentifier = 'A8CF4655-99FB-43D4-9A05-37F05E000003';
DECLARE @ROLE_COL_ADM  uniqueidentifier = 'A8CF4655-99FB-43D4-9A05-37F05E000004';

INSERT INTO ColRole (ColRoleId, ColRoleCode, ColRoleName, Description, IsActive) 
	VALUES (@ROLE_COL_COLL, 'COL_COLL', N'Collector', N'ใช้ในการติดตามหนี้รายวัน', 1),
	(@ROLE_COL_SUPV, 'COL_SUPV', N'Supervisor', N'ควบคุมทีม ดูงานลูกทีม', 1),
	(@ROLE_COL_MGR, 'COL_MGR', N'Manager', N'ดูภาพรวมทีม, อนุมัติระดับสูง', 1),
	(@ROLE_COL_ADM, 'COL_ADM', N'Admin', N'ดูแลระบบ และแก้ไขสิทธิ์ทั้งหมด', 1);


-- ColRolePermission
-- COL_COLL
INSERT INTO ColRolePermission (ColRoleId,ColPermissionId,IsActive) 
VALUES (@ROLE_COL_COLL, @PER_VIEW_CONTRACT_DETAIL, 1),
(@ROLE_COL_COLL, @PER_REASSIGN_WORKLIST , 1),
(@ROLE_COL_COLL, @PER_APPROVE_REASSIGNMENT, 0),
(@ROLE_COL_COLL, @PER_MANAGE_COLLECTOR_ROLE, 0),
(@ROLE_COL_COLL, @PER_MANAGE_COLLECTOR_PERMISSION, 0),
(@ROLE_COL_COLL, @PER_MANAGE_TEAM_ASSIGNMENT, 0),
(@ROLE_COL_COLL, @PER_MANAGE_TEAM_STRUCTURE, 0),
(@ROLE_COL_COLL, @PER_VIEW_TEAM_DASHBOARD, 0),
(@ROLE_COL_COLL, @PER_VIEW_MANAGEMENT_DASHBOARD, 0);
-- COL_SUPV
INSERT INTO ColRolePermission (ColRoleId,ColPermissionId,IsActive) 
VALUES (@ROLE_COL_SUPV, @PER_VIEW_CONTRACT_DETAIL, 1),
(@ROLE_COL_SUPV, @PER_REASSIGN_WORKLIST , 1),
(@ROLE_COL_SUPV, @PER_APPROVE_REASSIGNMENT, 0),
(@ROLE_COL_SUPV, @PER_MANAGE_COLLECTOR_ROLE, 0),
(@ROLE_COL_SUPV, @PER_MANAGE_COLLECTOR_PERMISSION, 0),
(@ROLE_COL_SUPV, @PER_MANAGE_TEAM_ASSIGNMENT, 0),
(@ROLE_COL_SUPV, @PER_MANAGE_TEAM_STRUCTURE, 0),
(@ROLE_COL_SUPV, @PER_VIEW_TEAM_DASHBOARD, 1),
(@ROLE_COL_SUPV, @PER_VIEW_MANAGEMENT_DASHBOARD, 0);
-- COL_MGR
INSERT INTO ColRolePermission (ColRoleId,ColPermissionId,IsActive) 
VALUES (@ROLE_COL_MGR, @PER_VIEW_CONTRACT_DETAIL, 1),
(@ROLE_COL_MGR, @PER_REASSIGN_WORKLIST , 1),
(@ROLE_COL_MGR, @PER_APPROVE_REASSIGNMENT, 1),
(@ROLE_COL_MGR, @PER_MANAGE_COLLECTOR_ROLE, 0),
(@ROLE_COL_MGR, @PER_MANAGE_COLLECTOR_PERMISSION, 0),
(@ROLE_COL_MGR, @PER_MANAGE_TEAM_ASSIGNMENT, 0),
(@ROLE_COL_MGR, @PER_MANAGE_TEAM_STRUCTURE, 1),
(@ROLE_COL_MGR, @PER_VIEW_TEAM_DASHBOARD, 1),
(@ROLE_COL_MGR, @PER_VIEW_MANAGEMENT_DASHBOARD, 1);
-- COL_ADM
INSERT INTO ColRolePermission (ColRoleId,ColPermissionId,IsActive) 
VALUES (@ROLE_COL_ADM, @PER_VIEW_CONTRACT_DETAIL, 0),
(@ROLE_COL_ADM, @PER_REASSIGN_WORKLIST , 0),
(@ROLE_COL_ADM, @PER_APPROVE_REASSIGNMENT, 0),
(@ROLE_COL_ADM, @PER_MANAGE_COLLECTOR_ROLE, 1),
(@ROLE_COL_ADM, @PER_MANAGE_COLLECTOR_PERMISSION, 1),
(@ROLE_COL_ADM, @PER_MANAGE_TEAM_ASSIGNMENT, 1),
(@ROLE_COL_ADM, @PER_MANAGE_TEAM_STRUCTURE, 1),
(@ROLE_COL_ADM, @PER_VIEW_TEAM_DASHBOARD, 1),
(@ROLE_COL_ADM, @PER_VIEW_MANAGEMENT_DASHBOARD, 1);

-- ColTeam
DECLARE @TEAM_Phone0  uniqueidentifier = '95D75E51-ADDB-4F45-936F-5AB5FA000001';
DECLARE @TEAM_Phone1  uniqueidentifier = '95D75E51-ADDB-4F45-936F-5AB5FA000002';
DECLARE @TEAM_Phone2  uniqueidentifier = '95D75E51-ADDB-4F45-936F-5AB5FA000003';
DECLARE @TEAM_Field  uniqueidentifier = '95D75E51-ADDB-4F45-936F-5AB5FA000004';

INSERT INTO dbo.ColTeam (ColTeamId,ColTeamCode,ColTeamName,ColTeamType,Capacity, IsActive)
VALUES (@TEAM_Phone0, 'Phone0','Phone0', 'Phone', 500, 1),
(@TEAM_Phone1, 'Phone1','Phone1', 'Phone', 500, 1),
(@TEAM_Phone2, 'Phone2','Phone2', 'Phone', 500, 1),
(@TEAM_Field, 'Field','Field', 'Field', 500, 1);

-- CollectorProfile
DECLARE @COL_ID_61001  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B061001'; -- kriengkraim (Manager)

-- team Phone0
DECLARE @COL_ID_62001  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B062001'; -- piyanee (supervisor)
DECLARE @COL_ID_61056  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B061056'; -- surind

-- team Phone1
DECLARE @COL_ID_62103  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B062103'; -- sudam (supervisor)
DECLARE @COL_ID_63103  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B063103'; -- banyongb
DECLARE @COL_ID_64001  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B064001'; -- ariyan
DECLARE @COL_ID_64040  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B064040'; -- wannaratg
DECLARE @COL_ID_64057  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B064057'; -- taengonc
DECLARE @COL_ID_64081  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B064081'; -- ekkapolo

-- team Phone2
DECLARE @COL_ID_63001  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B063001'; -- chanokp (supervisor)
DECLARE @COL_ID_64101  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B064101'; -- natthidaj
DECLARE @COL_ID_64102  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B064102'; -- weerachais
DECLARE @COL_ID_64103  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B064103'; -- sombatm
DECLARE @COL_ID_64104  uniqueidentifier = '6D27070C-AAFE-46BA-A45A-93D20B064104'; -- korakojt

INSERT INTO CollectorProfile (CollectorId, UserId, ColRoleId, IsActive) 
VALUES 
(@COL_ID_61001, 'E1A1B2C3-D4E5-4F67-8A90-123456761001', @ROLE_COL_MGR, 1),

-- team Phone0
(@COL_ID_62001, 'E1A1B2C3-D4E5-4F67-8A90-123456762001', @ROLE_COL_SUPV, 1),
(@COL_ID_61056, 'E1A1B2C3-D4E5-4F67-8A90-123456761056', @ROLE_COL_COLL, 1),

-- team Phone1
(@COL_ID_62103, 'E1A1B2C3-D4E5-4F67-8A90-123456762103', @ROLE_COL_SUPV, 1),
(@COL_ID_63103, 'E1A1B2C3-D4E5-4F67-8A90-123456763103', @ROLE_COL_COLL, 1),
(@COL_ID_64001, 'E1A1B2C3-D4E5-4F67-8A90-123456764001', @ROLE_COL_COLL, 1),
(@COL_ID_64040, 'E1A1B2C3-D4E5-4F67-8A90-123456764040', @ROLE_COL_COLL, 1),
(@COL_ID_64057, 'E1A1B2C3-D4E5-4F67-8A90-123456764057', @ROLE_COL_COLL, 1),
(@COL_ID_64081, 'E1A1B2C3-D4E5-4F67-8A90-123456764081', @ROLE_COL_COLL, 1),

-- team Phone2
(@COL_ID_63001, 'E1A1B2C3-D4E5-4F67-8A90-123456763001', @ROLE_COL_SUPV, 1),
(@COL_ID_64101, 'E1A1B2C3-D4E5-4F67-8A90-123456764101', @ROLE_COL_COLL, 1),
(@COL_ID_64102, 'E1A1B2C3-D4E5-4F67-8A90-123456764102', @ROLE_COL_COLL, 1),
(@COL_ID_64103, 'E1A1B2C3-D4E5-4F67-8A90-123456764103', @ROLE_COL_COLL, 1),
(@COL_ID_64104, 'E1A1B2C3-D4E5-4F67-8A90-123456764104', @ROLE_COL_COLL, 1);


-- ColTeamAssignment
INSERT INTO ColTeamAssignment (AssignmentId, ColTeamId, CollectorId,IsSupervisor,Capacity, EffectiveDate, [ExpireDate],IsActive)
VALUES 
-- ('59BFDA11-0230-40BF-939D-2073B6000001', @TEAM_Phone0,@COL_ID_61001,1,100, '2025-01-01', null, 1), Team Manager

-- team Phone0
('59BFDA11-0230-40BF-939D-2073B6000003', @TEAM_Phone0,@COL_ID_62001,1,100, '2025-01-01', null, 1),
('59BFDA11-0230-40BF-939D-2073B6000004', @TEAM_Phone0,@COL_ID_61056,0,100, '2025-01-01', null, 1),

-- team Phone1
('59BFDA11-0230-40BF-939D-2073B6000005', @TEAM_Phone1,@COL_ID_62103,1,100, '2025-01-01', null, 1),
('59BFDA11-0230-40BF-939D-2073B6000006', @TEAM_Phone1,@COL_ID_63103,0,100, '2025-01-01', null, 1),
('59BFDA11-0230-40BF-939D-2073B6000007', @TEAM_Phone1,@COL_ID_64001,0,100, '2025-01-01', null, 1),
('59BFDA11-0230-40BF-939D-2073B6000008', @TEAM_Phone1,@COL_ID_64040,0,100, '2025-01-01', null, 1),
('59BFDA11-0230-40BF-939D-2073B6000009', @TEAM_Phone1,@COL_ID_64057,0,100, '2025-01-01', null, 1),
('59BFDA11-0230-40BF-939D-2073B6000010', @TEAM_Phone1,@COL_ID_64081,0,100, '2025-01-01', null, 1),

-- team Phone2
('59BFDA11-0230-40BF-939D-2073B6000011', @TEAM_Phone2,@COL_ID_63001,1,100, '2025-01-01', null, 1),
('59BFDA11-0230-40BF-939D-2073B6000012', @TEAM_Phone2,@COL_ID_64101,0,100, '2025-01-01', null, 1),
('59BFDA11-0230-40BF-939D-2073B6000013', @TEAM_Phone2,@COL_ID_64102,0,100, '2025-01-01', null, 1),
('59BFDA11-0230-40BF-939D-2073B6000014', @TEAM_Phone2,@COL_ID_64103,0,100, '2025-01-01', null, 1),
('59BFDA11-0230-40BF-939D-2073B6000015', @TEAM_Phone2,@COL_ID_64104,0,100, '2025-01-01', null, 1);

-- ColNoteAction
SET IDENTITY_INSERT ColNoteAction ON;
INSERT INTO ColNoteAction (ActionId, ActionCode, ActionDescription, IsActive) VALUES (1, 'CallOutbound', 'โทรออกหาลูกหนี้', 1);
INSERT INTO ColNoteAction (ActionId, ActionCode, ActionDescription, IsActive) VALUES (2, 'CallInBound', 'รับสายโทรเข้า', 1);
SET IDENTITY_INSERT ColNoteAction OFF;

-- ColNoteResult
SET IDENTITY_INSERT ColNoteResult ON;
INSERT INTO ColNoteResult (ResultId, ActionId, ResultCode, ResultDescription, FollowupStatus, IsActive) VALUES (1, 1, 'NoAnswer', 'ไม่รับสาย', 'COV', 1);
INSERT INTO ColNoteResult (ResultId, ActionId, ResultCode, ResultDescription, FollowupStatus, IsActive) VALUES (2, 1, 'Busy', 'สายไม่ว่าง', 'COV', 1);
INSERT INTO ColNoteResult (ResultId, ActionId, ResultCode, ResultDescription, FollowupStatus, IsActive) VALUES (3, 1, 'WrongNumber', 'เบอร์โทรศัพท์ผิด', 'COV', 1);
INSERT INTO ColNoteResult (ResultId, ActionId, ResultCode, ResultDescription, FollowupStatus, IsActive) VALUES (4, 1, 'NoPromiseToPay', 'โทรสำเร็จไม่นัดชำระเงิน', 'REF', 1);
INSERT INTO ColNoteResult (ResultId, ActionId, ResultCode, ResultDescription, FollowupStatus, IsActive) VALUES (5, 1, 'PromiseToPay', 'นัดชำระเงิน', 'PTP', 1);
INSERT INTO ColNoteResult (ResultId, ActionId, ResultCode, ResultDescription, FollowupStatus, IsActive) VALUES (6, 1, 'Dispute', 'โต้แย้งข้อมูลหนี้', 'REF', 1);
INSERT INTO ColNoteResult (ResultId, ActionId, ResultCode, ResultDescription, FollowupStatus, IsActive) VALUES (7, 2, 'NoAnswer', 'ไม่รับสาย', 'COV', 1);
INSERT INTO ColNoteResult (ResultId, ActionId, ResultCode, ResultDescription, FollowupStatus, IsActive) VALUES (8, 2, 'Busy', 'สายไม่ว่าง', 'COV', 1);
INSERT INTO ColNoteResult (ResultId, ActionId, ResultCode, ResultDescription, FollowupStatus, IsActive) VALUES (9, 2, 'WrongNumber', 'เบอร์โทรศัพท์ผิด', 'COV', 1);
INSERT INTO ColNoteResult (ResultId, ActionId, ResultCode, ResultDescription, FollowupStatus, IsActive) VALUES (10, 2, 'NoPromiseToPay', 'โทรสำเร็จไม่นัดชำระเงิน', 'REF', 1);
INSERT INTO ColNoteResult (ResultId, ActionId, ResultCode, ResultDescription, FollowupStatus, IsActive) VALUES (11, 2, 'PromiseToPay', 'นัดชำระเงิน', 'PTP', 1);
INSERT INTO ColNoteResult (ResultId, ActionId, ResultCode, ResultDescription, FollowupStatus, IsActive) VALUES (12, 2, 'Dispute', 'โต้แย้งข้อมูลหนี้', 'REF', 1);
SET IDENTITY_INSERT ColNoteResult OFF;


-- ColArea
/*
SET IDENTITY_INSERT ColArea ON;
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (1, NULL, '1001', 'ภาคกลาง', '10', NULL, NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (2, NULL, '1002', 'ภาคอิสาน', '10', NULL, NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (3, NULL, '1003', 'ภาคเหนือ', '10', NULL, NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (4, NULL, '1004', 'ภาคตะวันออก', '10', NULL, NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (5, NULL, '1005', 'ตะวันตก', '10', NULL, NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (6, NULL, '1006', 'ภาคใต้', '10', NULL, NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (7, 1, '1110', 'กรุงเทพมหานคร', '11', '10', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (8, 1, '1111', 'สมุทรปราการ', '11', '11', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (9, 1, '1112', 'นนทบุรี', '11', '12', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (10, 1, '1113', 'ปทุมธานี', '11', '13', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (11, 1, '1114', 'พระนครศรีอยุธยา', '11', '14', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (12, 1, '1115', 'อ่างทอง', '11', '15', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (13, 1, '1116', 'ลพบุรี', '11', '16', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (14, 1, '1117', 'สิงห์บุรี', '11', '17', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (15, 1, '1118', 'ชัยนาท', '11', '18', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (16, 1, '1119', 'สระบุรี', '11', '19', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (17, 1, '1126', 'นครนายก', '11', '26', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (18, 1, '1160', 'นครสวรรค์', '11', '60', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (19, 1, '1161', 'อุทัยธานี', '11', '61', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (20, 1, '1162', 'กำแพงเพชร', '11', '62', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (21, 1, '1164', 'สุโขทัย', '11', '64', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (22, 1, '1165', 'พิษณุโลก', '11', '65', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (23, 1, '1166', 'พิจิตร', '11', '66', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (24, 1, '1172', 'สุพรรณบุรี', '11', '72', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (25, 1, '1173', 'นครปฐม', '11', '73', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (26, 1, '1174', 'สมุทรสาคร', '11', '74', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (27, 1, '1175', 'สมุทรสงคราม', '11', '75', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (28, 1, '1176', 'เพชรบุรี', '11', '76', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (29, 4, '1120', 'ชลบุรี', '11', '20', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (30, 4, '1121', 'ระยอง', '11', '21', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (31, 4, '1122', 'จันทบุรี', '11', '22', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (32, 4, '1123', 'ตราด', '11', '23', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (33, 4, '1124', 'ฉะเชิงเทรา', '11', '24', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (34, 4, '1125', 'ปราจีนบุรี', '11', '25', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (35, 4, '1127', 'สระแก้ว', '11', '27', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (36, 3, '1150', 'เชียงใหม่', '11', '50', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (37, 3, '1151', 'ลำพูน', '11', '51', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (38, 3, '1152', 'ลำปาง', '11', '52', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (39, 3, '1153', 'อุตรดิตถ์', '11', '53', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (40, 3, '1154', 'แพร่', '11', '54', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (41, 3, '1155', 'น่าน', '11', '55', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (42, 3, '1156', 'พะเยา', '11', '56', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (43, 3, '1157', 'เชียงราย', '11', '57', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (44, 3, '1158', 'แม่ฮ่องสอน', '11', '58', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (45, 2, '1130', 'นครราชสีมา', '11', '30', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (46, 2, '1131', 'บุรีรัมย์', '11', '31', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (47, 2, '1132', 'สุรินทร์', '11', '32', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (48, 2, '1133', 'ศรีสะเกษ', '11', '33', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (49, 2, '1134', 'อุบลราชธานี', '11', '34', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (50, 2, '1135', 'ยโสธร', '11', '35', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (51, 2, '1136', 'ชัยภูมิ', '11', '36', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (52, 2, '1137', 'อำนาจเจริญ', '11', '37', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (53, 2, '1138', 'บึงกาฬ', '11', '38', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (54, 2, '1139', 'หนองบัวลำภู', '11', '39', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (55, 2, '1140', 'ขอนแก่น', '11', '40', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (56, 2, '1141', 'อุดรธานี', '11', '41', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (57, 2, '1142', 'เลย', '11', '42', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (58, 2, '1143', 'หนองคาย', '11', '43', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (59, 2, '1144', 'มหาสารคาม', '11', '44', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (60, 2, '1145', 'ร้อยเอ็ด', '11', '45', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (61, 2, '1146', 'กาฬสินธุ์', '11', '46', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (62, 2, '1147', 'สกลนคร', '11', '47', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (63, 2, '1148', 'นครพนม', '11', '48', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (64, 2, '1149', 'มุกดาหาร', '11', '49', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (65, 6, '1180', 'นครศรีธรรมราช', '11', '80', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (66, 6, '1181', 'กระบี่', '11', '81', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (67, 6, '1182', 'พังงา', '11', '82', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (68, 6, '1183', 'ภูเก็ต', '11', '83', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (69, 6, '1184', 'สุราษฎร์ธานี', '11', '84', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (70, 6, '1185', 'ระนอง', '11', '85', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (71, 6, '1186', 'ชุมพร', '11', '86', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (72, 6, '1190', 'สงขลา', '11', '90', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (73, 6, '1191', 'สตูล', '11', '91', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (74, 6, '1192', 'ตรัง', '11', '92', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (75, 6, '1193', 'พัทลุง', '11', '93', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (76, 6, '1194', 'ปัตตานี', '11', '94', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (77, 6, '1195', 'ยะลา', '11', '95', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (78, 6, '1196', 'นราธิวาส', '11', '96', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (79, 6, '1163', 'ตาก', '11', '63', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (80, 5, '1167', 'เพชรบูรณ์', '11', '67', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (81, 5, '1170', 'ราชบุรี', '11', '70', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (82, 5, '1171', 'กาญจนบุรี', '11', '71', NULL, NULL, 1);
INSERT INTO ColArea (AreaId, ParentId, AreaCode, AreaName, AreaLevelId, ProvinceId, DistrictId, SubDistrictId, IsActive) VALUES (83, 5, '1177', 'ประจวบคีรีขันธ์', '11', '77', NULL, NULL, 1);
SET IDENTITY_INSERT ColArea OFF;
*/
