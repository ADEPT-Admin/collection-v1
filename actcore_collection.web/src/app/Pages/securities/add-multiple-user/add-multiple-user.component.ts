import { CommonModule } from '@angular/common';
import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Employee, SecurityService, User, UserGroup } from '../security.service';
import { ToolbarComponent } from 'src/app/Shared/Components/toolbar/toolbar.component';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CanComponentDeactivate } from 'src/app/Shared/Guards/confirm-exit.guard';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons'
import { ModalService } from 'src/app/Shared/Services/modal.service';
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
import { EmployeeModalComponent } from 'src/app/Modals/employee-modal/employee-modal.component';
import { DatePickerModule, DateTimePickerModule } from '@syncfusion/ej2-angular-calendars';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { HeaderComponent } from 'src/app/Shared/Components/header/header.component';
import { CalendarComponent } from 'src/app/Shared/Components/calendar/calendar.component';
import { GridAllModule, GridComponent, ResizeService } from '@syncfusion/ej2-angular-grids';
@Component({
  selector: 'app-add-multiple-user',
  imports: [
    CommonModule,
    FormsModule,
    ToolbarComponent,
    TranslateModule,
    ButtonModule,
    SelectComponent,
    DateTimePickerModule,
    DatePickerModule,
    HeaderComponent,
    CalendarComponent,
    GridAllModule,
  ],

  templateUrl: './add-multiple-user.component.html',
  styleUrl: './add-multiple-user.component.scss',
  providers: [SecurityService, Utils, ModalService, ResizeService]
})
export class AddMultipleUserComponent implements OnInit, AfterViewInit, CanComponentDeactivate {
  async canDeactivate(): Promise<boolean> {
    if (!this.form.dirty) return true;
    const result = await this.utils.dialogConfirmSaveChange()
    return result;
  }
  @ViewChild('myForm') form: NgForm;

