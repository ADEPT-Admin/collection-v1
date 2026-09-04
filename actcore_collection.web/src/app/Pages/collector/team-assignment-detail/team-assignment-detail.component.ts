import { CommonModule } from '@angular/common';
import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { InputWithButtonComponent } from 'src/app/Shared/Components/input-with-button/input-with-button.component';
import { ToolbarComponent } from 'src/app/Shared/Components/toolbar/toolbar.component';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CanComponentDeactivate } from 'src/app/Shared/Guards/confirm-exit.guard';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { ModalService } from 'src/app/Shared/Services/modal.service';
import dayjs from 'dayjs';
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
import { DatePickerAllModule, DateTimePickerModule } from '@syncfusion/ej2-angular-calendars';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { SecurityService } from '../../securities/security.service';
import { CollectorService, CollectorTeam, CollectorTeamAssignment } from '../collector.service';
import { InputNumberComponent } from 'src/app/Shared/Components/input-number/input-number.component';
import { HeaderComponent } from "src/app/Shared/Components/header/header.component";
import { CollectorModalComponent } from 'src/app/Modals/collector-modal/collector-modal.component';
import { CalendarComponent } from 'src/app/Shared/Components/calendar/calendar.component';

@Component({
  selector: 'app-collector-team-detail',
  imports: [
    CommonModule,
    FormsModule,
    ToolbarComponent,
    ReactiveFormsModule,
    TranslateModule,
    InputWithButtonComponent,
    InputNumberComponent,
    SelectComponent,
    HeaderComponent,
    DatePickerAllModule,
    CalendarComponent
  ],
  templateUrl: './team-assignment-detail.component.html',
  styleUrl: './team-assignment-detail.component.scss',
  providers: [CollectorService]
})
export class TeamAssignmentDetailComponent implements OnInit, AfterViewInit, CanComponentDeactivate {
  async canDeactivate(): Promise<boolean> {
    if (!this.form.dirty) return true;
    const result = await this.utils.dialogConfirmSaveChange()
    return result;
  }
  @ViewChild('myForm') form: NgForm;

  actionListbtn: any = {
    new: { show: true, text: null, disabled: false },
    save: { show: true, text: null, disabled: true },
    delete: { show: true, text: null, disabled: false },
    cancel: { show: true, text: null, disabled: true },
    back: { show: true, text: null, disabled: false }
  }

  detail: CollectorTeamAssignment = { collectorEmpNameLang: { 'en': '', 'th': '' }, capacity: 0 };
  id: any = '';
  previousId: string = null;
  original: any = {};
  employees: any = [];
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
  lang = 'en';
  langSub: Subscription = null;
  colTeamList: any[] = [];
  public maxStartDate: Date | null = null;
  public minEndDate: Date | null = null;
  constructor(
    private securityService: SecurityService,
    private collotorService: CollectorService,
    private routed: ActivatedRoute,
    private router: Router,
    private utils: Utils,
    private cdRef: ChangeDetectorRef,
    private translate: TranslateService,
    private modal: ModalService,
    private loader: AppLoaderService,
  ) {
    this.lang = this.translate.currentLang;
    this.langSub = this.translate.onLangChange.subscribe(data => {
      this.lang = this.translate.currentLang;
    })
    this.routed.paramMap.subscribe(async params => {
      this.id = params.get('id');
      const loaderId = this.loader.show();
      this.menuId = this.routed.snapshot.data?.menuId || '0403';
      this.permission = await this.utils.getPermission(this.menuId);
      if (this.permission.allowView) {
        this.cdRef.detectChanges();
      }
      this.colTeamList = await this.collotorService.getcollectorTeam();
      this.colTeamList.map(item => {
        item.value = item.colTeamId;
        item.label = item.colTeamName;
        return item;
      })
      this.header = await this.utils.getMenu(this.menuId)
      this.header.path[this.header.path.length - 1].canLink = true;
      if (this.id != 'new') {
        await this.getData();
        this.header.path = [...this.header.path, ...[{ menuId: 'detail', itemName: { th: `รายละเอียด (${this.detail.colTeamName} - ${this.detail.collectorEmpId})`, en: `Detail (${this.detail.colTeamName} - ${this.detail.collectorEmpId})` } }]]
        this.loader.hide(loaderId);
      } else {
        this.id = null;
        this.original = _.cloneDeep(this.detail);
        this.actionListbtn.delete.disabled = true;
        this.actionListbtn.new.disabled = true;
        this.header.path = [...this.header.path, ...[{ itemName: { th: 'สร้าง', en: 'New' } }]]
        this.loader.hide(loaderId);
      }
      this.previousId = this.id;
      if (!this.permission?.allowView || (!this.permission?.allowNew || !this.permission?.allowEdit)) {
        setTimeout(() => {
          this.form?.control.disable();
        }, 200);
      }
      if (this.permission.allowNew && this.id == null) {
        setTimeout(() => {
          this.form?.control.enable();
          this.form.controls['colRoleName'].disable();
          this.form.controls['teamCapacity'].disable();

        }, 200);
      }
      if (this.permission.allowEdit && this.id != null) {
        setTimeout(() => {
          this.form?.control.enable();
          this.form.controls['colTeamId'].disable();
          this.form.controls['collectorEmpName'].disable();
          this.form.controls['colRoleName'].disable();
          this.form.controls['teamCapacity'].disable();
        }, 200);
      }
    })
  }

