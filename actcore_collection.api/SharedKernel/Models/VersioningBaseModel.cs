using System.ComponentModel.DataAnnotations;
namespace SharedKernel.Models
{
    public class VersionBaseModel
    {
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [MaxLength(100)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }
}
