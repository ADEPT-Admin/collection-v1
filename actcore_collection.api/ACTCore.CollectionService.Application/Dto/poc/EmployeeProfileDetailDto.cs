namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class EmployeeProfileDetailDto
    {
        public string EmployeeId { get; set; }
        public int? PrefixId { get; set; }
        public string PrefixName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FirstNameEn { get; set; }
        public string LastNameEn { get; set; }
        public string Email { get; set; }
        public string BranchName { get; set; }
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string NationalID { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string WorkStatus { get; set; }
        public Guid? CurrentAddressId { get; set; }
        public AddressDto CurrentAddresses { get; set; }
        public Guid? RegistrationAddressId { get; set; }
        public AddressDto RegistrationAddresses { get; set; }
        public DateTime? StartWorkingDate { get; set; }
        public DateTime? ProbationDate { get; set; }
        public DateTime? WorkEffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? TeamId { get; set; }
        public TeamDto Team { get; set; }
        public int? RoleId { get; set; }
        public CollectionRoleDto Role { get; set; }
        public int? DepartmentId { get; set; }
        public DepartmentDto Department { get; set; }
        public int? PositionId { get; set; }
        public PositionDto Position { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? SalaryEffectiveDate { get; set; }
        public EmployeeProfileSimpleDto Manager { get; set; }
        public List<EmployeeProfileSimpleDto> Subordinates { get; set; }
        //public List<SysUserSimpleDto> SysUsers { get; set; }
        //public List<ColTeamGroupSimpleDto> ColTeamGroups { get; set; }
        //public List<CollectorSimpleDto> Collectors { get; set; }
    }

    public class AddressDto
    {
        public Guid AddressId { get; set; }
        public string AddressDetail { get; set; }
    }
    //public class TeamDto { public int TeamId { get; set; } public string TeamName { get; set; } }
    public class EmployeeProfileSimpleDto { public string EmployeeId { get; set; } public string FullName { get; set; } }
    //public class SysUserSimpleDto { public string UserId { get; set; } }
    public class ColTeamGroupSimpleDto { public int ColTeamGroupId { get; set; } }
    public class CollectorSimpleDto { public int CollectorId { get; set; } }
}