CREATE OR ALTER VIEW vw_ReassignWorklist
AS
SELECT w.WorklistId
, w.AssignCollectorId
, w.AssignCollectorName
, w.ContractNo
, pf.PrefixName + ' ' + person.FirstName + ' ' + person.LastName AS CustomerName
, ca.AssetType
, enum_constatus.EnumDescription AS ContractStatus
, w.FollowupStatusDesc AS FollowupStatus
, w.ReassignById 
, w.ReassignByName AS ReassignBy
, w.ReassignFromId 
, w.ReassignFromName AS ReassignFrom
, w.AssignTeamId
, w.AssignTeamName AS TeamFrom
, w.ReassignToId
, w.ReassignToName AS ReassignTo
, w.ReassignToTeamId
, w.ReassignToTeamName AS TeamTo
, w.ReassignRequestDate as RequestedDate
, w.ReassignReason AS Reason
, w.ReassignStatusCode AS ReassignStatus
, CASE 
        WHEN w.AssignTeamId = w.ReassignToTeamId THEN CAST(1 AS BIT)
        ELSE CAST(0 AS BIT)
    END AS IsMyTeam
FROM [dbo].[Worklist] w 
INNER JOIN [dbo].[Contract] c ON c.ContractNo = w.ContractNo
LEFT OUTER JOIN [dbo].[SysEnum] enum_constatus ON enum_constatus.EnumName = 'ContractStatus' and enum_constatus.EnumCode = c.ContractStatus
LEFT OUTER JOIN [dbo].[ContractPerson] person ON person.ContractNo = w.ContractNo and person.PersonType = 'B'
LEFT OUTER JOIN [dbo].[Prefix] pf ON pf.PrefixId = person.PrefixId
LEFT OUTER JOIN [dbo].[ContractAsset] ca on ca.ContractNo = w.ContractNo
LEFT OUTER JOIN [dbo].[ColTeamAssignment] ta on ta.CollectorId = w.ReassignToId 
and (ta.EffectiveDate is not null or ta.EffectiveDate <= getdate())
and (ta.EffectiveDate is not null or ta.[ExpireDate] >= getdate())
WHERE ReassignStatusCode = 'WaitForApprove'