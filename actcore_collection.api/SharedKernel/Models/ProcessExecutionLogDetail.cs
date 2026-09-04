using SharedKernel.CommonConstants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Models
{
    [Table("ProcessExecutionLogDetail")]
    public class ProcessExecutionLogDetail
    {
        [Key]
        public long DetailId { get; set; }

        [Required]
        public long LogId { get; set; }

        [ForeignKey(nameof(LogId))]
        public ProcessExecutionLog ProcessExecutionLog { get; set; }

        /// <summary>
        /// ชื่อไฟล์ (เช่น data_20251121.csv)
        /// </summary>
        [MaxLength(250)]
        public string FileName { get; set; }

        /// <summary>
        /// เวลาเริ่มประมวลผลไฟล์นี้
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// เวลาเสร็จสำหรับไฟล์นี้
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// จำนวน record ทั้งหมดในไฟล์นี้
        /// </summary>
        public int? TotalRecords { get; set; }

        /// <summary>
        /// จำนวน record ที่สำเร็จในไฟล์นี้
        /// </summary>
        public int? SuccessCount { get; set; }

        /// <summary>
        /// จำนวน record ที่ fail ในไฟล์นี้
        /// </summary>
        public int? FailedCount { get; set; }

        /// <summary>
        /// สถานะของไฟล์นี้: Running / Success / Failed
        /// ใช้ ExecutionStatus Constant เหมือนตัวหลัก
        /// </summary>
        [Required, MaxLength(20)]
        public string Status { get; set; } = ExecutionStatus.RUNNING;

        public string ErrorMessage { get; set; }

        public string ErrorStackTrace { get; set; }

    }
}
