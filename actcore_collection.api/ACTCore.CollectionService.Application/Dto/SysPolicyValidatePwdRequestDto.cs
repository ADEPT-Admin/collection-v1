using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.Application.Dto
{ 
    public class SysPolicyValidatePwdRequestDto
    {
        [Required]
        public string NewPwd { get; set; }
    }
}
