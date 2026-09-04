using ACTCore.CollectionService.Domain.Entities.Masters;
using ACTCore.CollectionService.Domain.Entities.Profiles;
using ACTCore.CollectionService.Domain.Entities.Securities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ACTCore.CollectionService.Infrastructure
{
    public class DataSeeder
    {
        static readonly PasswordHasher<SysUser> _passwordHasher = new();
        public static async Task SeedDataAsync(IServiceProvider services, AppDbContext context)
        {


            // ใช้ Path.Combine เพื่อความถูกต้องของ path
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // Create View
            var sqlFilePath = Path.Combine(baseDir, "SQLScript", "vw_WorklistListing.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            sqlFilePath = Path.Combine(baseDir, "SQLScript", "vw_ReassignWorklist.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            sqlFilePath = Path.Combine(baseDir, "SQLScript", "vw_UnassignWorklist.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            // Create Trigger
            sqlFilePath = Path.Combine(baseDir, "SQLScript", "tr_WorklistToHistory.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            // seed geographic info like province, district, sub-district
            sqlFilePath = Path.Combine(baseDir, "SQLScript", "1_script-geographic-info.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            // seed master data and system security
            await SeedDataAsync(context);

            // dummy user for testing
            sqlFilePath = Path.Combine(baseDir, "SQLScript", "2_script-dummy-user.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            sqlFilePath = Path.Combine(baseDir, "SQLScript", "3_script-collection.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            sqlFilePath = Path.Combine(baseDir, "SQLScript", "4_script-contract.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            sqlFilePath = Path.Combine(baseDir, "SQLScript", "5_script-worklist.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            sqlFilePath = Path.Combine(baseDir, "SQLScript", "6_script_mapping-import.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);


            // Create View
            sqlFilePath = Path.Combine(baseDir, "SQLScript", "vw_WorklistListing.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            sqlFilePath = Path.Combine(baseDir, "SQLScript", "vw_ReassignWorklist.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            sqlFilePath = Path.Combine(baseDir, "SQLScript", "vw_UnassignWorklist.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            sqlFilePath = Path.Combine(baseDir, "SQLScript", "tr_WorklistToHistory.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            sqlFilePath = Path.Combine(baseDir, "SQLScript", "vw_CollectorAndTeamStatus.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);

            sqlFilePath = Path.Combine(baseDir, "SQLScript", "sp_GetContractOverdueSummary.sql");
            await SeedDataFromSqlFileAsync(context, sqlFilePath);
            //await CreateProcedureAsync(context);
        }

        public static async Task SeedDataAsync(AppDbContext context)
        {
            #region CompanyProfile
            await context.Database.OpenConnectionAsync();
            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [CompanyProfile] ON");
            context.CompanyProfiles.AddRange(
                 new CompanyProfile { CompanyId = 1, CompanyCode = "ADEPT", CompanyName = "Adaptivate", IsActive = true }
            );
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [CompanyProfile] OFF");
            await context.Database.CloseConnectionAsync();
            #endregion

            #region Prefix
            if (!await context.Prefixs.AnyAsync())
            {
                await using var tx = await context.Database.BeginTransactionAsync();
                await context.Database.OpenConnectionAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Prefix] ON");

                context.Prefixs.AddRange(
                    new Prefix { PrefixId = 1, PrefixCode = "1", PrefixName = "นาย", PrefixNameEn = "Mr.", IsActive = true },
                    new Prefix { PrefixId = 2, PrefixCode = "2", PrefixName = "นาง", PrefixNameEn = "Mrs.", IsActive = true },
                    new Prefix { PrefixId = 3, PrefixCode = "3", PrefixName = "นางสาว", PrefixNameEn = "Miss.", IsActive = true }
                );

                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Prefix] OFF");
                await tx.CommitAsync();
                await context.Database.CloseConnectionAsync();
            }
            #endregion

            #region Department
            await context.Database.OpenConnectionAsync();
            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Department] ON");
            context.Departments.AddRange(
                new Department { DepartmentId = 1, DepartmentCode = "MGR", DepartmentName = "บริหาร", DepartmentNameEn = "Management", IsActive = true },
                new Department { DepartmentId = 2, DepartmentCode = "PHN", DepartmentName = "ติดตามหนี้ทางโทรศัพท์", DepartmentNameEn = "Phone Collectoion", IsActive = true },
                new Department { DepartmentId = 3, DepartmentCode = "FLD", DepartmentName = "ติดตามหนี้ภาคสนาม", DepartmentNameEn = "Field Collection", IsActive = true },
                new Department { DepartmentId = 4, DepartmentCode = "LGL", DepartmentName = "กฎหมาย", DepartmentNameEn = "Legal", IsActive = true },
                new Department { DepartmentId = 5, DepartmentCode = "IT", DepartmentName = "สารสนเทศ", DepartmentNameEn = "Information Technology", IsActive = true }
            );
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Department] OFF");
            await context.Database.CloseConnectionAsync();
            #endregion

            #region Position
            if (!await context.Positions.AnyAsync())
            {
                await using var tx = await context.Database.BeginTransactionAsync();
                await context.Database.OpenConnectionAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Position] ON");

                context.Positions.AddRange(
                    new Position { PositionId = 1, PositionCode = "CEO", PositionName = "กรรมการบริหาร", PositionNameEn = "CEO", Level = 1, IsActive = true },
                    new Position { PositionId = 2, PositionCode = "DIR-101", PositionName = "ผู้อำนวยการฝ่ายปฏิบัติการ", PositionNameEn = "Operations Director", Level = 2, IsActive = true },
                    new Position { PositionId = 3, PositionCode = "MGR-101", PositionName = "ผู้จัดการฝ่ายติดตามหนี้ทางโทรศัพท์", PositionNameEn = "Phone Collector Manager", Level = 3, IsActive = true },
                    new Position { PositionId = 4, PositionCode = "PHN-100", PositionName = "หัวหน้าทีมติดตามหนี้ทางโทรศัพท์", PositionNameEn = "Phone Collector Team Lead", Level = 4, IsActive = true },
                    new Position { PositionId = 5, PositionCode = "PHN-101", PositionName = "เจ้าหน้าที่ติดตามหนี้ทางโทรศัพท์อาวุโส", PositionNameEn = "Senior Phone Collector", Level = 5, IsActive = true },
                    new Position { PositionId = 6, PositionCode = "PHN-102", PositionName = "เจ้าหน้าที่ติดตามหนี้ทางโทรศัพท์", PositionNameEn = "Phone Collector / Telesales Collector", Level = 5, IsActive = true },
                    new Position { PositionId = 7, PositionCode = "MGR-102", PositionName = "ผู้จัดการฝ่ายติดตามหนี้ภาคสนาม", PositionNameEn = "Field Collector Manager", Level = 3, IsActive = true },
                    new Position { PositionId = 8, PositionCode = "FLD-100", PositionName = "หัวหน้าทีมติดตามหนี้ภาคสนาม", PositionNameEn = "Field Collector Team Lead", Level = 4, IsActive = true },
                    new Position { PositionId = 9, PositionCode = "FLD-101", PositionName = "เจ้าหน้าที่ติดตามหนี้ภาคสนามอาวุโส", PositionNameEn = "Senior Field Collector", Level = 5, IsActive = true },
                    new Position { PositionId = 10, PositionCode = "FLD-102", PositionName = "เจ้าหน้าที่ติดตามหนี้ภาคสนาม", PositionNameEn = "Field Collector", Level = 5, IsActive = true },
                    new Position { PositionId = 11, PositionCode = "MGR-201", PositionName = "ผู้จัดการฝ่ายกฎหมาย", PositionNameEn = "Legal Manager", Level = 3, IsActive = true },
                    new Position { PositionId = 12, PositionCode = "LGL-201", PositionName = "เจ้าหน้าที่ฝ่ายกฎหมาย", PositionNameEn = "Legal Officer", Level = 4, IsActive = true },
                    new Position { PositionId = 13, PositionCode = "LGL-202", PositionName = "ทนายความ", PositionNameEn = "Lawyer", Level = 4, IsActive = true },
                    new Position { PositionId = 14, PositionCode = "LGL-203", PositionName = "เจ้าหน้าที่ธุรการ", PositionNameEn = "Administrative Officer", Level = 5, IsActive = true },
                    new Position { PositionId = 15, PositionCode = "IT-201", PositionName = "ผู้จัดการสารสนเทศ", PositionNameEn = "IT Manager", Level = 3, IsActive = true },
                    new Position { PositionId = 16, PositionCode = "IT-202", PositionName = "เจ้าหน้าที่สารสนเทศ", PositionNameEn = "IT Officer", Level = 4, IsActive = true },
                    new Position { PositionId = 17, PositionCode = "MIS-201", PositionName = "นักวิเคราะห์ข้อมูล / เจ้าหน้าที่ MIS", PositionNameEn = "Data Analyst / MIS Officer", Level = 4, IsActive = true }
                );

                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Position] OFF");
                await tx.CommitAsync();
                await context.Database.CloseConnectionAsync();
            }
            #endregion

            #region EmployeeProfile
            context.EmployeeProfiles.AddRange(
                new EmployeeProfile { EmployeeId = "00000", UserName = "Admin", PrefixId = 1, FirstName = "admin", LastName = null, FirstNameEn = "admin", LastNameEn = null, NickName = null, Gender = "M", DateOfBirth = null, WorkStatus = "Active", Email = "Admin@XYZ.com", Branch = null, SupervisorId = null, TeamId = null, DepartmentId = null, PositionId = null, IsActive = true },
                new EmployeeProfile { EmployeeId = "00001", UserName = "Admin2", PrefixId = 1, FirstName = "admin2", LastName = null, FirstNameEn = "admin2", LastNameEn = null, NickName = null, Gender = "M", DateOfBirth = null, WorkStatus = "Active", Email = "Admin2@XYZ.com", Branch = null, SupervisorId = null, TeamId = null, DepartmentId = null, PositionId = null, IsActive = true },
                new EmployeeProfile { EmployeeId = "00002", UserName = "ITAdmin", PrefixId = 1, FirstName = "ITAdmin", LastName = null, FirstNameEn = "ITAdmin", LastNameEn = null, NickName = null, Gender = "M", DateOfBirth = null, WorkStatus = "Active", Email = "ITAdmin@XYZ.com", Branch = null, SupervisorId = null, TeamId = null, DepartmentId = null, PositionId = null, IsActive = true },
                new EmployeeProfile { EmployeeId = "00003", UserName = "ITAdmin2", PrefixId = 1, FirstName = "ITAdmin2", LastName = null, FirstNameEn = "ITAdmin2", LastNameEn = null, NickName = null, Gender = "M", DateOfBirth = null, WorkStatus = "Active", Email = "ITAdmin2@XYZ.com", Branch = null, SupervisorId = null, TeamId = null, DepartmentId = null, PositionId = null, IsActive = true },
                new EmployeeProfile { EmployeeId = "60001", UserName = "somchaip", PrefixId = 1, FirstName = "สมชาย", LastName = "พัฒนา", FirstNameEn ="Somchai" , LastNameEn ="Phatthana" , NickName ="แมน" , Gender ="M" , WorkStatus ="Active" , Email ="somchaip@XYZ.com" , Branch=null , SupervisorId=null , TeamId=null , DepartmentId=1 , PositionId=1 , IsActive = true },
                new EmployeeProfile { EmployeeId = "61001", UserName = "kriengkraim", PrefixId = 1, FirstName = "เกรียงไกร", LastName = "มั่นคง", FirstNameEn = "Kriangkrai", LastNameEn = "Mankong", NickName = "กานต์", Gender = "M", WorkStatus = "Active", Email = "kriengkraim@XYZ.com", Branch = null, SupervisorId = "60001", TeamId = null, DepartmentId = 2, PositionId = 3, IsActive = true },
                new EmployeeProfile { EmployeeId = "61002", UserName = "patimar", PrefixId = 1, FirstName = "ปฏิมา", LastName = "รุ่งเรือง", FirstNameEn = "Patima", LastNameEn = "Rungrueang", NickName = "ปิ๊ง", Gender = "M", WorkStatus = "Active", Email = "patimar@XYZ.com", Branch = null, SupervisorId = "61001", TeamId = null, DepartmentId = 1, PositionId = 2, IsActive = true },
                new EmployeeProfile { EmployeeId = "61029", UserName = "thaipornn", PrefixId = 2, FirstName = "ธัยภรณ์", LastName = "ชาญชิต", FirstNameEn = "Thaiporn", LastNameEn = "Charnchit", NickName = "ปุ๋ย", Gender = "F", WorkStatus = "Active", Email = "thaipornn@XYZ.com", Branch = null, SupervisorId = "66033", TeamId = null, DepartmentId = 4, PositionId = 12, IsActive = true },
                new EmployeeProfile { EmployeeId = "61042", UserName = "sumaleek", PrefixId = 2, FirstName = "สุมาลี", LastName = "เค้าแดง", FirstNameEn = "Sumalee", LastNameEn = "Khaodaeng", NickName = "ยุ้ย", Gender = "F", WorkStatus = "Active", Email = "sumaleek@XYZ.com", Branch = null, SupervisorId = "66047", TeamId = null, DepartmentId = 5, PositionId = 16, IsActive = true },
                new EmployeeProfile { EmployeeId = "61056", UserName = "surind", PrefixId = 1, FirstName = "สุรินทร์", LastName = "ดังเด่น", FirstNameEn = "Surin", LastNameEn = "Dangden", NickName = "แว่น", Gender = "M", WorkStatus = "Active", Email = "surind@XYZ.com", Branch = null, SupervisorId = "62001", TeamId = null, DepartmentId = 2, PositionId = 6, IsActive = true },
                new EmployeeProfile { EmployeeId = "61060", UserName = "nokkaewn", PrefixId = 2, FirstName = "นกแก้ว", LastName = "เหมือนเขียว", FirstNameEn = "Nokkaew", LastNameEn = "Mueankhiaw", NickName = "อ้อ", Gender = "F", WorkStatus = "Active", Email = "nokkaewn@XYZ.com", Branch = null, SupervisorId = "62001", TeamId = null, DepartmentId = 2, PositionId = 5, IsActive = true },
                new EmployeeProfile { EmployeeId = "61065", UserName = "tasaneeh", PrefixId = 2, FirstName = "ทัศนีย์", LastName = "ทนคําดี", FirstNameEn = "Tasanee", LastNameEn = "Tonkhamdee", NickName = "ต่าย", Gender = "F", WorkStatus = "Active", Email = "tasaneeh@XYZ.com", Branch = null, SupervisorId = "62074", TeamId = null, DepartmentId = 3, PositionId = 9, IsActive = true },
                new EmployeeProfile { EmployeeId = "62001", UserName = "piyanee", PrefixId = 3, FirstName = "ปิยะณี", LastName = "วรรณคดี", FirstNameEn = "Piyanee", LastNameEn = "Wannakhadi", NickName = "แพม", Gender = "F", WorkStatus = "Active", Email = "piyanee@XYZ.com", Branch = null, SupervisorId = "61001", TeamId = null, DepartmentId = 2, PositionId = 4, IsActive = true },
                new EmployeeProfile { EmployeeId = "62029", UserName = "kasemaneep", PrefixId = 3, FirstName = "เกษมณี", LastName = "ปราบปราม", FirstNameEn = "Kasemanee", LastNameEn = "Prabpram", NickName = "มิ้นท์", Gender = "F", WorkStatus = "Active", Email = "kasemaneep@XYZ.com", Branch = null, SupervisorId = "62001", TeamId = null, DepartmentId = 2, PositionId = 6, IsActive = true },
                new EmployeeProfile { EmployeeId = "62032", UserName = "rungpetchh", PrefixId = 1, FirstName = "รุ่งเพ็ชร", LastName = "โพธิ์เทศ", FirstNameEn = "Rungpetch", LastNameEn = "Phothet", NickName = "เปิ้ล", Gender = "M", WorkStatus = "Active", Email = "rungpetchh@XYZ.com", Branch = null, SupervisorId = "62074", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "62051", UserName = "jiraporna", PrefixId = 3, FirstName = "จิราพร", LastName = "ชตารุ่ง", FirstNameEn = "Jiraporn", LastNameEn = "Chatarung", NickName = "จี๊ป", Gender = "F", WorkStatus = "Active", Email = "jiraporna@XYZ.com", Branch = null, SupervisorId = "62001", TeamId = null, DepartmentId = 2, PositionId = 5, IsActive = true },
                new EmployeeProfile { EmployeeId = "62059", UserName = "veerawann", PrefixId = 3, FirstName = "วีระวรรณ", LastName = "ชื่นชม", FirstNameEn = "Veerawan", LastNameEn = "Chuenchom", NickName = "ป่าน", Gender = "F", WorkStatus = "Active", Email = "veerawann@XYZ.com", Branch = null, SupervisorId = "62074", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "62073", UserName = "suntornw", PrefixId = 1, FirstName = "สุนทร", LastName = "กล่อมวาจา", FirstNameEn = "Suntorn", LastNameEn = "Klomwaja", NickName = "เต้", Gender = "M", WorkStatus = "Active", Email = "suntornw@XYZ.com", Branch = null, SupervisorId = "62074", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "62074", UserName = "nopparati", PrefixId = 1, FirstName = "นพรัตน์", LastName = "สุขยิ่งผา", FirstNameEn = "Nopparat", LastNameEn = "Sukyingpha", NickName = "เจมส์", Gender = "M", WorkStatus = "Active", Email = "nopparati@XYZ.com", Branch = null, SupervisorId = "65062", TeamId = null, DepartmentId = 3, PositionId = 8, IsActive = true },
                new EmployeeProfile { EmployeeId = "62082", UserName = "wirata", PrefixId = 1, FirstName = "วิรัตน์", LastName = "ปานมา", FirstNameEn = "Wirat", LastNameEn = "Panma", NickName = "บี", Gender = "M", WorkStatus = "Active", Email = "wirata@XYZ.com", Branch = null, SupervisorId = "66033", TeamId = null, DepartmentId = 4, PositionId = 14, IsActive = true },
                new EmployeeProfile { EmployeeId = "62098", UserName = "jaruratr", PrefixId = 1, FirstName = "จารุรัตน์", LastName = "ขันรัมย์", FirstNameEn = "Jarurat", LastNameEn = "Khanram", NickName = "โบว์", Gender = "M", WorkStatus = "Active", Email = "jaruratr@XYZ.com", Branch = null, SupervisorId = "63012", TeamId = null, DepartmentId = 3, PositionId = 9, IsActive = true },
                new EmployeeProfile { EmployeeId = "62101", UserName = "thanakornc", PrefixId = 1, FirstName = "ธนากร", LastName = "โชคดี", FirstNameEn = "Thanakorn", LastNameEn = "Chokdee", NickName = "แบงค์", Gender = "M", WorkStatus = "Active", Email = "thanakornc@XYZ.com", Branch = null, SupervisorId = "61001", TeamId = null, DepartmentId = 2, PositionId = 4, IsActive = true },
                new EmployeeProfile { EmployeeId = "62102", UserName = "manitm", PrefixId = 1, FirstName = "มานิตย์", LastName = "มีชัย", FirstNameEn = "Manit", LastNameEn = "Meechai", NickName = "นิดหน่อย", Gender = "M", WorkStatus = "Active", Email = "manitm@XYZ.com", Branch = null, SupervisorId = "62101", TeamId = null, DepartmentId = 2, PositionId = 5, IsActive = true },
                new EmployeeProfile { EmployeeId = "62103", UserName = "sudam", PrefixId = 3, FirstName = "สุดาพร", LastName = "มีสุข", FirstNameEn = "Sudaporn", LastNameEn = "Meesuk", NickName = "แพรว", Gender = "F", WorkStatus = "Active", Email = "sudam@XYZ.com", Branch = null, SupervisorId = "61001", TeamId = null, DepartmentId = 2, PositionId = 4, IsActive = true },
                new EmployeeProfile { EmployeeId = "62104", UserName = "chanink", PrefixId = 1, FirstName = "ชนินทร์", LastName = "คำสุข", FirstNameEn = "Chanin", LastNameEn = "Khamsuk", NickName = "นนท์", Gender = "M", WorkStatus = "Active", Email = "chanink@XYZ.com", Branch = null, SupervisorId = "62101", TeamId = null, DepartmentId = 2, PositionId = 5, IsActive = true },
                new EmployeeProfile { EmployeeId = "62105", UserName = "apichartl", PrefixId = 1, FirstName = "อภิชาติ", LastName = "เลิศล้ำ", FirstNameEn = "Apichat", LastNameEn = "Loetlam", NickName = "เอฟ", Gender = "M", WorkStatus = "Active", Email = "apichartl@XYZ.com", Branch = null, SupervisorId = "62101", TeamId = null, DepartmentId = 2, PositionId = 5, IsActive = true },
                new EmployeeProfile { EmployeeId = "63001", UserName = "chanokp", PrefixId = 3, FirstName = "ชนกนาถ", LastName = "พลอยงาม", FirstNameEn = "Chanoknat", LastNameEn = "Ployngam", NickName = "จอย", Gender = "F", WorkStatus = "Active", Email = "chanokp@XYZ.com", Branch = null, SupervisorId = "61001", TeamId = null, DepartmentId = 2, PositionId = 4, IsActive = true },
                new EmployeeProfile { EmployeeId = "63012", UserName = "viroja", PrefixId = 1, FirstName = "วิโรจน์", LastName = "วัฒนปรานีกูล", FirstNameEn = "Viroj", LastNameEn = "Wattanapranikun", NickName = "ตั้ม", Gender = "M", WorkStatus = "Active", Email = "viroja@XYZ.com", Branch = null, SupervisorId = "65062", TeamId = null, DepartmentId = 3, PositionId = 8, IsActive = true },
                new EmployeeProfile { EmployeeId = "63044", UserName = "usah", PrefixId = 2, FirstName = "อุษา", LastName = "ศรีจันทร์", FirstNameEn = "Usa", LastNameEn = "Srichan", NickName = "ป้าจอย", Gender = "F", WorkStatus = "Active", Email = "usah@XYZ.com", Branch = null, SupervisorId = "66047", TeamId = null, DepartmentId = 5, PositionId = 16, IsActive = true },
                new EmployeeProfile { EmployeeId = "63080", UserName = "somnueka", PrefixId = 1, FirstName = "สมนึก", LastName = "เสลาคุณ", FirstNameEn = "Somnuek", LastNameEn = "Saolakun", NickName = "ต้อม", Gender = "M", WorkStatus = "Active", Email = "somnueka@XYZ.com", Branch = null, SupervisorId = "66047", TeamId = null, DepartmentId = 5, PositionId = 17, IsActive = true },
                new EmployeeProfile { EmployeeId = "63090", UserName = "phanaratn", PrefixId = 2, FirstName = "พนารัตน์", LastName = "รัตนเมธากร", FirstNameEn = "Phanarat", LastNameEn = "Ratanamethakon", NickName = "นีน่า", Gender = "F", WorkStatus = "Active", Email = "phanaratn@XYZ.com", Branch = null, SupervisorId = "63012", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "63096", UserName = "tasanao", PrefixId = 1, FirstName = "ทัศนะ", LastName = "เขียวขํา", FirstNameEn = "Tasana", LastNameEn = "Khiaokham", NickName = "โต้ง", Gender = "M", WorkStatus = "Active", Email = "tasanao@XYZ.com", Branch = null, SupervisorId = "63012", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "63101", UserName = "thanapornb", PrefixId = 3, FirstName = "ธนาพร", LastName = "บุญมา", FirstNameEn = "Thanaporn", LastNameEn = "Boonma", NickName = "น้ำตาล", Gender = "F", WorkStatus = "Active", Email = "thanapornb@XYZ.com", Branch = null, SupervisorId = "62101", TeamId = null, DepartmentId = 2, PositionId = 5, IsActive = true },
                new EmployeeProfile { EmployeeId = "63102", UserName = "somsakj", PrefixId = 1, FirstName = "สมศักดิ์", LastName = "ใจดี", FirstNameEn = "Somsak", LastNameEn = "Jaidee", NickName = "โอ๊ต", Gender = "M", WorkStatus = "Active", Email = "somsakj@XYZ.com", Branch = null, SupervisorId = "61001", TeamId = null, DepartmentId = 2, PositionId = 4, IsActive = true },
                new EmployeeProfile { EmployeeId = "63103", UserName = "banyongb", PrefixId = 1, FirstName = "บัญชา", LastName = "บุญสุข", FirstNameEn = "Bancha", LastNameEn = "Boonsuk", NickName = "บิ๊ก", Gender = "M", WorkStatus = "Active", Email = "banyongb@XYZ.com", Branch = null, SupervisorId = "62103", TeamId = null, DepartmentId = 2, PositionId = 5, IsActive = true },
                new EmployeeProfile { EmployeeId = "64001", UserName = "ariyan", PrefixId = 3, FirstName = "อาริยา", LastName = "งามวงศ์", FirstNameEn = "Ariya", LastNameEn = "Ngamwong", NickName = "มีน", Gender = "F", WorkStatus = "Active", Email = "ariyan@XYZ.com", Branch = null, SupervisorId = "62103", TeamId = null, DepartmentId = 2, PositionId = 5, IsActive = true },
                new EmployeeProfile { EmployeeId = "64040", UserName = "wannaratg", PrefixId = 3, FirstName = "วรรณรัตน์", LastName = "ช้างน้ํา", FirstNameEn = "Wannarat", LastNameEn = "Changnam", NickName = "เมย์", Gender = "F", WorkStatus = "Active", Email = "wannaratg@XYZ.com", Branch = null, SupervisorId = "62103", TeamId = null, DepartmentId = 2, PositionId = 6, IsActive = true },
                new EmployeeProfile { EmployeeId = "64045", UserName = "saengchank", PrefixId = 2, FirstName = "แสงจันทร์", LastName = "แก้วอ่วม", FirstNameEn = "Saengchan", LastNameEn = "Kaeouam", NickName = "พิม", Gender = "F", WorkStatus = "Active", Email = "saengchank@XYZ.com", Branch = null, SupervisorId = "63012", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "64052", UserName = "ongarti", PrefixId = 1, FirstName = "องอาจ", LastName = "ทับทิม", FirstNameEn = "Ongart", LastNameEn = "Tubtim", NickName = "โจ", Gender = "M", WorkStatus = "Active", Email = "ongarti@XYZ.com", Branch = null, SupervisorId = "63012", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "64055", UserName = "thongchaio", PrefixId = 1, FirstName = "ธงชัย", LastName = "แกล้วกสิกรรม", FirstNameEn = "Thongchai", LastNameEn = "Klaeokasikam", NickName = "อ๊อฟ", Gender = "M", WorkStatus = "Active", Email = "thongchaio@XYZ.com", Branch = null, SupervisorId = "66033", TeamId = null, DepartmentId = 4, PositionId = 13, IsActive = true },
                new EmployeeProfile { EmployeeId = "64057", UserName = "taengonc", PrefixId = 3, FirstName = "แตงอ่อน", LastName = "โชคชุมชูดวง", FirstNameEn = "Taengon", LastNameEn = "Chokchumchuduang", NickName = "ทับทิม", Gender = "F", WorkStatus = "Active", Email = "taengonc@XYZ.com", Branch = null, SupervisorId = "62103", TeamId = null, DepartmentId = 2, PositionId = 6, IsActive = true },
                new EmployeeProfile { EmployeeId = "64059", UserName = "wattanah", PrefixId = 1, FirstName = "วัฒนา", LastName = "ศรีจันทร์", FirstNameEn = "Wattana", LastNameEn = "Srichan", NickName = "หน่อย", Gender = "M", WorkStatus = "Active", Email = "wattanah@XYZ.com", Branch = null, SupervisorId = "63012", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "64081", UserName = "ekkapolo", PrefixId = 1, FirstName = "เอกพล", LastName = "นาคน้อย", FirstNameEn = "Ekkapol", LastNameEn = "Naknoi", NickName = "ป๊อบ", Gender = "M", WorkStatus = "Active", Email = "ekkapolo@XYZ.com", Branch = null, SupervisorId = "62103", TeamId = null, DepartmentId = 2, PositionId = 6, IsActive = true },
                new EmployeeProfile { EmployeeId = "64101", UserName = "natthidaj", PrefixId = 3, FirstName = "ณัฐธิดา", LastName = "เจริญ", FirstNameEn = "Natthida", LastNameEn = "Charoen", NickName = "อิ่ม", Gender = "F", WorkStatus = "Active", Email = "natthidaj@XYZ.com", Branch = null, SupervisorId = "63001", TeamId = null, DepartmentId = 2, PositionId = 6, IsActive = true },
                new EmployeeProfile { EmployeeId = "64102", UserName = "weerachais", PrefixId = 1, FirstName = "วีระชัย", LastName = "สุขสบาย", FirstNameEn = "Weerachai", LastNameEn = "Suksabai", NickName = "อาร์ท", Gender = "M", WorkStatus = "Active", Email = "weerachais@XYZ.com", Branch = null, SupervisorId = "63001", TeamId = null, DepartmentId = 2, PositionId = 6, IsActive = true },
                new EmployeeProfile { EmployeeId = "64103", UserName = "sombatm", PrefixId = 1, FirstName = "สมบัติ", LastName = "มีโชค", FirstNameEn = "Sombat", LastNameEn = "Meechok", NickName = "ต๊อบ", Gender = "M", WorkStatus = "Active", Email = "sombatm@XYZ.com", Branch = null, SupervisorId = "63001", TeamId = null, DepartmentId = 2, PositionId = 5, IsActive = true },
                new EmployeeProfile { EmployeeId = "64104", UserName = "korakojt", PrefixId = 3, FirstName = "กรกต", LastName = "ไทยเจริญ", FirstNameEn = "Korakot", LastNameEn = "Thaijaroen", NickName = "ก้อง", Gender = "F", WorkStatus = "Active", Email = "korakojt@XYZ.com", Branch = null, SupervisorId = "63001", TeamId = null, DepartmentId = 2, PositionId = 6, IsActive = true },
                new EmployeeProfile { EmployeeId = "64105", UserName = "ongardk", PrefixId = 1, FirstName = "องอาจ", LastName = "คำมี", FirstNameEn = "Ongart", LastNameEn = "Khammee", NickName = "ตูน", Gender = "M", WorkStatus = "Active", Email = "ongardk@XYZ.com", Branch = null, SupervisorId = "63102", TeamId = null, DepartmentId = 2, PositionId = 6, IsActive = true },
                new EmployeeProfile { EmployeeId = "65014", UserName = "rangnapas", PrefixId = 3, FirstName = "รุ่งนภา", LastName = "สุขใส", FirstNameEn = "Rungnapha", LastNameEn = "Suksai", NickName = "ฟ้า", Gender = "F", WorkStatus = "Active", Email = "rangnapas@XYZ.com", Branch = null, SupervisorId = "63102", TeamId = null, DepartmentId = 2, PositionId = 6, IsActive = true },
                new EmployeeProfile { EmployeeId = "65036", UserName = "nanthanaphornk", PrefixId = 2, FirstName = "นันทนาภรณ์", LastName = "แก้วอําพันธ์", FirstNameEn = "Nanthanaphorn", LastNameEn = "Kaeoamphan", NickName = "พลอย", Gender = "F", WorkStatus = "Active", Email = "nanthanaphornk@XYZ.com", Branch = null, SupervisorId = "65050", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "65039", UserName = "atitiyaa", PrefixId = 3, FirstName = "อาทิติยา", LastName = "ศรียะโสธร", FirstNameEn = "Atitiya", LastNameEn = "Sriyasothorn", NickName = "ติ๊ก", Gender = "F", WorkStatus = "Active", Email = "atitiyaa@XYZ.com", Branch = null, SupervisorId = "66033", TeamId = null, DepartmentId = 4, PositionId = 12, IsActive = true },
                new EmployeeProfile { EmployeeId = "65041", UserName = "chaiwatw", PrefixId = 1, FirstName = "ชัยวัฒน์", LastName = "วงศ์สุภาพ", FirstNameEn = "Chaiwat", LastNameEn = "Wongsuphap", NickName = "ต้น", Gender = "M", WorkStatus = "Active", Email = "chaiwatw@XYZ.com", Branch = null, SupervisorId = "63102", TeamId = null, DepartmentId = 2, PositionId = 6, IsActive = true },
                new EmployeeProfile { EmployeeId = "65050", UserName = "chanokorni", PrefixId = 1, FirstName = "ชนกร", LastName = "กระถินทอง", FirstNameEn = "Chanokorn", LastNameEn = "Kratinthong", NickName = "อ๋อง", Gender = "M", WorkStatus = "Active", Email = "chanokorni@XYZ.com", Branch = null, SupervisorId = "65062", TeamId = null, DepartmentId = 3, PositionId = 8, IsActive = true },
                new EmployeeProfile { EmployeeId = "65051", UserName = "amphani", PrefixId = 2, FirstName = "อําพันธ์", LastName = "ยอดจิตต์", FirstNameEn = "Amphan", LastNameEn = "Yodjit", NickName = "แอม", Gender = "F", WorkStatus = "Active", Email = "amphani@XYZ.com", Branch = null, SupervisorId = "65062", TeamId = null, DepartmentId = 3, PositionId = 8, IsActive = true },
                new EmployeeProfile { EmployeeId = "65062", UserName = "arakt", PrefixId = 1, FirstName = "อารักษ์", LastName = "ขันทอง", FirstNameEn = "Arak", LastNameEn = "Khanthong", NickName = "นัท", Gender = "M", WorkStatus = "Active", Email = "arakt@XYZ.com", Branch = null, SupervisorId = "61002", TeamId = null, DepartmentId = 3, PositionId = 7, IsActive = true },
                new EmployeeProfile { EmployeeId = "65091", UserName = "phoonsrio", PrefixId = 2, FirstName = "พูนศรี", LastName = "สุดสงคราม", FirstNameEn = "Phoonsri", LastNameEn = "Sudsongkhram", NickName = "อ้อย", Gender = "F", WorkStatus = "Active", Email = "phoonsrio@XYZ.com", Branch = null, SupervisorId = "65050", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "65095", UserName = "sittichokg", PrefixId = 1, FirstName = "สิทธิโชค", LastName = "แดงกุล", FirstNameEn = "Sittichok", LastNameEn = "Daengkul", NickName = "กอฟล์", Gender = "M", WorkStatus = "Active", Email = "sittichokg@XYZ.com", Branch = null, SupervisorId = "63102", TeamId = null, DepartmentId = 2, PositionId = 6, IsActive = true },
                new EmployeeProfile { EmployeeId = "66019", UserName = "wandeey", PrefixId = 2, FirstName = "วันดี", LastName = "สัตยาลักษณ์", FirstNameEn = "Wandee", LastNameEn = "Satayalak", NickName = "ใหม่", Gender = "F", WorkStatus = "Active", Email = "wandeey@XYZ.com", Branch = null, SupervisorId = "65050", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "66021", UserName = "khamkhunm", PrefixId = 1, FirstName = "คําคุน", LastName = "ชาติมนตรี", FirstNameEn = "Khamkhun", LastNameEn = "Chatmontri", NickName = "แจ็ค", Gender = "M", WorkStatus = "Active", Email = "khamkhunm@XYZ.com", Branch = null, SupervisorId = "65050", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "66027", UserName = "sirinunj", PrefixId = 1, FirstName = "ศิรินันท์", LastName = "ยงจิรกุลพงศ์", FirstNameEn = "Sirinun", LastNameEn = "Yongjirakulphong", NickName = "ติ๊ก", Gender = "M", WorkStatus = "Active", Email = "sirinunj@XYZ.com", Branch = null, SupervisorId = "65051", TeamId = null, DepartmentId = 3, PositionId = 9, IsActive = true },
                new EmployeeProfile { EmployeeId = "66033", UserName = "pornchaip", PrefixId = 1, FirstName = "พรชัย", LastName = "ชาติพุก", FirstNameEn = "Pornchai", LastNameEn = "Chatphuk", NickName = "พี", Gender = "M", WorkStatus = "Active", Email = "pornchaip@XYZ.com", Branch = null, SupervisorId = "61002", TeamId = null, DepartmentId = 4, PositionId = 11, IsActive = true },
                new EmployeeProfile { EmployeeId = "66036", UserName = "nuchareen", PrefixId = 3, FirstName = "นุชรี", LastName = "วาทิน", FirstNameEn = "Nucharee", LastNameEn = "Watin", NickName = "นุ้ย", Gender = "F", WorkStatus = "Active", Email = "nuchareen@XYZ.com", Branch = null, SupervisorId = "65051", TeamId = null, DepartmentId = 3, PositionId = 10, IsActive = true },
                new EmployeeProfile { EmployeeId = "66047", UserName = "ampapanp", PrefixId = 2, FirstName = "อัมพาพันธ์", LastName = "วังปลาทอง", FirstNameEn = "Ampapan", LastNameEn = "Wangplathong", NickName = "ปราง", Gender = "F", WorkStatus = "Active", Email = "ampapanp@XYZ.com", Branch = null, SupervisorId = "61002", TeamId = null, DepartmentId = 5, PositionId = 15, IsActive = true },
                new EmployeeProfile { EmployeeId = "66050", UserName = "nanticham", PrefixId = 2, FirstName = "นันทิชา", LastName = "บุญมาก", FirstNameEn = "Nanticha", LastNameEn = "Boonmak", NickName = "เจน", Gender = "F", WorkStatus = "Active", Email = "nanticham@XYZ.com", Branch = null, SupervisorId = "66033", TeamId = null, DepartmentId = 4, PositionId = 13, IsActive = true },
                new EmployeeProfile { EmployeeId = "66055", UserName = "chatchanana", PrefixId = 1, FirstName = "ชัชนันท์", LastName = "มั่นใจตน", FirstNameEn = "Chatchanan", LastNameEn = "Manjaiton", NickName = "เฟิร์น", Gender = "M", WorkStatus = "Active", Email = "chatchanana@XYZ.com", Branch = null, SupervisorId = "65051", TeamId = null, DepartmentId = 3, PositionId = 9, IsActive = true }
            );
            await context.SaveChangesAsync();
            #endregion

            #region SysParameter
            await context.Database.OpenConnectionAsync();
            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [SysParameter] ON");
            context.SysParameters.AddRange(
                new SysParameter { Id = 1, ParameterCategory = "System Parameter", ParameterName = "AuthenMode", ParameterValue = "basic", IsSystem = true, Description = "Basic Authend: basic, AD: ad, ...." },
                new SysParameter { Id = 2, ParameterCategory = "System Parameter", ParameterName = "DefaultPassword", ParameterValue = "password", IsSystem = true, Description = "Default password for new user login" },
                new SysParameter
                {
                    Id = 3,
                    ParameterCategory = "System Parameter",
                    ParameterName = "Language",
                    ParameterValue = @"[{""text"": ""EN - English"",""value"": ""EN"",""locale"": ""En""},{""text"": ""TH - ไทย"",""value"": ""TH"",""locale"": ""Th""}]",
                    IsSystem = true,
                    Description = "Available Languages"
                }
            );
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [SysParameter] OFF");
            await context.Database.CloseConnectionAsync();
            #endregion

            #region SysPolicy
            await context.Database.OpenConnectionAsync();
            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [SysPolicy] ON");
            context.SysPolicies.AddRange(
                new SysPolicy { Id = 1, PolicyCategory = "Password Policy", PolicyCode = "password_age", PolicyName = "อายุการใช้งานของ Password", PolicyValue = "90", IsActive = true },
                new SysPolicy { Id = 2, PolicyCategory = "Password Policy", PolicyCode = "locked_user", PolicyName = "ล็อกผู้ใช้เมื่อใส่ Password ไม่ถูกต้องเป็นจำนวน", PolicyValue = "3", IsActive = true },
                new SysPolicy { Id = 3, PolicyCategory = "Password Policy", PolicyCode = "password_his_length", PolicyName = "จำนวนครั้งของ Password เดิมที่เก็บย้อนหลัง", PolicyValue = "3", IsActive = true },
                new SysPolicy { Id = 4, PolicyCategory = "Password Validation", PolicyCode = "min_length", PolicyName = "จำนวนตัวอักษรของ Password ต้องใส่อย่างน้อย", PolicyValue = "8", IsActive = false },
                new SysPolicy { Id = 5, PolicyCategory = "Password Validation", PolicyCode = "max_length", PolicyName = "จำนวนตัวอักษรของ Password สามารถใส่ได้ไม่เกิน", PolicyValue = "20", IsActive = false },
                new SysPolicy { Id = 6, PolicyCategory = "Password Validation", PolicyCode = "consecutive_char", PolicyName = "ห้ามใช้ Password ที่มีตัวอักษรที่ซ้ำหรือต่อเนื่องกัน", PolicyValue = null, IsActive = false },
                new SysPolicy { Id = 7, PolicyCategory = "Password Validation", PolicyCode = "same_position", PolicyName = "ห้ามใช้ Password ที่มีตำแหน่งของตัวอักษรซ้ำกับตำแหน่งเดิมของ Password ที่เคยใช้ครั้งก่อน", PolicyValue = null, IsActive = true },
                new SysPolicy { Id = 8, PolicyCategory = "Password Validation", PolicyCode = "contain_3types", PolicyName = "โปรดระบุ Passwordที่ประกอบด้วยประเภทของตัวอักษร 3 ใน 4 ประเภท", PolicyValue = null, IsActive = false },
                new SysPolicy { Id = 9, PolicyCategory = "Password Validation", PolicyCode = "notallow_userid", PolicyName = "ห้ามใช้ User ID มาตั้งเป็นส่วนหนึ่งส่วนใดของ Password", PolicyValue = null, IsActive = false }
            );
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [SysPolicy] OFF");
            await context.Database.CloseConnectionAsync();
            #endregion

            #region SysUserGroup
            Guid groupAdmin = Guid.Parse("723dd726-a572-4059-bb70-12833320d5c0");
            Guid groupItSupport = Guid.Parse("3a5814d8-14b8-4ae3-91da-38c42498ffe1");
            Guid groupColAdmin = Guid.Parse("dad5a7c6-408b-454b-b667-94d6a0e3fbe6");
            Guid groupManager = Guid.Parse("b166f7e0-91b0-4b82-90f1-4be6e3270560");
            Guid groupSupervisor = Guid.Parse("a5ee4446-b8d9-425d-874b-1258ebbcf08e");
            Guid groupOperator = Guid.Parse("e10d2a87-d135-4972-9923-ebb9d7c63162");
            Guid groupViewer = Guid.Parse("0d439ca1-7cbf-4581-b14b-b6c2d94f2022");

            context.SysUserGroups.AddRange(
               new SysUserGroup { UserGroupId = groupAdmin, UserGroupCode = "10", UserGroupName = "Administrator", IsActive = true },
               new SysUserGroup { UserGroupId = groupItSupport, UserGroupCode = "20", UserGroupName = "IT Support", IsActive = true },
               new SysUserGroup { UserGroupId = groupColAdmin, UserGroupCode = "30", UserGroupName = "Collection Admin", IsActive = true },
               new SysUserGroup { UserGroupId = groupManager, UserGroupCode = "40", UserGroupName = "Manager", IsActive = true },
               new SysUserGroup { UserGroupId = groupSupervisor, UserGroupCode = "50", UserGroupName = "Supervisor", IsActive = true },
               new SysUserGroup { UserGroupId = groupOperator, UserGroupCode = "60", UserGroupName = "Operator", IsActive = true },
               new SysUserGroup { UserGroupId = groupViewer, UserGroupCode = "90", UserGroupName = "Viewer", IsActive = true }
            );
            await context.SaveChangesAsync();
            #endregion

            #region SysPermission (Next Phase2)
            //Guid permission_import = Guid.Parse("9afe2a9d-e65e-4426-9356-128525711791");
            //Guid permission_export = Guid.Parse("b03a5898-c687-4595-88ea-1e94e07d11ea");
            //Guid permission_view_report = Guid.Parse("8c349c9e-04f8-4034-aee3-63c70d1e1693");
            //Guid permission_view_log = Guid.Parse("9f4d120a-3f59-4e3d-b709-556049d74474");
            //context.SysPermission.AddRange(
            //   new SysPermission { PermissionId = permission_import, PermissionCode = "Import", PermissionName = "Import", IsActive = true },
            //   new SysPermission { PermissionId = permission_export, PermissionCode = "Export", PermissionName = "Export", IsActive = true },
            //   new SysPermission { PermissionId = permission_view_report, PermissionCode = "Report.View", PermissionName = "View report dashboard", IsActive = true },
            //   new SysPermission { PermissionId = permission_view_log, PermissionCode = "Audit.View", PermissionName = "View log data", IsActive = true }
            //);
            //await context.SaveChangesAsync();
            #endregion

            #region SysRole (Next Phase2)
            //Guid role_sys_admin = Guid.Parse("c86b873a-b45d-4018-8efa-0759ea2a364e");
            //Guid role_user_admin = Guid.Parse("58147596-e8a6-4dbb-8a91-5c12a91538c9");
            //Guid role_it = Guid.Parse("494c459e-9ede-4a15-88c0-b69f8163ea9d");
            //Guid role_autdit = Guid.Parse("fade6502-b852-4657-8e1f-52cf27d3ed27");
            //context.SysRole.AddRange(
            //   new SysRole { RoleId = role_sys_admin, RoleCode = "SYS_ADMIN", RoleName = "System Admin", IsSystem = true, IsActive = true },
            //   new SysRole { RoleId = role_user_admin, RoleCode = "USER_ADMIN", RoleName = "User Admin", IsSystem = false, IsActive = true },
            //   new SysRole { RoleId = role_it, RoleCode = "IT", RoleName = "IT", IsSystem = false, IsActive = true },
            //   new SysRole { RoleId = role_autdit, RoleCode = "AUDIT", RoleName = "Audit", IsSystem = false, IsActive = true }
            //);
            //await context.SaveChangesAsync();
            #endregion

            #region SysRolePermission (Next Phase2)
            //List<SysRolePermission> SysRolePermissions = new List<SysRolePermission>
            //{
            //    // sys admin
            //    new SysRolePermission { RoleId =role_sys_admin, PermissionId=permission_import, IsActive= true  },
            //    new SysRolePermission { RoleId =role_sys_admin, PermissionId=permission_export, IsActive= true  },
            //    new SysRolePermission { RoleId =role_sys_admin, PermissionId=permission_view_report, IsActive= true  },
            //    new SysRolePermission { RoleId =role_sys_admin, PermissionId=permission_view_log, IsActive= true  },

            //    // user admin
            //    new SysRolePermission { RoleId =role_user_admin, PermissionId=permission_import, IsActive= true  },
            //    new SysRolePermission { RoleId =role_user_admin, PermissionId=permission_export, IsActive= true  },
            //    new SysRolePermission { RoleId =role_user_admin, PermissionId=permission_view_report, IsActive= true  },
            //    //new SysRolePermission { RoleId =role_user_admin, PermissionId=permission_view_log, IsActive= true  },

            //    // it
            //    //new SysRolePermission { RoleId =role_it, PermissionId=permission_import, IsActive= true  },
            //    //new SysRolePermission { RoleId =role_it, PermissionId=permission_export, IsActive= true  },
            //    new SysRolePermission { RoleId =role_it, PermissionId=permission_view_report, IsActive= true  },
            //    new SysRolePermission { RoleId =role_it, PermissionId=permission_view_log, IsActive= true  },

            //    // audit
            //    //new SysRolePermission { RoleId =role_autdit, PermissionId=permission_import, IsActive= true  },
            //    //new SysRolePermission { RoleId =role_autdit, PermissionId=permission_export, IsActive= true  },
            //    //new SysRolePermission { RoleId =role_autdit, PermissionId=permission_view_report, IsActive= true  },
            //    new SysRolePermission { RoleId =role_autdit, PermissionId=permission_view_log, IsActive= true  },
            //};
            //SysRolePermissions.ForEach(m => context.SysRolePermission.Add(m));
            //await context.SaveChangesAsync();
            #endregion

            #region SysUserGroupRole (Next Phase2)
            //List<SysUserGroupRole> SysUserGroupRoles = new List<SysUserGroupRole>
            //{
            //    // Administrator
            //    new SysUserGroupRole { UserGroupId =groupAdmin, RoleId=role_sys_admin, IsActive= true  },
            //    new SysUserGroupRole { UserGroupId =groupAdmin, RoleId=role_user_admin, IsActive= true  },
            //    new SysUserGroupRole { UserGroupId =groupAdmin, RoleId=role_it, IsActive= true  },
            //    new SysUserGroupRole { UserGroupId =groupAdmin, RoleId=role_autdit, IsActive= true  },

            //    // Manager
            //    new SysUserGroupRole { UserGroupId =managerGroup, RoleId=role_user_admin, IsActive= true  },

            //    // It
            //    new SysUserGroupRole { UserGroupId =itSupportGroup, RoleId=role_it, IsActive= true  },
            //    new SysUserGroupRole { UserGroupId =itSupportGroup, RoleId=role_autdit, IsActive= true  },

            //    // Audit
            //    new SysUserGroupRole { UserGroupId =auditGroup, RoleId=role_autdit, IsActive= true  },
            //};
            //SysUserGroupRoles.ForEach(m => context.SysUserGroupRole.Add(m));
            //await context.SaveChangesAsync();
            #endregion

            #region SysUser
            Guid user00000 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456700000");
            Guid user00001 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456700001");
            Guid user00002 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456700002");
            Guid user60001 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456760001");
            Guid user61001 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456761001");
            Guid user61002 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456761002");
            Guid user61029 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456761029");
            Guid user61042 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456761042");
            Guid user61056 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456761056");
            Guid user61060 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456761060");
            Guid user61065 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456761065");
            Guid user62001 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762001");
            Guid user62029 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762029");
            Guid user62032 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762032");
            Guid user62051 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762051");
            Guid user62059 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762059");
            Guid user62073 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762073");
            Guid user62074 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762074");
            Guid user62082 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762082");
            Guid user62098 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762098");
            Guid user62101 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762101");
            Guid user62102 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762102");
            Guid user62103 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762103");
            Guid user62104 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762104");
            Guid user62105 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456762105");
            Guid user63001 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456763001");
            Guid user63012 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456763012");
            Guid user63044 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456763044");
            Guid user63080 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456763080");
            Guid user63090 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456763090");
            Guid user63096 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456763096");
            Guid user63101 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456763101");
            Guid user63102 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456763102");
            Guid user63103 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456763103");
            Guid user64001 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764001");
            Guid user64040 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764040");
            Guid user64045 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764045");
            Guid user64052 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764052");
            Guid user64055 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764055");
            Guid user64057 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764057");
            Guid user64059 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764059");
            Guid user64081 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764081");
            Guid user64101 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764101");
            Guid user64102 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764102");
            Guid user64103 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764103");
            Guid user64104 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764104");
            Guid user64105 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456764105");
            Guid user65014 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456765014");
            Guid user65036 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456765036");
            Guid user65039 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456765039");
            Guid user65041 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456765041");
            Guid user65050 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456765050");
            Guid user65051 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456765051");
            Guid user65062 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456765062");
            Guid user65091 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456765091");
            Guid user65095 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456765095");
            Guid user66019 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456766019");
            Guid user66021 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456766021");
            Guid user66027 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456766027");
            Guid user66033 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456766033");
            Guid user66036 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456766036");
            Guid user66047 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456766047");
            Guid user66050 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456766050");
            Guid user66055 = Guid.Parse("e1a1b2c3-d4e5-4f67-8a90-123456766055");

            var sysUser00000 = new SysUser { UserId = user00000, UserName = "admin", EmployeeId = "00000", IsActive = true, IsLockUser = false, IsAdmin = true, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser00001 = new SysUser { UserId = user00001, UserName = "itadmin", EmployeeId = "00002", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser60001 = new SysUser { UserId = user60001, UserName = "somchaip", EmployeeId = "60001", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser61001 = new SysUser { UserId = user61001, UserName = "kriengkraim", EmployeeId = "61001", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser61002 = new SysUser { UserId = user61002, UserName = "patimar", EmployeeId = "61002", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser61029 = new SysUser { UserId = user61029, UserName = "thaipornn", EmployeeId = "61029", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser61042 = new SysUser { UserId = user61042, UserName = "sumaleek", EmployeeId = "61042", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser61056 = new SysUser { UserId = user61056, UserName = "surind", EmployeeId = "61056", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser61060 = new SysUser { UserId = user61060, UserName = "nokkaewn", EmployeeId = "61060", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser61065 = new SysUser { UserId = user61065, UserName = "tasaneeh", EmployeeId = "61065", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62001 = new SysUser { UserId = user62001, UserName = "piyanee", EmployeeId = "62001", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62029 = new SysUser { UserId = user62029, UserName = "kasemaneep", EmployeeId = "62029", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62032 = new SysUser { UserId = user62032, UserName = "rungpetchh", EmployeeId = "62032", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62051 = new SysUser { UserId = user62051, UserName = "jiraporna", EmployeeId = "62051", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62059 = new SysUser { UserId = user62059, UserName = "veerawann", EmployeeId = "62059", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62073 = new SysUser { UserId = user62073, UserName = "suntornw", EmployeeId = "62073", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62074 = new SysUser { UserId = user62074, UserName = "nopparati", EmployeeId = "62074", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62082 = new SysUser { UserId = user62082, UserName = "wirata", EmployeeId = "62082", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62098 = new SysUser { UserId = user62098, UserName = "jaruratr", EmployeeId = "62098", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62101 = new SysUser { UserId = user62101, UserName = "thanakornc", EmployeeId = "62101", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62102 = new SysUser { UserId = user62102, UserName = "manitm", EmployeeId = "62102", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62103 = new SysUser { UserId = user62103, UserName = "sudam", EmployeeId = "62103", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62104 = new SysUser { UserId = user62104, UserName = "chanink", EmployeeId = "62104", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser62105 = new SysUser { UserId = user62105, UserName = "apichartl", EmployeeId = "62105", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser63001 = new SysUser { UserId = user63001, UserName = "chanokp", EmployeeId = "63001", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser63012 = new SysUser { UserId = user63012, UserName = "viroja", EmployeeId = "63012", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser63044 = new SysUser { UserId = user63044, UserName = "usah", EmployeeId = "63044", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser63080 = new SysUser { UserId = user63080, UserName = "somnueka", EmployeeId = "63080", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser63090 = new SysUser { UserId = user63090, UserName = "phanaratn", EmployeeId = "63090", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser63096 = new SysUser { UserId = user63096, UserName = "tasanao", EmployeeId = "63096", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser63101 = new SysUser { UserId = user63101, UserName = "thanapornb", EmployeeId = "63101", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser63102 = new SysUser { UserId = user63102, UserName = "somsakj", EmployeeId = "63102", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser63103 = new SysUser { UserId = user63103, UserName = "banyongb", EmployeeId = "63103", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64001 = new SysUser { UserId = user64001, UserName = "ariyan", EmployeeId = "64001", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64040 = new SysUser { UserId = user64040, UserName = "wannaratg", EmployeeId = "64040", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64045 = new SysUser { UserId = user64045, UserName = "saengchank", EmployeeId = "64045", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64052 = new SysUser { UserId = user64052, UserName = "ongarti", EmployeeId = "64052", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64055 = new SysUser { UserId = user64055, UserName = "thongchaio", EmployeeId = "64055", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64057 = new SysUser { UserId = user64057, UserName = "taengonc", EmployeeId = "64057", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64059 = new SysUser { UserId = user64059, UserName = "wattanah", EmployeeId = "64059", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64081 = new SysUser { UserId = user64081, UserName = "ekkapolo", EmployeeId = "64081", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64101 = new SysUser { UserId = user64101, UserName = "natthidaj", EmployeeId = "64101", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64102 = new SysUser { UserId = user64102, UserName = "weerachais", EmployeeId = "64102", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64103 = new SysUser { UserId = user64103, UserName = "sombatm", EmployeeId = "64103", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64104 = new SysUser { UserId = user64104, UserName = "korakojt", EmployeeId = "64104", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser64105 = new SysUser { UserId = user64105, UserName = "ongardk", EmployeeId = "64105", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser65014 = new SysUser { UserId = user65014, UserName = "rangnapas", EmployeeId = "65014", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser65036 = new SysUser { UserId = user65036, UserName = "nanthanaphornk", EmployeeId = "65036", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser65039 = new SysUser { UserId = user65039, UserName = "atitiyaa", EmployeeId = "65039", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser65041 = new SysUser { UserId = user65041, UserName = "chaiwatw", EmployeeId = "65041", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser65050 = new SysUser { UserId = user65050, UserName = "chanokorni", EmployeeId = "65050", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser65051 = new SysUser { UserId = user65051, UserName = "amphani", EmployeeId = "65051", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser65062 = new SysUser { UserId = user65062, UserName = "arakt", EmployeeId = "65062", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser65091 = new SysUser { UserId = user65091, UserName = "phoonsrio", EmployeeId = "65091", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser65095 = new SysUser { UserId = user65095, UserName = "sittichokg", EmployeeId = "65095", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser66019 = new SysUser { UserId = user66019, UserName = "wandeey", EmployeeId = "66019", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser66021 = new SysUser { UserId = user66021, UserName = "khamkhunm", EmployeeId = "66021", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser66027 = new SysUser { UserId = user66027, UserName = "sirinunj", EmployeeId = "66027", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser66033 = new SysUser { UserId = user66033, UserName = "pornchaip", EmployeeId = "66033", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser66036 = new SysUser { UserId = user66036, UserName = "nuchareen", EmployeeId = "66036", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser66047 = new SysUser { UserId = user66047, UserName = "ampapanp", EmployeeId = "66047", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser66050 = new SysUser { UserId = user66050, UserName = "nanticham", EmployeeId = "66050", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };
            var sysUser66055 = new SysUser { UserId = user66055, UserName = "chatchanana", EmployeeId = "66055", IsActive = true, IsLockUser = false, IsAdmin = false, IsNewUser = false, EffectiveDate = DateTime.Now, };

            sysUser00000.PasswordHash = _passwordHasher.HashPassword(sysUser00000, "password");
            sysUser00001.PasswordHash = _passwordHasher.HashPassword(sysUser00001, "password");
            sysUser60001.PasswordHash = _passwordHasher.HashPassword(sysUser60001, "password");
            sysUser61001.PasswordHash = _passwordHasher.HashPassword(sysUser61001, "password");
            sysUser61002.PasswordHash = _passwordHasher.HashPassword(sysUser61002, "password");
            sysUser61029.PasswordHash = _passwordHasher.HashPassword(sysUser61029, "password");
            sysUser61042.PasswordHash = _passwordHasher.HashPassword(sysUser61042, "password");
            sysUser61056.PasswordHash = _passwordHasher.HashPassword(sysUser61056, "password");
            sysUser61060.PasswordHash = _passwordHasher.HashPassword(sysUser61060, "password");
            sysUser61065.PasswordHash = _passwordHasher.HashPassword(sysUser61065, "password");
            sysUser62001.PasswordHash = _passwordHasher.HashPassword(sysUser62001, "password");
            sysUser62029.PasswordHash = _passwordHasher.HashPassword(sysUser62029, "password");
            sysUser62032.PasswordHash = _passwordHasher.HashPassword(sysUser62032, "password");
            sysUser62051.PasswordHash = _passwordHasher.HashPassword(sysUser62051, "password");
            sysUser62059.PasswordHash = _passwordHasher.HashPassword(sysUser62059, "password");
            sysUser62073.PasswordHash = _passwordHasher.HashPassword(sysUser62073, "password");
            sysUser62074.PasswordHash = _passwordHasher.HashPassword(sysUser62074, "password");
            sysUser62082.PasswordHash = _passwordHasher.HashPassword(sysUser62082, "password");
            sysUser62098.PasswordHash = _passwordHasher.HashPassword(sysUser62098, "password");
            sysUser62101.PasswordHash = _passwordHasher.HashPassword(sysUser62101, "password");
            sysUser62102.PasswordHash = _passwordHasher.HashPassword(sysUser62102, "password");
            sysUser62103.PasswordHash = _passwordHasher.HashPassword(sysUser62103, "password");
            sysUser62104.PasswordHash = _passwordHasher.HashPassword(sysUser62104, "password");
            sysUser62105.PasswordHash = _passwordHasher.HashPassword(sysUser62105, "password");
            sysUser63001.PasswordHash = _passwordHasher.HashPassword(sysUser63001, "password");
            sysUser63012.PasswordHash = _passwordHasher.HashPassword(sysUser63012, "password");
            sysUser63044.PasswordHash = _passwordHasher.HashPassword(sysUser63044, "password");
            sysUser63080.PasswordHash = _passwordHasher.HashPassword(sysUser63080, "password");
            sysUser63090.PasswordHash = _passwordHasher.HashPassword(sysUser63090, "password");
            sysUser63096.PasswordHash = _passwordHasher.HashPassword(sysUser63096, "password");
            sysUser63101.PasswordHash = _passwordHasher.HashPassword(sysUser63101, "password");
            sysUser63102.PasswordHash = _passwordHasher.HashPassword(sysUser63102, "password");
            sysUser63103.PasswordHash = _passwordHasher.HashPassword(sysUser63103, "password");
            sysUser64001.PasswordHash = _passwordHasher.HashPassword(sysUser64001, "password");
            sysUser64040.PasswordHash = _passwordHasher.HashPassword(sysUser64040, "password");
            sysUser64045.PasswordHash = _passwordHasher.HashPassword(sysUser64045, "password");
            sysUser64052.PasswordHash = _passwordHasher.HashPassword(sysUser64052, "password");
            sysUser64055.PasswordHash = _passwordHasher.HashPassword(sysUser64055, "password");
            sysUser64057.PasswordHash = _passwordHasher.HashPassword(sysUser64057, "password");
            sysUser64059.PasswordHash = _passwordHasher.HashPassword(sysUser64059, "password");
            sysUser64081.PasswordHash = _passwordHasher.HashPassword(sysUser64081, "password");
            sysUser64101.PasswordHash = _passwordHasher.HashPassword(sysUser64101, "password");
            sysUser64102.PasswordHash = _passwordHasher.HashPassword(sysUser64102, "password");
            sysUser64103.PasswordHash = _passwordHasher.HashPassword(sysUser64103, "password");
            sysUser64104.PasswordHash = _passwordHasher.HashPassword(sysUser64104, "password");
            sysUser64105.PasswordHash = _passwordHasher.HashPassword(sysUser64105, "password");
            sysUser65014.PasswordHash = _passwordHasher.HashPassword(sysUser65014, "password");
            sysUser65036.PasswordHash = _passwordHasher.HashPassword(sysUser65036, "password");
            sysUser65039.PasswordHash = _passwordHasher.HashPassword(sysUser65039, "password");
            sysUser65041.PasswordHash = _passwordHasher.HashPassword(sysUser65041, "password");
            sysUser65050.PasswordHash = _passwordHasher.HashPassword(sysUser65050, "password");
            sysUser65051.PasswordHash = _passwordHasher.HashPassword(sysUser65051, "password");
            sysUser65062.PasswordHash = _passwordHasher.HashPassword(sysUser65062, "password");
            sysUser65091.PasswordHash = _passwordHasher.HashPassword(sysUser65091, "password");
            sysUser65095.PasswordHash = _passwordHasher.HashPassword(sysUser65095, "password");
            sysUser66019.PasswordHash = _passwordHasher.HashPassword(sysUser66019, "password");
            sysUser66021.PasswordHash = _passwordHasher.HashPassword(sysUser66021, "password");
            sysUser66027.PasswordHash = _passwordHasher.HashPassword(sysUser66027, "password");
            sysUser66033.PasswordHash = _passwordHasher.HashPassword(sysUser66033, "password");
            sysUser66036.PasswordHash = _passwordHasher.HashPassword(sysUser66036, "password");
            sysUser66047.PasswordHash = _passwordHasher.HashPassword(sysUser66047, "password");
            sysUser66050.PasswordHash = _passwordHasher.HashPassword(sysUser66050, "password");
            sysUser66055.PasswordHash = _passwordHasher.HashPassword(sysUser66055, "password");

            context.SysUsers.AddRange(sysUser00000);
            context.SysUsers.AddRange(sysUser00001);
            context.SysUsers.AddRange(sysUser60001);
            context.SysUsers.AddRange(sysUser61001);
            context.SysUsers.AddRange(sysUser61002);
            context.SysUsers.AddRange(sysUser61029);
            context.SysUsers.AddRange(sysUser61042);
            context.SysUsers.AddRange(sysUser61056);
            context.SysUsers.AddRange(sysUser61060);
            context.SysUsers.AddRange(sysUser61065);
            context.SysUsers.AddRange(sysUser62001);
            context.SysUsers.AddRange(sysUser62029);
            context.SysUsers.AddRange(sysUser62032);
            context.SysUsers.AddRange(sysUser62051);
            context.SysUsers.AddRange(sysUser62059);
            context.SysUsers.AddRange(sysUser62073);
            context.SysUsers.AddRange(sysUser62074);
            context.SysUsers.AddRange(sysUser62082);
            context.SysUsers.AddRange(sysUser62098);
            context.SysUsers.AddRange(sysUser62101);
            context.SysUsers.AddRange(sysUser62102);
            context.SysUsers.AddRange(sysUser62103);
            context.SysUsers.AddRange(sysUser62104);
            context.SysUsers.AddRange(sysUser62105);
            context.SysUsers.AddRange(sysUser63001);
            context.SysUsers.AddRange(sysUser63012);
            context.SysUsers.AddRange(sysUser63044);
            context.SysUsers.AddRange(sysUser63080);
            context.SysUsers.AddRange(sysUser63090);
            context.SysUsers.AddRange(sysUser63096);
            context.SysUsers.AddRange(sysUser63101);
            context.SysUsers.AddRange(sysUser63102);
            context.SysUsers.AddRange(sysUser63103);
            context.SysUsers.AddRange(sysUser64001);
            context.SysUsers.AddRange(sysUser64040);
            context.SysUsers.AddRange(sysUser64045);
            context.SysUsers.AddRange(sysUser64052);
            context.SysUsers.AddRange(sysUser64055);
            context.SysUsers.AddRange(sysUser64057);
            context.SysUsers.AddRange(sysUser64059);
            context.SysUsers.AddRange(sysUser64081);
            context.SysUsers.AddRange(sysUser64101);
            context.SysUsers.AddRange(sysUser64102);
            context.SysUsers.AddRange(sysUser64103);
            context.SysUsers.AddRange(sysUser64104);
            context.SysUsers.AddRange(sysUser64105);
            context.SysUsers.AddRange(sysUser65014);
            context.SysUsers.AddRange(sysUser65036);
            context.SysUsers.AddRange(sysUser65039);
            context.SysUsers.AddRange(sysUser65041);
            context.SysUsers.AddRange(sysUser65050);
            context.SysUsers.AddRange(sysUser65051);
            context.SysUsers.AddRange(sysUser65062);
            context.SysUsers.AddRange(sysUser65091);
            context.SysUsers.AddRange(sysUser65095);
            context.SysUsers.AddRange(sysUser66019);
            context.SysUsers.AddRange(sysUser66021);
            context.SysUsers.AddRange(sysUser66027);
            context.SysUsers.AddRange(sysUser66033);
            context.SysUsers.AddRange(sysUser66036);
            context.SysUsers.AddRange(sysUser66047);
            context.SysUsers.AddRange(sysUser66050);
            context.SysUsers.AddRange(sysUser66055);
            await context.SaveChangesAsync();
            #endregion

            #region SysUserGroupAccessRight
            List<SysUserGroupAccessRight> UserGroupAccesses = new List<SysUserGroupAccessRight>
            {
                new SysUserGroupAccessRight { UserId = sysUser00000.UserId, UserGroupId=groupAdmin, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser00001.UserId, UserGroupId=groupItSupport, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser60001.UserId, UserGroupId=groupColAdmin, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser61001.UserId, UserGroupId=groupManager, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser61002.UserId, UserGroupId=groupManager, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser61029.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser61042.UserId, UserGroupId=groupItSupport, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser61056.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser61060.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser61065.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62001.UserId, UserGroupId=groupSupervisor, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62029.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62032.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62051.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62059.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62073.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62074.UserId, UserGroupId=groupSupervisor, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62082.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62098.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62101.UserId, UserGroupId=groupSupervisor, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62102.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62103.UserId, UserGroupId=groupSupervisor, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62104.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser62105.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser63001.UserId, UserGroupId=groupSupervisor, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser63012.UserId, UserGroupId=groupSupervisor, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser63044.UserId, UserGroupId=groupItSupport, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser63080.UserId, UserGroupId=groupViewer, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser63090.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser63096.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser63101.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser63102.UserId, UserGroupId=groupSupervisor, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser63103.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64001.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64040.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64045.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64052.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64055.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64057.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64059.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64081.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64101.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64102.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64103.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64104.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser64105.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser65014.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser65036.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser65039.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser65041.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser65050.UserId, UserGroupId=groupSupervisor, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser65051.UserId, UserGroupId=groupSupervisor, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser65062.UserId, UserGroupId=groupManager, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser65091.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser65095.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser66019.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser66021.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser66027.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser66033.UserId, UserGroupId=groupManager, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser66036.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser66047.UserId, UserGroupId=groupManager, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser66050.UserId, UserGroupId=groupOperator, IsActive= true  },
                new SysUserGroupAccessRight { UserId = sysUser66055.UserId, UserGroupId=groupOperator, IsActive= true  },
            };
            UserGroupAccesses.ForEach(m => context.UserGroupAccesses.Add(m));
            await context.SaveChangesAsync();
            #endregion

            #region SysItem
            context.SysItems.AddRange(
                // Main System
                new SysItem { ItemId = "0000", ParentId = null, ItemName = "{\"en\":\"Collection Worklist\",\"th\":\"Collection Worklist\"}", RouteName = "", ItemLevel = 0, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 0 },

                // Dashboard
                new SysItem { ItemId = "0100", ParentId = "0000", ItemName = "{\"en\":\"Dashboard\",\"th\":\"แดชบอร์ด\"}", RouteName = "dashboard", ItemLevel = 1, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 1 },
                new SysItem { ItemId = "0101", ParentId = "0100", ItemName = "{\"en\":\"Worklist Assignment & Utilization\",\"th\":\"Worklist Assignment & Utilization\"}", RouteName = "dashboard-worklist-assignment", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 1 },
                new SysItem { ItemId = "0102", ParentId = "0100", ItemName = "{\"en\":\"Collection Effectiveness Dashboard\",\"th\":\"Collection Effectiveness Dashboard\"}", RouteName = "dashboard-collection-effective", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 2 },

                // Worklist Management
                new SysItem { ItemId = "0200", ParentId = "0000", ItemName = "{\"en\":\"Worklist Management\",\"th\":\"รายการงาน\"}", RouteName = "worklist", ItemLevel = 1, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 2 },
                new SysItem { ItemId = "0201", ParentId = "0200", ItemName = "{\"en\":\"My Worklist\",\"th\":\"รายการงานของฉัน\"}", RouteName = "my-worklist", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 1 },
                new SysItem { ItemId = "0202", ParentId = "0200", ItemName = "{\"en\":\"Approve Reassignment\",\"th\":\"อนุมัติการโอนย้ายงาน\"}", RouteName = "approve-reassign", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 2 },
                new SysItem { ItemId = "0203", ParentId = "0200", ItemName = "{\"en\":\"Unassigned Worklist\",\"th\":\"รายการงานที่ยังไม่ได้มอบหมาย\"}", RouteName = "unassigned-worklist", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 3 },
                new SysItem { ItemId = "0204", ParentId = "0200", ItemName = "{\"en\":\"Do Not Call/Field Worklist\",\"th\":\"รายการห้ามโทร/งานภาคสนาม\"}", RouteName = "do-not-call", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 4 },
                new SysItem { ItemId = "0205", ParentId = "0200", ItemName = "{\"en\":\"Worklist History\",\"th\":\"Worklist History\"}", RouteName = "worklist-history", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 5 },

                // Assignment Management
                new SysItem { ItemId = "0300", ParentId = "0000", ItemName = "{\"en\":\"Assignment Management\",\"th\":\"Assignment Management\"}", RouteName = "assignment", ItemLevel = 1, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 3 },
                new SysItem { ItemId = "0301", ParentId = "0300", ItemName = "{\"en\":\"Assignment Rule\",\"th\":\"Assignment Rule\"}", RouteName = "assignment-rule", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 1 },
                new SysItem { ItemId = "0302", ParentId = "0300", ItemName = "{\"en\":\"SLA Setup\",\"th\":\"SLA Setup\"}", RouteName = "sla-setup", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 2 },
                new SysItem { ItemId = "0303", ParentId = "0300", ItemName = "{\"en\":\"Assignment History\",\"th\":\"Assignment History\"}", RouteName = "assignment-history", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 3 },

                // Collection Management
                new SysItem { ItemId = "0400", ParentId = "0000", ItemName = "{\"en\":\"Collection Management\",\"th\":\"Collection Management\"}", RouteName = "collector", ItemLevel = 1, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 4 },
                new SysItem { ItemId = "0401", ParentId = "0400", ItemName = "{\"en\":\"Collector Profile\",\"th\":\"ข้อมูลเจ้าหน้าที่ติดตาม\"}", RouteName = "collector-profile", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 1 },
                new SysItem { ItemId = "0402", ParentId = "0400", ItemName = "{\"en\":\"Collector Team\",\"th\":\"ทีมเจ้าหน้าที่ติดตาม\"}", RouteName = "collector-team", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 2 },
                new SysItem { ItemId = "0403", ParentId = "0400", ItemName = "{\"en\":\"Collector Team Assignment\",\"th\":\"จัดการทีมเจ้าหน้าที่ติดตาม\"}", RouteName = "team-assignment", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 3 },
                new SysItem { ItemId = "0404", ParentId = "0400", ItemName = "{\"en\":\"Collector Role\",\"th\":\"Collector Role\"}", RouteName = "collector-role", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 4 },
                new SysItem { ItemId = "0405", ParentId = "0400", ItemName = "{\"en\":\"Collector Permission\",\"th\":\"Collector Permission\"}", RouteName = "collector-permission", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 5 },
                new SysItem { ItemId = "0406", ParentId = "0400", ItemName = "{\"en\":\"Business Parameter\",\"th\":\"Business Parameter\"}", RouteName = "business-parameter", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 6 },

                // Report & Analytics
                new SysItem { ItemId = "0500", ParentId = "0000", ItemName = "{\"en\":\"Report & Analytics\",\"th\":\"Report & Analytics\"}", RouteName = "reports", ItemLevel = 1, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 5 },
                new SysItem { ItemId = "0501", ParentId = "0500", ItemName = "{\"en\":\"Reassignment Report \",\"th\":\"Reassignment Report \"}", RouteName = "reassignment-report", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 1 },
                new SysItem { ItemId = "0502", ParentId = "0500", ItemName = "{\"en\":\"Collector Performance Report \",\"th\":\"Collector Performance Report \"}", RouteName = "performance-report", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 2 },
                new SysItem { ItemId = "0503", ParentId = "0500", ItemName = "{\"en\":\"Aging & Overdue Report \",\"th\":\"Aging & Overdue Report \"}", RouteName = "aging-report", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 3 },

                //  Master Data
                new SysItem { ItemId = "0600", ParentId = "0000", ItemName = "{\"en\":\"Master Data\",\"th\":\"การตั้งค่าข้อมูลหลัก\"}", RouteName = "master", ItemLevel = 1, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 6 },
                new SysItem { ItemId = "0601", ParentId = "0600", ItemName = "{\"en\":\"Prefix \",\"th\":\"Prefix \"}", RouteName = "prefix", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 1 },
                new SysItem { ItemId = "0602", ParentId = "0600", ItemName = "{\"en\":\"Area Code\",\"th\":\"Area Code\"}", RouteName = "area-code", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 2 },
                new SysItem { ItemId = "0603", ParentId = "0600", ItemName = "{\"en\":\"Follow-up Action\",\"th\":\"Follow-up Action\"}", RouteName = "followup-action", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 3 },
                new SysItem { ItemId = "0604", ParentId = "0600", ItemName = "{\"en\":\"Follow-up Result\",\"th\":\"Follow-up Result\"}", RouteName = "followup-result", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 4 },
                new SysItem { ItemId = "0605", ParentId = "0600", ItemName = "{\"en\":\"Province\",\"th\":\"Province\"}", RouteName = "province", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 5 },
                new SysItem { ItemId = "0606", ParentId = "0600", ItemName = "{\"en\":\"District \",\"th\":\"District \"}", RouteName = "district", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 6 },
                new SysItem { ItemId = "0607", ParentId = "0600", ItemName = "{\"en\":\"Subdistrict \",\"th\":\"Subdistrict \"}", RouteName = "subdistrict", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 7 },

                //  Organization
                new SysItem { ItemId = "0700", ParentId = "0000", ItemName = "{\"en\":\"Organization\",\"th\":\"Organization\"}", RouteName = "org", ItemLevel = 1, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 7 },
                new SysItem { ItemId = "0701", ParentId = "0700", ItemName = "{\"en\":\"Employee Profile  \",\"th\":\"ข้อมูลพนักงาน \"}", RouteName = "employee", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 1 },
                new SysItem { ItemId = "0702", ParentId = "0700", ItemName = "{\"en\":\"Department \",\"th\":\"Department\"}", RouteName = "department ", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 2 },
                new SysItem { ItemId = "0703", ParentId = "0700", ItemName = "{\"en\":\"Position\",\"th\":\"Position\"}", RouteName = "position", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 3 },
                new SysItem { ItemId = "0704", ParentId = "0700", ItemName = "{\"en\":\"Team \",\"th\":\"Team \"}", RouteName = "team ", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 4 },
                new SysItem { ItemId = "0705", ParentId = "0700", ItemName = "{\"en\":\"Company Profile\",\"th\":\"Company Profile\"}", RouteName = "company", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 5 },

                // User Management
                new SysItem { ItemId = "9100", ParentId = "0000", ItemName = "{\"en\":\"User Management\",\"th\":\"User Management\"}", RouteName = "security", ItemLevel = 1, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 8 },
                new SysItem { ItemId = "9101", ParentId = "9100", ItemName = "{\"en\":\"User Permission\",\"th\":\"User Permission\"}", RouteName = "userpermission", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 1 },
                new SysItem { ItemId = "9102", ParentId = "9100", ItemName = "{\"en\":\"User Role\",\"th\":\"User Role\"}", RouteName = "userrole", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 2 },
                new SysItem { ItemId = "9103", ParentId = "9100", ItemName = "{\"en\":\"User Group\",\"th\":\"กลุ่มผู้ใช้งาน\"}", RouteName = "usergroup", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 3 },
                new SysItem { ItemId = "9104", ParentId = "9100", ItemName = "{\"en\":\"Users\",\"th\":\"ผู้ใช้งาน\"}", RouteName = "user", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 4 },

                // System Administration
                new SysItem { ItemId = "9200", ParentId = "0000", ItemName = "{\"en\":\"System Administration\",\"th\":\"System Administration\"}", RouteName = "system", ItemLevel = 1, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 9 },
                new SysItem { ItemId = "9201", ParentId = "9200", ItemName = "{\"en\":\"Master Menu\",\"th\":\"Master Menu\"}", RouteName = "master-menu", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 1 },
                new SysItem { ItemId = "9202", ParentId = "9200", ItemName = "{\"en\":\"System Parameter\",\"th\":\"System Parameter\"}", RouteName = "system-parameter", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 2 },
                new SysItem { ItemId = "9203", ParentId = "9200", ItemName = "{\"en\":\"Language / Translator Setup\",\"th\":\"Language / Translator Setup\"}", RouteName = "language", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = true, ItemOrder = 3 },
                new SysItem { ItemId = "9204", ParentId = "9200", ItemName = "{\"en\":\"Activity Log\",\"th\":\"Activity Log\"}", RouteName = "activity-log", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 4 },
                new SysItem { ItemId = "9205", ParentId = "9200", ItemName = "{\"en\":\"Password/Security Policy\",\"th\":\"Password/Security Policy\"}", RouteName = "policy", ItemLevel = 2, Icon = "", ToolTip = "", IsActive = false, ItemOrder = 4 }

            );
            await context.SaveChangesAsync();
            #endregion

            #region SysItemAccessRight
            context.SysItemAccessRights.AddRange(
            #region Administrator
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0000", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            // dashboard
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0100", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0101", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0102", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },

            // worklist management
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0200", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0201", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0202", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0203", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // assignment worklist
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0300", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0301", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0302", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0303", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // collection management
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0400", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0401", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0402", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0403", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0404", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0405", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0406", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // report & analytics
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0500", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0501", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0502", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0503", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // master data
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0600", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0601", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0602", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0603", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0604", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0605", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0606", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0607", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // organization
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0700", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0701", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0702", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0703", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0704", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "0705", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // user management
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "9100", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "9101", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "9102", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "9103", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "9104", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // system administrator
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "9200", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "9201", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "9202", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "9203", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "9204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupAdmin, ItemId = "9205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            #endregion

            #region IT Support
            // dashboard
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0100", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0101", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0102", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },

            // worklist management
            new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0200", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0201", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0202", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0203", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // assignment worklist
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0300", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0301", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0302", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0303", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // collection management
            new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0400", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0401", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0402", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0403", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0404", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0405", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0406", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // report & analytics
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0500", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0501", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0502", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0503", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // master data
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0600", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0601", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0602", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0603", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0604", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0605", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0606", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0607", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // organization
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0700", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0701", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0702", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0703", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0704", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "0705", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // user management
            new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "9100", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "9101", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "9102", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "9103", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "9104", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // system administrator
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "9200", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "9201", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "9202", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "9203", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "9204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupItSupport, ItemId = "9205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true }

            #endregion

            #region Collector Admin
            // dashboard
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0100", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0101", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0102", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },

            // worklist management
            new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0200", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0201", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0202", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0203", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // assignment worklist
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0300", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0301", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0302", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0303", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // collection management
            new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0400", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0401", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0402", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0403", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0404", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0405", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0406", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // report & analytics
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0500", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0501", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0502", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0503", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // master data
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0600", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0601", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0602", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0603", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0604", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0605", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0606", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0607", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // organization
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0700", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0701", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0702", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0703", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0704", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "0705", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // user management
            new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "9100", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "9101", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "9102", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "9103", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "9104", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // system administrator
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "9200", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "9201", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "9202", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "9203", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "9204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupColAdmin, ItemId = "9205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true }

            #endregion

            #region Manager
            // dashboard
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0100", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0101", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0102", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },

            // worklist management
            new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0200", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0201", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0202", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0203", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // assignment worklist
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0300", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0301", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0302", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0303", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // collection management
            new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0400", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0401", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0402", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0403", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0404", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0405", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0406", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // report & analytics
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0500", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0501", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0502", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0503", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // master data
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0600", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0601", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0602", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0603", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0604", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0605", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0606", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0607", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // organization
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0700", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0701", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0702", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0703", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0704", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "0705", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // user management
            new SysItemAccessRight { UserGroupId = groupManager, ItemId = "9100", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "9101", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "9102", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupManager, ItemId = "9103", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupManager, ItemId = "9104", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // system administrator
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "9200", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "9201", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "9202", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "9203", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "9204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupManager, ItemId = "9205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true }

            #endregion

            #region Supervisor
            // dashboard
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0100", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0101", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0102", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },

            // worklist management
            new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0200", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0201", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0202", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0203", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // assignment worklist
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0300", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0301", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0302", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0303", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // collection management
            new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0400", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0401", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0402", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0403", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0404", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0405", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0406", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // report & analytics
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0500", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0501", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0502", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0503", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // master data
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0600", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0601", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0602", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0603", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0604", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0605", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0606", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0607", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // organization
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0700", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0701", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0702", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0703", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0704", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "0705", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // user management
            new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "9100", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "9101", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "9102", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "9103", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "9104", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // system administrator
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "9200", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "9201", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "9202", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "9203", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "9204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupSupervisor, ItemId = "9205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true }

            #endregion

            #region Operator
            // dashboard
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0100", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0101", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0102", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },

            // worklist management
            new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0200", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0201", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0202", AllowAccess = false, AllowView = false, AllowNew = false, AllowEdit = false, AllowDelete = false },
            new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0203", AllowAccess = false, AllowView = false, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // assignment worklist
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0300", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0301", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0302", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0303", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // collection management
            new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0400", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0401", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0402", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0403", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0404", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0405", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0406", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // report & analytics
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0500", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0501", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0502", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0503", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // master data
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0600", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0601", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0602", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0603", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0604", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0605", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0606", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0607", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // organization
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0700", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0701", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0702", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0703", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0704", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "0705", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // user management
            new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "9100", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "9101", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "9102", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "9103", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "9104", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // system administrator
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "9200", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "9201", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "9202", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "9203", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "9204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupOperator, ItemId = "9205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true }

            #endregion

            #region Viewer
            // dashboard
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0100", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0101", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0102", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },

            // worklist management
            new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0200", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0201", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0202", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0203", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // assignment worklist
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0300", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0301", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0302", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0303", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // collection management
            new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0400", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0401", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0402", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0403", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0404", AllowAccess = true, AllowView = true, AllowNew = false, AllowEdit = false, AllowDelete = false },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0405", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0406", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // report & analytics
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0500", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0501", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0502", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0503", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // master data
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0600", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0601", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0602", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0603", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0604", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0605", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0606", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0607", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // organization
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0700", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0701", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0702", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0703", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0704", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "0705", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },

            // user management
            new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "9100", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "9101", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "9102", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "9103", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "9104", AllowAccess = false, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true }

            // system administrator
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "9200", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "9201", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "9202", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "9203", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "9204", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true },
            //new SysItemAccessRight { UserGroupId = groupViewer, ItemId = "9205", AllowAccess = true, AllowView = true, AllowNew = true, AllowEdit = true, AllowDelete = true }

            #endregion
            );
            await context.SaveChangesAsync();
            #endregion

            #region SysEnum
            if (!await context.SysEnums.AnyAsync())
            {
                await using var tx = await context.Database.BeginTransactionAsync();
                await context.Database.OpenConnectionAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [SysEnum] ON");

                context.SysEnums.AddRange(
                    new SysEnum { Id = 1, EnumName = "ContractStatus", EnumCode = "CUR", EnumDescription = "Current Due", EnumDescriptionEn = "Current Due", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 2, EnumName = "ContractStatus", EnumCode = "OD1", EnumDescription = "ค้างชำระ 1-30 วัน", EnumDescriptionEn = "Overdue 1-30 Days", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 3, EnumName = "ContractStatus", EnumCode = "OD2", EnumDescription = "ค้างชำระ 31-60 วัน", EnumDescriptionEn = "Overdue 31-60 Days", EnumOrder = 3, Note = null },
                    new SysEnum { Id = 4, EnumName = "ContractStatus", EnumCode = "OD3", EnumDescription = "ค้างชำระ 61-90 วัน", EnumDescriptionEn = "Overdue 61-90 Days", EnumOrder = 4, Note = null },
                    new SysEnum { Id = 5, EnumName = "ContractStatus", EnumCode = "NPL", EnumDescription = "ค้างชำระ 91 วันขึ้นไป", EnumDescriptionEn = "Overdue Over  91 Days", EnumOrder = 5, Note = null },
                    new SysEnum { Id = 6, EnumName = "ContractStatus", EnumCode = "RES", EnumDescription = "ปรับโครงสร้างหนี้", EnumDescriptionEn = "Restructure", EnumOrder = 6, Note = null },
                    new SysEnum { Id = 7, EnumName = "ContractStatus", EnumCode = "LEG", EnumDescription = "อยู่ระหว่างดำเนินคดี", EnumDescriptionEn = "Legal Process", EnumOrder = 7, Note = null },
                    new SysEnum { Id = 8, EnumName = "ContractStatus", EnumCode = "REP", EnumDescription = "ยึดทรัพย์คืน", EnumDescriptionEn = "Repossessed", EnumOrder = 8, Note = null },
                    new SysEnum { Id = 9, EnumName = "ContractStatus", EnumCode = "SET", EnumDescription = "จ่ายครบแล้ว", EnumDescriptionEn = "Settled", EnumOrder = 9, Note = null },
                    new SysEnum { Id = 10, EnumName = "ContractStatus", EnumCode = "WOF", EnumDescription = "ตัดหนี้สูญ", EnumDescriptionEn = "Write-Off", EnumOrder = 10, Note = null },
                    new SysEnum { Id = 11, EnumName = "ContractStatus", EnumCode = "CLS", EnumDescription = "ปิดสัญญา", EnumDescriptionEn = "Closed", EnumOrder = 11, Note = null },
                    new SysEnum { Id = 12, EnumName = "FollowupStatus", EnumCode = "NEW", EnumDescription = "เคสใหม่", EnumDescriptionEn = "New Case", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 13, EnumName = "FollowupStatus", EnumCode = "COV", EnumDescription = "เคสเดิมติดตามต่อ", EnumDescriptionEn = "Carry Over", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 14, EnumName = "FollowupStatus", EnumCode = "PTP", EnumDescription = "นัดชำระ", EnumDescriptionEn = "Promise To Pay", EnumOrder = 3, Note = null },
                    new SysEnum { Id = 15, EnumName = "FollowupStatus", EnumCode = "BRK", EnumDescription = "ไม่จ่ายตามที่นัดไว้", EnumDescriptionEn = "Broken Promise", EnumOrder = 4, Note = null },
                    new SysEnum { Id = 16, EnumName = "FollowupStatus", EnumCode = "REF", EnumDescription = "ปฏิเสธการชำระ", EnumDescriptionEn = "Refuse To Pay", EnumOrder = 5, Note = null },
                    new SysEnum { Id = 17, EnumName = "FollowupStatus", EnumCode = "RES", EnumDescription = "ปรับโครงสร้างหนี้", EnumDescriptionEn = "Restructure", EnumOrder = 6, Note = null },
                    new SysEnum { Id = 18, EnumName = "FollowupStatus", EnumCode = "LEG", EnumDescription = "อยู่ระหว่างดำเนินคดี", EnumDescriptionEn = "Legal Process", EnumOrder = 7, Note = null },
                    new SysEnum { Id = 19, EnumName = "FollowupStatus", EnumCode = "REP", EnumDescription = "ยึดทรัพย์คืน", EnumDescriptionEn = "Repossessed", EnumOrder = 8, Note = null },
                    new SysEnum { Id = 20, EnumName = "FollowupStatus", EnumCode = "CLS", EnumDescription = "ปิดเคสการติดตาม", EnumDescriptionEn = "Closed Case", EnumOrder = 9, Note = null },
                    new SysEnum { Id = 21, EnumName = "AssetGroup", EnumCode = "Vehicle", EnumDescription = "ยานพาหนะ", EnumDescriptionEn = "Vehicle", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 22, EnumName = "AssetGroup", EnumCode = "RegisterationBook", EnumDescription = "เล่มทะเบียน", EnumDescriptionEn = "Registration Book", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 23, EnumName = "AssetGroup", EnumCode = "House", EnumDescription = "บ้าน", EnumDescriptionEn = "House", EnumOrder = 3, Note = null },
                    new SysEnum { Id = 24, EnumName = "WorkStatus", EnumCode = "Active", EnumDescription = "ทำงานอยู่", EnumDescriptionEn = "Active", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 25, EnumName = "WorkStatus", EnumCode = "OnLeave", EnumDescription = "ลา", EnumDescriptionEn = "OnLeave", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 26, EnumName = "WorkStatus", EnumCode = "Terminated", EnumDescription = "สิ้นสุดการจ้าง", EnumDescriptionEn = "Terminated", EnumOrder = 3, Note = null },
                    new SysEnum { Id = 27, EnumName = "WorkStatus", EnumCode = "Suspended", EnumDescription = "พักงาน", EnumDescriptionEn = "Suspended", EnumOrder = 4, Note = null },
                    new SysEnum { Id = 28, EnumName = "WorkStatus", EnumCode = "Retired", EnumDescription = "เกษียณอายุ", EnumDescriptionEn = "Retired", EnumOrder = 5, Note = null },
                    new SysEnum { Id = 29, EnumName = "JobType", EnumCode = "New", EnumDescription = "งานใหม่", EnumDescriptionEn = "New", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 30, EnumName = "JobType", EnumCode = "CarryOver", EnumDescription = "งานติดตามต่อ", EnumDescriptionEn = "CarryOver", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 31, EnumName = "PersonType", EnumCode = "B", EnumDescription = "ผู้กู้", EnumDescriptionEn = "Borrower", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 32, EnumName = "PersonType", EnumCode = "G", EnumDescription = "ผู้ค้ำ", EnumDescriptionEn = "Guarantor", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 33, EnumName = "OverdueType", EnumCode = "Installment", EnumDescription = "ค่างวด", EnumDescriptionEn = "Installment", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 34, EnumName = "OverdueType", EnumCode = "Penalty", EnumDescription = "ค่าปรับ", EnumDescriptionEn = "Pernalty", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 35, EnumName = "OverdueType", EnumCode = "OtherFees", EnumDescription = "ค่าอื่นๆ", EnumDescriptionEn = "Other Fees", EnumOrder = 3, Note = null },
                    new SysEnum { Id = 36, EnumName = "AddressType", EnumCode = "Current", EnumDescription = "ที่อยู่ปัจจุบัน", EnumDescriptionEn = "Current Address", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 37, EnumName = "AddressType", EnumCode = "IDCard", EnumDescription = "ที่อยู่ตามบัตรประชาชน", EnumDescriptionEn = "IDCard Address", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 38, EnumName = "AddressType", EnumCode = "Work", EnumDescription = "ที่อยูู่ที่ทำงาน", EnumDescriptionEn = "Work Address", EnumOrder = 3, Note = null },
                    new SysEnum { Id = 39, EnumName = "PhoneType", EnumCode = "Main", EnumDescription = "เบอร์โทรหลัก", EnumDescriptionEn = "Main Phone Number", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 40, EnumName = "PhoneType", EnumCode = "Other", EnumDescription = "เบอร์โทรสำรองอื่น", EnumDescriptionEn = "Other Phone Number", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 41, EnumName = "PhoneType", EnumCode = "Work", EnumDescription = "เบอร์โทรที่ทำงาน", EnumDescriptionEn = "Work Phone Number", EnumOrder = 3, Note = null },
                    new SysEnum { Id = 42, EnumName = "AllocateMode", EnumCode = "Actual", EnumDescription = "การมอบหมายงายจริง", EnumDescriptionEn = "Actual", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 43, EnumName = "AllocateMode", EnumCode = "Simulation", EnumDescription = "การจำลองการมอบหมายงาน", EnumDescriptionEn = "Simulation", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 44, EnumName = "AssignType", EnumCode = "AssignEOD", EnumDescription = "มอบหมายงานสิ้นวัน", EnumDescriptionEn = "Assign End of Day", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 45, EnumName = "AssignType", EnumCode = "AssignAdhoc", EnumDescription = "มอบหมายงานเฉพาะกิจ", EnumDescriptionEn = "Assign Adhoc", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 46, EnumName = "AssignType", EnumCode = "ReassignAuto", EnumDescription = "โอนงานอัตโนมัติ", EnumDescriptionEn = "Reassgin Auto", EnumOrder = 3, Note = null },
                    new SysEnum { Id = 47, EnumName = "AssignType", EnumCode = "ReassignManual", EnumDescription = "โอนงานด้วยตนเอง ", EnumDescriptionEn = "Reassign Manual", EnumOrder = 4, Note = null },
                    new SysEnum { Id = 48, EnumName = "ReassignStatus", EnumCode = "WaitForApprove", EnumDescription = "รออนุมัติ", EnumDescriptionEn = "Wait for Approve", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 49, EnumName = "ReassignStatus", EnumCode = "Approved", EnumDescription = "อนุมัติ", EnumDescriptionEn = "Approved", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 50, EnumName = "ReassignStatus", EnumCode = "Rejected", EnumDescription = "ไม่อนุมัติ", EnumDescriptionEn = "Reject", EnumOrder = 3, Note = null },
                    new SysEnum { Id = 51, EnumName = "CollectionTeamType", EnumCode = "Phone", EnumDescription = "ทีมโทร", EnumDescriptionEn = "Phone", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 52, EnumName = "CollectionTeamType", EnumCode = "Field", EnumDescription = "ทีมภาคสนาม", EnumDescriptionEn = "Field", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 53, EnumName = "CollectionTeamType", EnumCode = "Legal", EnumDescription = "ทีมกฎหมาย", EnumDescriptionEn = "Legal", EnumOrder = 3, Note = null },

                    new SysEnum { Id = 54, EnumName = "AssignFlag", EnumCode = "Y", EnumDescription = "Assign", EnumDescriptionEn = "Assign", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 55, EnumName = "AssignFlag", EnumCode = "N", EnumDescription = "Not Assign", EnumDescriptionEn = "Not Assign", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 56, EnumName = "AssignFlag", EnumCode = "X", EnumDescription = "Exclude", EnumDescriptionEn = "Exclude", EnumOrder = 3, Note = null },
                    new SysEnum { Id = 57, EnumName = "AssignMethod", EnumCode = "RoundRobin", EnumDescription = "Round Robin", EnumDescriptionEn = "Round Robin", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 58, EnumName = "AssignTool", EnumCode = "Phone", EnumDescription = "Phone", EnumDescriptionEn = "Phone", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 59, EnumName = "AssignTool", EnumCode = "Field", EnumDescription = "Field", EnumDescriptionEn = "Field", EnumOrder = 2, Note = null },
                    new SysEnum { Id = 60, EnumName = "AssignOverCapacity", EnumCode = "AssignTeam", EnumDescription = "Assign Team", EnumDescriptionEn = "Assign Team", EnumOrder = 1, Note = null },
                    new SysEnum { Id = 61, EnumName = "AssignOverCapacity", EnumCode = "AssignPool", EnumDescription = "Assign Pool", EnumDescriptionEn = "Assign Pool", EnumOrder = 2, Note = null }

                );

                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [SysEnum] OFF");
                await tx.CommitAsync();
                await context.Database.CloseConnectionAsync();
            }
            #endregion

            #region Language and Message
            context.Language.AddRange(
                new Language { Key = "Msg_SessionExpiredandRevoked", Value = "{\"en\":\"Session expired and revoked\",\"th\":\"เซสชันหมดอายุและถูกยกเลิกแล้ว\"}", DefaultValue = "{\"en\":\"Session expired and revoked\",\"th\":\"เซสชันหมดอายุและถูกยกเลิกแล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_InvalidInput", Value = "{\"en\":\"Invalid input\",\"th\":\"ข้อมูลที่ป้อนไม่ถูกต้อง\"}", DefaultValue = "{\"en\":\"Invalid input\",\"th\":\"ข้อมูลที่ป้อนไม่ถูกต้อง\"}", Ordering = 1 },
                new Language { Key = "Msg_RefreshTokenRevokedSuccessfully", Value = "{\"en\":\"Refresh token revoked successfully\",\"th\":\"เพิกถอนรีเฟรชโทเค็นเรียบร้อยแล้ว\"}", DefaultValue = "{\"en\":\"Refresh token revoked successfully\",\"th\":\"เพิกถอนรีเฟรชโทเค็นเรียบร้อยแล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_InvalidRefreshToken", Value = "{\"en\":\"Invalid refresh token\",\"th\":\"โทเคนรีเฟรชไม่ถูกต้อง\"}", DefaultValue = "{\"en\":\"Invalid refresh token\",\"th\":\"โทเคนรีเฟรชไม่ถูกต้อง\"}", Ordering = 1 },
                new Language { Key = "Msg_RefreshTokenReuseDetected", Value = "{\"en\":\"Refresh token reuse detected. Session revoked.\",\"th\":\"ตรวจพบการใช้โทเค็นรีเฟรชซ้ำ เซสชันถูกเพิกถอน\"}", DefaultValue = "{\"en\":\"Refresh token reuse detected. Session revoked.\",\"th\":\"ตรวจพบการใช้โทเค็นรีเฟรชซ้ำ เซสชันถูกเพิกถอน\"}", Ordering = 1 },
                new Language { Key = "Msg_TokenRefreshedSuccessfully", Value = "{\"en\":\"Token refreshed successfully\",\"th\":\"โทเค็นได้รับการรีเฟรชสำเร็จแล้ว\"}", DefaultValue = "{\"en\":\"Token refreshed successfully\",\"th\":\"โทเค็นได้รับการรีเฟรชสำเร็จแล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_UserAccountSuspended", Value = "{\"en\":\"User account has been suspended.\",\"th\":\"บัญชีผู้ใช้ถูกระงับการใช้งานแล้ว\"}", DefaultValue = "{\"en\":\"User account has been suspended.\",\"th\":\"บัญชีผู้ใช้ถูกระงับการใช้งานแล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_UserAccountLocked", Value = "{\"en\":\"User account is locked.\",\"th\":\"บัญชีผู้ใช้ถูกล็อก\"}", DefaultValue = "{\"en\":\"User account is locked.\",\"th\":\"บัญชีผู้ใช้ถูกล็อก\"}", Ordering = 1 },
                new Language { Key = "Msg_UserAccountEffectiveDateNotSet", Value = "{\"en\":\"User account effective date is not set.\",\"th\":\"ยังไม่ได้กำหนดวันที่เริ่มมีผลของบัญชีผู้ใช้\"}", DefaultValue = "{\"en\":\"User account effective date is not set.\",\"th\":\"ยังไม่ได้กำหนดวันที่เริ่มมีผลของบัญชีผู้ใช้\"}", Ordering = 1 },
                new Language { Key = "Msg_UserAccountEffectiveOn", Value = "{\"en\":\"User account is effective on {0}.\",\"th\":\"บัญชีผู้ใช้มีผลตั้งแต่วันที่ {0}\"}", DefaultValue = "{\"en\":\"User account is effective on {0}.\",\"th\":\"บัญชีผู้ใช้มีผลตั้งแต่วันที่ {0}\"}", Ordering = 1 },
                new Language { Key = "Msg_UserAccountExpired", Value = "{\"en\":\"User account has been expired.\",\"th\":\"บัญชีผู้ใช้หมดอายุแล้ว\"}", DefaultValue = "{\"en\":\"User account has been expired.\",\"th\":\"บัญชีผู้ใช้หมดอายุแล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_PasswordIncorrectMissingLocked", Value = "{\"en\":\"Password is incorrect. ,Missing :  {0} times!, This user is Locked.\",\"th\":\"รหัสผ่านไม่ถูกต้อง, กรอกรหัสผิด {0} ครั้ง, ผู้ใช้นี้ถูกล็อกบัญชีแล้ว\"}", DefaultValue = "{\"en\":\"Password is incorrect. ,Missing :  {0} times!, This user is Locked.\",\"th\":\"รหัสผ่านไม่ถูกต้อง, กรอกรหัสผิด {0} ครั้ง, ผู้ใช้นี้ถูกล็อกบัญชีแล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_PasswordIncorrectMissing", Value = "{\"en\":\"Password is incorrect. ,Missing :  {0} times!\",\"th\":\"รหัสผ่านไม่ถูกต้อง, กรอกรหัสผิด {0} ครั้ง\"}", DefaultValue = "{\"en\":\"Password is incorrect. ,Missing :  {0} times!\",\"th\":\"รหัสผ่านไม่ถูกต้อง, กรอกรหัสผิด {0} ครั้ง\"}", Ordering = 1 },
                new Language { Key = "Msg_LoginSuccessful", Value = "{\"en\":\"Login successful\",\"th\":\"เข้าสู่ะบบสำเร็จ\"}", DefaultValue = "{\"en\":\"Login successful\",\"th\":\"เข้าสู่ะบบสำเร็จ\"}", Ordering = 1 },
                new Language { Key = "Msg_NoFileUploaded", Value = "{\"en\":\"No file uploaded.\",\"th\":\"ไม่มีการอัพโหลดไฟล์\"}", DefaultValue = "{\"en\":\"No file uploaded.\",\"th\":\"ไม่มีการอัพโหลดไฟล์\"}", Ordering = 1 },
                new Language { Key = "Msg_DuplicateKeysInRequest", Value = "{\"en\":\"Duplicate keys found in request: {0}\",\"th\":\"พบคีย์ซ้ำในคำขอ: {0}\"}", DefaultValue = "{\"en\":\"Duplicate keys found in request: {0}\",\"th\":\"พบคีย์ซ้ำในคำขอ: {0}\"}", Ordering = 1 },
                new Language { Key = "Msg_DuplicateDataInColumn", Value = "{\"en\":\"Duplicate data in {0}\",\"th\":\"ข้อมูลซ้ำใน {0}\"}", DefaultValue = "{\"en\":\"Duplicate data in {0}\",\"th\":\"ข้อมูลซ้ำใน {0}\"}", Ordering = 1 },
                new Language { Key = "Msg_InvalidRequestData", Value = "{\"en\":\"Invalid request data.\",\"th\":\"ข้อมูลคำขอไม่ถูกต้อง\"}", DefaultValue = "{\"en\":\"Invalid request data.\",\"th\":\"ข้อมูลคำขอไม่ถูกต้อง\"}", Ordering = 1 },
                new Language { Key = "Msg_CollectorTeamRefCollector", Value = "{\"en\":\"Collector Team Id {0} referenced by Collector data.\",\"th\":\"มีเจ้าหน้าที่ติดตามอยู่ในทีมนี้ รหัสทีม {0}\"}", DefaultValue = "{\"en\":\"Collector Team Id {0} referenced by Collector data.\",\"th\":\"มีเจ้าหน้าที่ติดตามอยู่ในทีมนี้ รหัสทีม {0}\"}", Ordering = 1 },
                new Language { Key = "Msg_EmployeeAlreadyUse", Value = "{\"en\":\"This employee is already in use in the system.!\",\"th\":\"มีพนักงานคนนี้อยู่ในระบบแล้ว\"}", DefaultValue = "{\"en\":\"This employee is already in use in the system.!\",\"th\":\"มีพนักงานคนนี้อยู่ในระบบแล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_EmployeeIdsAlreadyUse", Value = "{\"en\":\"The employee Id {0} are already in use in the system.!\",\"th\":\"ไอดีพนักงาน {0} มีอยู่ในระบบแล้ว\"}", DefaultValue = "{\"en\":\"The employee Id {0} are already in use in the system.!\",\"th\":\"ไอดีพนักงาน {0} มีอยู่ในระบบแล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_UsernameExist", Value = "{\"en\":\"Username already exists!\",\"th\":\"มีชื่อผู้ใช้นี้อยู่แล้ว\"}", DefaultValue = "{\"en\":\"Username already exists!\",\"th\":\"มีชื่อผู้ใช้นี้อยู่แล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_CannotDeleteOwnAccount", Value = "{\"en\":\"You cannot delete your own account.\",\"th\":\"ไม่สามารถลบบัญชีตัวเองได้\"}", DefaultValue = "{\"en\":\"You cannot delete your own account.\",\"th\":\"ไม่สามารถลบบัญชีตัวเองได้\"}", Ordering = 1 },
                new Language { Key = "Msg_NewandConfirmPasswordNotMatch", Value = "{\"en\":\"New password and confirm password do not match!\",\"th\":\"รหัสผ่านใหม่และยืนยันรหัสผ่านไม่ตรงกัน\"}", DefaultValue = "{\"en\":\"New password and confirm password do not match!\",\"th\":\"รหัสผ่านใหม่และยืนยันรหัสผ่านไม่ตรงกัน\"}", Ordering = 1 },
                new Language { Key = "Msg_CurrentPasswordIncorrect.", Value = "{\"en\":\"Current password is incorrect.\",\"th\":\"รหัสผ่านปัจจุบันไม่ถูกต้อง\"}", DefaultValue = "{\"en\":\"Current password is incorrect.\",\"th\":\"รหัสผ่านปัจจุบันไม่ถูกต้อง\"}", Ordering = 1 },
                new Language { Key = "Msg_PasswordMeetsAllPolicy", Value = "{\"en\":\"Password meets all policy requirements.\",\"th\":\"รหัสผ่านตรงตามข้อกำหนดทั้งหมด\"}", DefaultValue = "{\"en\":\"Password meets all policy requirements.\",\"th\":\"รหัสผ่านตรงตามข้อกำหนดทั้งหมด\"}", Ordering = 1 },
                new Language { Key = "Msg_PolicyNameUpperLowerNonalpha", Value = "{\"en\":\"{0} (uppercase, lowercase, digit, non-alphanumeric).\",\"th\":\"{0} ตัวพิมพ์ใหญ่ ตัวพิมพ์เล็ก ตัวเลข และตัวอักษรที่ไม่ใช่ตัวอักษรและไม่ใช่ตัวเลข\"}", DefaultValue = "{\"en\":\"{0} (uppercase, lowercase, digit, non-alphanumeric).\",\"th\":\"{0} ตัวพิมพ์ใหญ่ ตัวพิมพ์เล็ก ตัวเลข และตัวอักษรที่ไม่ใช่ตัวอักษรและไม่ใช่ตัวเลข\"}", Ordering = 1 },
                new Language { Key = "Msg_NewPassWordCannotBeTheSame", Value = "{\"en\":\"New password cannot be the same as any of your recent passwords. Please choose a different password.\",\"th\":\"รหัสผ่านใหม่ต้องไม่ตรงกับรหัสผ่านปัจจุบันของคุณ โปรดเลือกรหัสผ่านอื่น\"}", DefaultValue = "{\"en\":\"New password cannot be the same as any of your recent passwords. Please choose a different password.\",\"th\":\"รหัสผ่านใหม่ต้องไม่ตรงกับรหัสผ่านปัจจุบันของคุณ โปรดเลือกรหัสผ่านอื่น\"}", Ordering = 1 },
                new Language { Key = "Msg_PasswordChangedSuccess", Value = "{\"en\":\"Password changed successfully.\",\"th\":\"เปลี่ยนรหัสผ่านเรียบร้อยแล้ว\"}", DefaultValue = "{\"en\":\"Password changed successfully.\",\"th\":\"เปลี่ยนรหัสผ่านเรียบร้อยแล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_InvalidData", Value = "{\"en\":\"Invalid data\",\"th\":\"ข้อมูลไม่ถูกต้อง\"}", DefaultValue = "{\"en\":\"Invalid data\",\"th\":\"ข้อมูลไม่ถูกต้อง\"}", Ordering = 1 },
                new Language { Key = "Msg_UserGroupCodeorNameExists", Value = "{\"en\":\"UserGroupCode or UserGroupName already exists!\",\"th\":\"รหัสกลุ่มผู้ใข้ หรือ ชื่อกลุ่มผู้ใช้ มีอยู่แล้ว!\"}", DefaultValue = "{\"en\":\"UserGroupCode or UserGroupName already exists!\",\"th\":\"รหัสกลุ่มผู้ใข้ หรือ ชื่อกลุ่มผู้ใช้ มีอยู่แล้ว!\"}", Ordering = 1 },
                new Language { Key = "Msg_NoDataFound", Value = "{\"en\":\"No data found.\",\"th\":\"ไม่พบข้อมูล\"}", DefaultValue = "{\"en\":\"No data found.\",\"th\":\"ไม่พบข้อมูล\"}", Ordering = 1 },
                new Language { Key = "Msg_UserGroupCannotDelete", Value = "{\"en\":\"UserGroup {0} cannot delete.\",\"th\":\"กลุ่มผู้ใช้ {0} ไม่สามารถลบได้\"}", DefaultValue = "{\"en\":\"UserGroup {0} cannot delete.\",\"th\":\"กลุ่มผู้ใช้ {0} ไม่สามารถลบได้\"}", Ordering = 1 },
                new Language { Key = "Msg_CollectorAlreadyUse", Value = "{\"en\":\"This collector is already in use in the system.!\",\"th\":\"มีเจ้าหน้าที่ติดตามคนนี้อยู่ในระบบแล้ว\"}", DefaultValue = "{\"en\":\"This collector is already in use in the system.!\",\"th\":\"มีเจ้าหน้าที่ติดตามคนนี้อยู่ในระบบแล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_InvalidRequest", Value = "{\"en\":\"Invalid request.\",\"th\":\"คำขอไม่ถูกต้อง\"}", DefaultValue = "{\"en\":\"Invalid request.\",\"th\":\"คำขอไม่ถูกต้อง\"}", Ordering = 1 },
                new Language { Key = "Msg_CollectorRoleExists", Value = "{\"en\":\"Collector Role Code already exists.\",\"th\":\"มีรหัสเจ้าหน้าที่ติดตามนี้อยู่แล้ว\"}", DefaultValue = "{\"en\":\"Collector Role Code already exists.\",\"th\":\"มีรหัสเจ้าหน้าที่ติดตามนี้อยู่แล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_CollectorRoleIdRefCollector", Value = "{\"en\":\"Collector Role Id {0} referenced by Collector data.\",\"th\":\"มีเจ้าหน้าที่ติดตามอยู่ในบทบาทนี้ รหัสบทบาท {0}\"}", DefaultValue = "{\"en\":\"Collector Role Id {0} referenced by Collector data.\",\"th\":\"มีเจ้าหน้าที่ติดตามอยู่ในบทบาทนี้ รหัสบทบาท {0}\"}", Ordering = 1 },
                new Language { Key = "Msg_AccessDenyColOrTeamInactive", Value = "{\"en\":\"Access denied. Collector, Team or Team Assignment is inactive.\",\"th\":\"ไม่สามารถเข้าใช้งานได้ เนื่องจากเจ้าหน้าที่ติดตาม, ทีม หรือ จัดการทีมเจ้าหน้าที่ติดตาม ไม่อยู่ในสถานะเปิดใช้งาน\"}", DefaultValue = "{\"en\":\"Access denied. Collector, Team or Team Assignment is inactive.\",\"th\":\"ไม่สามารถเข้าใช้งานได้ เนื่องจากเจ้าหน้าที่ติดตาม, ทีม หรือ จัดการทีมเจ้าหน้าที่ติดตาม ไม่อยู่ในสถานะเปิดใช้งาน\"}", Ordering = 1 },
                new Language { Key = "Msg_ValidationFailed", Value = "{\"en\":\"Validation failed\",\"th\":\"การตรวจสอบความถูกต้องล้มเหลว\"}", DefaultValue = "{\"en\":\"Validation failed\",\"th\":\"การตรวจสอบความถูกต้องล้มเหลว\"}", Ordering = 1 },
                new Language { Key = "Msg_LogoutSuccessful", Value = "{\"en\":\"Logout successful\",\"th\":\"ออกจากระบบสำเร็จ\"}", DefaultValue = "{\"en\":\"Logout successful\",\"th\":\"ออกจากระบบสำเร็จ\"}", Ordering = 1 },
                new Language { Key = "Msg_SessionExpired", Value = "{\"en\":\"Session expired.\",\"th\":\"เซสชั่นหมดอายุ\"}", DefaultValue = "{\"en\":\"Session expired.\",\"th\":\"เซสชั่นหมดอายุ\"}", Ordering = 1 },
                new Language { Key = "Msg_ItemIdAlreadyExists", Value = "{\"en\":\"Item already exists!\",\"th\":\"มีรายการอยู่แล้ว\"}", DefaultValue = "{\"en\":\"Item already exists!\",\"th\":\"มีรายการอยู่แล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_ItemNameAlreadyExists", Value = "{\"en\":\"Item Name already exists!\",\"th\":\"มีชื่อรายการอยู่แล้ว\"}", DefaultValue = "{\"en\":\"Item Name already exists!\",\"th\":\"มีชื่อรายการอยู่แล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_NotFound", Value = "{\"en\":\"{0} not found.\",\"th\":\"ไม่พบ {0}\"}", DefaultValue = "{\"en\":\"{0} not found.\",\"th\":\"ไม่พบ {0}\"}", Ordering = 1 },
                new Language { Key = "Msg_2ParamsNotFound", Value = "{\"en\":\"{0} {1} not found.\",\"th\":\"ไม่พบ {0} {1}\"}", DefaultValue = "{\"en\":\"{0} {1} not found.\",\"th\":\"ไม่พบ {0} {1}\"}", Ordering = 1 },
                new Language { Key = "Yes", Value = "{\"en\":\"Yes\",\"th\":\"ใช่\"}", DefaultValue = "{\"en\":\"Yes\",\"th\":\"ใช่\"}", Ordering = 1 },
                new Language { Key = "No", Value = "{\"en\":\"No\",\"th\":\"ไม่ใช่\"}", DefaultValue = "{\"en\":\"No\",\"th\":\"ไม่ใช่\"}", Ordering = 1 },
                new Language { Key = "Msg_DuplicateCollectorInTeamAssignment", Value = "{\"en\":\"Duplicated collector Id {0} in the team {1}.\",\"th\":\"พบเจ้าหน้าที่ติดตามรหัส {0} ซ้ำในทีม {1}\"}", DefaultValue = "{\"en\":\"Duplicated collector Id {0} in the team {1}.\",\"th\":\"พบเจ้าหน้าที่ติดตามรหัส {0} ซ้ำในทีม {1}\"}", Ordering = 1 },

                new Language { Key = "Msg_SavedSuccessfully", Value = "{\"en\":\"Saved successfully.\",\"th\":\"บันทึกข้อมูลสำเร็จ\"}", DefaultValue = "{\"en\":\"Saved successfully.\",\"th\":\"บันทึกข้อมูลสำเร็จ\"}", Ordering = 1 },
                new Language { Key = "Msg_DeletedSuccessfully", Value = "{\"en\":\"Deleted successfully.\",\"th\":\"ลบข้อมูลสำเร็จ\"}", DefaultValue = "{\"en\":\"Deleted successfully.\",\"th\":\"ลบข้อมูลสำเร็จ\"}", Ordering = 1 },
                new Language { Key = "Msg_PasswordChangedSuccessfully", Value = "{\"en\":\"Password changed successfully.\",\"th\":\"เปลี่ยนรหัสผ่านสำเร็จ\"}", DefaultValue = "{\"en\":\"Password changed successfully.\",\"th\":\"เปลี่ยนรหัสผ่านสำเร็จ\"}", Ordering = 1 },
                new Language { Key = "Msg_UserUnlockedSuccessfully", Value = "{\"en\":\"Unlocked successfully.\",\"th\":\"ปลดล็อกสำเร็จ\"}", DefaultValue = "{\"en\":\"Unlocked successfully.\",\"th\":\"ปลดล็อกสำเร็จ\"}", Ordering = 1 },
                new Language { Key = "Msg_PasswordResetSuccessfully", Value = "{\"en\":\"Password reset successfully.\",\"th\":\"รีเซ็ตข้อมูลสำเร็จ\"}", DefaultValue = "{\"en\":\"Password reset successfully.\",\"th\":\"รีเซ็ตข้อมูลสำเร็จ\"}", Ordering = 1 },
                new Language { Key = "Msg_LanguageResetSuccessfully", Value = "{\"en\":\"Language reset successfully.\",\"th\":\"รีเซ็ตภาษาสำเร็จ\"}", DefaultValue = "{\"en\":\"Language reset successfully.\",\"th\":\"รีเซ็ตภาษาสำเร็จ\"}", Ordering = 1 },
                new Language { Key = "Msg_LoginSuccessfully", Value = "{\"en\":\"Login successfully.\",\"th\":\"เข้าสู่ระบบสำเร็จ\"}", DefaultValue = "{\"en\":\"Login successfully.\",\"th\":\"เข้าสู่ระบบสำเร็จ\"}", Ordering = 1 },
                new Language { Key = "Msg_LogoutSuccessfully", Value = "{\"en\":\"Logout successfully.\",\"th\":\"ออกจากระบบสำเร็จ\"}", DefaultValue = "{\"en\":\"Logout successfully.\",\"th\":\"ออกจากระบบสำเร็จ\"}", Ordering = 1 },
                new Language { Key = "Msg_CollectorExist", Value = "{\"en\":\"User {0} already exists in collector profile.\",\"th\":\"ผู้ใช้งาน {0} มีอยู่แล้วในข้อมูลผู้ติดตาม\"}", DefaultValue = "{\"en\":\"User {0} already exists in collector profile.\",\"th\":\"ผู้ใช้งาน {0} มีอยู่แล้วในข้อมูลผู้ติดตาม\"}", Ordering = 1 },
                new Language { Key = "Msg_ComfirmOverwriteSupervisor", Value = "{\"en\":\"Supervisor is already assigned. Do you want to overwrite it?\",\"th\":\"มีหัวหน้าทีมกำหนดอยู่แล้ว ต้องการบันทึกทับหรือไม่?\"}", DefaultValue = "{\"en\":\"Supervisor is already assigned. Do you want to overwrite it?\",\"th\":\"มีหัวหน้าทีมกำหนดอยู่แล้ว ต้องการบันทึกทับหรือไม่?\"}", Ordering = 1 },
                new Language { Key = "Msg_SupervisorAlreadyAssigned", Value = "{\"en\":\"Superviser is already assigned.\",\"th\":\"มีหัวหน้าทีมกำหนดไว้อยู่แล้ว\"}", DefaultValue = "{\"en\":\"___EN___\",\"th\":\"Superviser is already assigned.\"}", Ordering = 1 },
                new Language { Key = "Msg_NotHavePermissionToAccessContract", Value = "{\"en\":\"You do not have permission to access this contract.\",\"th\":\"คุณไม่มีสิทธิ์เข้าดูข้อมูลในสัญญานี้\"}", DefaultValue = "{\"en\":\"You do not have permission to access this contract.\",\"th\":\"คุณไม่มีสิทธิ์เข้าดูข้อมูลในสัญญานี้\"}", Ordering = 1 },
                new Language { Key = "Msg_CrossTeamReasignmentNotAllow", Value = "{\"en\":\"Cross-team reasignment is not allowed.\",\"th\":\"ไม่สามารถโอนย้ายงานข้ามทีมได้\"}", DefaultValue = "{\"en\":\"Cross-team reasignment is not allowed.\",\"th\":\"ไม่สามารถโอนย้ายงานข้ามทีมได้\"}", Ordering = 1 },
                new Language { Key = "Msg_ReassignmentCompleted", Value = "{\"en\":\"Reassignment completed.\",\"th\":\"การโอนย้ายงานเสร็จสมบูรณ์\"}", DefaultValue = "{\"en\":\"Reassignment completed.\",\"th\":\"การโอนย้ายงานเสร็จสมบูรณ์\"}", Ordering = 1 },
                new Language { Key = "Msg_ApprovedSuccessfully", Value = "{\"en\":\"Approved successfully.\",\"th\":\"อนุมัติเรียบร้อยแล้ว\"}", DefaultValue = "{\"en\":\"Approved successfully.\",\"th\":\"อนุมัติเรียบร้อยแล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_RejectedSuccessfully", Value = "{\"en\":\"Rejected successfully.\",\"th\":\"ไม่อนุมัติเรียบร้อยแล้ว\"}", DefaultValue = "{\"en\":\"Rejected successfully.\",\"th\":\"ไม่อนุมัติเรียบร้อยแล้ว\"}", Ordering = 1 },
                new Language { Key = "Msg_NoPermissiontoApproveContactID", Value = "{\"en\":\"No permission to approve the contract with ID {0}.\",\"th\":\"ไม่มีสิทธิ์อนุมัติสัญญาเลขที่ {0}\"}", DefaultValue = "{\"en\":\"No permission to approve the contract with ID {0}.\",\"th\":\"ไม่มีสิทธิ์อนุมัติสัญญาเลขที่ {0}\"}", Ordering = 1 },
                new Language { Key = "Msg_NotHavePermissionToAccessCollectorList", Value = "{\"en\":\"You do not have permission to access collector list.\",\"th\":\"คุณไม่มีสิทธิ์เข้าดูข้อมูลรายชื่อเจ้าหน้าที่ติดตาม\"}", DefaultValue = "{\"en\":\"You do not have permission to access collector list.\",\"th\":\"คุณไม่มีสิทธิ์เข้าดูข้อมูลรายชื่อเจ้าหน้าที่ติดตาม\"}", Ordering = 1 },
                new Language { Key = "Msg_CannotDeleteCollectorExistTeam", Value = "{\"en\":\"Cannot delete Collector because team mapping still exists.\",\"th\":\"ไม่สามารถลบเจ้าหน้าที่ติดตามได้ เนื่องจากยังมีการผูกกับทีมอยู่\"}", DefaultValue = "{\"en\":\"Cannot delete Collector because team mapping still exists.\",\"th\":\"ไม่สามารถลบเจ้าหน้าที่ติดตามได้ เนื่องจากยังมีการผูกกับทีมอยู่\"}", Ordering = 1 },
                new Language { Key = "Msg_UserAccessDenied", Value = "{\"en\":\"Access denied. Please contact the administrator.\",\"th\":\"คุณไม่มีสิทธิ์เข้าใช้งาน กรุณาติดต่อผู้ดูแลระบบ\"}", DefaultValue = "{\"en\":\"Access denied. Please contact the administrator.\",\"th\":\"คุณไม่มีสิทธิ์เข้าใช้งาน กรุณาติดต่อผู้ดูแลระบบ\"}", Ordering = 1 },
                new Language { Key = "Msg_AtLeastOneRequired", Value = "{\"en\":\"At least one {0} is required.\",\"th\":\"ต้องมี {0} อย่างน้อยหนึ่งรายการ\"}", DefaultValue = "{\"en\":\"At least one {0} is required.\",\"th\":\"ต้องมี {0} อย่างน้อยหนึ่งรายการ\"}", Ordering = 1 },
                new Language { Key = "Msg_EffectiveDateMustBeLessThanExpireDate", Value = "{\"en\":\"Effective date must be less than expire date.\",\"th\":\"วันที่เริ่มมีผลต้องน้อยกว่าวันที่หมดอายุ\"}", DefaultValue = "{\"en\":\"Effective date must be less than expire date.\",\"th\":\"วันที่เริ่มมีผลต้องน้อยกว่าวันที่หมดอายุ\"}", Ordering = 1 },
                new Language { Key = "Msg_Inactive", Value = "{\"en\":\"{0} is inactive.\",\"th\":\"{0} ไม่อยู่ในสถานะเปิดใช้งาน\"}", DefaultValue = "{\"en\":\"{0} is inactive.\",\"th\":\"{0} ไม่อยู่ในสถานะเปิดใช้งาน\"}", Ordering = 1 },
                new Language{ Key = "Msg_ExistingWorklistWithTeamAssignment", Value = "{\"en\":\"Existing worklist with team assignment, CollectorId {0}, Team {1}.\",\"th\":\"มีรายการงานที่มีการจัดการทีมอยู่ เจ้าหน้าที่ติดตามรหัส {0} ทีม {1}\"}", DefaultValue = "{\"en\":\"Existing worklist with team assignment, CollectorId {0}, Team {1}.\",\"th\":\"มีรายการงานที่มีการจัดการทีมอยู่ เจ้าหน้าที่ติดตามรหัส {0} ทีม {1}\"}", Ordering = 1 },
                new Language{ Key = "Msg_InvalidCapacity", Value = "{\"en\":\"Invalid capacity. Capacity must be more than or equal to zero.\",\"th\":\"ความสามารถไม่ถูกต้อง ความสามารถต้องมากกว่าหรือเท่ากับศูนย์\"}", DefaultValue = "{\"en\":\"Invalid capacity. Capacity must be more than or equal to zero.\",\"th\":\"ความสามารถไม่ถูกต้อง ความสามารถต้องมากกว่าหรือเท่ากับศูนย์\"}", Ordering = 1 },
                new Language { Key = "Msg_CollectorExistByEmployeeId", Value = "{\"en\":\"Collector already exists for Employee ID {0}.\",\"th\":\"เป็นเจ้าหน้าที่ติดตามอยู่แล้วสำหรับรหัสพนักงาน {0}\"}", DefaultValue = "{\"en\":\"Collector already exists for Employee ID {0}.\",\"th\":\"เป็นเจ้าหน้าที่ติดตามอยู่แล้วสำหรับรหัสพนักงาน {0}\"}", Ordering = 1 },
                new Language { Key = "Msg_CannotDeleteUserWithExistCollectorFromUserName", Value = "{\"en\":\"Cannot delete user with username {0} because it is associated with an existing collector profile.\",\"th\":\"ไม่สามารถลบบัญชีผู้ใช้ที่มีชื่อผู้ใช้ {0} ได้ เนื่องจากมีการเชื่อมโยงกับข้อมูลเจ้าหน้าที่ติดตามที่มีอยู่\"}", DefaultValue = "{\"en\":\"Cannot delete user with username {0} because it is associated with an existing collector profile.\",\"th\":\"ไม่สามารถลบบัญชีผู้ใช้ที่มีชื่อผู้ใช้ {0} ได้ เนื่องจากมีการเชื่อมโยงกับข้อมูลเจ้าหน้าที่ติดตามที่มีอยู่\"}", Ordering = 1 },
                new Language { Key = "TotalAmountDue", Value = "{\"en\":\"<b>Total Amount Due</b>\",\"th\":\"<b>ยอดเงินที่ต้องชำระรวม</b>\"}", DefaultValue = "{\"en\":\"<b>Total Amount Due</b>\",\"th\":\"<b>ยอดเงินที่ต้องชำระรวม</b>\"}", Ordering = 1 }
            //new Language { Key = "Msg_", Value = "{\"en\":\"___EN___\",\"th\":\"___TH___\"}", DefaultValue = "{\"en\":\"___EN___\",\"th\":\"___TH___\"}", Ordering = 1 }


            );
            await context.SaveChangesAsync();
            #endregion
        }

        public static async Task CreateProcedureAsync(AppDbContext context)
        {
            string sql = @"";

            await context.Database.ExecuteSqlRawAsync(sql);
        }

        public static async Task SeedDataFromSqlFileAsync(AppDbContext context, string sqlFilePath)
        {
            if (!File.Exists(sqlFilePath))
                throw new FileNotFoundException($"SQL file not found: {sqlFilePath}");

            string sql = await File.ReadAllTextAsync(sqlFilePath);

            if (!string.IsNullOrWhiteSpace(sql))
            {
                await context.Database.ExecuteSqlRawAsync(sql);
            }
        }
    }
}
