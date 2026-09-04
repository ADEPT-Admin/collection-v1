using ACTCore.CollectionService.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Application.Dto
{
    public class DistrictDto
    {
        public string DistrictId { get; set; }

        public string ProvinceId { get; set; }

        public string DistrictName { get; set; }

        public string DistrictNameEn { get; set; }

    }
}
