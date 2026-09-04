using ACTCore.CollectionService.Application.Collection;
using ACTCore.CollectionService.Application.Dto.Validation;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Model;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.Entities.Collections;
using SharedKernel.CommonConstants;

namespace ACTCore.CollectionService.Application.Validations
{
    public class TeamAssignmentValidationService
    {
        private readonly TeamAssignmentService _teamAssignmentService;
        private readonly CollectorService _collectorProfileService;
        private readonly LanguageService _languageService;
        private readonly ColTeamService _colTeamService;
        private readonly WorklistService _worklistService;

        public TeamAssignmentValidationService(TeamAssignmentService teamAssignmentService, CollectorService collectorProfileService
        , LanguageService languageService, ColTeamService colTeamService, WorklistService worklistService)
        {
            _teamAssignmentService = teamAssignmentService;
            _collectorProfileService = collectorProfileService;
            _colTeamService = colTeamService;
            _languageService = languageService;
            _worklistService = worklistService;
        }

        public async Task<ValidationResultModel> ValidateCreateAsync(TeamAssignmentValidationCreateDto dto, bool overwrite)
        {
            // Validate collector exists
            var collectorValidation = await ValidateCollectorExist(dto.CollectorId);
            if (!collectorValidation.IsValid)   return collectorValidation;
            
            // Validate team exists
            var colTeamValidation = await ValidateColTeamExist(dto.ColTeamId);
            if (!colTeamValidation.IsValid) return colTeamValidation;

            // Validate capacity
            var capacityValidation = await ValidateCapacity(dto.Capacity);
            if (!capacityValidation.IsValid) return capacityValidation;

            // Validate duplicate in same team
            var duplicateInSameTeamValidation = await ValidateDuplicationInSameTeam(dto.CollectorId, dto.ColTeamId);
            if (!duplicateInSameTeamValidation.IsValid) return duplicateInSameTeamValidation;

            // Validate duplicate out team (date overlap)
            var duplicateOutTeamValidation = await ValidateDuplicationInOtherTeamWithDateOverlap(dto.CollectorId, dto.ColTeamId, dto.EffectiveDate, dto.ExpireDate);
            if (!duplicateOutTeamValidation.IsValid) return duplicateOutTeamValidation;
            
            // Validate supervisor rule
            if(dto.IsSupervisor && overwrite == false)
            {
                var supervisorValidation = await ValidateSupervisorExistsinTeam(dto.ColTeamId);
                if (!supervisorValidation.IsValid) return supervisorValidation;
            }

            return ValidationResultModel.Success();
        }

        public async Task<ValidationResultModel> ValidateUpdateAsync(TeamAssignmentValidationUpdateDto dto, bool overwrite)
        {
            // Validate collector exists
            var collectorValidation = await ValidateCollectorExist(dto.CollectorId);
            if (!collectorValidation.IsValid)   return collectorValidation;
            
            // Validate team exists
            var colTeamValidation = await ValidateColTeamExist(dto.ColTeamId);
            if (!colTeamValidation.IsValid) return colTeamValidation;

            // Validate capacity
            var capacityValidation = await ValidateCapacity(dto.Capacity);
            if (!capacityValidation.IsValid) return capacityValidation;

            // Validate assignment exists
            var (isSupervisor, assignmentValidation) = await ValidateAssignmentExist(dto.AssignmentId);
            if (!assignmentValidation.IsValid) return assignmentValidation;

            // Validate duplicate out team (date overlap)
            var duplicateOutTeamValidation = await ValidateDuplicationInOtherTeamWithDateOverlap(dto.CollectorId, dto.ColTeamId, dto.EffectiveDate, dto.ExpireDate);
            if (!duplicateOutTeamValidation.IsValid) return duplicateOutTeamValidation;
            
            // Validate supervisor rule
            if(!isSupervisor && dto.IsSupervisor && overwrite == false)
            {
                var supervisorValidation = await ValidateSupervisorExistsinTeam(dto.ColTeamId);
                if (!supervisorValidation.IsValid) return supervisorValidation;
            }

            return ValidationResultModel.Success();
        }

        public async Task<ValidationResultModel> ValidateBulkAssignTeamAsync(CollectorAssignTeamValidationCreateDto dto)
        {
            // 1. Validate collector exists
            var collectorValidation = await ValidateCollectorListExist(dto.CollectorIds);
            if (!collectorValidation.IsValid)   return collectorValidation;
            
            // 2. Validate team exists
            var colTeamValidation = await ValidateColTeamExist(dto.ColTeamId);
            if (!colTeamValidation.IsValid) return colTeamValidation;

            foreach(var collectorId in dto.CollectorIds)
            {
                // 3. Validate duplicate in same team
                var duplicateInSameTeamValidation = await ValidateDuplicationInSameTeam(collectorId, dto.ColTeamId);
                if (!duplicateInSameTeamValidation.IsValid) return duplicateInSameTeamValidation;

                // 4. Validate duplicate out team (date overlap)
                var duplicateOutTeamValidation = await ValidateDuplicationInOtherTeamWithDateOverlap(collectorId, dto.ColTeamId, dto.EffectiveDate, dto.ExpireDate);
                if (!duplicateOutTeamValidation.IsValid) return duplicateOutTeamValidation;
            }

            return ValidationResultModel.Success();
        }

