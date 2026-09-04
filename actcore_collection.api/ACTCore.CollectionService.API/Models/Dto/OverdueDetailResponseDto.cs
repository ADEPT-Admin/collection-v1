namespace ACTCore.CollectionService.API.Models.Dto
{
    public class OverdueDetailResponseDto
    {
        public string OverdueType { get; set; }

        public string OverdueName { get; set; }

        public int OverdueTerm { get; set; }

        public decimal OverdueAmount { get; set; }

        public int? Number { get; set; }
    }
}
