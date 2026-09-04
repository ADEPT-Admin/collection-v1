-- View WorklistListing
CREATE OR ALTER VIEW vw_WorklistListing
AS
SELECT w.WorklistId, w.ContractNo
, pf.PrefixName + ' ' + person.FirstName + ' ' + person.LastName AS CustomerName
, ca.AssetType AS AssetType
, enum_constatus.EnumDescription as ContractStatus
, c.Bucket, c.DueDate, c.DayPastDue, c.OverdueAmount, c.OutstandingBalance
, w.AssignCollectorName as CollectorName
, w.ReassignFromName as  ReassignFrom, w.ReassignByName as  ReassignBy , w.ReassignStatusCode, w.ReassignStatusDesc, w.ReassignRequestDate
, w.AssignDate, w.FollowupStatusDesc  as FollowupStatus, w.FollowupDate as LastFollowupDate
, n.NextFollowupDate as NextFollowupDate, n.PromiseToPayDate as PromiseToPayDate
, w.AssignTeamId, w.AssignTeamCode, w.AssignTeamName
, w.AssignCollectorId, w.AssignCollectorEmpId, w.AssignCollectorName
FROM [dbo].[Worklist] w 
INNER JOIN [dbo].[Contract] c ON c.ContractNo = w.ContractNo
LEFT OUTER JOIN [dbo].[SysEnum] enum_constatus ON enum_constatus.EnumName = 'ContractStatus' and enum_constatus.EnumCode = c.ContractStatus
LEFT OUTER JOIN [dbo].[ContractPerson] person ON person.ContractNo = w.ContractNo and person.PersonType = 'B'
LEFT OUTER JOIN [dbo].[Prefix] pf ON pf.PrefixId = person.PrefixId
LEFT OUTER JOIN [dbo].[ContractAsset] ca on ca.ContractNo = w.ContractNo
LEFT OUTER JOIN CollectionNote n on n.WorklistId = w.WorklistId and n.FollowupDate = w.FollowupDate

WHERE ISNULL(ReassignStatusCode,'') <> 'WaitForApprove';