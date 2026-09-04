import { CommonModule, DatePipe } from '@angular/common';
import { AfterViewInit, ChangeDetectorRef, Component, OnInit, viewChild, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

import * as _ from 'lodash';
import dayjs from 'dayjs';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { debounceTime, Subject, Subscription, timer } from 'rxjs';
import { CanComponentDeactivate } from 'src/app/Shared/Guards/confirm-exit.guard';
import { TabsComponent } from 'src/app/Shared/Components/tabs/tabs.component';
import { TabComponent } from 'src/app/Shared/Components/tabs/tab/tab.component';
import { ToolbarComponent } from 'src/app/Shared/Components/toolbar/toolbar.component';
import { ContractDescriptionComponent } from './tabs/contract-description/contract-description.component';
import { OutstandingBalanceComponent } from './tabs/outstanding-balance/outstanding-balance.component';
import { CollectionNoteComponent } from './tabs/collection-note/collection-note.component';
import { PaymentHistoryComponent } from './tabs/payment-history/payment-history.component';
import { AddressComponent } from './tabs/address/address.component';
import { ContractPhoneComponent } from './tabs/contract-phone/contract-phone.component';
import { ApplicantAndGuarantorComponent } from './tabs/applicant-and-guarantor/applicant-and-guarantor.component';
import { Task } from '../task.model';
import { DateTimePickerModule } from '@syncfusion/ej2-angular-calendars'
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { InputNumberComponent } from 'src/app/Shared/Components/input-number/input-number.component';
import { ButtonAllModule } from '@syncfusion/ej2-angular-buttons';
import { WorkService } from '../work.service';
import { HeaderComponent } from 'src/app/Shared/Components/header/header.component';
import { CalendarComponent } from 'src/app/Shared/Components/calendar/calendar.component';
import { OverviewComponent } from './tabs/overview/overview.component';
import { ContractInformationComponent } from './tabs/contract-information/contract-information.component';
@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [
    TranslateModule,
    RouterModule,
    CommonModule,
    FormsModule,
    TabsComponent,
    TabComponent,
    ToolbarComponent,
    CollectionNoteComponent,
    PaymentHistoryComponent,
    DateTimePickerModule,
    SelectComponent,
    ShareDirectiveModule,
    InputNumberComponent,
    ButtonAllModule,
    HeaderComponent,
    CalendarComponent,
    OverviewComponent,
    ContractInformationComponent

  ],
  templateUrl: './task-detail.component.html',
  styleUrl: './task-detail.component.scss',
  providers: [Utils]
})
export class TaskDetailComponent implements OnInit, CanComponentDeactivate, AfterViewInit {

  @ViewChild('myForm') form: NgForm;

  @ViewChild('overviewTab', { static: false }) overviewTab: OverviewComponent;
  @ViewChild('contractDescTab', { static: false }) contractDescTab: ContractDescriptionComponent;
  @ViewChild('outstandingBalanceTab', { static: false }) outstandingBalanceTab: OutstandingBalanceComponent;
  @ViewChild('collectionNoteTab', { static: false }) collectionNoteTab: CollectionNoteComponent;
  @ViewChild('paymentHistoryTab', { static: false }) paymentHistoryTab: PaymentHistoryComponent;
  @ViewChild('addressTab', { static: false }) addressTab: AddressComponent;
  @ViewChild('contractPhoneTab', { static: false }) contractPhoneTab: ContractPhoneComponent;
  @ViewChild('applicantGuarantorTab', { static: false }) applicantGuarantorTab: ApplicantAndGuarantorComponent;
  @ViewChild('contractInformationComponentTab', { static: false }) contractInformationComponentTab: ContractInformationComponent;


  id = null;
  contractHeader: Task = {};
  collectionNotes: any[] = [];
  collectionNote: any = {promiseToPayAmount: 0};
  contractDescription: any = {};
  outstandingBalances: any = {};
  contractPayments: any = {}
  contractAddress: any[] = [];
  contractPhones: any[] = [];
  contractPerson: any[] = [];
  followupActionList: any = [];
  followupResultList: any = [];
  actionListbtn: any = null;
  detailLookUppersons : any = [];
  detailLookUpPhone : any = [];

  async canDeactivate(): Promise<boolean> {
    if (!this.form.dirty) return true;
    const result = await this.utils.dialogConfirmSaveChange()
    return result;
  }

  lang$: Subscription = null;
  permission = {
    "allowView": null,
    "allowNew": false,
    "allowEdit": false,
    "allowDelete": false
  };

  menuId = null;
  header = null;


  constructor(
    private routed: ActivatedRoute,
    private router: Router,
    private translate: TranslateService,
    private utils: Utils,
    private loaderService: AppLoaderService,
    private cd: ChangeDetectorRef,
    private workService: WorkService
  ) {
    this.routed.paramMap.subscribe(async params => {
      this.id = params.get('id');
      const loaderId = this.loaderService.show();

      this.menuId = this.routed.snapshot.data?.menuId || '0901';
      this.permission = await this.utils.getPermission(this.menuId);
      this.header = await this.utils.getMenu(this.menuId)
      this.header.path[this.header.path.length -1].canLink = true;

      await this.getDetail();

      this.header.path = [...this.header.path , ...[{menuId : 'detail' , itemName : {th : `รายละเอียด (${this.contractHeader.contractNo})` , en : `Detail (${this.contractHeader.contractNo})`}} ]]
      await this.getfollowupActionUrlUrl();
      this.loaderService.hide(loaderId);
    });

    this.lang$ = this.translate.onLangChange.subscribe((lang) => {
      // this.followupStatusList = this.followupStatusListSource.map(item => { return {text : this.translate.instant(`followStatus.${item}`) , value : item }});
      // this.detail.contractStatusDisplay = this.translate.instant(`contractStatus.${this.detail.contractStatusDesc}`);
    })
  }

