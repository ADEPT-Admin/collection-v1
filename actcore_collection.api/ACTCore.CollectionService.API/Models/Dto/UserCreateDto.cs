using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class UserCreateDto
    {
        [Required]
        public string EmployeeId { get; set; }

        public string UserName { get; set; }

        [Required]
        public List<Guid> UserGroupIds { get; set; } = new List<Guid>();

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool ForceChangePassword { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpireDate { get; set; }
    }
}