  async ngOnInit() {

  }

  ngOnDestroy() {
    this.langSub.unsubscribe();
  }

  onStartDateChange(value: Date | null) {
    this.detail.effectiveDate = value;
    this.minEndDate = value ? new Date(value) : null;
  }

  onEndDateChange(value: Date | null) {
    this.detail.expireDate = value;
    this.maxStartDate = value ? new Date(value) : null;
  }

  async openCollector() {
    const res: any = await this.modal.open(CollectorModalComponent, { header: this.translate.instant('Collector'), width: '75%', })
    if (res) {
      this.detail.collectorId = res.collectorId;
      this.detail.colRoleName = res.colRoleName;
      this.detail.collectorEmpName = res.collectorName;
      this.form.control.markAsDirty();
    }
  }

  async onSave() {
    this.form.onSubmit(null);
    this.form.control.markAllAsTouched();
    if (this.form.valid) {
      let res = null
      let requestModel = null;
      if (this.id != null) {
        requestModel = {
          colTeamId: this.detail.colTeamId,
          collectorId: this.detail.collectorId,
          capacity: this.detail.capacity,
          isActive: this.detail.isActive,
          isSupervisor: this.detail.isSupervisor,
          effectiveDate: this.utils.formatDateToUTCString(this.detail.effectiveDate),
          expireDate: this.utils.formatDateToUTCString(this.detail.expireDate),
          assignmentId: this.id,
        }
        res = await this.collotorService.updateTeamAssignment(requestModel);
      } else {
        requestModel = {
          colTeamId: this.detail.colTeamId,
          collectorId: this.detail.collectorId,
          capacity: this.detail.capacity,
          isActive: this.detail.isActive,
          isSupervisor: this.detail.isSupervisor,
          effectiveDate: this.utils.formatDateToUTCString(this.detail.effectiveDate),
          expireDate: this.utils.formatDateToUTCString(this.detail.expireDate),
        };
        res = await this.collotorService.createTeamAssignment(requestModel);
      }

      if (res?.status) {
        const message = res.message[this.translate.currentLang]
        this.utils.dialogMessageAll('success', message);
        this.markFormPristine();
        if (this.id == null) {
          this.router.navigate(['../', res.data.assignmentId], { relativeTo: this.routed })
          this.id = res.data.userId;
        } else {
          this.getData();
        }
      } else {
        if (res?.isRequireConfirmation) {
          let message = res.message[this.translate.currentLang];
          let title = res.warningMessages[this.translate.currentLang];
          const isConfirmed = this.utils.dialogMessageAll('confirm', message, false, title);
          isConfirmed.then(async (confirm) => {
            if (confirm) {
              let res_confirm = null;
              if (this.id != null) {
                res_confirm = await this.collotorService.updateTeamAssignment(requestModel, true);
              } else {
                res_confirm = await this.collotorService.createTeamAssignment(requestModel, true);
              }
              if (res_confirm?.status) {
                const message = res_confirm.message[this.translate.currentLang]
                this.utils.dialogMessageAll('success', message);
                this.markFormPristine();
                if (this.id == null) {
                  this.router.navigate(['../', res_confirm.data.assignmentId], { relativeTo: this.routed })
                } else {
                  this.getData();
                }
              } else {
                const message = res_confirm.message[this.translate.currentLang];
                this.utils.dialogMessageAll('error', message);
              }
            }
          })
        } else {
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('error', message);
        }

      }
    }
  }

