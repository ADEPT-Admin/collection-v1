using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTCore.CollectionService.Application.Dto
{
    public class AssignmentWorklistListTeamPhoneDto
    {
        
        public string AssignTeam { get; set; }
        public int? AmountOfContact { get; set; }
        public string TeamID { get; set; }
        public string AssignMethodSortField { get; set; }
        public string AssignMethodSortOrder { get; set; }
        public int? Capacity { get; set; }
        //public string SupervisorID { get; set; }
        public int? AmountOfCollector { get; set; }
        public string AssignMethod { get; set; }
        public string AssignOverCapacity { get; set; }
        public int? TeamTotalCurrentWork { get; set; }


    }

    public class TeamMemberDetailsDto
    {
        public string? AssignTeam { get; set; }
        public string? CollectorID { get; set; }
        public int? CollectorCapacity { get; set; }
        public int? CountCollectorWorkAssignment { get; set; }      

    }

    public class ListOfContractForTeamDto
    {
        public string? ContractNo { get; set; }
        public string? RiskLevel { get; set; }
        public string? SourceField { get; set; }       

    }

    public class AllocateWorklistListUpdateWorkListDto
    {
        public string? ContractNo { get; set; }
        public DateTime? AssignDate { get; set; }
        public string? JobTypeCode { get; set; }
        public string? JobTypeDesc { get; set; }
        public string? AssignTool { get; set; }
        public Guid? AssignAreaId { get; set; }
        public string? AssignAreaCode { get; set; }
        public Guid? AssignTeamId { get; set; }
        public string? AssignTeamCode { get; set; }
        public string? AssignTeamName { get; set; }
        public Guid? AssignCollectorId { get; set; }
        public string? AssignCollectorEmpId { get; set; }
        public string? AssignCollectorName { get; set; }
        public string? AssignTypeCode { get; set; }
        public string? AssignTypeDesc { get; set; }
        public string? FollowupStatusCode { get; set; }
        public string? FollowupStatusDesc { get; set; }
        public int? IsFoundInWorkList { get; set; }

    }

    public class AssignUpdateDto
    {
        public string ContractNo { get; set; }
        public string AssignCollectorId { get; set; }
        public string SupervisorId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class WorklistDto
    {
        public string WorklistId { get; set; }
        public DateTime? AssignDate { get; set; }
        public string ContractNo { get; set; }
        public int? JobTypeId { get; set; }
        public string JobType { get; set; }
        public string RiskLevel { get; set; }
        public string? AssignTool { get; set; }
        public int? AssignAreaId { get; set; }
        public string? AssignAreaCode { get; set; }  
        public int? AssignTeamId { get; set; }
        public string? AssignTeamCode { get; set; }
        public string? AssignCollectorId { get; set; }
        public string? SupervisorId { get; set; }
        public int? AssignTypeId { get; set; }
        public string AssignTypeCode { get; set; }
        public string ReassignFrom { get; set; }
        public string ReassignTo { get; set; }
        public string ReassignReason { get; set; }
        public int? ReassignStatusId { get; set; }
        public string ReassignStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }

    public class WorklistHistoryDto
    {        
        public string Id { get; set; }
        public string WorklistId { get; set; }
        public DateTime? AssignDate { get; set; }
        public string ContractNo { get; set; }
        public int? JobTypeId { get; set; }
        public string JobType { get; set; }
        public string RiskLevel { get; set; }
        public string? AssignTool { get; set; }
        public int? AssignAreaId { get; set; }
        public string? AssignAreaCode { get; set; }
        public int? AssignTeamId { get; set; }
        public string? AssignTeamCode { get; set; }
        public string? AssignCollectorId { get; set; }
        public string? SupervisorId { get; set; }
        public int? AssignTypeId { get; set; }
        public string AssignTypeCode { get; set; }
        public string ReassignFrom { get; set; }
        public string ReassignTo { get; set; }
        public string ReassignReason { get; set; }
        public int? ReassignStatusId { get; set; }
        public string ReassignStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }



}
