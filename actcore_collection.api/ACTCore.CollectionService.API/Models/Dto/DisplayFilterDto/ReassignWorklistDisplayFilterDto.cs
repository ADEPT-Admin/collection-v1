using ACTCore.CollectionService.API.Models.Attributes;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class ReassignWorklistDisplayFilterDto
    {
        [ColumnWidth(120)]
        public string ContractNo { get; set; }

        public bool IsMyTeam { get; set; }

        [ColumnWidth(200)]
        public string CustomerName { get; set; }

        [ColumnWidth(120)]
        public string AssetType { get; set; }

        [ColumnWidth(120)]
        public string ContractStatus { get; set; }

        [ColumnWidth(120)]
        public string FollowupStatus { get; set; }

        [ColumnWidth(200)]
        public string ReassignBy { get; set; }

        [ColumnWidth(200)]
        public string TeamFrom { get; set; }

        [ColumnWidth(200)]
        public string ReassignTo { get; set; }

        [ColumnWidth(200)]
        public string TeamTo { get; set; }

        public DateTime RequestedDate { get; set; }

        [ColumnWidth(200)]
        public string Reason { get; set; }

    }
}