  setNew() {
    this.id = null;
    this.detail = { collectorEmpNameLang: { 'en': '', 'th': '' }, capacity: 0 };
    this.original = _.cloneDeep(this.detail);
    this.maxStartDate = null;
    this.minEndDate = null;
    this.markFormPristine();
  }

  async doNew() {
    let res = false
    if (this.form.dirty) {
      const result = await this.utils.dialogConfirmSaveChange()
      res = result;
      this.setNew();
    } else {
      res = true;
      this.setNew();
    }
    if (res == true) this.router.navigate(['../new'], { relativeTo: this.routed });
  }

  doBack() {
    this.router.navigate(['..'], { relativeTo: this.routed })
  }

  async getData() {
    this.actionListbtn.new.disabled = !this.permission.allowNew;
    this.actionListbtn.delete.disabled = !this.permission.allowDelete;
    const res = await this.collotorService.getTeamAssignmentById(this.id);
    if (res.status == true) {
      this.detail = (res as any).data || {};
      this.detail.collectorCapacity = res.data.capacity;
      this.detail.effectiveDate = this.utils.transformToLocalDateTime(this.detail.effectiveDate);
      this.detail.expireDate = this.utils.transformToLocalDateTime(this.detail.expireDate);
      this.minEndDate = this.detail.effectiveDate;
      this.maxStartDate = this.detail.expireDate;
      this.original = _.cloneDeep(this.detail);
    } else {
      const message = res.message[this.translate.currentLang];
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
        const payload = [{ assignmentId: this.id }];
        const res: any = await this.collotorService.deleteTeamAssignment(payload);
        this.loader.hide(loaderId)

        if (res.status == true) {
          const message = res.message[this.translate.currentLang]
          const res_msg = await this.utils.dialogMessageAll('success', message);
          if (res_msg) {
            this.markFormPristine();
            this.router.navigate(['..'], { relativeTo: this.routed });
          }
        } else {
          const message = res.message[this.translate.currentLang];
          this.loader.hideAll();
          await this.utils.dialogMessageAll('error', message);
        }
      }
    })
  }

  async doCancel() {
    const messagebody = this.translate.instant('DialogCancel');
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then(async (res) => {
      if (res) {
        this.maxStartDate = null;
        this.minEndDate = null;
        if (this.id == null) {
          this.markFormPristine();
          if (this.previousId != null) {
            this.setNew();
            this.router.navigate(['..', this.previousId], { relativeTo: this.routed });
          } else {
            this.doBack();
          }
        } else {
          await this.getData();
          this.markFormPristine();
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
            this.router.navigate(['..', this.previousId], { relativeTo: this.routed });
          });
        }

      }
    });
  }

  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
  }

  setTeamCapacity(teamId) {
    const team = this.colTeamList.filter(item => item.value == teamId);
    if (team?.length > 0) {
      this.detail.teamCapacity = team[0].capacity || null;
    }else{
      this.detail.teamCapacity = 0;
    }
  }
}
