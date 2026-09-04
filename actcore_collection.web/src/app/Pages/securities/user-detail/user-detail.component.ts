import { CommonModule } from '@angular/common';
import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Employee, SecurityService, User, UserGroup } from '../security.service';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CanComponentDeactivate } from 'src/app/Shared/Guards/confirm-exit.guard';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons'
import { ModalService } from 'src/app/Shared/Services/modal.service';
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
import { DatePickerModule, DateTimePickerModule } from '@syncfusion/ej2-angular-calendars';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { HeaderComponent } from 'src/app/Shared/Components/header/header.component';
import { CalendarComponent } from 'src/app/Shared/Components/calendar/calendar.component';
@Component({
  selector: 'app-user-detail',
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    ButtonModule,
    SelectComponent,
    DateTimePickerModule,
    DatePickerModule,
    HeaderComponent,
    CalendarComponent
  ],
  templateUrl: './user-detail.component.html',
  styleUrl: './user-detail.component.scss',
  providers: [SecurityService, Utils, ModalService]
})
export class UserDetailComponent implements OnInit, AfterViewInit, CanComponentDeactivate {
  async canDeactivate(): Promise<boolean> {
    if (!this.form.dirty) return true;
    const result = await this.utils.dialogConfirmSaveChange()
    return result;
  }
  @ViewChild('myForm') form: NgForm;

  actionListbtn: any = {
    new: { show: false, text: null, disabled: false },
    save: { show: true, text: null, disabled: true },
    delete: { show: true, text: null, disabled: false },
    cancel: { show: true, text: null, disabled: true },
    back: { show: true, text: null, disabled: false }
  }

  detail: User = {};
  detailEmployee: Employee = {};
  id: any = '';
  previousId: string = null;
  original: any = {};
  userGroups: UserGroup[] = [];
  userGroupslist: any = [];
  payload = {
    pageNumber: 1,
    pageSize: 10,
    sortColumn: null,
    sortDirection: 'asc',
    filters: {}
  }
  permission = {
    "allowView": null,
    "allowNew": false,
    "allowEdit": false,
    "allowDelete": false
  };


  menuId = null;
  header = null;
  datetime = new Date();
  public maxStartDate: Date | null = null;
  public minEndDate: Date | null = null;

  constructor(
    private securityService: SecurityService,
    private routed: ActivatedRoute,
    private router: Router,
    private utils: Utils,
    private cdRef: ChangeDetectorRef,
    private translate: TranslateService,
    private modal: ModalService,
    private loader: AppLoaderService,
  ) {

    this.routed.paramMap.subscribe(async params => {
      this.id = params.get('id');
      const loaderId = this.loader.show();
      this.menuId = this.routed.snapshot.data?.menuId || '9104';
      this.permission = await this.utils.getPermission(this.menuId);
      if (this.permission.allowView) {
        this.cdRef.detectChanges();
      }

      this.header = await this.utils.getMenu(this.menuId)
      this.header.path[this.header.path.length - 1].canLink = true;
      await this.getData();
      this.loader.hide(loaderId);
      this.header.path = [...this.header.path, ...[{ menuId: 'detail', itemName: { th: `รายละเอียด (${this.detail.userName})`, en: `Detail (${this.detail.userName})` } }]]
      this.previousId = this.id;
      if (!this.permission?.allowView || (!this.permission?.allowNew || !this.permission?.allowEdit)) {
        setTimeout(() => {
          this.form?.control.disable();
          this.form?.controls['UserisLocked']?.disable();
        }, 200);
      }
      if (this.permission.allowEdit && this.id != null) {
        setTimeout(() => {
          this.form?.control.enable();
          this.form?.controls['UserisLocked']?.disable();
        }, 200);

      }

      let res: any = await this.securityService.getGroupList();
      if (res.data.length > 0) {
        this.userGroupslist = res.data.map(element => {
          return {
            label: element.userGroupName,
            value: element.userGroupId
          };
        });
      }
    })


  }

  async ngOnInit() {

  }

  async onSave() {
    this.form.onSubmit(null);
    this.form.control.markAllAsTouched();
    if (this.form.valid) {
      let res = null;
      const payload = _.cloneDeep(this.detail);
      payload.effectiveDate = this.utils.formatDateString(payload.effectiveDate);
      payload.expireDate = this.utils.formatDateString(payload.expireDate);
      payload.userGroupIds = [this.detail.userGroupIds];
      payload.forceChangePassword = this.detail.isNewUser;

      if (this.id != null) {
        res = await this.securityService.updateUser(payload);
      } else {
        res = await this.securityService.createUser(payload);
      }
      if (res?.status) {
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('success', message);
        this.markFormPristine();
        if (this.id == null) {
          this.router.navigate(['../', res.data.userId], { relativeTo: this.routed })
          this.id = res.data.userId;
        } else {
          this.getData();
        }
      } else {
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('error', message);
      }
    }
  }

