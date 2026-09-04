using ACTCore.CollectionService.API.Data.Extensions;
using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Domain.Entities.Contracts;
using ACTCore.CollectionService.Domain.Entities.Imports;
using ACTCore.CollectionService.Domain.Entities.Masters;
using ACTCore.CollectionService.Domain.Entities.Profiles;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Domain.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Models;
using System.Text.Json;

namespace ACTCore.CollectionService.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region System & System
        public virtual DbSet<SysUser> SysUsers { get; set; }
        public virtual DbSet<SysUserGroup> SysUserGroups { get; set; }
        public virtual DbSet<SysPermission> SysPermission { get; set; }
        public virtual DbSet<SysRole> SysRole { get; set; }
        public virtual DbSet<SysRolePermission> SysRolePermission { get; set; }
        public virtual DbSet<SysUserGroupRole> SysUserGroupRole { get; set; }
        public virtual DbSet<SysUserGroupAccessRight> UserGroupAccesses { get; set; }
        public virtual DbSet<SysPasswordHistory> SysPasswordHistories { get; set; }
        public virtual DbSet<ActiveSession> SysUserActiveSessions { get; set; }
        public virtual DbSet<SysItem> SysItems { get; set; }
        public virtual DbSet<SysItemAccessRight> SysItemAccessRights { get; set; }
        public virtual DbSet<SysUserItemFavorite> SysUserItemFavorites { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<SysParameter> SysParameters { get; set; }
        public virtual DbSet<SysEnum> SysEnums { get; set; }
        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<ActivityLogDetail> AuditLogDetails { get; set; }
        public virtual DbSet<ApiLog> ApiLogs { get; set; }
        public virtual DbSet<AuthLog> AuthLogs { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<SysPolicy> SysPolicies { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<ProcessExecutionLog> ProcessExecutionLogs { get; set; }
        public virtual DbSet<ProcessExecutionLogDetail> ProcessExecutionLogDetails { get; set; }

        public virtual DbSet<vw_ReassignWorklist> vw_ReassignWorklist { get; set; } = null!;
        public virtual DbSet<vw_UnassignWorklist> vw_UnassignWorklist { get; set; } = null!;

        #endregion

        #region Profiles
        public virtual DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public virtual DbSet<CompanyProfile> CompanyProfiles { get; set; }
        #endregion

        #region Collection        
        public virtual DbSet<ColPermission> ColPermissions { get; set; }
        public virtual DbSet<ColRole> ColRoles { get; set; }
        public virtual DbSet<ColRolePermission> ColRolePermissions { get; set; }
        public virtual DbSet<ColTeam> ColTeams { get; set; }
        public virtual DbSet<CollectorProfile> Collectors { get; set; }
        public virtual DbSet<ColTeamAssignment> ColTeamAssignments { get; set; }
        public virtual DbSet<Worklist> Worklists { get; set; }
        public virtual DbSet<WorklistHistory> WorklistHistorys { get; set; }
        public virtual DbSet<ColNoteAction> ColNoteActions { get; set; }
        public virtual DbSet<ColNoteResult> ColNoteResults { get; set; }
        public virtual DbSet<ColArea> ColAreas { get; set; }
        public virtual DbSet<AssignmentWorklist> AssignmentWorklists { get; set; }
        #endregion

        #region Contract
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<ContractAddress> ContractAddresses { get; set; }
        public virtual DbSet<ContractAsset> ContractAssets { get; set; }
        public virtual DbSet<ContractAssetVehicle> ContractAssetVehicles { get; set; }
        public virtual DbSet<ContractOverdue> ContractOverdues { get; set; }
        public virtual DbSet<ContractPayment> ContractPayments { get; set; }
        public virtual DbSet<ContractPerson> ContractPersons { get; set; }
        public virtual DbSet<ContractPhone> ContractPhones { get; set; }
        #endregion

        #region Master
        public virtual DbSet<Prefix> Prefixs { get; set; }

        //public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<SubDistrict> SubDistricts { get; set; }
        #endregion

        #region Organization
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        //public virtual DbSet<Team> Teams { get; set; }
        #endregion

        #region WorkFlow
        /*
        public virtual DbSet<WorkFlow> WorkFlows { get; set; }
        public virtual DbSet<WorkFlowApplication> WorkFlowApplications { get; set; }
        public virtual DbSet<WorkFlowApplicationHistory> WorkFlowApplicationHistories { get; set; }
        public virtual DbSet<WorkFlowStep> WorkFlowSteps { get; set; }
        public virtual DbSet<WorkflowActionListMaster> WorkflowActionListMasters { get; set; }
        public virtual DbSet<WorkflowStepMaster> WorkflowStepMasters { get; set; }
        */
        #endregion

        #region Mapping Import
        public virtual DbSet<MappingImport> MappingImports { get; set; }
        public virtual DbSet<MappingImportDetail> MappingImportDetails { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // disable cascade delete globally
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            // Call Extension Method if needed
            this.ConfigurationIdentityTable(modelBuilder);

            // Options for preventing Unicode for other languages.
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false,
                PropertyNamingPolicy = null,
                PropertyNameCaseInsensitive = true
            };
            //var languageValueConverter = new ValueConverter<LanguageValue, string>(
            //    v => JsonSerializer.Serialize(v, options),
            //    v => JsonSerializer.Deserialize<LanguageValue>(v, options) ?? new LanguageValue()
            //);

            #region system and security fluent api

            modelBuilder.Entity<SysEnum>()
                .HasIndex(e => new { e.EnumName, e.EnumCode })
                .IsUnique();

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.HasIndex(u => u.UserName).IsUnique();
                entity.HasIndex(u => u.EmployeeId).IsUnique();
            });

            modelBuilder.Entity<SysUserGroup>(entity =>
            {
                entity.HasIndex(u => u.UserGroupCode).IsUnique();
                entity.HasIndex(u => u.UserGroupName).IsUnique();
            });

            modelBuilder.Entity<SysRole>(entity =>
            {
                entity.HasIndex(e => e.RoleCode).IsUnique();
                entity.HasIndex(e => e.RoleName).IsUnique();
            });

            modelBuilder.Entity<SysPermission>(entity =>
            {
                entity.HasIndex(e => e.PermissionCode).IsUnique();
                entity.HasIndex(e => e.PermissionName).IsUnique();
            });

            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.HasIndex(x => x.UserId);
                entity.HasIndex(x => x.Action);
                entity.HasIndex(x => x.EntityName);
                entity.HasIndex(x => x.Timestamp);
            });

            modelBuilder.Entity<SysPolicy>(entity =>
            {
                entity.HasIndex(p => p.PolicyCode).IsUnique();
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(l => l.Value)
                    .HasColumnType("nvarchar(max)");
                //.HasConversion(languageValueConverter);
                entity.Property(l => l.DefaultValue)
                    .HasColumnType("nvarchar(max)");
                //.HasConversion(languageValueConverter);
                entity.Property(l => l.ValueEn)
                    .HasComputedColumnSql("JSON_VALUE([Value], '$.en')", stored: false);
                entity.Property(l => l.ValueTh)
                    .HasComputedColumnSql("JSON_VALUE([Value], '$.th')", stored: false);
            });

            modelBuilder.Entity<SysItem>(entity =>
            {
                entity.Property(i => i.ItemNameEn).HasComputedColumnSql("JSON_VALUE(ItemName, '$.en')", stored: false);
                entity.Property(i => i.ItemNameTh).HasComputedColumnSql("JSON_VALUE(ItemName, '$.th')", stored: false);
            });

            #endregion

            #region master fluent api

            modelBuilder.Entity<Prefix>(entity =>
            {
                entity.HasIndex(e => e.PrefixCode).IsUnique();
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasIndex(e => e.ProvinceName).IsUnique();
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasIndex(e => new { e.ProvinceId, e.DistrictName })
                .IsUnique();
            });

            modelBuilder.Entity<SubDistrict>(entity =>
            {
                // can't set index unique (example data is duplicate)
                //entity.HasIndex(e => new { e.DistrictId, e.SubDistrictName }).IsUnique();
            });

            modelBuilder.Entity<EmployeeProfile>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.DepartmentCode).IsUnique();
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasIndex(e => e.PositionCode).IsUnique();
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(e => e.TeamCode).IsUnique();
            });

            #endregion

            #region collection fluent api

            modelBuilder.Entity<ColPermission>()
                .HasIndex(p => new { p.ColPermissionCode, p.ColPermissionName })
                .IsUnique();

            modelBuilder.Entity<ColRole>()
                .HasIndex(r => new { r.ColRoleCode, r.ColRoleName })
                .IsUnique();

            modelBuilder.Entity<ColRolePermission>()
                .HasKey(rp => new { rp.ColRoleId, rp.ColPermissionId });

            modelBuilder.Entity<ColRolePermission>()
                .HasOne(rp => rp.ColRole)
                .WithMany(r => r.ColRolePermissions)
                .HasForeignKey(rp => rp.ColRoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ColRolePermission>()
                .HasOne(rp => rp.ColPermission)
                .WithMany(p => p.ColRolePermissions)
                .HasForeignKey(rp => rp.ColPermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ColTeam>()
                .HasIndex(t => new { t.ColTeamCode, t.ColTeamName })
                .IsUnique();

            modelBuilder.Entity<CollectorProfile>()
                .HasIndex(c => c.UserId)
                .IsUnique();

            modelBuilder.Entity<CollectorProfile>()
                .HasOne(c => c.User)
                .WithMany(u => u.Collectors)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CollectorProfile>()
                .HasOne(c => c.ColRole)
                .WithMany(r => r.Collectors)
                .HasForeignKey(c => c.ColRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ColTeamAssignment>()
                .HasOne(a => a.ColTeam)
                .WithMany(t => t.ColTeamAssignments)
                .HasForeignKey(a => a.ColTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ColTeamAssignment>()
                .HasOne(a => a.Collector)
                .WithMany(c => c.ColTeamAssignments)
                .HasForeignKey(a => a.CollectorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ColNoteAction>(entity =>
            {
                entity.HasIndex(i => i.ActionCode).IsUnique();
            });

            modelBuilder.Entity<ColNoteResult>(entity =>
            {
                entity.HasIndex(e => new { e.ActionId, e.ResultCode })
                    .IsUnique();
            });

            modelBuilder.Entity<ContractAddress>(entity => { 
                entity.Property(x => x.Id)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();
                entity.HasOne(a => a.ContractPerson)
                    .WithMany(cp => cp.ContractAddresses)
                    .HasForeignKey(a => a.PersonRefId)
                    .HasPrincipalKey(cp => cp.PersonRefId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.PersonRefId, e.AddressType })
                    .IsUnique();
            });

            modelBuilder.Entity<ContractPerson>(entity => {
                entity.HasIndex(e => e.PersonRefId).IsUnique();
            });

            modelBuilder.Entity<ContractAsset>()
                .Property(x => x.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ContractAssetVehicle>()
                .Property(x => x.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ContractOverdue>()
                .Property(x => x.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ContractPayment>()
                .Property(x => x.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();




            modelBuilder.Entity<ContractPhone>(entity => {
                entity.Property(x => x.Id)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();
                entity.HasOne(p => p.ContractPerson)
                    .WithMany(cp => cp.ContractPhones)
                    .HasForeignKey(p => p.PersonRefId)
                    .HasPrincipalKey(cp => cp.PersonRefId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.PersonRefId, e.PhoneType })
                    .IsUnique();
            });
                
            #endregion

            #region view model
            modelBuilder.Entity<vw_ReassignWorklist>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_ReassignWorklist");
            });

            modelBuilder.Entity<vw_UnassignWorklist>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_UnassignWorklist");
            });

            #endregion

            #region Trigger
            modelBuilder.Entity<Worklist>()
                .ToTable(tb => tb.UseSqlOutputClause(false))
                .ToTable(tb => tb.HasTrigger("tr_WorkList_History"));
            #endregion

            #region Mapping Import fluent api
            modelBuilder.Entity<MappingImport>(entity =>
            {
                entity.Property(m => m.Id).ValueGeneratedOnAdd();
                entity.HasIndex(m => m.TableName).IsUnique();
            });

            modelBuilder.Entity<MappingImportDetail>(entity =>
            {
                entity.Property(m => m.Id).ValueGeneratedOnAdd();
                entity.HasOne(d => d.MappingImport)
                      .WithMany(m => m.MappingImportDetails)
                      .HasForeignKey(d => d.MappingId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(d => new { d.MappingId, d.FieldName  }).IsUnique();
                entity.HasIndex(d => new { d.MappingId, d.IsActive  });
            });
            #endregion
        }
    }
}
