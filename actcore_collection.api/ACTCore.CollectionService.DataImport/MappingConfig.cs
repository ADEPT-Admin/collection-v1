using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Domain.Entities.Contracts;
using ACTCore.CollectionService.Domain.Entities.Masters;
using ACTCore.CollectionService.Domain.Entities.Profiles;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Domain.ValueObjects;
using ACTCore.CollectionService.Domain.ViewModel;
using AutoMapper;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ACTCore.CollectionService.DataImport
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            CreateMap<Prefix, PrefixDto>().ReverseMap();


            CreateMap<SysUser, UserDto>().ReverseMap();
            CreateMap<SysUser, SysPolicyValidatePwdRequestDto>().ReverseMap();

            CreateMap<SysUserGroup, UserGroupDto>().ReverseMap();

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
            
            CreateMap<EmployeeProfile, EmployeeProfileDto>()
                .ForMember(dest => dest.NameLanguage,
                           opt => opt.MapFrom(src => new LanguageValue
                           {
                               Th = $"{src.FirstName} {src.LastName}",
                               En = $"{src.FirstNameEn} {src.LastNameEn}"
                           }))
                .ReverseMap();
           
            CreateMap<SysItemAccessRight, SysItemAccessRightDto>().ReverseMap();
            CreateMap<SysUserItemFavorite, SysUserItemFavoriteDto>().ReverseMap();

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

            CreateMap<ColRole, ColRoleDto>().ReverseMap();

            CreateMap<Province, ProvinceDto>().ReverseMap();
            CreateMap<District, DistrictDto>().ReverseMap();
            CreateMap<SubDistrict, SubDistrictDto>().ReverseMap();
        }
    }
}