  doBack() {
    this.router.navigate(['..'], { relativeTo: this.routed })
  }

  async getData() {
    let dataUser: any = [];
    let dataEmployeebyid: any = [];
    this.actionListbtn.new.disabled = !this.permission.allowNew;
    this.actionListbtn.delete.disabled = !this.permission.allowDelete;
    dataUser = await this.securityService.getUser(this.id);
    if (dataUser.status == true) {
      this.detail = (dataUser as any).data || {};
      this.detail.employeeId = this.detail.employee.employeeId;
      dataEmployeebyid = await this.securityService.getEmployeegetbyid(this.detail.employeeId);
      this.detailEmployee = (dataEmployeebyid as any).data || {};
      this.detail.userGroupIds = this.detail.userGroups[0]?.userGroupId;
      this.detail.expireDate = this.detail.expireDate;
      this.detail.effectiveDate = this.utils.transformToLocalDateTime(this.detail.effectiveDate);
      this.detail.expireDate = this.utils.transformToLocalDateTime(this.detail.expireDate);
      this.detail.updatedDate = this.utils.transformToLocalDateTime(this.detail.updatedDate);
      this.minEndDate = this.detail.effectiveDate;
      this.maxStartDate = this.detail.expireDate;
      this.original = _.cloneDeep(this.detail);
    } else {
      const message = dataUser.message[this.translate.currentLang];
      this.loader.hideAll();
      await this.utils.dialogMessageAll('error', message);
      this.doBack();
    }
  }

  markFormPristine() {
    Object.values(this.form.controls).forEach(control => {
      control.markAsPristine();
      control.markAsUntouched(); // Optional: also reset "touched"
      control.updateValueAndValidity();
    });
  }

  async doDel() {
    let messagebodyDialogDelete = this.translate.instant('DialogDelete');
    let messagebody = `${messagebodyDialogDelete}`
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then(async (confirm) => {
      if (confirm) {
        const loaderId = this.loader.show();
        const payload = [{ userId: this.id }];
        const res: any = await this.securityService.deleteUser(payload);
        this.loader.hide(loaderId)
        if (res.status == true) {
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('success', message);
          if (res) {
            this.markFormPristine();
            this.router.navigate(['..'], { relativeTo: this.routed });
          }
        } else {
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('error', message);
        }
      }
    })
  }

  async unlockUser() {
    let messagebodyDialogDelete = this.translate.instant('unlock User');
    let messagebody = `${messagebodyDialogDelete} ${this.detail.userName}`
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then(async (confirm) => {
      if (confirm) {
        const payload = { userId: this.id };
        const res: any = await this.securityService.unlockUser(payload);
        if (res.status == true) {
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('success', message);
          if (res) {
            this.markFormPristine();
            this.router.navigate(['..'], { relativeTo: this.routed });
          }
        } else {
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('error', message);
        }
      }
    })
  }

  async resetPassword() {
    let messagebodyDialogDelete = this.translate.instant('Reset Rassword');
    let messagebody = `${messagebodyDialogDelete}`
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then(async (confirm) => {
      if (confirm) {
        const payload = { userId: this.id };
        const res: any = await this.securityService.resetpasswordUser(payload);
        if (res.status == true) {
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('success', message);
          if (res) {
            this.markFormPristine();
            this.router.navigate(['..'], { relativeTo: this.routed });
          }
        } else {
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('error', message);
        }
      }
    })
  }

  async doCancel() {
    const messagebody = this.translate.instant('DialogCancel');
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then((res) => {
      if (res) {
        const original = _.cloneDeep(this.original);
        this.detail = (original as any) || {};
        this.markFormPristine();
      }
    });
  }

  onChange(value: any) {
    this.detail.userGroupIds = value;
  }

  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
  }

  onStartDateChange(value: Date | null) {
    this.detail.effectiveDate = value;
    this.minEndDate = value ? new Date(value) : null;
  }

  onEndDateChange(value: Date | null) {
    this.detail.expireDate = value;
    this.maxStartDate = value ? new Date(value) : null;
  }

}
