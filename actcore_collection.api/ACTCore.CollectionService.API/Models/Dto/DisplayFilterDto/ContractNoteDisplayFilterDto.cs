using ACTCore.CollectionService.API.Models.Attributes;
using SharedKernel.CommonConstants;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayFilterDto
{
    public class ContractNoteDisplayFilterDto
    {
        [DateFormat(CommonConstants.DateTimeFormat)]
        public DateTime? FollowupDate { get; set; }

        [ColumnWidth(120)]
        public string FollowupAction { get; set; }

        [ColumnWidth(120)]
        public string FollowupResult { get; set; }

        [ColumnWidth(150)]
        public string ContractPersonFullName { get; set; }

        [ColumnWidth(120)]
        public string ContractPhoneNumber { get; set; }

        [DateFormat(CommonConstants.DateTimeFormat)]
        public DateTime? NextFollowupDate { get; set; }

        [DateFormat(CommonConstants.DateTimeFormat)]
        public DateTime? PromiseToPayDate { get; set; }

        [ColumnWidth(120)]
        public decimal? PromiseToPayAmount { get; set; }

        [ColumnWidth(150)]
        public string CollectionRemark { get; set; }

        [ColumnWidth(150)]
        public string UpdatedBy { get; set; }
    }
}
