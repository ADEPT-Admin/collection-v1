using ACTCore.CollectionService.Domain.Entities.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTCore.CollectionService.Application.Dto
{
    public class ColAreaDto
    {
        public int AreaId { get; set; }

        public int? ParentId { get; set; }

        public string AreaCode { get; set; }

        public string AreaName { get; set; }

        public string AreaLevelId { get; set; }

        public ProvinceDto AreaLevel { get; set; }

        public string ProvinceId { get; set; }

        public ProvinceDto Province { get; set; }

        public string DistrictId { get; set; }

        public DistrictDto District { get; set; }

        public string SubDistrictId { get; set; }

        public SubDistrictDto SubDistrict { get; set; }

        public bool IsActive { get; set; }
    }
}
