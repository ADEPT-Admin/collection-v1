import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject , lastValueFrom } from 'rxjs';
import { ConfigService } from 'src/app/Shared/Services/config.service';

export interface User{
  id?: number;
  userId?: string;
  employee?: any;
  employeeName?: string;
  employeeId?: string;
  userName?: string;
  userGroupIds?: any;
  userGroup?: string;
  expiredDate?: Date;
  lastSignOnDate?: Date;
  isActive?: boolean;
  isLockUser?: boolean;
  isNewUser?: boolean;
  phone?: string;
  branch?: any[];
  changePassAfterFirst?: boolean;
  userGroups?: UserGroup[];
  effectiveDate?: any;
  updatedDate?: Date;
  updatedBy?: string;
  forceChangePassword?: any;
  expireDate?:any;
}


export interface UserGroup {
  userGroupId?: number;
  userGroupCode?: string;
  userGroupName?: string;
  description?: string;
  isActive?: boolean;
}


export interface Role{
  id?: number;
  company?: string;
  companyName?: string;
  code?: string;
  roleName?: string;
  isActive?: boolean;
  permission?: Permission[];
  updatedDate?: Date;
  updatedBy?: string;

}


export interface Menu{
  id?: string;
  main? : string;
  itemLevel?: number;
  sub? : Permission[];
}

export interface Permission{
  id?: string;
  parentId?: string;
  function?: string;
  access?: Access;
}


export interface Access{
  allowNew?: boolean;
  allowSave?: boolean;
  allowDelete?: boolean;
  allowCancel?: boolean;
  allowQuery?: boolean;
  allowPrint?: boolean;
}

export const roles = ['Admin', 'User', 'Manager', 'Viewer'];
export const statuses = ['Active', 'Inactive', 'Suspended'];


//// Team ////

export interface Team{
  teamId?: string;
  teamName?: string;
  isActive?: string;
}

export interface teamMappingArea{
  id?: string;
  teamId?: string;
  team?: string;
  areaId?: string;
  area?: string;
}


export interface Employee {
  employeeId?: string;
  employeeName?: string;
  prefix?:any;
  phoneNo?:string;
  prefixId?: number;
  prefixName?: string;
  firstName?: string;
  lastName?: string;
  firstNameEn?: string;
  lastNameEn?: string;
  fullName?: string;
  fullNameEn?: string;
  email?: string;
  branchName?: string;
  managerId?: string;
  managerName?: string;
  nickName?: string;
  gender?: string;
  dateOfBirth?: Date;
  age?: number;
  nationalID?: string;
  maritalStatus?: string;
  nationality?: string;
  religion?: string;
  workStatus?: string;
  currentAddressId?: number;
  currentAddresses?: any;
  registrationAddressId?: number;
  registrationAddresses?: any;
  startWorkingDate?: Date;
  probationDate?: Date;
  workEffectiveDate?: Date;
  terminationDate?: Date;
  teamId?: number;
  team?: any;
  departmentId?: number;
  department?: {
    departmentId?: number;
    departmentCode?: string;
    departmentName?: string;
  };
  divisionId?: number;
  division?: any;
  positionId?: number;
  position?: {
    positionId?: number;
    positionCode?: string;
    thaiDesc?: string;
  };
  salary?: number;
  salaryEffectiveDate?: Date;
  manager?: any;
  subordinates?: any[];
  managerFullName?: string;
}

  export interface Collector {
    collectorId?: string;
    collectorName?: string;
    colTeamGroupId?: number;
    capacity?: number;
    isActive?: boolean;
    colTeamGroupDisplay?: string;
  }

export interface GroupPermissionItem {
  itemId?: string;
  parentId?: string;
  userGroupId?: number;
  itemNameTh?: string;
  itemNameEn?: string;
  routePath?: string;
  itemOrder?: number;
  itemLevel?: number;
  description?: string;
  isActive?: boolean;
  allowNew?: boolean;
  allowEdit?: boolean;
  allowDelete?: boolean;
  allowCancel?: boolean;
  allowQuery?: boolean;
  allowPrint?: boolean;
  allowAccess?: boolean;
}
export interface TeamMappingArea{
  colTeamMappingID?: string;
  collectorTeamID?: string;
  areaCodeID?: string;
}

