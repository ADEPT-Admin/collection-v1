using ACTCore.CollectionService.API.Models.Dto.poc;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class UserGroupUpdateDto
    {
        public UserGroupResponseDto UserGroup { get; set; } = new();
        public List<UserGroupMenuPermissionDto> MenuItems { get; set; } = [];
    }
}
