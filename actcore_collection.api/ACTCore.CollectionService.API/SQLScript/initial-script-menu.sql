-- SysItem (Menu)
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('0','','Collection Worklist','Collection','','1','0','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('1000','0','ภาพรวมระบบ','Dashboard','/dashboard','2','1','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('1010','1000','ภาพรวมของฉัน','My Dashboard','/dashboard/my','3','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('1020','1000','ภาพรวมทีม','Team Dashboard','/dashboard/team','4','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('1030','1000','ภาพรวมประสิทธิภาพ','Overall Performance','/dashboard/overall','5','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('1040','1000','จัดการบัญชี','Accounts Management','/accounts','6','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('2000','0','ค้นหาบัญชี','Search Accounts','/accounts/search','7','1','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('2010','2000','อัปโหลดข้อมูลบัญชี','Upload Account Data','/accounts/upload','8','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('2020','2000','รายการห้ามติดต่อ','Do Not Call List','/accounts/dnc','9','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('2030','2000','บันทึกการยึดรถ','Repossession Records','/accounts/repossession','10','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('2040','2000','ติดตามคดี','Litigation Tracking','/accounts/litigation','11','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('3000','0','จัดการ Worklist','Worklist Management','/worklists','12','1','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('3010','3000','Worklist ของฉัน','My Worklists','/worklists/my','13','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('3020','3000','Worklist ของทีม','Team Worklists','/worklists/team','14','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('3030','3000','Worklist ทั้งหมด','All Worklists','/worklists/all','15','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('3040','3000','สร้าง Worklist','Create Worklist','/worklists/create','16','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('3050','3000','จัดการ Pool','Manage Pools','/worklists/pools','17','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('4000','0','การจัดสรรงาน','Assignment & Allocation','/assignments','18','1','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('4010','4000','กฎการมอบหมายงาน','Assignment Rules','/assignments/rules','19','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('4020','4000','มอบหมายงานด้วยตนเอง','Manual Assignment','/assignments/manual','20','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('4030','4000','จำลองการมอบหมายงาน','Assignment Simulation','/assignments/simulation','21','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('4040','4000','อนุมัติการมอบหมายงาน','Assignment Approvals','/assignments/approvals','22','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('5000','0','จัดการข้อมูลหลัก','Master Data Management','/master-data','23','1','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('5010','5000','ประเภทงาน','Assignment Types','/master-data/assignment-types','24','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('5020','5000','กลุ่มผลิตภัณฑ์','Product Groups','/master-data/product-groups','25','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('5030','5000','ข้อมูลภูมิศาสตร์','Geographical Data','/master-data/geography','26','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('5040','5000','ขีดจำกัดเจ้าหน้าที่','Collector Capacity','/master-data/capacity','27','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('5050','5000','กลุ่มงานเจ้าหน้าที่','Collector Work Groups','/master-data/work-groups','28','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('6000','0','รายงานและวิเคราะห์','Reports & Analytics','/reports','29','1','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('6010','6000','สรุปผลการติดตามประจำวัน','Daily Collection Summary','/reports/daily-summary','30','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('6020','6000','รายงานการมอบหมายงาน','Assignment Reports','/reports/assignments','31','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('6030','6000','รายงานการ Re-Assign','Re-Assignment Reports','/reports/reassignments','32','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('6040','6000','รายงานข้อผิดพลาดการมอบหมาย','Error Assignment Reports','/reports/errors','33','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('6050','6000','รายงานผล PTP','PTP Performance Report','/reports/ptp-performance','34','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('6060','6000','รายงาน SLA Breach','SLA Breach Report','/reports/sla-breach','35','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('6070','6000','สร้างรายงานเอง','Custom Report Builder','/reports/custom-builder','36','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('6080','6000','ควบคุมการเข้าถึงรายงาน','Report Access Control','/reports/access-control','37','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('7000','0','ผู้ใช้และความปลอดภัย','User & Security','/users-security','38','1','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('7010','7000','จัดการผู้ใช้','User Management','/users-security/user-management','39','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('7020','7000','จัดการบทบาทและสิทธิ์','Role & Permissions Management','/users-security/role-management','40','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('7030','7000','จัดการองค์กรและทีม','Organization & Team Management','/users-security/team-management','41','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('7040','7000','การกำหนดพื้นที่รับผิดชอบ','Area Assignments','/users-security/area-assignment','42','2','1')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('7050','7000','ปลดล็อกบัญชีผู้ใช้','Unlock User Account','/users-security/unlock-user-account','43','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('7060','7000','รีเซ็ตรหัสผ่านผู้ใช้','Reset User Password','/users-security/reset-password','44','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('8000','0','ตั้งค่าระบบ','System Settings','/system-settings','45','1','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('8010','8000','ข้อความข่าวประกาศ','System Announcements','/system-settings/announcements','46','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('8020','8000','ตั้งค่าการแจ้งเตือน','Notification Settings','/system-settings/notifications','47','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('8030','8000','ตั้งค่า SLA','SLA Settings','/system-settings/sla','48','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('8040','8000','การตั้งค่าระบบทั่วไป','System Configuration','/system-settings/config','49','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('9000','0','บันทึกการตรวจสอบ','Audit Log','/audit-log','50','1','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('9010','9000','ดูบันทึกการตรวจสอบ','View Audit Logs','/audit-log/view','51','2','0')
INSERT INTO [dbo].[SysItem] ([ItemID],[ParentID],[ItemNameTH],[ItemNameEN],[RoutePath],[ItemOrder],[ItemLevel],[IsActive]) VALUES('9020','9000','บันทึกกิจกรรมของฉัน','My Activity Log','/audit-log/my-activity','52','2','0')

-- SysItemAccessRight
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'1000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'1010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'1020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'1030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'1040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'2000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'2010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'2020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'2030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'2040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'3000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'3010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'3020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'3030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'3040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'3050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'4000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'4010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'4020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'4030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'4040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'5000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'5010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'5020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'5030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'5040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'5050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'6000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'6010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'6020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'6030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'6040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'6050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'6060',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'6070',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'6080',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'7000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'7010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'7020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'7030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'7040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'7050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'7060',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'8000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'8010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'8020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'8030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'8040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'9000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'9010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(1,'9020',1,1,1,1,1,1);

INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'1000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'1010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'1020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'1030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'1040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'2000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'2010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'2020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'2030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'2040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'3000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'3010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'3020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'3030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'3040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'3050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'4000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'4010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'4020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'4030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'4040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'5000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'5010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'5020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'5030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'5040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'5050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'6000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'6010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'6020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'6030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'6040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'6050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'6060',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'6070',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'6080',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'7000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'7010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'7020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'7030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'7040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'7050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'7060',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'8000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'8010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'8020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'8030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'8040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'9000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'9010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(2,'9020',1,1,1,1,1,1);

INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'1000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'1010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'1020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'1030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'1040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'2000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'2010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'2020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'2030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'2040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'3000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'3010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'3020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'3030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'3040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'3050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'4000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'4010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'4020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'4030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'4040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'5000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'5010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'5020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'5030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'5040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'5050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'6000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'6010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'6020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'6030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'6040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'6050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'6060',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'6070',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'6080',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'7000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'7010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'7020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'7030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'7040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'7050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'7060',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'8000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'8010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'8020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'8030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'8040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'9000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'9010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(3,'9020',1,1,1,1,1,1);

INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'1000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'1010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'1020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'1030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'1040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'2000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'2010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'2020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'2030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'2040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'3000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'3010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'3020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'3030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'3040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'3050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'4000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'4010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'4020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'4030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'4040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'5000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'5010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'5020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'5030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'5040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'5050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'6000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'6010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'6020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'6030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'6040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'6050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'6060',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'6070',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'6080',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'7000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'7010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'7020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'7030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'7040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'7050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'7060',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'8000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'8010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'8020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'8030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'8040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'9000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'9010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(4,'9020',1,1,1,1,1,1);

INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'1000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'1010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'1020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'1030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'1040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'2000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'2010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'2020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'2030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'2040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'3000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'3010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'3020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'3030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'3040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'3050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'4000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'4010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'4020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'4030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'4040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'5000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'5010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'5020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'5030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'5040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'5050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'6000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'6010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'6020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'6030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'6040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'6050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'6060',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'6070',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'6080',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'7000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'7010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'7020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'7030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'7040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'7050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'7060',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'8000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'8010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'8020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'8030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'8040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'9000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'9010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(5,'9020',1,1,1,1,1,1);

INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'1000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'1010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'1020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'1030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'1040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'2000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'2010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'2020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'2030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'2040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'3000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'3010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'3020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'3030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'3040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'3050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'4000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'4010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'4020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'4030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'4040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'5000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'5010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'5020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'5030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'5040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'5050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'6000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'6010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'6020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'6030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'6040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'6050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'6060',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'6070',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'6080',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'7000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'7010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'7020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'7030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'7040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'7050',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'7060',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'8000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'8010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'8020',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'8030',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'8040',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'9000',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'9010',1,1,1,1,1,1);
INSERT INTO [dbo].[SysItemAccessRight]([UserGroupID],[ItemID],[AllowNew],[AllowSave],[AllowDelete],[AllowCancel],[AllowQuery],[AllowPrint]) VALUES(6,'9020',1,1,1,1,1,1);
