namespace SharedKernel.CommonConstants
{
    public static class CommonConstants
    {
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        public const string DateFormat = "dd/MM/yyyy";
    }

    public static class FilterType
    {
        public const string STRING = "STRING";
        public const string INTEGER = "INTEGER";
        public const string DECIMAL = "DECIMAL";
        public const string DATETIME = "DATETIME";
        public const string DATE = "DATE";
        public const string ENUM = "ENUM";
        public const string GUID = "GUID";
    }

    public static class IconType
    {
        public const string CHECKGREEN = "<i class=\"fa-solid fa-circle-check\" style=\"color:green\"></i>";
        public const string CHECKRED = "<i class=\"fa-solid fa-circle-xmark\" style=\"color:red\"></i>";
    }
    public static class ActivityLogAction
    {
        public static readonly string VIEW = "VIEW";
        public static readonly string CREATE = "CREATE";
        public static readonly string UPDATE = "UPDATE";
        public static readonly string DELETE = "DELETE";
        public static readonly string LOGIN = "LOGIN";
        public static readonly string LOGOUT = "LOGOUT";
        public static readonly string CHANGE_PASSWPRD = "CHANGE_PASSWPRD";
        public static readonly string RESET_PASSWORD = "RESET_PASSWORD";
        public static readonly string FORGOT_PASSWORD = "FORGOT_PASSWORD";
        public static readonly string UNLOCK_USER = "UNLOCK_USER";
        public static readonly string UPLOAD = "UPLOAD";
        public static readonly string DOWNLOAD = "DOWNLOAD";
        public static readonly string EXPORT = "EXPORT";
        public static readonly string IMPORT = "IMPORT";
    }
    public static class AuthLogStatus
    {
        public static readonly string LOGIN_SUCCESS = "LOGIN_SUCCESS";
        public static readonly string LOGIN_FAILED = "LOGIN_FAILED";
        public static readonly string LOGOUT = "LOGOUT";
        public static readonly string SESSION_EXPIRED = "SESSION_EXPIRED";
        public static readonly string SESSION_REVOKE = "SESSION_REVOKED";
        public static readonly string TOKEN_EXPIRED = "TOKEN_EXPIRED";
        public static readonly string TOKEN_VALID = "TOKEN_VALID";
        public static readonly string TOKEN_INVALID = "TOKEN_INVALID";
    }
    public static class ActivityLogStatus
    {
        public static readonly string SUCCESS = "SUCCESS";
        public static readonly string FAILED = "FAILED";
        public static readonly string WARNING = "WARNING";
    }
    public static class Message
    {
        public static readonly string Msg_UserAccessDenied = "Msg_UserAccessDenied";
        public static readonly string Msg_SessionExpiredandRevoked = "Msg_SessionExpiredandRevoked";
        public static readonly string Msg_InvalidInput = "Msg_InvalidInput";
        public static readonly string Msg_RefreshTokenRevokedSuccessfully = "Msg_RefreshTokenRevokedSuccessfully";
        public static readonly string Msg_InvalidRefreshToken = "Msg_InvalidRefreshToken";
        public static readonly string Msg_RefreshTokenReuseDetected = "Msg_RefreshTokenReuseDetected";
        public static readonly string Msg_TokenRefreshedSuccessfully = "Msg_TokenRefreshedSuccessfully";
        public static readonly string Msg_UserAccountSuspended = "Msg_UserAccountSuspended";
        public static readonly string Msg_UserAccountLocked = "Msg_UserAccountLocked";
        public static readonly string Msg_UserAccountEffectiveDateNotSet = "Msg_UserAccountEffectiveDateNotSet";
        public static readonly string Msg_UserAccountEffectiveOn = "Msg_UserAccountEffectiveOn";
        public static readonly string Msg_UserAccountExpired = "Msg_UserAccountExpired";
        public static readonly string Msg_PasswordIncorrectMissingLocked = "Msg_PasswordIncorrectMissingLocked";
        public static readonly string Msg_PasswordIncorrectMissing = "Msg_PasswordIncorrectMissing";
        public static readonly string Msg_LoginSuccessful = "Msg_LoginSuccessful";
        public static readonly string Msg_NoFileUploaded = "Msg_NoFileUploaded";
        public static readonly string Msg_DuplicateKeysInRequest = "Msg_DuplicateKeysInRequest";
        public static readonly string Msg_DuplicateDataInColumn = "Msg_DuplicateDataInColumn";
        public static readonly string Msg_InvalidRequestData = "Msg_InvalidRequestData";
        public static readonly string Msg_CollectorTeamRefCollector = "Msg_CollectorTeamRefCollector";
        public static readonly string Msg_EmployeeAlreadyUse = "Msg_EmployeeAlreadyUse";
        public static readonly string Msg_EmployeeIdsAlreadyUse = "Msg_EmployeeIdsAlreadyUse"; 
        public static readonly string Msg_UsernameExist = "Msg_UsernameExist";
        public static readonly string Msg_CannotDeleteOwnAccount = "Msg_CannotDeleteOwnAccount";
        public static readonly string Msg_NewandConfirmPasswordNotMatch = "Msg_NewandConfirmPasswordNotMatch";
        public static readonly string Msg_CurrentPasswordIncorrect = "Msg_CurrentPasswordIncorrect.";
        public static readonly string Msg_PasswordMeetsAllPolicy = "Msg_PasswordMeetsAllPolicy";
        public static readonly string Msg_PolicyNameUpperLowerNonalpha = "Msg_PolicyNameUpperLowerNonalpha";
        public static readonly string Msg_NewPassWordCannotBeTheSame = "Msg_NewPassWordCannotBeTheSame";
        public static readonly string Msg_PasswordChangedSuccess = "Msg_PasswordChangedSuccess";
        public static readonly string Msg_InvalidData = "Msg_InvalidData";
        public static readonly string Msg_UserGroupCodeorNameExists = "Msg_UserGroupCodeorNameExists";
        public static readonly string Msg_NoDataFound = "Msg_NoDataFound";
        public static readonly string Msg_UserGroupCannotDelete = "Msg_UserGroupCannotDelete";
        public static readonly string Msg_CollectorAlreadyUse = "Msg_CollectorAlreadyUse";
        public static readonly string Msg_InvalidRequest = "Msg_InvalidRequest";
        public static readonly string Msg_CollectorRoleExists = "Msg_CollectorRoleExists";
        public static readonly string Msg_CollectorRoleIdRefCollector = "Msg_CollectorRoleIdRefCollector";
        public static readonly string Msg_AccessDenyColOrTeamInactive = "Msg_AccessDenyColOrTeamInactive";
        public static readonly string Msg_ValidationFailed = "Msg_ValidationFailed";
        public static readonly string Msg_LogoutSuccessful = "Msg_LogoutSuccessful";
        public static readonly string Msg_SessionExpired = "Msg_SessionExpired";
        public static readonly string Msg_ItemIdAlreadyExists = "Msg_ItemIdAlreadyExists";
        public static readonly string Msg_ItemNameAlreadyExists = "Msg_ItemNameAlreadyExists";
        public static readonly string Msg_NotFound = "Msg_NotFound";
        public static readonly string Msg_2ParamsNotFound = "Msg_2ParamsNotFound";
        public static readonly string Msg_DuplicateCollectorInTeamAssignment = "Msg_DuplicateCollectorInTeamAssignment";
        public static readonly string Msg_SavedSuccessfully = "Msg_SavedSuccessfully";
        public static readonly string Msg_DeletedSuccessfully = "Msg_DeletedSuccessfully";
        public static readonly string Msg_PasswordChangedSuccessfully = "Msg_PasswordChangedSuccessfully";
        public static readonly string Msg_UserUnlockedSuccessfully = "Msg_UserUnlockedSuccessfully";
        public static readonly string Msg_PasswordResetSuccessfully = "Msg_PasswordResetSuccessfully";
        public static readonly string Msg_LanguageResetSuccessfully = "Msg_LanguageResetSuccessfully";
        public static readonly string Msg_LoginSuccessfully = "Msg_LoginSuccessfully";
        public static readonly string Msg_LogoutSuccessfully = "Msg_LogoutSuccessfully";
        public static readonly string Msg_CollectorExist = "Msg_CollectorExist";
        public static readonly string Msg_SupervisorAlreadyAssigned = "Msg_SupervisorAlreadyAssigned";
        public static readonly string Msg_ComfirmOverwriteSupervisor = "Msg_ComfirmOverwriteSupervisor";
        public static readonly string Msg_NotHavePermissionToAccessContract = "Msg_NotHavePermissionToAccessContract";
        public static readonly string Msg_NotHavePermissionToAccessCollectorList = "Msg_NotHavePermissionToAccessCollectorList";
        public static readonly string Msg_CrossTeamReasignmentNotAllow = "Msg_CrossTeamReasignmentNotAllow";
        public static readonly string Msg_ReassignmentCompleted = "Msg_ReassignmentCompleted";
        public static readonly string Msg_ApprovedSuccessfully = "Msg_ApprovedSuccessfully";
        public static readonly string Msg_RejectedSuccessfully = "Msg_RejectedSuccessfully";
        public static readonly string Msg_NoPermissiontoApproveContactID = "Msg_NoPermissiontoApproveContactID";
        public static readonly string Msg_CannotDeleteCollectorExistTeam = "Msg_CannotDeleteCollectorExistTeam";
        public static readonly string Msg_AtLeastOneRequired = "Msg_AtLeastOneRequired";
        public static readonly string Msg_EffectiveDateMustBeLessThanExpireDate = "Msg_EffectiveDateMustBeLessThanExpireDate";
        public static readonly string Msg_Inactive = "Msg_Inactive";
        public static readonly string Msg_ExistingWorklistWithTeamAssignment = "Msg_ExistingWorklistWithTeamAssignment";
        public static readonly string Msg_InvalidCapacity = "Msg_InvalidCapacity";
        public static readonly string Msg_CollectorExistByEmployeeId = "Msg_CollectorExistByEmployeeId"; 
        public static readonly string Msg_CannotDeleteUserWithExistCollectorFromUserName = "Msg_CannotDeleteUserWithExistCollectorFromUserName";
        public static readonly string TotalAmountDue = "TotalAmountDue";
    }
    public static class ModeType
    {
        public static readonly string Page = "PAGE";
        public static readonly string Dialog = "DIALOG";
    }
    public static class ExecutionStatus
    {
        public static readonly string RUNNING = "RUNNING";
        public static readonly string SUCCESS = "SUCCESS";
        public static readonly string FAILED = "FAILED";
        public static readonly string PARTIAL = "PARTIAL";
    }
    public static class ExecutionJobType
    {
        public static readonly string IMPORT = "IMPORT";
        public static readonly string SYNC = "SYNC";
        public static readonly string ASSIGNMENT = "ASSIGNMENT";
    }
    public static class ExecutionSourceSystem
    {
        public static readonly string CSV = "CSV";
        public static readonly  string API = "API";
    }
    public static class PersonType
    {
        public static readonly string BORROWER = "Borrower";
        public static readonly string GUARANTOR = "Guarantor";
    }
}

