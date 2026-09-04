using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Models
{
    [Table("ApiLog")]
    public class ApiLog
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid TraceId { get; set; }

        public Guid? UserId { get; set; }

        [MaxLength(100)]
        public string UserName { get; set; }

        [MaxLength(100)]
        public string UserGroup { get; set; }

        [MaxLength(250)]
        public string RequestPath { get; set; }

        [MaxLength(50)]
        public string HttpMethod { get; set; }

        public string RequestBody { get; set; }

        public int ResponseCode { get; set; }
        public string ResponseBody { get; set; }

        public int DurationSeconds { get; set; }

        public string IPAddress { get; set; }

        public string UserAgent { get; set; }

        public string Status { get; set; }

        public string ErrorMessage { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
