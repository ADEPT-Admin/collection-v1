using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class UserBulkCreateDto
    {
        [Required]
        public Guid UserGroupId { get; set; }

        [Required]
        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpireDate { get; set; }
        
        public bool IsActive { get; set; }

        public bool ForceChangePassword { get; set; }

        [Required]
        public List<string> EmployeeIds { get; set; }

    }
}
