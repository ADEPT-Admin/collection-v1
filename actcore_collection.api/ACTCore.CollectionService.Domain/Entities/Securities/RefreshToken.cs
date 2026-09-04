using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SessionId { get; set; }

        [ForeignKey(nameof(SessionId))]
        public virtual ActiveSession ActiveSession { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string RefreshTokenHash { get; set; }

        [MaxLength(100)]
        public string JwtTokenId { get; set; }

        public DateTime IssuedAt { get; set; } = DateTime.Now;

        public DateTime? UsedAt { get; set; }

        public bool IsUsed { get; set; } = false;

        public bool IsValid { get; set; } = true;

        public DateTime ExpireAt { get; set; }
    }
}