        // Delete 
        public async Task<ValidationResultModel> ValidateDeleteAsync(List<Guid> assignmentIds)
        {
            // 1. Validate assignment exists
            var existingAssignments = await _teamAssignmentService.GetListAsync(
                x => assignmentIds.Contains(x.AssignmentId),
                includeProperties: "ColTeam,Collector.User.Employee");
            
            var notFoundIds = assignmentIds.Except(existingAssignments.Select(a => a.AssignmentId)).ToList();
            if (notFoundIds.Any())
                return ValidationResultModel.Fail(await LangHelper.GetResponseMsgAsync(
                    _languageService, Message.Msg_2ParamsNotFound, "Assignment Id", 
                    string.Join(", ", notFoundIds)));

            // 2. Build assignment dictionary for efficient lookup
            var assignmentDict = existingAssignments.ToDictionary(
                x => (ColTeamId: (Guid?)x.ColTeamId, CollectorId: (Guid?)x.CollectorId),
                x => x);

            var teamIds = assignmentDict.Keys.Select(k => (Guid?)k.ColTeamId).Distinct().ToList();

            var candidateWorklists = await _worklistService.GetListAsync(
                x => teamIds.Contains(x.AssignTeamId),
                asNoTracking: true);

            // 3. Check for conflicting worklist
            var matchedWorklist = candidateWorklists
                .FirstOrDefault(w => assignmentDict.ContainsKey(
                    (ColTeamId: w.AssignTeamId, CollectorId: w.AssignCollectorId)));

            if (matchedWorklist != null && 
                assignmentDict.TryGetValue(
                    (ColTeamId: matchedWorklist.AssignTeamId, 
                     CollectorId: matchedWorklist.AssignCollectorId), 
                    out var assignment))
            {
                var errorMsg = await LangHelper.GetResponseMsgAsync(_languageService,
                    Message.Msg_ExistingWorklistWithTeamAssignment,
                    assignment.Collector.User.Employee.EmployeeId,
                    assignment.ColTeam.ColTeamName);
                    
                return ValidationResultModel.Fail(errorMsg);
            }

            return ValidationResultModel.Success();
        }

        #region Private validation methods
        private async Task<ValidationResultModel> ValidateCollectorExist(Guid collectorId)
        {
            var collector = await _collectorProfileService.GetAsync(x => x.CollectorId == collectorId && x.IsActive == true);

            if (collector == null)
                return ValidationResultModel.Fail(await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Collector"));

            return ValidationResultModel.Success();
        }

        private async Task<ValidationResultModel> ValidateCollectorListExist(IEnumerable<Guid> collectorIds)
        {
            var collectors = await _collectorProfileService.GetListAsync(x => collectorIds.Contains(x.CollectorId) && x.IsActive == true);
            var notFoundIds = collectorIds.Except(collectors.Select(c => c.CollectorId)).ToList();

            if (notFoundIds.Any())
                return ValidationResultModel.Fail(await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, $"Collectors with IDs: {string.Join(", ", notFoundIds)}"));

            return ValidationResultModel.Success();
        }

        private async Task<ValidationResultModel> ValidateColTeamExist(Guid colTeamId)
        {
            var colTeam = await _colTeamService.GetAsync(x => x.ColTeamId == colTeamId && x.IsActive == true);
            if (colTeam == null)
                return ValidationResultModel.Fail(await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Collector Team"));
            
            return ValidationResultModel.Success();
        }

        private async Task<ValidationResultModel> ValidateDuplicationInSameTeam(Guid collectorId, Guid colTeamId)
        {
            var sameTeam = await _teamAssignmentService.GetAsync(x => 
                    x.ColTeamId == colTeamId
                    && x.CollectorId == collectorId
                    , includeProperties: "ColTeam,Collector.User.Employee");

            if(sameTeam != null)
                return ValidationResultModel.Fail(await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_DuplicateCollectorInTeamAssignment
                    , sameTeam.Collector.User?.Employee?.EmployeeId, sameTeam?.ColTeam?.ColTeamName));
            return ValidationResultModel.Success();
        }

        private async Task<ValidationResultModel> ValidateDuplicationInOtherTeamWithDateOverlap(Guid collectorId, Guid colTeamId, DateTime? effectiveDate, DateTime? expireDate)
        {
            var otherTeam = await _teamAssignmentService.GetAsync(x => 
                    x.ColTeamId != colTeamId 
                    && x.CollectorId == collectorId
                    && x.IsActive == true
                    && effectiveDate <= (x.ExpireDate ?? DateTime.MaxValue)
                    && (expireDate ?? DateTime.MaxValue) >= x.EffectiveDate
                , includeProperties: "ColTeam,Collector.User.Employee");
            
            if(otherTeam != null)
                return ValidationResultModel.Fail(await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_DuplicateCollectorInTeamAssignment
                    , otherTeam.Collector.User?.Employee?.EmployeeId, otherTeam?.ColTeam?.ColTeamName));
            
            return ValidationResultModel.Success();
        }

        private async Task<ValidationResultModel> ValidateSupervisorExistsinTeam(Guid colTeamId)
        {            
            var checkSupervisor = await _teamAssignmentService.GetAsync(x => x.ColTeamId == colTeamId && x.IsSupervisor == true);
            
            if(checkSupervisor != null)
                return ValidationResultModel.Confirmation(
                    await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_ComfirmOverwriteSupervisor),
                    await LangHelper.GetResponseMsgAsync(_languageService, "Warning"));

            return ValidationResultModel.Success();
        }

        private async Task<(bool isSupervisor, ValidationResultModel result)> ValidateAssignmentExist(Guid id)
        {
            var assignment = await _teamAssignmentService.GetAsync(x => x.AssignmentId == id,asNoTracking: false);
            if (assignment == null)
                return (false, ValidationResultModel.Fail(await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Team Assignment")));

            return (assignment.IsSupervisor, ValidationResultModel.Success());
        }

        private async Task<ValidationResultModel> ValidateCapacity(int capacity)
        {
            if(capacity < 0)
                return ValidationResultModel.Fail(await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidCapacity));

            return ValidationResultModel.Success();
        }

        #endregion
    }
}