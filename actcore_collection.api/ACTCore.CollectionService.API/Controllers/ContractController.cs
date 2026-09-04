using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.DisplayFilterDto;
using ACTCore.CollectionService.API.Models.Paginations;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application;
using ACTCore.CollectionService.Application.Collection;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.Entities.Contracts;
using ACTCore.CollectionService.Domain.ValueObjects;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using SharedKernel.Models;
using System.Data;
using System.Net;


namespace ACTCore.CollectionService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ContractController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _logger;
        private readonly IActivityLogService _activityLog;
        private readonly WorklistService _worklistService;
        private readonly TeamAssignmentService _teamAssignmentService;
        private readonly ContractService _contractService;
        private readonly EnumService _enumService;
        private readonly CollectionNoteService _collectionNoteService;
        private readonly EmployeeService _employeeService;
        private readonly ColNoteResultService _colNoteResultService;
        private readonly LanguageService _languageService;

        public ContractController(IMapper mapper, IErrorLogService logger, IActivityLogService activityLog
            , WorklistService worklistService, TeamAssignmentService teamAssignmentService
            , ContractService contractService, EnumService enumService, CollectionNoteService collectionNoteService
            , EmployeeService employeeService, ColNoteResultService colNoteResultService
            , LanguageService languageService) : base(logger)
        {
            _mapper = mapper;
            _logger = logger;
            _activityLog = activityLog;
            _worklistService = worklistService;
            _teamAssignmentService = teamAssignmentService;
            _contractService = contractService;
            _enumService = enumService;
            _collectionNoteService = collectionNoteService;
            _employeeService = employeeService;
            _colNoteResultService = colNoteResultService;
            _languageService = languageService;
        }

        [HttpGet("detail/{contractNo}")]
        public async Task<ActionResult<APIResponse>> Getbyid(string contractNo)
        {
            var response = new APIResponse();
            try
            {
                var workList = await _worklistService.GetAsync(x => x.ContractNo == contractNo, includeProperties: "Collector.User");
                if (workList == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Contract");
                    response.Status = false;
                    return Ok(response);
                }

                var hasPermission = await WorklistPermissionHelpers.CheckCollectorPermission(workList, CurrentUserInfo.UserId, _teamAssignmentService);
                if (!hasPermission)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotHavePermissionToAccessContract);
                    response.Status = false;
                    return Ok(response);
                }

                var contract = await _contractService.GetAsync(x => x.ContractNo == contractNo, includeProperties: "ContractAssets,ContractAssetVehicles");
                
                response.Data = _mapper.Map<ContractDetailResponseDto>(contract);
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
        }

        [HttpPost("note-list-paged/{contractNo}")]
        public async Task<IActionResult> GetNoteList(string contractNo, [FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var workList = await _worklistService.GetAsync(x => x.ContractNo == contractNo, includeProperties: "Collector.User");
                if (workList == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Contract");
                    response.Status = false;
                    return Ok(response);
                }

                var hasPermission = await WorklistPermissionHelpers.CheckCollectorPermission(workList, CurrentUserInfo.UserId, _teamAssignmentService);
                if (!hasPermission)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotHavePermissionToAccessContract);
                    response.Status = false;
                    return Ok(response);
                }

                if (!string.IsNullOrEmpty(request.SortColumn))
                {
                    if (request.SortColumn == "FollowupAction") request.SortColumn = "ColNoteAction.ActionDescription";
                    if (request.SortColumn == "FollowupResult") request.SortColumn = "ColNoteResult.ResultDescription";
                }

                var notes = await _collectionNoteService.GetListPaginationAsync(
                    filter: x => x.ContractNo == contractNo,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn,
                    sortDirection: request.SortDirection,
                    filterColumns: request.Filters,
                    includeProperties: ""
                );

                var noteDtos = _mapper.Map<IEnumerable<ContractNoteResponseDto>>(notes.Items);
                var noteResults = await _colNoteResultService.GetListAsync(includeProperties: "ColNoteAction");

                foreach (var item in noteDtos)
                {
                    var profile = await _employeeService.GetAsync(x => x.UserName == item.UpdatedBy);
                    if (profile != null)
                        item.UpdatedBy = $"{profile?.FirstName} {profile?.LastName}";

                    if (int.TryParse(item.FollowupAction, out int actionId) && int.TryParse(item.FollowupResult, out int resultId))
                    {
                        var result = noteResults.FirstOrDefault(r => r.ResultId == resultId && r.ActionId == actionId);
                        item.FollowupAction = result?.ColNoteAction?.ActionDescription;
                        item.FollowupResult = result?.ResultDescription;
                    }
                }

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                var filterContractNoteProperties = Utilities.DataTableHelper.GetDisplayColumnAndDisplayProperties<ContractNoteDisplayFilterDto>(enumValues);
                response = new ApiPaginationResponse()
                {
                    Data = new PaginationData<ContractNoteResponseDto>
                    {
                        Datatables = noteDtos,
                        DisplayColumns = filterContractNoteProperties,
                        TotalRecords = notes.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)notes.TotalCount / request.PageSize),
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
        }

        [HttpGet("overdues/{contractNo}")]
        public async Task<IActionResult> GetContractOverdues(string contractNo)
        {
            var response = new APIResponse();
            try
            {
                var workList = await _worklistService.GetAsync(x => x.ContractNo == contractNo, includeProperties: "Collector.User");
                if (workList == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Contract");
                    response.Status = false;
                    return Ok(response);
                }

                var hasPermission = await WorklistPermissionHelpers.CheckCollectorPermission(workList, CurrentUserInfo.UserId, _teamAssignmentService);
                if (!hasPermission)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotHavePermissionToAccessContract);
                    response.Status = false;
                    return Ok(response);
                }

                var contract = await _contractService.GetAsync(x => x.ContractNo == contractNo);

                if (contract == null)
                {
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Contract");
                    response.StatusCode = HttpStatusCode.NotFound;
                    return Ok(response);
                }

                var items = await _contractService.GetListContractOverduesAsync(x => x.ContractNo == contractNo);
                var itemDtos = _mapper.Map<IEnumerable<ContractOverdueReponseDto>>(items).OrderBy(i => i.No).ToList();

                var summaryDto = new ContractOverdueReponseDto
                {
                    No = null,
                    Type = await LangHelper.GetResponseMsgAsync(_languageService, Message.TotalAmountDue),
                    Amount = items.Sum(i => i.OverdueAmount) ?? 0
                };
                itemDtos.Add(summaryDto);

                var displayProperties = DataTableHelper.GetDisplayColumnAndDisplayProperties<ContractOverdueDisplayFilterDto>(null);

                response.Data = new 
                {
                    Datatables = itemDtos,
                    DisplayColumns = displayProperties,
                };

                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
        }

        [HttpPost("payment-list-paged/{contractNo}")]
        public async Task<IActionResult> GetPaymentList(string contractNo, [FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var workList = await _worklistService.GetAsync(x => x.ContractNo == contractNo, includeProperties: "Collector.User");
                if (workList == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Contract");
                    response.Status = false;
                    return Ok(response);
                }

                var hasPermission = await WorklistPermissionHelpers.CheckCollectorPermission(workList, CurrentUserInfo.UserId, _teamAssignmentService);
                if (!hasPermission)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotHavePermissionToAccessContract);
                    response.Status = false;
                    return Ok(response);
                }

                var contract = await _contractService.GetAsync(x => x.ContractNo == contractNo);
                if (contract == null)
                {
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Contract");
                    response.StatusCode = HttpStatusCode.NotFound;
                    return Ok(response);
                }

                var payments = await _contractService.GetListPaymentPaginationAsync(
                    filter: x => x.ContractNo == contractNo,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn,
                    sortDirection: request.SortDirection,
                    filterColumns: request.Filters,
                    includeProperties: ""
                );

                var PaymentDto = _mapper.Map<IEnumerable<PaymentDetailResponseDto>>(payments.Items);

                response = new ApiPaginationResponse()
                {
                    Data = new PaymentPaginationData<PaymentDetailResponseDto>
                    {
                        ContractTerm = contract?.Term,
                        PaidTerm = contract?.PaymentReceivedTerm,
                        PaidAmount = contract?.PaymentReceivedAmount,
                        Datatables = PaymentDto,
                        TotalRecords = payments.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)payments.TotalCount / request.PageSize),
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
        }

        [HttpPost("address-list-paged/{contractNo}")]
        public async Task<IActionResult> GetAddressList(string contractNo, [FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var workList = await _worklistService.GetAsync(x => x.ContractNo == contractNo, includeProperties: "Collector.User");
                if (workList == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Contract");
                    response.Status = false;
                    return Ok(response);
                }

                var hasPermission = await WorklistPermissionHelpers.CheckCollectorPermission(workList, CurrentUserInfo.UserId, _teamAssignmentService);
                if (!hasPermission)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotHavePermissionToAccessContract);
                    response.Status = false;
                    return Ok(response);
                }

                var contractPersons = await _contractService.GetListContractPersonAsync(
                    x => x.ContractNo == contractNo);
                if (contractPersons == null || !contractPersons.Any())
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "ContractPerson");
                    response.Status = false;
                    return Ok(response);
                }

                var addresses = await _contractService.GetListAddressPaginationAsync(
                    filter: x => contractPersons.Select(p => p.PersonRefId).Contains(x.PersonRefId),
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn,
                    sortDirection: request.SortDirection,
                    filterColumns: request.Filters,
                    includeProperties: "ContractPerson"
                );

                var enumAddress = await _enumService.GetListAsync(x => x.EnumName == "AddressType");
                var enumPerson = await _enumService.GetListAsync(x => x.EnumName == "PersonType");
                var enumAddressDict = enumAddress.ToDictionary(e => e.EnumCode, e => e.EnumDescription);
                var enumPersonDict = enumPerson.ToDictionary(e => e.EnumCode, e => e.EnumDescription);

                foreach (var addr in addresses.Items)
                {
                    if (!string.IsNullOrEmpty(addr.AddressType) && enumAddressDict.TryGetValue(addr.AddressType, out var desc))
                    {
                        addr.AddressType = desc;
                    }
                }

                var addressDtos = _mapper.Map<IEnumerable<ContractAddressResponseDto>>(addresses.Items);

                response = new ApiPaginationResponse()
                {
                    Data = new PaginationData<ContractAddressResponseDto>
                    {
                        Datatables = addressDtos,
                        TotalRecords = addresses.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)addresses.TotalCount / request.PageSize),
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
        }

        [HttpPost("phone-list-paged/{contractNo}")]
        public async Task<IActionResult> GetPhoneListPaged(string contractNo, [FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var workList = await _worklistService.GetAsync(x => x.ContractNo == contractNo, includeProperties: "Collector.User");
                if (workList == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Contract");
                    response.Status = false;
                    return Ok(response);
                }

                var hasPermission = await WorklistPermissionHelpers.CheckCollectorPermission(workList, CurrentUserInfo.UserId, _teamAssignmentService);
                if (!hasPermission)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotHavePermissionToAccessContract);
                    response.Status = false;
                    return Ok(response);
                }

                var contractPersons = await _contractService.GetListContractPersonAsync(
                    x => x.ContractNo == contractNo);
                if (contractPersons == null || !contractPersons.Any())
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "ContractPerson");
                    response.Status = false;
                    return Ok(response);
                }

                var phones = await _contractService.GetListPhonePaginationAsync(
                    filter: x => contractPersons.Select(p => p.PersonRefId).Contains(x.PersonRefId),
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn,
                    sortDirection: request.SortDirection,
                    filterColumns: request.Filters,
                    includeProperties: "ContractPerson"
                );

                var phoneDtos = _mapper.Map<IEnumerable<ContractPhoneResponseDto>>(phones.Items);
                var enumPhoneType = await _enumService.GetListAsync(x => x.EnumName == "PhoneType");
                var enumPersonType = await _enumService.GetListAsync(x => x.EnumName == "PersonType");

                var enumPhoneTypeDict = enumPhoneType.ToDictionary(e => e.EnumCode, e => e.EnumDescription);
                var enumPersonTypeDict = enumPersonType.ToDictionary(e => e.EnumCode, e => e.EnumDescription);

                foreach (var phone in phoneDtos)
                {
                    if (!string.IsNullOrEmpty(phone.PhoneType) && enumPhoneTypeDict.TryGetValue(phone.PhoneType, out var desc))
                    {
                        phone.PhoneType = desc;
                    }
                }

                response = new ApiPaginationResponse()
                {
                    Data = new PaginationData<object>
                    {
                        Datatables = phoneDtos,
                        DisplayColumns = null,
                        TotalRecords = phones.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)phones.TotalCount / request.PageSize),
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
        }

        [HttpGet("lookup-phones-by-person/{contractPersonId}")]
        public async Task<IActionResult> GetBorrowerPhones(Guid contractPersonId)
        {
            var response = new APIResponse();
            try
            {
                var phonesList = (await _contractService.GetListContractPhoneAsync(
                    x => x.ContractPerson.Id == contractPersonId))?
                    .OrderBy(x => x.PhoneType)?
                    .ToList();
                
                var enumPhoneTypeDict = (await _enumService.GetListAsync(x => x.EnumName == "PhoneType"))
                    .ToDictionary(e => e.EnumCode, e => e.EnumDescription);
                
                var phoneDtos = new List<DropdownOptionDto>();

                foreach (var phone in phonesList)
                {
                    if (!string.IsNullOrEmpty(phone.PhoneType) && enumPhoneTypeDict.TryGetValue(phone.PhoneType, out var desc))
                    {
                        phoneDtos.Add(new DropdownOptionDto 
                        { 
                            Value = phone.Id.ToString(),
                            Label = $"({desc}) {phone.PhoneNo}", 
                        });
                    }
                }
                response.Data = phoneDtos;

                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
        }

        [HttpGet("lookup-persons/{contractNo}")]
        public async Task<IActionResult> GetBorrowerPerson(string contractNo)
        {
            var response = new APIResponse();
            try
            {
                var workList = await _worklistService.GetAsync(x => x.ContractNo == contractNo, includeProperties: "Collector.User");
                if (workList == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Contract");
                    response.Status = false;
                    return Ok(response);
                }

                var hasPermission = await WorklistPermissionHelpers.CheckCollectorPermission(workList, CurrentUserInfo.UserId, _teamAssignmentService);
                if (!hasPermission)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotHavePermissionToAccessContract);
                    response.Status = false;
                    return Ok(response);
                }

                var personList = (await _contractService.GetListContractPersonAsync(
                    x => x.ContractNo == contractNo, includeProperties: "Prefix"))
                    .OrderBy(x => x.PersonType)
                    .ToList();

                var enumPersonTypeDict = (await _enumService.GetListAsync(x => x.EnumName == "PersonType"))
                    .ToDictionary(e => e.EnumCode, e => e.EnumDescription);
                
                var personDtos = new List<DropdownOptionDto>();

                foreach (var person in personList)
                {
                    if (!string.IsNullOrEmpty(person.PersonType) && enumPersonTypeDict.TryGetValue(person.PersonType, out var desc))
                    {
                        personDtos.Add(new DropdownOptionDto { 
                            Value = person.Id.ToString(),
                            Label = $"({desc}) {person.FullName}"
                        });
                    }
                }
                response.Data = personDtos;
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
        }

        [HttpGet("persons/{contractNo}")]
        public async Task<IActionResult> GetPersonList(string contractNo)
        {
            var response = new APIResponse();
            try
            {
                var workList = await _worklistService.GetAsync(x => x.ContractNo == contractNo, includeProperties: "Collector.User");
                if (workList == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Contract");
                    response.Status = false;
                    return Ok(response);
                }

                var hasPermission = await WorklistPermissionHelpers.CheckCollectorPermission(workList, CurrentUserInfo.UserId, _teamAssignmentService);
                if (!hasPermission)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotHavePermissionToAccessContract);
                    response.Status = false;
                    return Ok(response);
                }

                var contractPersons = await _contractService.GetListContractPersonAsync(x => x.ContractNo == contractNo, includeProperties: "Prefix");

                var enumGenders = await _enumService.GetListAsync(x => x.EnumName == "Gender");
                var enumMaritals = await _enumService.GetListAsync(x => x.EnumName == "MaritalStatus");
                var enumGenderDict = enumGenders.ToDictionary(e => e.EnumCode, e => e.EnumDescription);
                var enumMaritalDict = enumMaritals.ToDictionary(e => e.EnumCode, e => e.EnumDescription);

                foreach (var p in contractPersons)
                {
                    if (!string.IsNullOrEmpty(p.Gender) && enumGenderDict.TryGetValue(p?.Gender, out var gender))
                    {
                        p.Gender = gender;
                    }
                    if (!string.IsNullOrEmpty(p.MaritalStatus) && enumMaritalDict.TryGetValue(p?.MaritalStatus, out var marital))
                    {
                        p.MaritalStatus = marital;
                    }
                }

                response.Data = _mapper.Map<IEnumerable<PersonResponseDto>>(contractPersons);
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
        }
    }
}
