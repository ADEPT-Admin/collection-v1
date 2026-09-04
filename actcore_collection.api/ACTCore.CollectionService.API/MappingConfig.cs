using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.ListPagedDto;
using ACTCore.CollectionService.API.Models.Dto.poc;
using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Dto.Validation;
using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Domain.Entities.Contracts;
using ACTCore.CollectionService.Domain.Entities.Masters;
using ACTCore.CollectionService.Domain.Entities.Profiles;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Domain.ValueObjects;
using ACTCore.CollectionService.Domain.ViewModel;
using AutoMapper;
using SharedKernel.CommonConstants;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ACTCore.CollectionService.API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            // Request/Response DTO
            CreateMap<SysUserGroup, UserGroupResponseDto>().ReverseMap();
            CreateMap<SysUserGroup, SearchUserGroupResponseDto>().ReverseMap();
            CreateMap<SysUser, UserResponseDto>().ReverseMap();


            CreateMap<Prefix, PrefixResponseDto>().ReverseMap();
            CreateMap<Prefix, PrefixDto>().ReverseMap();

            // System and Securiy
            CreateMap<TokenRequestDto, Application.Dto.LoginResponseDto>().ReverseMap();

            CreateMap<SysEnum, EnumListResponseDto>().ReverseMap();

            CreateMap<SysUser, UserDto>().ReverseMap();
            CreateMap<SysUser, UserResponseDto>().ReverseMap();
            CreateMap<SysUser, UserCreateDto>()
                .ForMember(dest => dest.ForceChangePassword, opt => opt.MapFrom(src => src.IsNewUser))
                .ReverseMap();
            CreateMap<SysUser, UserUpdateDto>().ReverseMap();
            CreateMap<SysUser, SysPolicyValidatePwdRequestDto>().ReverseMap();

            CreateMap<SysUserGroup, UserGroupDto>().ReverseMap();
            CreateMap<SysUserGroup, UserGroupCreateDto>().ReverseMap();
            CreateMap<SysUserGroup, UserGroupUpdateDto>().ReverseMap();


            CreateMap<SysItem, SysItemDto>()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.ItemName)
                            ? null
                            : JsonSerializer.Deserialize<LanguageValue>(src.ItemName, jsonOptions)))
                .ReverseMap()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src =>
                        src.ItemName == null
                            ? null
                            : JsonSerializer.Serialize(src.ItemName, jsonOptions)));
            CreateMap<SysItem, SysItemCreateDto>()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.ItemName)
                            ? null
                            : JsonSerializer.Deserialize<LanguageValue>(src.ItemName, jsonOptions)))
                .ReverseMap()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src =>
                        src.ItemName == null
                            ? null
                            : JsonSerializer.Serialize(src.ItemName, jsonOptions)));
            CreateMap<SysItem, SysItemUpdateDto>()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.ItemName)
                            ? null
                            : JsonSerializer.Deserialize<LanguageValue>(src.ItemName, jsonOptions)))
                .ReverseMap()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src =>
                        src.ItemName == null
                            ? null
                            : JsonSerializer.Serialize(src.ItemName, jsonOptions)));
            CreateMap<SysItem, SysItemPermissionListingDto>()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.ItemName)
                            ? null
                            : JsonSerializer.Deserialize<LanguageValue>(src.ItemName, jsonOptions)))
                .ReverseMap()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src =>
                        src.ItemName == null
                            ? null
                            : JsonSerializer.Serialize(src.ItemName, jsonOptions)));
            CreateMap<SysItem, SysItemResponseDto>()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.ItemName)
                            ? null
                            : JsonSerializer.Deserialize<LanguageValue>(src.ItemName, jsonOptions)))
                .ReverseMap()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src =>
                        src.ItemName == null
                            ? null
                            : JsonSerializer.Serialize(src.ItemName, jsonOptions)));

            CreateMap<SysPolicy, SysPolicyResponseDto>().ReverseMap();
            CreateMap<SysPolicy, SysPolicyRequestDto>().ReverseMap();

            // Map SysItemAccessRight + SysItem (1:1) to SysItemPermissionListingResponseDto
            CreateMap<SysItemAccessRight, SysItemPermissionListingDto>()
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Item != null ? src.Item.ParentId : string.Empty))
                .ForPath(dest => dest.ItemName, opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.Item.ItemName)
                            ? null
                            : JsonSerializer.Deserialize<LanguageValue>(src.Item.ItemName, jsonOptions)))
                .ForMember(dest => dest.RouteName, opt => opt.MapFrom(src => src.Item != null ? src.Item.RouteName : string.Empty))
                .ForMember(dest => dest.ItemLevel, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemLevel : 0));
            CreateMap<SysItemPermissionListingDto, SysItemAccessRight>()
                .ForMember(dest => dest.Item, opt => opt.Ignore());

            // User for create/update in the UserGroupController only
            CreateMap<SysItemAccessRight, UserGroupMenuPermissionDto>()
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Item != null ? src.Item.ParentId : string.Empty))
                .ForPath(dest => dest.ItemName, opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.Item.ItemName)
                            ? null
                            : JsonSerializer.Deserialize<LanguageValue>(src.Item.ItemName, jsonOptions)))
                .ForMember(dest => dest.RouteName, opt => opt.MapFrom(src => src.Item != null ? src.Item.RouteName : string.Empty))
                .ForMember(dest => dest.ItemLevel, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemLevel : 0));
            // Do not recommend to use ReverseMap() with Navigation Entity
            CreateMap<UserGroupMenuPermissionDto,SysItemAccessRight>()
                .ForMember(dest => dest.Item, opt => opt.Ignore());

            CreateMap<SysItem, UserGroupMenuPermissionDto>()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.ItemName)
                            ? null
                            : JsonSerializer.Deserialize<LanguageValue>(src.ItemName, jsonOptions)))
                .ReverseMap()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src =>
                        src.ItemName == null
                            ? null
                            : JsonSerializer.Serialize(src.ItemName, jsonOptions)));

            CreateMap<EmployeeProfile, EmployeeProfileDto>()
                .ForMember(dest => dest.NameLanguage,
                           opt => opt.MapFrom(src => new LanguageValue
                           {
                               Th = $"{src.FirstName} {src.LastName}",
                               En = $"{src.FirstNameEn} {src.LastNameEn}"
                           }))
                .ReverseMap();
            CreateMap<EmployeeProfile, EmployeeResponseDto>().ReverseMap();
            CreateMap<EmployeeProfile, EmployeeResponseDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.EmployeeNameEn, opt => opt.MapFrom(src => src.FullNameEn))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : string.Empty))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position != null ? src.Position.PositionName : string.Empty))
                .ReverseMap();
            
            CreateMap<SysItemAccessRight, SysItemAccessRightDto>().ReverseMap();
            CreateMap<SysUserItemFavorite, SysUserItemFavoriteDto>().ReverseMap();

            CreateMap<SysParameter, SysParameterResponseDto>();
            CreateMap<SysParameter, SysParameterCreateDto>().ReverseMap();
            CreateMap<SysParameter, SysParameterUpdateDto>().ReverseMap();

            CreateMap<Language, LanguageResponseDto>().ReverseMap();
            CreateMap<Language, LanguageListsResponseDto>().ReverseMap();

            CreateMap<LanguageCreateDto, Language>()
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src =>
                        src.Value == null
                            ? null
                            : JsonSerializer.Serialize(src.Value, jsonOptions)))
                .ForMember(dest => dest.DefaultValue,
                    opt => opt.MapFrom(src =>
                        src.Value == null
                            ? null
                            : JsonSerializer.Serialize(src.Value, jsonOptions)))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Ordering, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.Value)
                            ? null
                            : JsonSerializer.Deserialize<LanguageValue>(src.Value, jsonOptions)))
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key));

            CreateMap<LanguageJsonDto, Language>()
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src =>
                        src.Value == null
                            ? null
                            : JsonSerializer.Serialize(src.Value, jsonOptions)))
                .ForMember(dest => dest.DefaultValue,
                    opt => opt.MapFrom(src =>
                        src.DefaultValue == null
                            ? null
                            : JsonSerializer.Serialize(src.DefaultValue, jsonOptions)))
                .ReverseMap()
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.Value)
                            ? null
                            : JsonSerializer.Deserialize<LanguageValue>(src.Value, jsonOptions)))
                .ForMember(dest => dest.DefaultValue,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.DefaultValue)
                            ? null
                            : JsonSerializer.Deserialize<LanguageValue>(src.DefaultValue, jsonOptions)));

            CreateMap<Team, TeamDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.NameLanguage, opt => opt.MapFrom(src => new LanguageValue { Th = src.DepartmentName, En = src.DepartmentNameEn }));
            CreateMap<Position, PositionDto>()
                .ForMember(dest => dest.NameLanguage, opt => opt.MapFrom(src => new LanguageValue { Th = src.PositionName, En = src.PositionNameEn })); ;

            CreateMap<Worklist, WorklistHistory>().ReverseMap();
            CreateMap<Worklist, WorklistDetailResponseDto>()
                .ForMember(dest => dest.ContractNo, opt => opt.MapFrom(src => src.ContractNo))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Contract != null ? src.Contract.ContractPersons.FirstOrDefault(y => y.PersonType == "B") != null ? src.Contract.ContractPersons.FirstOrDefault(y => y.PersonType == "B").FullName : string.Empty : string.Empty))
                .ForMember(dest => dest.NationalID, opt => opt.MapFrom(src => src.Contract != null ? src.Contract.ContractPersons.FirstOrDefault(y => y.PersonType == "B") != null ? src.Contract.ContractPersons.FirstOrDefault(y => y.PersonType == "B").IdCard : string.Empty : string.Empty))
                .ForMember(dest => dest.ContractStatus, opt => opt.MapFrom(src => src.Contract != null ? src.Contract.ContractStatus : string.Empty))
                .ForMember(dest => dest.PrimaryPhoneNo, opt => opt.MapFrom(src => src.Contract != null 
                    ? src.Contract.ContractPersons.FirstOrDefault(y => y.PersonType == "B") != null 
                    ? src.Contract.ContractPersons.FirstOrDefault(y => y.PersonType == "B").ContractPhones.FirstOrDefault() != null 
                    ? src.Contract.ContractPersons.FirstOrDefault(y => y.PersonType == "B").ContractPhones.FirstOrDefault().PhoneNo 
                    : string.Empty : string.Empty : string.Empty
                    ))
                .ForMember(dest => dest.DayPastDue, opt => opt.MapFrom(src => src.Contract != null ? src.Contract.DayPastDue : null))
                .ForMember(dest => dest.Bucket, opt => opt.MapFrom(src => src.Contract != null ? src.Contract.Bucket : null))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.Contract != null ? src.Contract.DueDate : null))
                .ForMember(dest => dest.LastPaymentDate, opt => opt.MapFrom(src => src.Contract != null ? src.Contract.LastPaymentDate : null))
                .ForMember(dest => dest.OverdueAmount, opt => opt.MapFrom(src => src.Contract != null ? src.Contract.OverdueAmount : null))
                .ForMember(dest => dest.OutstandingBalance, opt => opt.MapFrom(src => src.Contract != null ? src.Contract.OutstandingBalance : null))
                .ReverseMap();
            
            CreateMap<CollectionNote, CollectionNoteCreateDto>().ReverseMap();
            CreateMap<CollectionNote, CollectionNoteResponseDto>().ReverseMap();
            CreateMap<CollectionNote, ContractNoteResponseDto>()
                .ForMember(dest => dest.FollowupAction, opt => opt.MapFrom(src => src.FollowupActionId.ToString()))
                .ForMember(dest => dest.FollowupResult, opt => opt.MapFrom(src => src.FollowupResultId.ToString()))
                .ReverseMap();

            CreateMap<ColNoteAction, ColNoteActionResponseDto>().ReverseMap();
            CreateMap<ColNoteResult, ColNoteResultResponseDto>().ReverseMap();

            CreateMap<ContractPerson, PersonResponseDto>()
                .ForMember(dest => dest.PersonType, opt => opt.MapFrom(src => src.PersonType == "B" ? PersonType.BORROWER : PersonType.GUARANTOR))
                .ReverseMap();

            CreateMap<ContractOverdue, OverdueDetailResponseDto>().ReverseMap();
            CreateMap<ContractOverdue, ContractOverdueReponseDto>()
                .ForMember(dest => dest.No, opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => new LanguageValue { Th = src.OverdueType, En = src.OverdueType }))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.OverdueAmount))
                .ReverseMap();
            CreateMap<Contract, ContractDetailResponseDto>()
                .ForMember(dest => dest.AssetGroup, opt => opt.MapFrom(src => src.ContractAssets != null && src.ContractAssets.Any() ? src.ContractAssets.FirstOrDefault().AssetGroup : null))
                .ForMember(dest => dest.AssetType, opt => opt.MapFrom(src => src.ContractAssets != null && src.ContractAssets.Any() ? src.ContractAssets.FirstOrDefault().AssetType : null))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ContractAssets != null && src.ContractAssets.Any() ? src.ContractAssets.FirstOrDefault().AssetDescription : null))
                .ForMember(dest => dest.AssetPrice, opt => opt.MapFrom(src => src.ContractAssets != null && src.ContractAssets.Any() ? src.ContractAssets.FirstOrDefault().AssetPrice : null))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.ContractAssetVehicles != null && src.ContractAssetVehicles.Any() ? src.ContractAssetVehicles.FirstOrDefault().Brand : null))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.ContractAssetVehicles != null && src.ContractAssetVehicles.Any() ? src.ContractAssetVehicles.FirstOrDefault().Model : null))
                .ForMember(dest => dest.Series, opt => opt.MapFrom(src => src.ContractAssetVehicles != null && src.ContractAssetVehicles.Any() ? src.ContractAssetVehicles.FirstOrDefault().Series : null))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.ContractAssetVehicles != null && src.ContractAssetVehicles.Any() ? src.ContractAssetVehicles.FirstOrDefault().Year : null))
                .ForMember(dest => dest.EngineNo, opt => opt.MapFrom(src => src.ContractAssetVehicles != null && src.ContractAssetVehicles.Any() ? src.ContractAssetVehicles.FirstOrDefault().EngineNo : null))
                .ForMember(dest => dest.ChassisNo, opt => opt.MapFrom(src => src.ContractAssetVehicles != null && src.ContractAssetVehicles.Any() ? src.ContractAssetVehicles.FirstOrDefault().ChassisNo : null))
                .ForMember(dest => dest.PlateNo, opt => opt.MapFrom(src => src.ContractAssetVehicles != null && src.ContractAssetVehicles.Any() ? src.ContractAssetVehicles.FirstOrDefault().PlateNo : null))
                .ForMember(dest => dest.RegisterProvince, opt => opt.MapFrom(src => src.ContractAssetVehicles != null && src.ContractAssetVehicles.Any() ? src.ContractAssetVehicles.FirstOrDefault().RegisterProvince : null))
                .ReverseMap();

            CreateMap<ContractPayment, PaymentDetailResponseDto>().ReverseMap();
            CreateMap<ContractAddress, ContractAddressResponseDto>()
                .ForMember(dest => dest.PersonType, opt => opt.MapFrom(src => src.ContractPerson != null ? src.ContractPerson.PersonType == "B" ? PersonType.BORROWER : PersonType.GUARANTOR : string.Empty))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.Province) 
                    ? src.Address
                    : string.Join(" ", new[] 
                    { 
                        src.Address,
                        string.IsNullOrEmpty(src.SubDistrict) ? null : (src.Province == "กรุงเทพมหานคร" ? $"แขวง {src.SubDistrict}" : $"ตำบล {src.SubDistrict}"),
                        string.IsNullOrEmpty(src.District) ? null : (src.Province == "กรุงเทพมหานคร" ? $"เขต {src.District}" : $"อำเภอ {src.District}"),
                        string.IsNullOrEmpty(src.Province) ? null : $"จังหวัด {src.Province}",
                        src.ZipCode
                    }.Where(s => !string.IsNullOrEmpty(s)))))
                .ReverseMap();
            CreateMap<ContractPhone, ContractPhoneResponseDto>()
                .ForMember(dest => dest.PersonType, opt => opt.MapFrom(src => src.ContractPerson != null ? src.ContractPerson.PersonType == "B" ? PersonType.BORROWER : PersonType.GUARANTOR : string.Empty))
                .ReverseMap();
            
            CreateMap<CollectorProfile, CollectorCreateDto>().ReverseMap();
            CreateMap<CollectorProfile, CollectorUpdateDto>().ReverseMap();
            CreateMap<CollectorProfile, CollectorResponseDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.User != null &&src.User.Employee != null ? src.User.Employee.EmployeeId : string.Empty))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.User != null &&src.User.Employee != null ? src.User.Employee.FullName : string.Empty))
                .ReverseMap();
            CreateMap<CollectorProfile, CollectorGetByIdResponseDto>()
                .ForMember(dest => dest.CollectorGuid, opt => opt.MapFrom(src => src.CollectorId))
                .ForMember(dest => dest.CollectorId, opt => opt.MapFrom(src => src.User != null && src.User.Employee != null ? src.User.Employee.EmployeeId : string.Empty))
                .ForMember(dest => dest.CollectorName, opt => opt.MapFrom(src => src.User != null && src.User.Employee != null ? src.User.Employee.FullName : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null && src.User.Employee != null ? src.User.Employee.Email : string.Empty))
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.User != null && src.User.Employee != null ? src.User.Employee.PhoneNo : string.Empty))
                .ReverseMap();
            CreateMap<CollectorProfile, CollectorAssignTeamListPagedResponseDto>()
                .ForMember(dest => dest.CollectorGuid, opt => opt.MapFrom(src => src.CollectorId))
                .ForMember(dest => dest.CollectorId, opt => opt.MapFrom(src => src.User != null && src.User.Employee != null ? src.User.Employee.EmployeeId : string.Empty))
                .ForMember(dest => dest.CollectorName, opt => opt.MapFrom(src => src.User != null && src.User.Employee != null ? src.User.Employee.FullName : string.Empty))
                .ForMember(dest => dest.ColRoleName, opt => opt.MapFrom(src => src.ColRole != null ? src.ColRole.ColRoleName : string.Empty))
                .ReverseMap();

            CreateMap<ColTeam, ColTeamDto>().ReverseMap();
            CreateMap<ColTeam, ColTeamCreateDto>().ReverseMap();
            CreateMap<ColTeam, ColTeamUpdateDto>().ReverseMap();
            CreateMap<ColTeam, ColTeamResponseDto>().ReverseMap();
            CreateMap<ColTeam, ColTeamListResponseDto>().ReverseMap();
            
            CreateMap<ColTeamAssignment, TeamAssignmentResponseDto>()
                .ForMember(dest => dest.ColTeamName, opt => opt.MapFrom(src => src.ColTeam != null ? src.ColTeam.ColTeamName : string.Empty))
                .ForMember(dest => dest.ColTeamCode, opt => opt.MapFrom(src => src.ColTeam != null ? src.ColTeam.ColTeamCode : string.Empty))
                .ForMember(dest => dest.ColRoleName, opt => opt.MapFrom(src => src.Collector.ColRole != null ? src.Collector.ColRole.ColRoleName : string.Empty))
                .ForMember(dest => dest.CollectorEmpId, opt => opt.MapFrom(src => src.Collector.User.Employee != null ? src.Collector.User.Employee.EmployeeId : string.Empty))
                .ForMember(dest => dest.CollectorEmpName, opt => opt.MapFrom(src => src.Collector.User.Employee != null ? src.Collector.User.Employee.FullName : string.Empty))
                .ForMember(dest => dest.TeamCapacity, opt => opt.MapFrom(src => src.ColTeam != null ? (int?)src.ColTeam.Capacity : null))
                .ReverseMap();
            CreateMap<ColTeamAssignment, TeamAssignmentCreateDto>().ReverseMap();
            CreateMap<ColTeamAssignment, TeamAssignmentUpdateDto>().ReverseMap();
            CreateMap<ColTeamAssignment, TeamAssignmentFlagResponseDto>()
                .ForMember(dest => dest.ColTeamName, opt => opt.MapFrom(src => src.ColTeam != null ? src.ColTeam.ColTeamName : string.Empty))
                .ForMember(dest => dest.CollectorEmpName, opt => opt.MapFrom(src => new LanguageValue
                {
                    Th = src.Collector.User.Employee != null ? src.Collector.User.Employee.FullName : string.Empty,
                    En = src.Collector.User.Employee != null ? src.Collector.User.Employee.FullNameEn : string.Empty
                }))
                .ReverseMap();

            CreateMap<ColRole, ColRoleListResponseDto>().ReverseMap();
            CreateMap<ColRole, ColRoleResponseDto>().ReverseMap();
            CreateMap<ColRole, ColRoleRequestDto>().ReverseMap();
            CreateMap<ColRole, ColRoleUpdateDto>().ReverseMap();
            CreateMap<ColRole, ColRoleDeleteDto>().ReverseMap();
            CreateMap<ColRole, ColRoleDto>().ReverseMap();

            CreateMap<Province, ProvinceDto>().ReverseMap();
            CreateMap<District, DistrictDto>().ReverseMap();
            CreateMap<SubDistrict, SubDistrictDto>().ReverseMap();

        #region Dropdown
            CreateMap<DropdownOptionDto, ContractPhone>()
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.PhoneType, opt => opt.MapFrom(src => src.Label))
                .ReverseMap();
            CreateMap<DropdownOptionDto, ContractPerson>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.PersonType, opt => opt.MapFrom(src => src.Label))
                .ReverseMap();
        #endregion

        #region Paged
            CreateMap<SysUserGroup, UserGroupListPagedResponseDto>().ReverseMap();
            CreateMap<EmployeeProfile, EmployeeListPagedResponseDto>().ReverseMap();
            CreateMap<EmployeeProfile, EmployeeListPagedResponseDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.EmployeeNameEn, opt => opt.MapFrom(src => src.FullNameEn))
                .ReverseMap();
            CreateMap<EmployeeProfile, EmployeeDialogListPagedResponseDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : string.Empty))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position != null ? src.Position.PositionName : string.Empty))
                .ReverseMap();
            CreateMap<WorklistListringDto, WorklistListPagedResponseDto>().ReverseMap();
            CreateMap<WorklistListringDto, WorklistListPagedSupervisorResponseDto>().ReverseMap();
            CreateMap<vw_ReassignWorklist, ReassignWorklistListPagedResponseDto>().ReverseMap();
            CreateMap<vw_UnassignWorklist, UnassignWorklistListPagedResponseDto>().ReverseMap();
            CreateMap<ColTeam, ColTeamListPagedResponseDto>().ReverseMap();
            CreateMap<ColTeamAssignment, TeamAssignmentListPagedResponseDto>()
                .ForMember(dest => dest.ColTeamName, opt => opt.MapFrom(src => src.ColTeam != null ? src.ColTeam.ColTeamName : string.Empty))
                .ForMember(dest => dest.CollectorEmpId, opt => opt.MapFrom(src => src.Collector.User.Employee != null ? src.Collector.User.Employee.EmployeeId : string.Empty))
                .ForMember(dest => dest.CollectorEmpName, opt => opt.MapFrom(src => src.Collector.User.Employee != null ? src.Collector.User.Employee.FullName : string.Empty))
                .ReverseMap();
            CreateMap<SysUser, UserListPagedResponseDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.EmployeeId : string.Empty))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : string.Empty))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ? IconType.CHECKGREEN : IconType.CHECKRED))
                .ForMember(dest => dest.UserGroup, opt => opt.MapFrom(
                    src => src.UserGroupAccesses != null 
                    && src.UserGroupAccesses.Any() 
                    ? string.Join(", ", src.UserGroupAccesses.Select(uga => uga.UserGroup.UserGroupName)) 
                    : string.Empty))
                .ReverseMap();
            CreateMap<SysUser, UserDialogListPagedResponseDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : string.Empty))
                .ForMember(dest => dest.UserGroup, opt => opt.MapFrom(
                    src => src.UserGroupAccesses != null
                    && src.UserGroupAccesses.Any()
                    ? string.Join(", ", src.UserGroupAccesses.Select(uga => uga.UserGroup.UserGroupName))
                    : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Email : string.Empty))
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.PhoneNo : string.Empty))
                // .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ? IconType.CHECKGREEN : IconType.CHECKRED))
                .ReverseMap();
            CreateMap<ColTeamAssignment, ColTeamListAssignByTeamPagedResponseDto>()
                .ForMember(dest => dest.CollectorId, opt => opt.MapFrom(src => 
                    src.Collector != null  && 
                    src.Collector.User != null && 
                    src.Collector.User.Employee != null
                        ? src.Collector.User.Employee.EmployeeId 
                        : string.Empty))
                .ForMember(dest => dest.CollectorName, opt => opt.MapFrom(src => (
                    src.Collector != null  && 
                    src.Collector.User != null && 
                    src.Collector.User.Employee != null) 
                        ? src.Collector.User.Employee.FullName 
                        : string.Empty))
                .ForMember(dest => dest.ColRoleName, opt => opt.MapFrom(src => 
                    src.Collector != null &&
                    src.Collector.ColRole != null 
                        ? src.Collector.ColRole.ColRoleName 
                        : string.Empty))
                .ForMember(dest => dest.Supervisor, opt => opt.MapFrom(src => src.IsSupervisor ? IconType.CHECKGREEN : string.Empty))
                .ForMember(dest => dest.CollectorCapacity, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ? IconType.CHECKGREEN : IconType.CHECKRED));
            CreateMap<ColTeamAssignment, TeamAssignmentByCollectorListPagedResponseDto>()
                .ForMember(dest => dest.ColTeamName, opt => opt.MapFrom(src => src.ColTeam != null ? src.ColTeam.ColTeamName : string.Empty))
                .ForMember(dest => dest.Supervisor, opt => opt.MapFrom(src => src.IsSupervisor ? IconType.CHECKGREEN : string.Empty))
                .ForMember(dest => dest.CollectorCapacity, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ? IconType.CHECKGREEN : IconType.CHECKRED));
            CreateMap<CollectorProfile, CollectorListPagedResponseDto>()
                .ForMember(dest => dest.CollectorGuid, opt => opt.MapFrom(src => src.CollectorId))
                .ForMember(dest => dest.CollectorId, opt => opt.MapFrom(src => src.User != null && src.User.Employee != null ? src.User.Employee.EmployeeId : string.Empty))
                .ForMember(dest => dest.CollectorName, opt => opt.MapFrom(src => src.User != null && src.User.Employee != null ? src.User.Employee.FullName : string.Empty))
                .ForMember(dest => dest.ColRoleName, opt => opt.MapFrom(src => src.ColRole != null ? src.ColRole.ColRoleName : "-"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null && src.User.Employee != null ? src.User.Employee.Email : string.Empty))
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.User != null && src.User.Employee != null ? src.User.Employee.PhoneNo : string.Empty))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ? IconType.CHECKGREEN : IconType.CHECKRED))
                .ReverseMap();    

            #endregion

            #region  Validation
                // Team Assignment
                CreateMap<TeamAssignmentCreateDto, TeamAssignmentValidationCreateDto>().ReverseMap();
                CreateMap<TeamAssignmentUpdateDto, TeamAssignmentValidationUpdateDto>().ReverseMap();
                CreateMap<CollectorAssignTeamCreateDto, CollectorAssignTeamValidationCreateDto>().ReverseMap();
            #endregion

        }
    }
}
