using ACTCore.CollectionService.API.Models;
using AutoMapper;
using SharedKernel.Models;
using SharedKernel.Templates;

namespace ACTCore.CollectionService.API.Utilities
{
    public static class PaginationDataHelper
    {
        public static PaginationData<TResponseDto> BuildPaginationData<TDisplayFilterDto, TResponseDto, TSource>(
            PagedResult<TSource> sources,
            int pageNumber,
            int pageSize,
            EnumValue[] enumValues,
            IMapper mapper)
        {
            var displayProperties = Utilities.DataTableHelper
                .GetDisplayColumnAndDisplayProperties<TDisplayFilterDto>(enumValues);

            return new PaginationData<TResponseDto>
            {
                Datatables = mapper.Map<List<TResponseDto>>(sources.Items),
                DisplayColumns = displayProperties,
                TotalRecords = sources.TotalCount,
                TotalPages = (int)Math.Ceiling((double)sources.TotalCount / pageSize),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
