using ACTCore.CollectionService.API.Models.Attributes;
using SharedKernel.CommonConstants;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class CollectorAssignByTeamDisplayFilterDto
    {
        public String CollectorId { get; set; }

        public string CollectorName { get; set; }

        public string ColRoleName { get; set; }

        public bool Supervisor { get; set; }

        public int CollectorCapacity { get; set; }
        
        [DateFormat(CommonConstants.DateFormat)]        
        public DateTime EffectiveDate { get; set; }

        [DateFormat(CommonConstants.DateFormat)] 
        public DateTime ExpireDate { get; set; }

        public bool IsActive { get; set; }

    }
}