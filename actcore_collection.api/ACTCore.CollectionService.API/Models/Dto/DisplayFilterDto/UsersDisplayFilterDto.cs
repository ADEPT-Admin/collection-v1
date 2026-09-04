using ACTCore.CollectionService.API.Models.Attributes;
using SharedKernel.CommonConstants;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class UsersDisplayFilterDto
    {
        [ColumnWidth(120)]
        public string UserName { get; set; }

        [ColumnWidth(100)]
        public string EmployeeId { get; set; }

        [ColumnWidth(250)]
        public string EmployeeName { get; set; }

        [ColumnWidth(150)]
        public string UserGroup { get; set; }

        [DateFormat(CommonConstants.DateFormat)]
        public DateTime? EffectiveDate { get; set; }

        [DateFormat(CommonConstants.DateFormat)]
        public DateTime? ExpireDate { get; set; }

        [DateFormat(CommonConstants.DateTimeFormat)]
        public DateTime? LastSignOnDate { get; set; }

        public bool IsActive { get; set; }
    }
}