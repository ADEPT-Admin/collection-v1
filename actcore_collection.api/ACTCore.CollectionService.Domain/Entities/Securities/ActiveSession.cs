using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities
{
    [Table("ActiveSession")]
    public class ActiveSession
    {
        [Key]
        public Guid SessionId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual SysUser User { get; set; }

        [MaxLength(50)]
        public string IpAddress { get; set; }

        [MaxLength(250)]
        public string UserAgent { get; set; }

        [MaxLength(250)]
        public string DeviceInfo { get; set; }

        [MaxLength(250)]
        public string Location { get; set; }

        [Required]
        public DateTime LoginTime { get; set; }

        public DateTime LastActivityTime { get; set; }

        public DateTime SessionExpiryTime { get; set; }

        [Required]
        public string RefreshTokenHash { get; set; }

        public DateTime? LastRefreshedAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        [MaxLength(250)]
        public string RevokedReason { get; set; }

        public bool ReuseDetected { get; set; } = false;

        public bool IsActive => !IsRevoked && DateTime.Now <= SessionExpiryTime;

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
