using ACTCore.CollectionService.API.Models.Dto.poc;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class UserGroupPermissionResponseDto
    {
        public UserGroupResponseDto UserGroup { get; set; } = new();
        public List<UserGroupMenuPermissionDto> MenuItems { get; set; } = [];
    }
}