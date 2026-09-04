import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, lastValueFrom } from 'rxjs';
import { ConfigService } from 'src/app/Shared/Services/config.service';


export interface CollectorTeam {
  id?: number;
  colTeamId?: string;
  colTeamCode?: string;
  colTeamName?: string;
  supervisor?: any;
  supervisorId?: string;
  supervisorName?: string;
  supervisorNameLang?: any;
  capacity?: number;
  areaCode?: string;
  isActive?: boolean;
  colArea?: any;
  areaId?: string;
  description?: string;
}
export interface CollectorTeamAssignment {
  id?: number;
  colTeamId?: string;

  capacity?: number;
  isActive?: boolean;
  isSupervisor?: boolean;
  expireDate?: Date;
  effectiveDate?: Date;
  collectorEmpName?: string;
  collectorEmpNameLang?: any;
  collectorId?: string;
  colTeamName?: string;
  collectorEmpId?: string;
  teamCapacity?: number;
  collectorCapacity?: any;


  colTeamCode?: string;
  collectorName?: string;
  colRoleName?: string;
  collectorRole?: string;
}

export interface CollectorProfile {
  id?: number;
  collectorId?: string;
  employeeName?: string;
  colRoleName?: string;
  colTeamName?: string;
  capacity?: number;
  isActive?: boolean;
  collectorName?: string;
  userName?: string;
  email?: string;
  colRole?: any;
  colTeam?: any;
  employeeId?: string;
  userId?:string;
  phoneNo?:string;
  collectorGuid?: number;
}


export interface Employee {
  email?: string;
  employeeId?: string;
  employeeName?: string;
  employeeNameEn?: string;
  managerId?: string;
  phoneNo?: string;
  userName?: string;
}

@Injectable({
  providedIn: 'root',
})
export class CollectorService {

  teamSubject : BehaviorSubject<CollectorTeam> = new BehaviorSubject<CollectorTeam>({});

  constructor(
    private configService : ConfigService,
    private http : HttpClient

  ) { }

  async getTeamPaged(payload){
    const url =  this.configService.apiRoot + this.configService.teamListPagedUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    this.teamSubject.next(data);
    return data;
  }

  async getlistassigncollectorUrlUrlPaged(payload){
    const url =  this.configService.apiRoot + this.configService.listassigncollectorUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    return data;
  }

  getTeamDetail(){
        return this.teamSubject.asObservable();
    }

  async getTeamById(id): Promise<any> {
    try {
      const url = `${this.configService.apiRoot}${this.configService.teamDetailUrl}/${id}`;
      const response = await lastValueFrom(this.http.get<any>(url));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async createTeam(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.teamCreateUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
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


  async deleteTeam(payload = {}): Promise<any>{
    try{
      const url = this.configService.apiRoot + this.configService.teamDeleteUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    }catch(error){
      return { status: false, message: (error as any).error.message }
    }
  }




  async getCollectors(payload = {} , isActiveCollector = false): Promise<CollectorProfile[]>{
    const url = `${this.configService.apiRoot}${this.configService.collectorListUrl}${isActiveCollector == true ? '?isActiveCollector=true' : ''}`;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response || [];
    return data;
  }


  async deleteCollectors(payload = {}): Promise<any>{
    try{
      const url = this.configService.apiRoot + this.configService.collectorDeleteUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    }catch(error){
      return { status : false , message : (error as any).error.message}
    }

  }


  async getCollectorDetail(id) : Promise<any>{
      try{
        const url = `${this.configService.apiRoot}${this.configService.collectorDetailUrl}/${id}`;
        const response = await lastValueFrom(this.http.get<any>(url));
        const data = response || {};
        return data;
      }catch(error) {
        return { status : false , message : (error as any).error.message}
      }

  }


  async getcollectorRole() : Promise<any>{
      const url = `${this.configService.apiRoot}${this.configService.collectorRoleListUrl}`;
      const response = await lastValueFrom(this.http.get<any>(url));
      const data = response || {};
      return data;
  }

  async getcollectorTeam() : Promise<any>{
      const url = `${this.configService.apiRoot}${this.configService.collectorTeamListUrl}`;
      const response = await lastValueFrom(this.http.get<any>(url));
      const data = response.data || {};
      return data;
  }

  async createCollectors(payload = {}): Promise<any> {
    try {
      const url = this.configService.apiRoot + this.configService.collectorCreateUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || [];
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async updateCollectors(payload = {}): Promise<any>{
    try {
    const url = this.configService.apiRoot + this.configService.collectorUpdateUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response || [];
    return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async getTeamAssignmentPaged(payload){
    const url =  this.configService.apiRoot + this.configService.teamAssignmentListPagedUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    return data;
  }

  async getTeamAssignmentlistpagedbyteamUrlPaged(payload,ColTeamId: any) {
    const url = `${this.configService.apiRoot}${this.configService.TeamAssignmentlistpagedbyteamUrl}?ColTeamId=${ColTeamId}`;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    return data;
}

  async getTeamAssignmentlistpagedbycollectorUrl(payload,ColTeamId: any) {
    const url = `${this.configService.apiRoot}${this.configService.teamAssignmentlistpagedbycollectorUrl}?id=${ColTeamId}`;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    return data;
}

  async getTeamAssignmentById(id): Promise<any> {
    try {
      const url = `${this.configService.apiRoot}${this.configService.teamAssignmentDetailUrl}/${id}`;
      const response = await lastValueFrom(this.http.get<any>(url));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async createTeamAssignment(payload = {}, confirm = null): Promise<any> {
    try {
      let url = this.configService.apiRoot + this.configService.teamAssignmentCreateUrl;
      if (confirm == true) {
        url = `${url}?overwrite=true`
      }
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async bulkassignTeamAssignmentUrl(payload = {}): Promise<any> {
    try{
      const url = this.configService.apiRoot + this.configService.bulkassignTeamAssignmentUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async updateTeamAssignment(payload = {}, confirm = null): Promise<any> {
    try {
      let url = this.configService.apiRoot + this.configService.teamAssignmentUpdateUrl;
      if (confirm == true) {
        url = `${url}?overwrite=true`
      }
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async deleteTeamAssignment(payload = {}): Promise<any>{
    try{
      const url = this.configService.apiRoot + this.configService.teamAssignmentDeleteUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }

  async bulkCreateMultipleCollectorUrl(payload = {}): Promise<any>{
    try{
      const url = this.configService.apiRoot + this.configService.bulkCreatecollectorUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
    } catch (error) {
      return { status: false, message: (error as any).error.message }
    }
  }


}



