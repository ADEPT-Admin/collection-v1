import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, lastValueFrom } from 'rxjs';
import { ConfigService } from 'src/app/Shared/Services/config.service';


export interface ContractList {
  contractNo?: string;
  customerNo?: string;
  prefixId?: number;
  firstName?: string;
  lastName?: string;
  idCard?: string;
  age?: number;
  loanType?: string;
  contractDate?: Date;
  contractStatus?: number;
  contractStatusCode? : string;
  riskLevel?: string;
}

export interface WorkList {
  contractNo?: string;
  customerName?: string;
  dueDate?: Date;
  overdueAmount?: number;
  bucket?: string;
  jobType?: string;
  productGroup?: string;
  subProduct?: string;
  riskLevel?: string;
  appointmentDate?: Date;
  lastContractDate?: Date;
  resultCode?: string;
  collectorId?: string;
  collectorName?: string;
  supervisorId?: string;
  supervisorName?: string;
  status?: string;
  allocateDate?: Date;
}

export const contractStatusMap: { [key: number]: string } = {
  1: "Active",
  2: "Closed",
  3: "Cancelled",
  4: "Default",
  5: "Legal",
}
export const followupStatusMap: { [key: number]: string } = {
  6: 'New',
  7: 'Followed_Success',
  8: 'Followed_NotSuccess',
  9: 'PromiseToPay',
  10: 'Paid',
  15: 'WaitForApprove',
  16: 'Reassign_New',
  17: 'Reassign_New(Reject)',
}

export const COLOR_PIE: any[] = [
  '#289dfc', // Active
  '#189466', // Closed
  '#acb5bd', // Cancelled
  '#83b6e2', // Default
  '#fa2f4e' //  Legal
]
export const COLOR_PIE_HOVER: any[] = [
  '#81c5fd',
  '#24db98',
  '#e3e6e8',
  '#d5e6f5',
  '#fc8395'
]

export const COLOR_BAR: any[] = [
  '#60a5fa', // 'New'
  '#fde047', // 'Followed_Success'
  '#fde047', // 'Followed_NotSuccess'
  '#fde047', // 'WaitForApprove'
  '#fde047', // 'Reassign_New'
  '#fde047', // 'Reassign_New(Reject)'
  '#22c55e', // 'PromiseToPay'
  '#22c55e', // 'Paid'
]
export const COLOR_BAR_HOVER: any[] = [
  '#81c5fd', // 'New'
  '#24db98', // 'Followed_Success'
  '#e3e6e8', // 'Followed_NotSuccess'
  '#d5e6f5', // 'PromiseToPay'
  '#fc8395', // 'Paid'
  '#fc8395', // 'WaitForApprove'
  '#fc8395', // 'Reassign_New'
  '#fc8395', // 'Reassign_New(Reject)'
]

@Injectable()
export class DashboardService{

  contractListsSubject : BehaviorSubject<ContractList[]> = new BehaviorSubject<ContractList[]>([])
  workListsSubject : BehaviorSubject<WorkList[]> = new BehaviorSubject<WorkList[]>([])

  constructor(
    private http: HttpClient,
    private configService : ConfigService
  ) {}

  getContractList() {
    return this.contractListsSubject.asObservable();
  }

    getWorkList() {
    return this.workListsSubject.asObservable();
  }

  async getContractListData(payload = {}): Promise<ContractList[]> {
    const url =  this.configService.apiRoot + this.configService.contractListUrl;
    const response = await lastValueFrom(this.http.post<any>(url, payload));
    const data = response?.data || [];
    this.contractListsSubject.next(data);
    return data;

  }



  async getDashboardContractStatus(payload = {}): Promise<any[]> {
    const url =  this.configService.apiRoot + this.configService.dashboardContractStatusUrl;
    try{
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response?.data || [];
      return data;
    }catch (error: any) {
      return []; // return fallback empty array or handle differently
    }

  }

  async getDashboardFollowupStatus(payload = {}): Promise<any[]> {
    const url =  this.configService.apiRoot + this.configService.dashboardFollowupStatusUrl;
    try{
      const response = await lastValueFrom(this.http.post<any>(url, payload));
      const data = response?.data || [];
      return data;
    }catch (error: any) {
      return []; // return fallback empty array or handle differently
    }

  }


}
