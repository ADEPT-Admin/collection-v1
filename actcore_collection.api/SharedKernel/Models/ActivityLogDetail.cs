using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Models
{
    [Table("ActivityLogDetail")]
    public class ActivityLogDetail
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long ActivityLogId { get; set; }

        [ForeignKey(nameof(ActivityLogId))]
        public ActivityLog ActivityLog { get; set; }

        [Required, MaxLength(100)]
        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}
