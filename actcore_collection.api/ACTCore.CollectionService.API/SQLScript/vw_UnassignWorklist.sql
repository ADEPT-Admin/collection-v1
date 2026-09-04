CREATE OR ALTER VIEW vw_UnassignWorklist
AS
SELECT w.WorklistId
, w.AssignTeamId, w.AssignCollectorId
, w.ContractNo
, pf.PrefixName + ' ' + person.FirstName + ' ' + person.LastName AS CustomerName
, ca.AssetType
, enum_constatus.EnumDescription AS ContractStatus
, c.Bucket
, c.PaymentDueDate
, c.DayPastDue
, c.OverdueAmount
, c.OutstandingBalance
, w.AssignCollectorName as CollectorName
, w.AssignTeamName as TeamName
, w.ReassignFromName AS ReassignFrom
, w.AssignDate
, w.FollowupStatusDesc AS FollowupStatus
, w.FollowupDate AS LastFollowupDate
FROM [dbo].[Worklist] w 
INNER JOIN [dbo].[Contract] c ON c.ContractNo = w.ContractNo
LEFT OUTER JOIN [dbo].[SysEnum] enum_constatus ON enum_constatus.EnumName = 'ContractStatus' and enum_constatus.EnumCode = c.ContractStatus
LEFT OUTER JOIN [dbo].[ContractPerson] person ON person.ContractNo = w.ContractNo and person.PersonType = 'B'
LEFT OUTER JOIN [dbo].[Prefix] pf ON pf.PrefixId = person.PrefixId
LEFT OUTER JOIN [dbo].[ContractAsset] ca on ca.ContractNo = w.ContractNo
where w.AssignTeamId is null and w.AssignCollectorId is null
