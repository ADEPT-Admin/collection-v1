using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTCore.CollectionService.Application.Dto
{
    public class ProvinceDto
    {
        public string ProvinceId { get; set; }

        public string ProvinceName { get; set; }

        public string ProvinceAbbr { get; set; }

        public string ProvinceNameEn { get; set; }

        public string ProvinceAbbrEn { get; set; }

        public string RegionCode { get; set; }
    }
}
