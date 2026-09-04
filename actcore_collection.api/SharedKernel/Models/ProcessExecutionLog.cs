using SharedKernel.CommonConstants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace SharedKernel.Models
{
    [Table("ProcessExecutionLog")]
    public class ProcessExecutionLog
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime LogDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// eg. ImportCollector, ImportPayment
        /// </summary>
        [Required, MaxLength(50)]
        public string JobName { get; set; }

        /// <summary>
        /// eg. ExecutionJobType Constant for IMPORT, SYNC, ASSIGNMENT
        /// </summary>
        [Required, MaxLength(50)]
        public string JobType { get; set; }

        /// <summary>
        /// args[0] target table name
        /// </summary>
        [MaxLength(50)]
        public string TargetTable { get; set; }

        /// <summary>
        /// Path ของ Directory ที่มี CSV (มาจาก args[1])
        /// Path ต้นทางของข้อมูล
        /// </summary>
        [MaxLength(500)]
        public string SourcePath { get; set; }

        /// <summary>
        /// eg. CSV, API
        /// </summary>
        [Required, MaxLength(100)]
        public string SourceSystem { get; set; }

        /// <summary>
        /// รหัสประมวลผล TableName_YYYYMMDDHHMMSS
        /// </summary>
        [MaxLength(50)]
        public string BatchNo { get; set; }

        [Required]
        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime? EndTime { get; set; }

        public int? TotalFiles { get; set; }

        public int? TotalRecords { get; set; }

        public int? SuccessCount { get; set; }

        public int? FailedCount { get; set; }

        /// <summary>
        /// Using ProcessExecutionStatus Constant for Running / Success / Failed / Partial
        /// </summary>
        [Required, MaxLength(50)]
        public string Status { get; set; } = ExecutionStatus.RUNNING;

        [MaxLength(100)]
        public string ServerName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public ICollection<ProcessExecutionLogDetail> Details { get; set; } = new List<ProcessExecutionLogDetail>();

    }
}
