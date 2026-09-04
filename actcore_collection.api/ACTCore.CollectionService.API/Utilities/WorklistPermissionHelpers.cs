using ACTCore.CollectionService.Application.Collection;
using ACTCore.CollectionService.Domain.Entities.Collections;
using SharedKernel.Enums;

namespace ACTCore.CollectionService.API.Utilities
{
    public static class WorklistPermissionHelpers
    {
        public static async Task<bool> CheckCollectorPermission(Worklist workList, Guid currentUserId, TeamAssignmentService teamAssignmentService, bool isUnassign = false)
        {
            if (workList == null) return false;

            var today = DateTime.Now.Date;
            Guid? colTeamId = isUnassign ? null : workList.AssignTeamId;
            var teamAssignment = await teamAssignmentService.GetActiveColTeamAssignment(currentUserId, today, colTeamId: colTeamId);

            bool isAssignedCollector =
                workList?.AssignCollectorId == currentUserId
                || workList?.Collector?.User?.UserId == currentUserId;

            // check Active Permission of current User first , then check owner or supervisor
            return teamAssignment != null &&  (isAssignedCollector || teamAssignment.IsSupervisor);
        }
        public static async Task<bool> CheckCollectorPermissionByTeamAssignment(Worklist workList, Guid currentUserId, TeamAssignmentService teamAssignmentService, Dictionary<Guid, ColTeamAssignment> dictteamAssignment, bool isUnassign = false)
        {
            if (workList == null) return false;

            // assigned collector?
            bool isAssignedCollector =
                workList.AssignCollectorId == currentUserId ||
                workList.Collector?.User?.UserId == currentUserId;
            var teamAssignment = await teamAssignmentService.GetActiveColTeamAssignment(currentUserId, DateTime.Now.Date);

            if (isAssignedCollector && teamAssignment != null && teamAssignment.IsActive)
                return true;

            // supervisor?
                // -- ถ้า unassign = true => ไม่ต้อง match team => Check assignFrom >> Active permission && IsSupervisor
            if (isUnassign)
            {
                return teamAssignment != null && teamAssignment.IsSupervisor;
            }

                // -- ต้อง match team => check Active Permission && supervisor
            if (dictteamAssignment.TryGetValue((Guid)workList.AssignTeamId, out var assignment))
            {
                return teamAssignment != null && teamAssignment.IsActive && teamAssignment.IsSupervisor;
            }

            return false;
        }

        public static async Task<bool> CheckApprovePermission(Worklist workList, Guid currentUserId, TeamAssignmentService teamAssignmentService)
        {
            if (workList == null) return false;

            var today = DateTime.Now.Date;
            var teamAssignment = await teamAssignmentService.GetActiveColTeamAssignment(currentUserId, today,isSupervisor: true, colTeamId : workList.ReassignToTeamId);

            bool isWaitFoAprove = workList.ReassignStatusCode == Enums.ReassignStatus.WaitForApprove;

            // Check WaitForApprove Status + All IsActive (including IsSupervisor)
            return isWaitFoAprove && teamAssignment != null;
        }
    }
}