export interface SystemParameter {
  id?: number;
  parameterCategory?: string;
  parameterName?: string;
  value?: string;
  description?: string;
}

export interface Language {
  key?: string;
  value?: any;
}

export interface MasterMenu {
  value?: any;
  icon?: any;
  isActive?: boolean;
  itemId?: any;
  itemLevel?: any;
  itemName?: any;
  itemOrder?: any;
  parentId?: any;
  routeName?: string;
  toolTip?: string;
}

export interface employee {
  employeeId?: any;
  employeeName?: string;
  department?: string;
  position?: string;
  supervisorName?: string;
  email?: string;
  workStatus?: string;
  lastUpdated?: any;
  status?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class SecurityService {
  token = sessionStorage.getItem("token");
  usersSubject : BehaviorSubject<User[]> = new BehaviorSubject<User[]>([]);
  userSubject : BehaviorSubject<User> = new BehaviorSubject<User>({});
  userGroupsSubject : BehaviorSubject<UserGroup[]> = new BehaviorSubject<UserGroup[]>([]);
  userGroupSubject : BehaviorSubject<UserGroup> = new BehaviorSubject<UserGroup>({});
  employeesSubject : BehaviorSubject<Employee[]> = new BehaviorSubject<Employee[]>([]);
  employeeSubject : BehaviorSubject<Employee> = new BehaviorSubject<Employee>({});
  teamsSubject : BehaviorSubject<Team[]> = new BehaviorSubject<Team[]>([]);
  teamSubject : BehaviorSubject<Team> = new BehaviorSubject<Team>({});
  collectorsSubject : BehaviorSubject<Collector[]> = new BehaviorSubject<Collector[]>([]);
  collectorSubject : BehaviorSubject<Collector> = new BehaviorSubject<Collector>({});
  teamMappingAreaSubject : BehaviorSubject<TeamMappingArea[]> = new BehaviorSubject<TeamMappingArea[]>([]);
  teamMappingAreaDetailSubject : BehaviorSubject<TeamMappingArea> = new BehaviorSubject<TeamMappingArea>({});
  SysParametersSubject : BehaviorSubject<SystemParameter[]> = new BehaviorSubject<SystemParameter[]>([]);
  SysParameterDetailSubject : BehaviorSubject<SystemParameter> = new BehaviorSubject<SystemParameter>({});
  LanguageListSubject : BehaviorSubject<Language[]> = new BehaviorSubject<Language[]>([]);
  LanguageDetailSubject : BehaviorSubject<Language[]> = new BehaviorSubject<Language[]>([]);
  menuListSubject : BehaviorSubject<MasterMenu[]> = new BehaviorSubject<MasterMenu[]>([]);
  menuDetailSubject : BehaviorSubject<MasterMenu[]> = new BehaviorSubject<MasterMenu[]>([]);
  constructor(
    private configService : ConfigService,
    private http : HttpClient
  ) { }


  getUserList(){
      return this.usersSubject.asObservable();
  }

  async getUsers(payload = {} , isActiveUser = false): Promise<User[]>{
    const url = `${this.configService.apiRoot}${this.configService.userListUrl}${isActiveUser==true ? '?isActiveuser=true' : ''}`;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response || [];
    this.userSubject.next(data);
    return data;
  }

  async getUser(id: any): Promise<any> {
    try {
      const url = `${this.configService.apiRoot}${this.configService.userDetailUrl}/${id}`;
      const headers = new HttpHeaders().set('Authorization', `Bearer ${this.token}`);
      const response = await lastValueFrom(this.http.get<any>(url, { headers: headers }));
      const data = response || {};
      this.userSubject.next(data);
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message };
    }
  }

