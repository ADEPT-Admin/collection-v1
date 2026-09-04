using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTCore.CollectionService.Application.Dto
{
    public class SubDistrictDto
    {
        public string SubDistrictId { get; set; }

        public string DistrictId { get; set; }

        public string SubDistrictName { get; set; }

        public string SubDistrictNameEn { get; set; }

        public float? Latitude { get; set; }

        public float? Longtitude { get; set; }

        public string ZipCode { get; set; }
    }
}
