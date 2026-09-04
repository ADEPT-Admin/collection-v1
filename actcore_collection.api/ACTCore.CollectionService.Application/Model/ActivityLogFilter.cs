namespace ACTCore.CollectionService.Application.Model
{
    public class ActivityLogFilter
    {
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Status { get; set; }
    }
}
