import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {

  apiRoot : string | undefined = '';
  public loginUrl: string = 'v1/Auth/login';


  public areaCodeUrl : string = 'v1/AreaCode/getlist';
  public areaCodeDetailUrl : string = 'v1/AreaCode/getbyid';
  public adjacentAreaUrl : string = 'v1/AdjacentArea/getlist';
  public adjacentAreaDetailUrl : string = 'v1/AdjacentArea/getbyid';
  public adjacentAreaUpdateUrl : string = 'v1/AdjacentArea/update';
  public employeeDetailUrl : string =  'Employee/getbyid';
  public teamCapcityListUrl : string = 'v1/ColTeamGroup/getlist';
  public teamCapcityDetailUrl : string = 'v1/ColTeamGroup/getbyid';
  public teamCapcityUpdatelUrl : string = 'v1/ColTeamGroup/update';
  public workGroupListUrl : string =  'ColWorkGroup/getlist';
  public workGroupDetailUrl : string =  'ColWorkGroup/getbyid';
  public provinceListUrl : string = 'v1/Province/getlist';
  public districtListUrl : string = 'v1/District/getlist';
  public subDistrictListUrl : string = 'v1/SubDistrict/getlist';
  public userInfoUrl : string = 'v1/User/get-user';
  public permissionTemplateUrl : string = 'v1/UserGroup/group-permission-template';
  public teamMappingAreaListUrl : string = 'v1/ColTeamMappingArea/getlist';
  public teamMappingAreaDetailUrl : string = 'v1/ColTeamMappingArea/getbyid';
  public contractListUrl : string = 'v1/Dashboard/get-contract-list';
  public dashboardContractStatusUrl : string = 'v1/Dashboard/getlist-contract-status';
  public dashboardFollowupStatusUrl : string = 'v1/Dashboard/getlist-followup-status';


  public areaLevelListUrl : string = 'v1/AreaCode/getarealevels';




  public systemParameterListUrl : string = 'v1/Parameter/list-paged';
  public systemParameterDetailUrl : string = 'v1/Parameter/get-by-id';
  public systemParameterUpdateUrl : string = 'v1/Parameter/update';

  public changepasswordUrl : string = 'v1/User/change-password';

  public changePassUrl : string = 'v1/Auth/change-password';
  public logOutUrl : string = 'v1/Auth/logout';
  public refreshTokenUrl : string = 'v1/Auth/refresh';

  public validatePasswordPolicyUrl : string = 'v1/user/validate-password-policy';
  public changePassNewUserUrl : string = 'v1/user/change-password-new-user';

  public availableLangUrl : string = 'v1/Language/available-languages'

  public getLangUrl : string = 'v1/language/get-language/';

  public getPermissionUrl : string = 'v1/User/get-user-permission';
  public userGroupListPageUrl : string = 'v1/UserGroup/list-paged';
  public userGroupListUrl : string = 'v1/UserGroup/list';
  public deleteUserGroupListUrl : string = 'v1/UserGroup/delete';





  public userGroupDetailUrl : string = 'v1/UserGroup/get-by-id';
  public userGroupCreateUrl : string = 'v1/UserGroup/create';
  public userGroupUpdateUrl : string = 'v1/UserGroup/update';

  public userListUrl : string = 'v1/User/list-paged';
  public userDetailUrl : string = 'v1/User/get-by-id';
  public createUserUrl : string = 'v1/User/create';
  public deleteUserUrl : string = 'v1/User/delete';
  public updateUserUrl : string = 'v1/User/update';
  public unlockUserUrl : string = 'v1/User/unlock-user';
  public resetpasswordUserUrl : string = 'v1/User/reset-password';


  public employeeListUrl : string =  'v1/Employee/list-paged';
  public employeegetbyidUrl : string = 'v1/Employee/get-by-id';
  public workListPageUrl : string = 'v1/Worklist/list-paged';
  public workDetailUrl : string = 'v1/worklist/detail/';
  public contractDetailUrl : string = 'v1/contract/detail/';
  public overdueDetailUrl : string = 'v1/Contract/overdues/';
  public collectionNoteUrl : string = 'v1/Contract/note-list-paged/';

  public contractPersonsUrl : string = 'v1/Contract/persons';
  public contractphonelistUrl : string = 'v1/Contract/phone-list-paged';
  public contractnotelistUrl : string = 'v1/Contract/note-list-paged';
  public contractAddresslistUrl : string = 'v1/Contract/address-list-paged';
  public contractpaymentlistUrl : string = 'v1/Contract/payment-list-paged';
  public enumFollowupStatusUrl : string = 'v1/enum/list-by-enum-name';
  public createCollectionNoteUrl : string = 'v1/CollectionNote/create';

  public followupActionUrl : string = 'v1/worklist/followup-action-list';
  public followupResultUrl : string = 'v1/worklist/followup-result-list';

  public collectorListUrl : string = 'v1/Collector/list-paged';
  public collectorDetailUrl : string = 'v1/Collector/get-by-id';
  public collectorUpdateUrl : string = 'v1/Collector/update';
  public collectorDeleteUrl : string = 'v1/Collector/delete';
  public collectorCreateUrl : string = 'v1/Collector/create';

  public collectorRoleListUrl : string = 'v1/CollectorRole/list';
  public collectorTeamListUrl : string = 'v1/CollectorTeam/list';


  public collectorRoleDetailUrl : string = 'v1/CollectionRole/getbyid';
  public collectorRoleUpdateUrl : string = 'v1/CollectionRole/update';


  public teamListUrl : string = 'v1/CollectorTeam/list';
  public teamListPagedUrl : string = 'v1/CollectorTeam/list-paged';
  public teamDetailUrl : string = 'v1/CollectorTeam/get-by-id';
  public teamUpdateUrl : string = 'v1/CollectorTeam/update';
  public teamCreateUrl : string = 'v1/CollectorTeam/create';
  public teamDeleteUrl : string = 'v1/CollectorTeam/delete';

  public areaCodeListUrl : string = 'v1/ColArea/list';
  public LanguageListUrl : string = 'v1/Language/list-paged';
  public LanguageDetailUrl : string = 'v1/Language/get-by-id';
  public LanguageUpdateUrl : string = 'v1/Language/bulk-update';
  public LanguageResetUrl : string = 'v1/Language/reset-default';
  public MenuListPageUrl : string = 'v1/Menu/list-paged';
  public MenuDetailUrl : string = 'v1/Menu/get-by-id';
  public updateMenuDetailUrl : string = 'v1/Menu/update';
  public MenuListUrl : string = 'v1/Menu/list';


  public teamAssignmentListPagedUrl : string = 'v1/TeamAssignment/list-paged';
  public teamAssignmentDetailUrl : string = 'v1/TeamAssignment/get-by-id';
  public teamAssignmentUpdateUrl : string = 'v1/TeamAssignment/update';
  public teamAssignmentCreateUrl : string = 'v1/TeamAssignment/create';
  public teamAssignmentDeleteUrl : string = 'v1/TeamAssignment/delete';
  public teamAssignmentCollectorInTeamUrl : string = 'v1/TeamAssignment/get-collector-in-team/';
  public approvereassignlistUrl : string = 'v1/Worklist/approve-reassign-list-paged';
  public manualResaaignUrl : string = 'v1/Worklist/manual-reassign';
  public approvereassignUrl : string = 'v1/Worklist/approve-reassign';
  public unAssignWorkListPageUrl : string = 'v1/Worklist/unassign-worklist-list-paged';
  public userbulkcreateUrl : string = 'v1/user/bulk-create';
  public listassigncollectorUrl : string = 'v1/Collector/list-paged-assign-collector';
  public bulkassignTeamAssignmentUrl : string = 'v1/TeamAssignment/bulk-assign-team';

  public bulkCreatecollectorUrl : string = 'v1/collector/bulk-create';
  public teamAssignmentlistpagedbycollectorUrl : string = 'v1/TeamAssignment/list-paged-by-collector';




  public contractlookuppersonsUrl : string = 'v1/Contract/lookup-persons/';
  public contractlookupPhonesUrl : string = 'v1/Contract/lookup-phones-by-person/';

  public TeamAssignmentlistpagedbyteamUrl : string = 'v1/TeamAssignment/list-paged-assign-by-team';




  public setting : any = {};


  constructor() {}
}
