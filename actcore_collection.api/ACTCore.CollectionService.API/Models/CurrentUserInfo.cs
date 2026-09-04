namespace ACTCore.CollectionService.API.Models
{
    public class CurrentUserInfo
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Roles { get; set; }
        public string TokenId { get; set; }
        public Guid SessionId { get; set; }
        public string EmployeeId { get; set; }
    }
}
