using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Models
{
    [Table("ActivityLog")]
    public class ActivityLog
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid TraceId { get; set; }

        public Guid UserId { get; set; }

        [MaxLength(250)]
        public string UserName { get; set; }

        [MaxLength(250)]
        public string UserGroup { get; set; }

        [MaxLength(50)]
        public string Action { get; set; } // CREATE, UPDATE, DELETE, LOGIN, LOGOUT, ETC

        [MaxLength(250)]
        public string EntityName { get; set; }

        [MaxLength(50)]
        public string EntityId { get; set; }

        public string Description { get; set; }

        public string Status { get; set; } // SUCESS, FAIL

        public string ErrorMessage { get; set; }

        public string IPAddress { get; set; }

        public string UserAgent { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public ICollection<ActivityLogDetail> Details { get; set; } = new List<ActivityLogDetail>();
    }
}