  async createUser(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.createUserUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async createMultipleUser(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.userbulkcreateUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async updateUser(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.updateUserUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async unlockUser(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.unlockUserUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async resetpasswordUser(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.resetpasswordUserUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async deleteUser(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.deleteUserUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  getUserGroupList(){
      return this.userGroupsSubject.asObservable();
  }

  async getUserGroups(payload = {}): Promise<UserGroup[]>{
    const url = this.configService.apiRoot + this.configService.userGroupListPageUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response || [];
    this.userGroupsSubject.next(data);
    return data;

  }

  async getGroupList(){
    const url = `${this.configService.apiRoot}${this.configService.userGroupListUrl}?status=${true}`;
    const response = await lastValueFrom(this.http.get<any>(url));
    const data = response || [];
    return data;
  }

  getUserGroupDetail(){
      return this.userGroupSubject.asObservable();
  }

  async getUserGroup(id): Promise<any> {
    try{
      const url = `${this.configService.apiRoot}${this.configService.userGroupDetailUrl}\\${id || ''}`;
      const response = await lastValueFrom(this.http.get<any>(url));
      const data = response || [];
      return data;
    }catch(error){
      return {status : false , message : (error as any).error.message}
    }
  }

  async createUserGroup(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.userGroupCreateUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async updateUserGroup(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.userGroupUpdateUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }


  getEmployeeList(){
      return this.employeesSubject.asObservable();
  }

  async getEmployees(payload = {}): Promise<Employee[]>{
    const url = this.configService.apiRoot + this.configService.employeeListUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response || [];
    this.employeesSubject.next(data);
    return data;

  }

  async getEmployeegetbyid(id: any): Promise<any> {
    try {
      const url = `${this.configService.apiRoot}${this.configService.employeegetbyidUrl}/${id}`;
      const headers = new HttpHeaders().set('Authorization', `Bearer ${this.token}`);
      const response = await lastValueFrom(this.http.get<any>(url, { headers: headers }));
      const data = response || {};
      this.employeesSubject.next(data);
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message };
    }
  }

  getEmployeeDetail(){
      return this.employeeSubject.asObservable();
  }

  async getEmployee(payload = {}): Promise<Employee> {
    const url = this.configService.apiRoot + this.configService.employeeDetailUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    this.employeeSubject.next(data);
    return data;

  }


  async getPermissionTeamplete(payload = {}): Promise<GroupPermissionItem[]>{
    const url = this.configService.apiRoot + this.configService.permissionTemplateUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    return data;
  }

  getTeamList(){
      return this.teamsSubject.asObservable();
  }

  async getTeams(payload = {}): Promise<Team[]>{
    const url = this.configService.apiRoot + this.configService.teamListUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    this.teamsSubject.next(data);
    return data;
  }

  getTeamDetail(){
      return this.teamSubject.asObservable();
  }
  async getTeamById(payload = {}): Promise<Team>{
    const url =  this.configService.apiRoot + this.configService.teamDetailUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    this.teamSubject.next(data);
    return data;

  }

  async updateTeam(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.teamUpdateUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  getSysParameterList(){
      return this.SysParametersSubject.asObservable();
  }

  async getSysParameters(payload = {}): Promise<SystemParameter[]> {
  const url = this.configService.apiRoot + this.configService.systemParameterListUrl;
  const headers = new HttpHeaders({'Authorization': 'Bearer ' + this.token});
  const response = await lastValueFrom(this.http.post<any>(url, payload, { headers })
  );
  const data = response || {};
  this.SysParametersSubject.next(data);
  return data;
}

  getSysParameterDetail(){
      return this.SysParameterDetailSubject.asObservable();
  }

async getSysParameter(id: number): Promise<any> {
  try{
    const url = `${this.configService.apiRoot}${this.configService.systemParameterDetailUrl}/${id}`;
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.token}`);
    const response = await lastValueFrom(this.http.get<any>(url, { headers: headers }));
    const data = response || {};
    this.SysParameterDetailSubject.next(data);
    return data;
  }catch(error){
    return {status : false , message : (error as any).error.message}
  }
}

  async updateSysParameter(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.systemParameterUpdateUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  getCollectorList(){
      return this.collectorsSubject.asObservable();
  }
  async getCollectors(payload = {}): Promise<Collector[]>{
    const url = this.configService.apiRoot + this.configService.collectorListUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    this.collectorsSubject.next(data);
    return data;
  }

  getCollectorDetail(){
      return this.collectorSubject.asObservable();
  }
  async getCollector(payload = {}): Promise<Collector>{
    const url =  this.configService.apiRoot + this.configService.collectorDetailUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    this.collectorSubject.next(data);
    return data;
  }

  async getteamMappingArea(payload = {}): Promise<TeamMappingArea[]>{
    const url = this.configService.apiRoot + this.configService.teamMappingAreaListUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    this.teamMappingAreaSubject.next(data);
    return data;
  }

  async getDetailTeamMappingArea(payload = {}): Promise<TeamMappingArea> {
    const url = this.configService.apiRoot + this.configService.teamMappingAreaDetailUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    this.teamMappingAreaDetailSubject.next(data);
    return data;

  }

  async saveCollector(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.collectorUpdateUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async changepassword(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.changepasswordUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async deleteUserGroup(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.deleteUserGroupListUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async getLanguageList(payload = {}): Promise<any>{
    const url = this.configService.apiRoot + this.configService.LanguageListUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response || [];
    this.LanguageListSubject.next(data);
    return data;
  }

  async getLanguageDetail(key: any): Promise<any> {
    try {
      const url = `${this.configService.apiRoot}${this.configService.LanguageDetailUrl}/${key}`;
      const headers = new HttpHeaders().set('Authorization', `Bearer ${this.token}`);
      const response = await lastValueFrom(this.http.get<any>(url, { headers: headers }));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async updateLanguage(payload = {}): Promise<any>{
    try{
      const url = this.configService.apiRoot + this.configService.LanguageUpdateUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || [];
      return data;
    }catch(error){
      return {status : false , message : (error as any).error.message}
    }
  }

  async resetLanguage(key: any): Promise<any>{
    try{
      const url = `${this.configService.apiRoot}${this.configService.LanguageResetUrl}/${key}`;
      const headers = new HttpHeaders().set('Authorization', `Bearer ${this.token}`);
      const response = await lastValueFrom(this.http.post<any>(url, { headers: headers }));
      const data = response || {};
      return data;
    }catch(error){
      return {status : false , message : (error as any).error.message}
    }
  }

  async getMenuListUrl(): Promise<any>{
    try{
      const url = this.configService.apiRoot + this.configService.MenuListUrl;
      const response = await lastValueFrom(this.http.get<any>(url));
      const data = response || [];
      return data;
    }catch(error){
      return {status : false , message : (error as any).error.message}
    }
  }

  async getMenuListPageUrl(payload = {}): Promise<any>{
    try{
      const url = this.configService.apiRoot + this.configService.MenuListPageUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || [];
      return data;
    }catch(error){
      return {status : false , message : (error as any).error.message}
    }
  }

  async getMenuDetail(key: any): Promise<any> {
    try{
      const url = `${this.configService.apiRoot}${this.configService.MenuDetailUrl}/${key}`;
      const headers = new HttpHeaders().set('Authorization', `Bearer ${this.token}`);
      const response = await lastValueFrom(this.http.get<any>(url, { headers: headers }));
      const data = response || {};
      return data;
    }catch(error){
      return {status : false , message : (error as any).error.message}
    }
  }

  async updateMenu(payload = {}): Promise<any>{
    try{
      const url = this.configService.apiRoot + this.configService.updateMenuDetailUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || [];
      return data;
    }catch(error){
      return {status : false , message : (error as any).error.message}
    }
  }

  }
