using ACTCore.CollectionService.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace ACTCore.CollectionService.API.Models
{
    public class PaginationData<T>: BasePaginationData
    {
        public IEnumerable<T> Datatables { get; set; }
        
    }

    public abstract class BasePaginationData
    {
        public IEnumerable<DisplayColumnPropertyValue> DisplayColumns { get; set; } = null;

        [JsonPropertyOrder(10)]
        public int TotalRecords { get; set; }

        [JsonPropertyOrder(11)]
        public int TotalPages { get; set; }

        [JsonPropertyOrder(12)]
        public int PageNumber { get; set; }

        [JsonPropertyOrder(13)]
        public int PageSize { get; set; }
    }
}
