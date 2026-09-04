using ACTCore.CollectionService.API.Models.Attributes;
using SharedKernel.CommonConstants;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class TeamAssignmentDisplayFilterDto
    {
        [ColumnWidth(150)]
        public string ColTeamName { get; set; }

        [ColumnWidth(120)]
        public string CollectorEmpId { get; set; }

        [ColumnWidth(200)]
        public string CollectorEmpName { get; set; }

        public bool IsSupervisor { get; set; }

        [ColumnWidth(150)]
        public int Capacity { get; set; }

        [DateFormat(CommonConstants.DateFormat)]
        public DateTime? EffectiveDate { get; set; }

        [DateFormat(CommonConstants.DateFormat)]
        public DateTime? ExpireDate { get; set; }

        public bool IsActive { get; set; }
    }
}
