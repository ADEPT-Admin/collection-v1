namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class ColWorkGroupDto
    {
        public int WorkGroupId { get; set; }
        //public string WorkGroupCode { get; set; }
        public string WorkGroupName { get; set; }
        public bool IsActive { get; set; }
    }

    public class ColWorkGroupListingResponseDto
    {
        public int WorkGroupId { get; set; }
        //public string WorkGroupCode { get; set; }
        public string WorkGroupName { get; set; }
        public bool IsActive { get; set; }
    }

    public class ColWorkGroupCreateDto
    {
        //public string WorkGroupCode { get; set; }
        public string WorkGroupName { get; set; }
        public bool IsActive { get; set; }
    }

    public class ColWorkGroupUpdateDto
    {
        public int WorkGroupId { get; set; }
        //public string WorkGroupCode { get; set; }
        public string WorkGroupName { get; set; }
        public bool IsActive { get; set; }
    }

    public class ColWorkGroupIdRequestDto
    {
        public int WorkGroupId { get; set; }
    }
}