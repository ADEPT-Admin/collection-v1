namespace ACTCore.CollectionService.Application.Model
{
    public class ApiLogFilter
    {
        public string UserId { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Status { get; set; }
    }
}
