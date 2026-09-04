using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto;
using ACTCore.CollectionService.API.Models.Mappings;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Domain.ValueObjects;
using AutoMapper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using SharedKernel.Helpers;
using SharedKernel.Models;
using System.Net;
using System.Text.Json;

namespace ACTCore.CollectionService.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LanguageController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _logger;
        private readonly IActivityLogService _activityLog;
        private readonly LanguageService _languageService;
        private readonly ParameterService _sysParameterService;

        public LanguageController(IMapper mapper, IErrorLogService logger, IActivityLogService activityLog,
            LanguageService languageService, ParameterService sysParameterService) : base(logger)
        {
            _mapper = mapper;
            _logger = logger;
            _activityLog = activityLog;
            _languageService = languageService;
            _sysParameterService = sysParameterService;
        }

        [HttpPost("list-paged")]
        public async Task<ActionResult<APIResponse>> GetList(PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                // Fetch paged languages
                var pagedLanguages = await _languageService.GetListPaginationAsync(
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn,
                    sortDirection: request.SortDirection,
                    filterColumns: request.Filters
                );

                var displayProperties = Utilities.DataTableHelper.GetDisplayColumnAndDisplayProperties<LanguageDisplayFilterDto>();

                response = new ApiPaginationResponse()
                {
                    Data = new PaginationData<LanguageListsResponseDto>
                    {
                        Datatables = _mapper.Map<IEnumerable<LanguageListsResponseDto>>(pagedLanguages.Items),
                        DisplayColumns = displayProperties,
                        TotalRecords = pagedLanguages.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)pagedLanguages.TotalCount / request.PageSize),
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize,
                    },
                    StatusCode = HttpStatusCode.OK,
                    Status = true
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, "", ControllerContext, CurrentUserInfo, response)
                    );
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> GetById(string id)
        {
            var response = new APIResponse();
            try
            {
                var language = await _languageService.GetAsync(x => x.Key == id);
                if (language == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Key");
                    response.Status = false;

                    return Ok(response);
                }

                var languageDto = new LanguageResponseDto
                {
                    Key = language.Key,
                    Value = string.IsNullOrWhiteSpace(language.Value)
                        ? null
                        : JsonSerializer.Deserialize<LanguageValue>(language.Value),
                    DefaultValue = string.IsNullOrWhiteSpace(language.DefaultValue)
                        ? null
                        : JsonSerializer.Deserialize<LanguageValue>(language.DefaultValue),
                    Ordering = language.Ordering
                };

                response.Data = languageDto;
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, CurrentUserInfo.UserId.ToString(), ControllerContext, CurrentUserInfo, response)
                    );
            }
        }

        [AllowAnonymous]
        [HttpGet("available-languages")]
        public async Task<ActionResult<APIResponse>> GetAvailableLanguages()
        {
            var response = new APIResponse();
            try
            {
                var configLang = await _sysParameterService.GetAsync(filter: p => p.ParameterName == "Language");
                var jsonConfigLang = JsonSerializer.Deserialize<List<LanguageConfigResponseDto>>(configLang.ParameterValue);

                response.Data = jsonConfigLang;
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
            }
        }

        [AllowAnonymous]
        [HttpGet("get-language/{lang}")]
        public async Task<ActionResult<APIResponse>> GetLanguages(string lang)
        {
            var response = new APIResponse();
            try
            {
                var languages = await _languageService.GetListAsync();
                var selectedLanguage = _languageService.GetTranslatebyLanguage(languages, lang);

                response.Data = selectedLanguage;
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
            }
        }

        [HttpPost("bulk-update")]
        public async Task<ActionResult<APIResponse>> UpdateLanguages([FromBody] LanguageCreateDto[] createDto, bool isUpdateDefault = false)
        {
            var response = new APIResponse();
            try
            {
                var langs = _mapper.Map<List<Language>>(createDto);
                // Check for duplicate keys in createDto
                var duplicateKeys = createDto.GroupBy(x => x.Key)
                                             .Where(g => g.Count() > 1)
                                             .Select(g => g.Key)
                                             .ToList();
                if (duplicateKeys.Any())
                {
                    response.Status = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_DuplicateKeysInRequest, string.Join(", ", duplicateKeys));
                    return Ok(response);
                }

                foreach (var lang in langs)
                {
                    var existLang = _mapper.Map<Language>(await _languageService.GetAsync(x => x.Key == lang.Key));
                    if (existLang != null)
                    {
                        // update
                        existLang.Value = lang.Value;
                        if (isUpdateDefault)
                            existLang.DefaultValue = lang.Value;
                        VersioningModelHelper.SetUpdatedAudit(existLang, User.Identity.Name);
                        await _languageService.UpdateAsync(existLang);
                    }
                    else
                    {
                        // create new
                        lang.DefaultValue = lang.Value;
                        lang.Value = lang.Value;
                        VersioningModelHelper.SetCreatedAudit(lang, User.Identity.Name);
                        await _languageService.CreateAsync(lang);
                    }

                }

                response.StatusCode = HttpStatusCode.OK;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService,
                    Message.Msg_SavedSuccessfully);
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
            }

        }

        [AllowAnonymous]
        [HttpPost("import-csv")]
        public async Task<ActionResult<APIResponse>> ImportLanguagesFromCsv(IFormFile file)
        {
            var response = new APIResponse();
            try
            {
                if (file == null || file.Length == 0)
                {
                    response.Status = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NoFileUploaded);
                    return Ok(response);
                }

                List<LanguageCreateDto> createDtos;
                using (var stream = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvHelper.CsvReader(stream,
                    new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                    {
                        Delimiter = "|",
                        HasHeaderRecord = true,
                        HeaderValidated = null,
                        MissingFieldFound = null
                    }))
                {
                    // Register class map
                    csv.Context.RegisterClassMap<LanguageCsvMap>();
                    // Read records to List
                    createDtos = csv.GetRecords<LanguageCreateDto>().ToList();
                }

                // Reuse bulk update method
                return await UpdateLanguages(createDtos.ToArray(), true);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportLanguagesToCsv()
        {
            try
            {
                var languages = await _languageService.GetListAsync();
                var languageDtos = _mapper.Map<List<LanguageCreateDto>>(languages);

                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: true)))
                using (var csv = new CsvHelper.CsvWriter(streamWriter, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                {
                    Delimiter = "|",
                    HasHeaderRecord = true
                }))
                {
                    csv.Context.RegisterClassMap<LanguageCsvMap>();
                    csv.WriteRecords(languageDtos);
                    streamWriter.Flush();
                    memoryStream.Position = 0;
                    var fileName = $"languages_{DateTime.Now:yyyyMMddHHmmss}.csv";
                    return File(memoryStream.ToArray(), "text/csv", fileName);
                }
            }
            catch (Exception ex)
            {
                var response = new APIResponse();
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
        }

        [HttpPost("reset-default/{key}")]
        public async Task<ActionResult<APIResponse>> UpdateUser(string key)
        {
            var response = new APIResponse();
            Language updateLanguage = null;
            Language beforUpdate = null;
            try
            {
                updateLanguage = await _languageService.GetAsync(x => x.Key == key);
                if (updateLanguage == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Key");
                    response.Status = false;
                    return Ok(response);
                }

                beforUpdate = Utilities.ModelHelper.CloneModel(updateLanguage);

                updateLanguage.Value = updateLanguage.DefaultValue;
                VersioningModelHelper.SetUpdatedAudit(updateLanguage, User.Identity.Name);
                await _languageService.UpdateAsync(updateLanguage);

                var languageDto = new LanguageResponseDto
                {
                    Key = updateLanguage.Key,
                    Value = string.IsNullOrWhiteSpace(updateLanguage.Value)
                        ? null
                        : JsonSerializer.Deserialize<LanguageValue>(updateLanguage.Value),
                    DefaultValue = string.IsNullOrWhiteSpace(updateLanguage.DefaultValue)
                        ? null
                        : JsonSerializer.Deserialize<LanguageValue>(updateLanguage.DefaultValue),
                    Ordering = updateLanguage.Ordering
                };

                response.Data = languageDto;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService,
                    Message.Msg_LanguageResetSuccessfully);
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                var entityId = beforUpdate?.Key != null ? beforUpdate.Key.ToString() : "";
                await _activityLog.ActivityLogEditWithDetailsAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.UPDATE, entityId, ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActiviUpdateLog(beforUpdate, updateLanguage)
                    );
            }
        }
    }
}
