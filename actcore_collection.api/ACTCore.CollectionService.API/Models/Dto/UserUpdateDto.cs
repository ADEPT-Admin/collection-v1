using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class UserUpdateDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        public List<Guid> UserGroupIds { get; set; } = new List<Guid>();
        public bool IsActive { get; set; }
        public bool IsLockUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsNewUser { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
