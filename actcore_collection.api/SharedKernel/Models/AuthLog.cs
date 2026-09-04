using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Models
{
    [Table("AuthLog")]
    public class AuthLog
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid TraceId { get; set; }

        public Guid? SessionId { get; set; }

        public Guid? UserId { get; set; }

        [MaxLength(100)]
        public string UserName { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        public DateTime? ExpiredTime { get; set; }

        [MaxLength(50)]
        public string IpAddress { get; set; }

        [MaxLength(250)]
        public string UserAgent { get; set; }

        [MaxLength(250)]
        public string Status { get; set; }

        [MaxLength(250)]
        public string FailReason { get; set; }

        [MaxLength(250)]
        public string Location { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
