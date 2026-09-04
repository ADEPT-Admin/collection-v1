CREATE TRIGGER [dbo].[tr_WorkList_History]
ON [dbo].[WorkList]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    ------------------------------------------------------
    -- 1) INSERT (rows existing only in inserted)
    ------------------------------------------------------
    INSERT INTO dbo.WorkListHistory (
        Id, WorklistId, AssignDate, ContractNo, JobTypeCode, JobTypeDesc,
        RiskLevel, AssignTool, AssignAreaId, AssignAreaCode,
        AssignTeamId, AssignTeamCode, AssignTeamName,
        AssignCollectorId, AssignCollectorEmpId, AssignCollectorName,
        AssignTypeCode, AssignTypeDesc,
        ReassignById, ReassignByEmpId, ReassignByName,
        ReassignRequestDate,
        ReassignFromId, ReassignFromEmpId, ReassignFromName,
        ReassignToId, ReassignToEmpId, ReassignToName,
        ReassignToTeamId, ReassignToTeamCode, ReassignToTeamName,
        ReassignReason, ReassignStatusCode, ReassignStatusDesc,
        ApprovedById, ApprovedEmpId, ApprovedName,
        ApproveReason, ApproveDate,
        FollowupStatusCode, FollowupStatusDesc, FollowupDate,
        DoNotCallFlag,
        CreatedBy, CreatedDate, UpdatedBy, UpdatedDate,
        HistoryActionType,
        RecordedTimestamp
    )
    SELECT 
        NEWID(), i.WorklistId, i.AssignDate, i.ContractNo, i.JobTypeCode, i.JobTypeDesc,
        i.RiskLevel, i.AssignTool, i.AssignAreaId, i.AssignAreaCode,
        i.AssignTeamId, i.AssignTeamCode, i.AssignTeamName,
        i.AssignCollectorId, i.AssignCollectorEmpId, i.AssignCollectorName,
        i.AssignTypeCode, i.AssignTypeDesc,
        i.ReassignById, i.ReassignByEmpId, i.ReassignByName,
        i.ReassignRequestDate,
        i.ReassignFromId, i.ReassignFromEmpId, i.ReassignFromName,
        i.ReassignToId, i.ReassignToEmpId, i.ReassignToName,
        i.ReassignToTeamId, i.ReassignToTeamCode, i.ReassignToTeamName,
        i.ReassignReason, i.ReassignStatusCode, i.ReassignStatusDesc,
        i.ApprovedById, i.ApprovedEmpId, i.ApprovedName,
        i.ApproveReason, i.ApproveDate,
        i.FollowupStatusCode, i.FollowupStatusDesc, i.FollowupDate,
        i.DoNotCallFlag,
        i.CreatedBy, i.CreatedDate, i.UpdatedBy, i.UpdatedDate,
        'INSERT',
        GETDATE()
    FROM inserted i
    LEFT JOIN deleted d ON i.WorklistId = d.WorklistId
    WHERE d.WorklistId IS NULL;


    ------------------------------------------------------
    -- 2) UPDATE (rows exist in both inserted and deleted)
    ------------------------------------------------------
    INSERT INTO dbo.WorkListHistory (
        Id, WorklistId, AssignDate, ContractNo, JobTypeCode, JobTypeDesc,
        RiskLevel, AssignTool, AssignAreaId, AssignAreaCode,
        AssignTeamId, AssignTeamCode, AssignTeamName,
        AssignCollectorId, AssignCollectorEmpId, AssignCollectorName,
        AssignTypeCode, AssignTypeDesc,
        ReassignById, ReassignByEmpId, ReassignByName,
        ReassignRequestDate,
        ReassignFromId, ReassignFromEmpId, ReassignFromName,
        ReassignToId, ReassignToEmpId, ReassignToName,
        ReassignToTeamId, ReassignToTeamCode, ReassignToTeamName,
        ReassignReason, ReassignStatusCode, ReassignStatusDesc,
        ApprovedById, ApprovedEmpId, ApprovedName,
        ApproveReason, ApproveDate,
        FollowupStatusCode, FollowupStatusDesc, FollowupDate,
        DoNotCallFlag,
        CreatedBy, CreatedDate, UpdatedBy, UpdatedDate,
        HistoryActionType,
        RecordedTimestamp
    )
    SELECT
        NEWID(), i.WorklistId, i.AssignDate, i.ContractNo, i.JobTypeCode, i.JobTypeDesc,
        i.RiskLevel, i.AssignTool, i.AssignAreaId, i.AssignAreaCode,
        i.AssignTeamId, i.AssignTeamCode, i.AssignTeamName,
        i.AssignCollectorId, i.AssignCollectorEmpId, i.AssignCollectorName,
        i.AssignTypeCode, i.AssignTypeDesc,
        i.ReassignById, i.ReassignByEmpId, i.ReassignByName,
        i.ReassignRequestDate,
        i.ReassignFromId, i.ReassignFromEmpId, i.ReassignFromName,
        i.ReassignToId, i.ReassignToEmpId, i.ReassignToName,
        i.ReassignToTeamId, i.ReassignToTeamCode, i.ReassignToTeamName,
        i.ReassignReason, i.ReassignStatusCode, i.ReassignStatusDesc,
        i.ApprovedById, i.ApprovedEmpId, i.ApprovedName,
        i.ApproveReason, i.ApproveDate,
        i.FollowupStatusCode, i.FollowupStatusDesc, i.FollowupDate,
        i.DoNotCallFlag,
        i.CreatedBy, i.CreatedDate, i.UpdatedBy, i.UpdatedDate,
        'UPDATE',
        GETDATE()        
    FROM inserted i
    INNER JOIN deleted d ON i.WorklistId = d.WorklistId;


    ------------------------------------------------------
    -- 3) DELETE (rows only in deleted)
    ------------------------------------------------------
    INSERT INTO dbo.WorkListHistory (
        Id, WorklistId, AssignDate, ContractNo, JobTypeCode, JobTypeDesc,
        RiskLevel, AssignTool, AssignAreaId, AssignAreaCode,
        AssignTeamId, AssignTeamCode, AssignTeamName,
        AssignCollectorId, AssignCollectorEmpId, AssignCollectorName,
        AssignTypeCode, AssignTypeDesc,
        ReassignById, ReassignByEmpId, ReassignByName,
        ReassignRequestDate,
        ReassignFromId, ReassignFromEmpId, ReassignFromName,
        ReassignToId, ReassignToEmpId, ReassignToName,
        ReassignToTeamId, ReassignToTeamCode, ReassignToTeamName,
        ReassignReason, ReassignStatusCode, ReassignStatusDesc,
        ApprovedById, ApprovedEmpId, ApprovedName,
        ApproveReason, ApproveDate,
        FollowupStatusCode, FollowupStatusDesc, FollowupDate,
        DoNotCallFlag,
        CreatedBy, CreatedDate, UpdatedBy, UpdatedDate,
        HistoryActionType,
        RecordedTimestamp
    )
    SELECT
        NEWID(), d.WorklistId, d.AssignDate, d.ContractNo, d.JobTypeCode, d.JobTypeDesc,
        d.RiskLevel, d.AssignTool, d.AssignAreaId, d.AssignAreaCode,
        d.AssignTeamId, d.AssignTeamCode, d.AssignTeamName,
        d.AssignCollectorId, d.AssignCollectorEmpId, d.AssignCollectorName,
        d.AssignTypeCode, d.AssignTypeDesc,
        d.ReassignById, d.ReassignByEmpId, d.ReassignByName,
        d.ReassignRequestDate,
        d.ReassignFromId, d.ReassignFromEmpId, d.ReassignFromName,
        d.ReassignToId, d.ReassignToEmpId, d.ReassignToName,
        d.ReassignToTeamId, d.ReassignToTeamCode, d.ReassignToTeamName,
        d.ReassignReason, d.ReassignStatusCode, d.ReassignStatusDesc,
        d.ApprovedById, d.ApprovedEmpId, d.ApprovedName,
        d.ApproveReason, d.ApproveDate,
        d.FollowupStatusCode, d.FollowupStatusDesc, d.FollowupDate,
        d.DoNotCallFlag,
        d.CreatedBy, d.CreatedDate, d.UpdatedBy, d.UpdatedDate,
        'DELETE',
        GETDATE()
    FROM deleted d
    LEFT JOIN inserted i ON i.WorklistId = d.WorklistId
    WHERE i.WorklistId IS NULL;

END;