  async getDetail() {
    if (!this.permission?.allowEdit) {
      setTimeout(() => {
        if(this.form != undefined){
          this.form.control.disable();
        }
      }, 200);
    }
    const res = await this.workService.getWorkDetail(this.id);
    if(res.status == true){
      this.contractHeader = res.data;


      await this.getContractLookUppersons(this.contractHeader.contractNo);
      timer(10).subscribe(() => {
        if(this.contractDescTab != undefined){
          this.contractDescTab.getData();

        }
      })
    }else{
      const message = res.message[this.translate.currentLang];
      this.loaderService.hideAll();
      await this.utils.dialogMessageAll('error', message);
      this.doBack();
    }

  }

  async getContractLookUppersons(contractNo: any) {
    if (contractNo != undefined) {
      const res = await this.workService.getContractLookUppersons(contractNo);
      this.detailLookUppersons = res.data.map(element => ({
        label: element.label,
        value: element.value
      }));
    }
  }

  async getContractLookUpPhone(id) {
    if (id != undefined) {
      const res = await this.workService.getContractLookUpPhone(id);
      this.detailLookUpPhone = res.data.map(element => ({
        label: element.label,
        value: element.value
      }));
    }
  }


  async getfollowupActionUrlUrl(){
    const res = await this.workService.getfollowupActionUrlUrl();
    this.followupActionList = res.map(element => ({
      label: element.actionDescription,
      value: element.actionId
    }));
  }

  async onFollowupActionChange(id){
    this.followupResultList = [];
    this.collectionNote.followupResult = '';
    if(id != null){
    await this.getfollowupResultUrl(id);
    }
  }

  async onContactPersonChange(id){
    if(id != null){
    await this.getContractLookUpPhone(id);
    }else{
      this.collectionNote.contactPhone = '';
      this.detailLookUpPhone = [];
    }
  }

  async getfollowupResultUrl(id) {
    const res = await this.workService.getfollowupResultUrl(id);
    this.followupResultList = res.map(element => ({
      label: element.resultDescription,
      value: element.resultId
    }));
  }

  ngOnInit() {

  }

  ngAfterViewInit() {
    timer(100).subscribe(() => {

      this.actionListbtn = {
        new: { show: false, text: null, disabled: false },
        save: { show: false, text: null, disabled: true },
        delete: { show: false, text: null, disabled: false },
        cancel: { show: false, text: null, disabled: true },
        back: { show: true, text: null, disabled: false }
      }
    })

    this.cd.detectChanges();

  }

  ngOnDestroy() {
    this.lang$.unsubscribe();
  }

  doBack() {
    this.router.navigate(['..'], { relativeTo: this.routed })
  }


  cancelCollection(){
    this.followupResultList = [];
    this.collectionNote.followupAction = '';
    this.collectionNote.contactPerson = '';
    this.collectionNote.contactPhone = '';
    this.collectionNote.followupResult = '';
    this.collectionNote.nextFollowupDate = '';
    this.collectionNote.promiseToPayDate = '';
    this.collectionNote.promiseToPayAmount = 0.00;
    this.collectionNote.collectionRemark = '';
    this.markFormPristine();
  }


  sortField: string | null = null;
  sortOrder: number | null = null;

  async saveCollection() {
    this.form.onSubmit(null);
    this.form.control.markAllAsTouched();
    if (this.form.valid) {
      const loaderId = this.loaderService.show();
      const payload = {
        worklistId: this.id,
        contractNo: this.contractHeader.contractNo,
        followupActionId: this.collectionNote.followupAction,
        contractPersonId: this.collectionNote.contactPerson,
        contractPhoneId: this.collectionNote.contactPhone,
        followupResultId: this.collectionNote.followupResult,
        nextFollowupDate: this.utils.formatDateToUTCString(this.collectionNote.nextFollowupDate),
        promiseToPayDate: this.utils.formatDateToUTCString(this.collectionNote.promiseToPayDate),
        promiseToPayAmount: this.collectionNote.promiseToPayAmount,
        collectionRemark: this.collectionNote.collectionRemark
      }
      const res = await this.workService.createCollectionNoteUrl(payload);
      if (res.status) {
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('success', message);
        this.followupResultList = [];
        this.collectionNote.followupResult = '';
        // const contract = {contractNo : this.taskDetail.contractNo};
        // this.collectionNotes = await this.taskService.getCollectNotes(contract);
        this.collectionNote = {};
        this.detailLookUpPhone = [];
        this.markFormPristine();
        this.getDetail();
        if (!_.isNil(this.collectionNoteTab)) {
          this.collectionNoteTab.getData();
        }
        this.loaderService.hide(loaderId);
      } else {
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('error', message);
        this.loaderService.hide(loaderId);
      }
    }

  }

  markFormPristine() {
    Object.values(this.form.controls).forEach(control => {
      control.markAsPristine();
      control.markAsUntouched(); // Optional: also reset "touched"
      control.updateValueAndValidity();
    });
  }

  conCatText(text1, text2) {
    return `${text1}.${text2}`
  }

  getSeverity(status: string) {
    switch (status) {
      case 'Paid':
        return 'success';
      case 'Promise to Pay':
        return 'success';
      case 'Followed Success':
        return 'warn';
      case 'Followed Not Success':
        return 'warn';
      case 'WaitForApprove':
        return 'warn';
      case 'Reassign_New':
        return 'warn';
      case 'Reassign_New(Reject)':
        return 'warn';
      case 'New':

        return null;
    }
  }



}
