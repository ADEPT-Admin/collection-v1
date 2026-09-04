import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { ConfigService } from './config.service';


export interface GridRequestBody {
skip?: number; take?: number; requiresCount?: boolean;
sorted?: Array<{ name: string; direction: 'ascending'|'descending' }>;
where?: Array<any>;
search?: Array<any>;
}


@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient , private configService : ConfigService) {}


  async post<T>(url: string, body: any, headers?: Record<string,string>): Promise<T> {
    const urls = this.configService.apiRoot + url;
    const httpHeaders = new HttpHeaders(headers ?? { 'Content-Type': 'application/json' });
    return await lastValueFrom(this.http.post<T>(urls, body, { headers: httpHeaders }));
  }


  async get<T>(url: string, params?: Record<string,string|number|boolean>): Promise<T> {
    let hp = new HttpParams();
    for (const [k,v] of Object.entries(params ?? {})) hp = hp.set(k, String(v));
    return await lastValueFrom(this.http.get<T>(url, { params: hp }));
  }

  async getAvailableLang(){
    let url = this.configService.apiRoot+this.configService.availableLangUrl;
    return await lastValueFrom(this.http.get(url));
  }


  async getPermission(){
     let url = this.configService.apiRoot+this.configService.getPermissionUrl;
    return await lastValueFrom(this.http.get(url ));
  }

}

