import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { ConfigService } from 'src/app/Shared/Services/config.service';

@Injectable({
  providedIn: 'root'
})
export class WorkService {

  constructor(
    private http: HttpClient,
    private configService : ConfigService,

  ) { }



  async getWorkListData(payload = {}): Promise<any> {
      const url =  this.configService.apiRoot + this.configService.workListPageUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
  }


  async getApprovereassignListData(payload = {}): Promise<any> {
      const url =  this.configService.apiRoot + this.configService.approvereassignlistUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
  }

  async getWorkDetail(id) : Promise<any>{
    try{
      const url = `${this.configService.apiRoot}${this.configService.workDetailUrl}${id}`;
      const response = await lastValueFrom(this.http.get<any>(url));
      const data = response || {};
      return data;
    }catch(error){

      return {status : false , message : (error as any).error.message}

    }

  }

  async getContractDetail(id) : Promise<any>{
      const url = `${this.configService.apiRoot}${this.configService.contractDetailUrl}${id}`;
      const response = await lastValueFrom(this.http.get<any>(url));
      const data = response || {};
      return data;
  }

  async getContractLookUppersons(id) : Promise<any>{
      const url = `${this.configService.apiRoot}${this.configService.contractlookuppersonsUrl}${id}`;
      const response = await lastValueFrom(this.http.get<any>(url));
      const data = response || {};
      return data;
  }

  async getContractLookUpPhone(id) : Promise<any>{
      const url = `${this.configService.apiRoot}${this.configService.contractlookupPhonesUrl}${id}`;
      const response = await lastValueFrom(this.http.get<any>(url));
      const data = response || {};
      return data;
  }

  async getOverDueDetail(id) : Promise<any>{
      const url = `${this.configService.apiRoot}${this.configService.overdueDetailUrl}${id}`;
      const response = await lastValueFrom(this.http.get<any>(url));
      const data = response || {};
      return data;
  }



  async getCollectionNoteDetail(id , payload) : Promise<any>{
      const url = `${this.configService.apiRoot}${this.configService.collectionNoteUrl}${id}`;
      const response = await lastValueFrom(this.http.post<any>(url , payload));
      const data = response || {};
      return data;
  }

  async getcontractPersonsUrl(id): Promise<any> {
    const url = `${this.configService.apiRoot}${this.configService.contractPersonsUrl}/${id}`;
    const response = await lastValueFrom(this.http.get<any>(url));
    const data = response?.data || [];
    return data;

  }

  async getcontractphonelistUrl(id: any , payload : any): Promise<any> {
    const url = `${this.configService.apiRoot}${this.configService.contractphonelistUrl}/${id}`;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response || [];
    return data;
  }

  async getcontractAddresslistUrl(id: any , payload : any): Promise<any> {
    const url = `${this.configService.apiRoot}${this.configService.contractAddresslistUrl}/${id}`;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response || [];
    return data;
  }


  async getcontractPaymentlistUrl(id: any , payload : any): Promise<any> {
    const url = `${this.configService.apiRoot}${this.configService.contractpaymentlistUrl}/${id}`;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response || [];
    return data;
  }

  async getfollowupActionUrlUrl(): Promise<any> {
    const url = `${this.configService.apiRoot}${this.configService.followupActionUrl}`;
    const response = await lastValueFrom(this.http.get<any>(url));
    const data = response?.data || [];
    return data;
  }

  async getfollowupResultUrl(id: any): Promise<any> {
    const url = `${this.configService.apiRoot}${this.configService.followupResultUrl}/${id}`;
    const response = await lastValueFrom(this.http.get<any>(url));
    const data = response?.data || [];
    return data;
  }

  async createCollectionNoteUrl(payload = {}): Promise<any>{
    try{
      const url = this.configService.apiRoot + this.configService.createCollectionNoteUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || [];
      return data;
    }catch(error){
      return {status : false , message : (error as any).error.message};
    }
  }


  async getCollectorInTeamUrl(inSameTeam) : Promise<any>{
    const url = this.configService.apiRoot + this.configService.teamAssignmentCollectorInTeamUrl + inSameTeam;
    const response = await lastValueFrom(this.http.get(url));
    const data = response || [];
    return data;
  }


  async manualRessign(payload , unassign = false) : Promise<any>{
    try{
      const url = `${this.configService.apiRoot}${this.configService.manualResaaignUrl}${unassign == true ? '?unassign=true' : ''}`;

      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || [];
      return data;
    }catch(error){
      return {status : false , message : (error as any).error.message}
    }
  }


  async getUnAssignWorkListData(payload = {}): Promise<any> {
      const url =  this.configService.apiRoot + this.configService.unAssignWorkListPageUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || {};
      return data;
  }

  async approvereassignUrl(payload) : Promise<any>{
    try{
      const url = this.configService.apiRoot + this.configService.approvereassignUrl;
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response || [];
      return data;
    }catch(error){
      return {status : false , message : (error as any).error.message}
    }
  }

}