  actionListbtn: any = {
    new: { show: false, text: null, disabled: false },
    save: { show: true, text: null, disabled: true },
    delete: { show: false, text: null, disabled: false },
    cancel: { show: true, text: null, disabled: true },
    back: { show: true, text: null, disabled: false }
  }
  public customAttributes: Object = { class: 'customcss' };
  public resizeSettings = { mode: 'Normal' };
  @ViewChild('grid') public grid!: GridComponent;
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
  forcepassswordchange: boolean = true;
  isActive: boolean = true;
  dataSource: any = [];
  selectAllChecked: boolean = false;
  isIndeterminate: boolean = false;
  selectedIds = new Set<string>();

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
      this.loader.hide(loaderId);
      this.header.path = [...this.header.path, ...[{ menuId: 'detail', itemName: { th: `รายละเอียด (สร้าง)`, en: `Detail (New)` } }]]
      this.previousId = this.id;
      if (!this.permission?.allowView || (!this.permission?.allowNew || !this.permission?.allowEdit )) {
          setTimeout(() => {
            this.form?.control.disable();
            this.form?.controls['UserisLocked']?.disable();
          }, 200);
      }
      if(this.permission.allowNew && this.id == null){
        setTimeout(() => {
            this.form?.control.enable();
            this.form?.controls['UserisLocked']?.disable();
          }, 200);
      }
      this.detail.forceChangePassword = true;
      this.detail.isActive = true;
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
      const employeeIds = this.dataSource.map(element => element.employeeid);
      if (employeeIds.length == 0) {
        const message = this.translate.instant('PleaseSelectUser');
        this.utils.dialogMessageAll('error', message);
        return false;
      }
      const loaderId = this.loader.show();
      let res = null;
      const payload = {
        userGroupId: this.detail.userGroupIds,
        effectiveDate: this.utils.formatDateString(this.detail.effectiveDate),
        expireDate: this.utils.formatDateString(this.detail.expireDate),
        isActive: this.isActive,
        forceChangePassword: this.forcepassswordchange,
        employeeIds: employeeIds
      }
      res = await this.securityService.createMultipleUser(payload);
      if (res?.status) {
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('success', message);
        this.markFormPristine();
        this.dataSource = [];
        const original = _.cloneDeep(this.original);
        this.detail = (original as any) || {};
        this.detail.forceChangePassword = true;
        this.detail.isActive = true;
        this.loader.hide(loaderId);
        this.markFormPristine();
      } else {
        this.loader.hideAll();
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('error', message);
      }
    }
  }

  doBack() {
    this.router.navigate(['..'], { relativeTo: this.routed })
  }

  markFormPristine() {
    Object.values(this.form.controls).forEach(control => {
      control.markAsPristine();
      control.markAsUntouched(); // Optional: also reset "touched"
      control.updateValueAndValidity();
    });
  }

  async doCancel() {
    const messagebody = this.translate.instant('DialogCancel');
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then((res) => {
      if (res) {
        this.dataSource = [];
        const original = _.cloneDeep(this.original);
        this.detail = (original as any) || {};
        this.detail.forceChangePassword = true;
        this.detail.isActive = true;
        this.markFormPristine();
      }
    });
  }

  onChange(value: any) {
    this.detail.userGroupIds = value;
  }

  onStartDateChange(value: Date | null) {
    this.detail.effectiveDate = value;
    this.minEndDate = value ? new Date(value) : null;
  }

  onEndDateChange(value: Date | null) {
    this.detail.expireDate = value;
    this.maxStartDate = value ? new Date(value) : null;
  }

  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
  }

  async Add() {
    const res: any = await this.modal.open(EmployeeModalComponent, { header: this.translate.instant('AddUsers'), width: '75%', });
    if (res) {
      res.forEach(element => {
        const existingItem = this.dataSource.find(r => r.employeeid === element.employeeId);
        if (!existingItem) {
          const item = {
            username: element.userName,
            employeeid: element.employeeId,
            employeename: element.employeeName,
            department: element.departmentName,
            position: element.positionName,
            email: element.email,
            phone: element.phoneNo
          };
          this.dataSource.push(item);
        }
      });
      this.grid.refresh();
      this.form.control.markAsDirty();
    }
  }

  async delete(data: any = null) {
    let targets = [];
    if (data != null) {
      targets = [...targets, data];
    } else {
      targets = this.dataSource.filter(item => item.checked == true);
    }
    if (!targets.length) return;
    targets.forEach(target => {
      const targetId = target.employeeid || target.employeeId;
      if (targetId) {
        const index = this.dataSource.findIndex(item => item.employeeid === targetId);
        if (index > -1) {
          this.dataSource.splice(index, 1);
        }
      }
    });
    this.selectedIds.clear();
    this.selectAllChecked = false;
    this.isIndeterminate = false;
    this.grid.refresh();
    this.grid.clearSelection();
  }

  toggleSelectAll(event: any) {
    const checked = event.target.checked;
    const records: any[] = this.grid.getCurrentViewRecords();
    if (checked) {
      records.forEach(r => this.selectedIds.add(r.employeeid));
    } else {
      records.forEach(r => this.selectedIds.delete(r.employeeid));
    }

    this.dataSource.forEach(element => {
      element.checked = this.selectedIds.has(element.employeeid);
    });

    this.updateHeaderCheckboxState();
    this.grid.refresh();
    this.grid.hideSpinner();
  }

  onRowCheckboxChange(data: any, event: any) {
    if (event.target.checked) {
      this.selectedIds.add(data.employeeid);
    } else {
      this.selectedIds.delete(data.employeeid);
    }
    this.dataSource.forEach(element => {
      element.checked = this.selectedIds.has(element.employeeid);
    });

    this.updateHeaderCheckboxState();
  }

  updateHeaderCheckboxState() {
    const records: any[] = this.grid.getCurrentViewRecords();
    const total = records.length;
    const checkedCount = records.filter(r => this.selectedIds.has(r.employeeid)).length;
    if (checkedCount === 0) {
      this.selectAllChecked = false;
      this.isIndeterminate = false;
    } else if (checkedCount === total) {
      this.selectAllChecked = true;
      this.isIndeterminate = false;
    } else {
      this.selectAllChecked = false;
      this.isIndeterminate = true;
    }
  }

}



