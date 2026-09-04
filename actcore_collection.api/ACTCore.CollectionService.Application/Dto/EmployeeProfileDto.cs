using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.Application.Dto
{
    public class EmployeeProfileDto
    {
        public string EmployeeId { get; set; }
        public int? PrefixId { get; set; }
        public LanguageValue NameLanguage { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string NationalID { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string Email { get; set; }
        public string Branch { get; set; }
        public string SupervisorId { get; set; }
        public string WorkStatus { get; set; }
        public DateTime? StartWorkingDate { get; set; }
        public DateTime? ProbationDate { get; set; }
        public DateTime? WorkEffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }

        public PrefixDto Prefix { get; set; }
        public EmployeeProfileDto Supervisor { get; set; }
        public DepartmentDto Department { get; set; }
        public PositionDto Position { get; set; }
        public TeamDto Team { get; set; }
    }
}