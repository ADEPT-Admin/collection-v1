-- View CollectorAndTeamStatus
CREATE OR ALTER VIEW vw_CollectorAndTeamStatus
AS
select ta.AssignmentId, ta.IsActive as AssignmentStatus , ta.ColTeamId, t.ColTeamCode, t.ColTeamName, t.IsActive as TeamStatus,t.Capacity as TeamCapacity
, ta.CollectorId, ta.IsSupervisor, ta.Capacity as CollectorCapacity, c.IsActive as CollectorStatus
from dbo.ColTeamAssignment ta 
INNER JOIN dbo.ColTeam t on ta.ColTeamId = t.ColTeamId AND t.IsActive = 1
INNER JOIN dbo.CollectorProfile c on ta.CollectorId = c.CollectorId AND c.IsActive = 1
where ta.IsActive = 1 AND (ta.EffectiveDate <= GETDATE()) AND (ta.ExpireDate > GETDATE() OR ta.ExpireDate IS NULL)