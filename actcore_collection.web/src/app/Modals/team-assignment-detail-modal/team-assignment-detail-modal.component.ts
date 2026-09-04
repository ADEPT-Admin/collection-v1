import { CommonModule } from '@angular/common';
import { AfterViewInit, ChangeDetectorRef, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { Subscription, timer } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CanComponentDeactivate } from 'src/app/Shared/Guards/confirm-exit.guard';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { ModalService } from 'src/app/Shared/Services/modal.service';
import { DatePickerAllModule } from '@syncfusion/ej2-angular-calendars';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { InputNumberComponent } from 'src/app/Shared/Components/input-number/input-number.component';
import { CalendarComponent } from 'src/app/Shared/Components/calendar/calendar.component';
import { CollectorService, CollectorTeamAssignment } from 'src/app/Pages/collector/collector.service';
import { ButtonModule, CheckBoxModule } from '@syncfusion/ej2-angular-buttons';
import { ModalChildBase } from '../base-modal/base-modal.component';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';

@Component({
  selector: 'app-team-assignment-detail-modal',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule,
    InputNumberComponent,
    DatePickerAllModule,
    CalendarComponent,
    ButtonModule,
    ShareDirectiveModule,
    CheckBoxModule,
  ],
  templateUrl: './team-assignment-detail-modal.component.html',
  styleUrl: './team-assignment-detail-modal.component.scss',
  providers: [CollectorService]
})
export class TeamAssignmentDetailModalComponent extends ModalChildBase implements OnInit, AfterViewInit, CanComponentDeactivate, OnDestroy {
  async canDeactivate(): Promise<boolean> {
    if (!this.form.dirty) return true;
    const result = await this.utils.dialogConfirmSaveChange()
    return result;
  }
  @ViewChild('myForm') form: NgForm;
  detail: CollectorTeamAssignment = { collectorEmpNameLang: { 'en': '', 'th': '' }, capacity: 0 };
  original: any = {};
  permission = {
    "allowView": null,
    "allowNew": false,
    "allowEdit": false,
    "allowDelete": false
  };
  menuId = null;
  lang = 'en';
  langSub: Subscription = null;
  id: any = null;
  public maxStartDate: Date | null = null;
  public minEndDate: Date | null = null;
  constructor(
    private collotorService: CollectorService,
    private routed: ActivatedRoute,
    private router: Router,
    private utils: Utils,
    private cdRef: ChangeDetectorRef,
    private translate: TranslateService,
    private modal: ModalService,
    private loader: AppLoaderService,
  ) {
    super();
    this.lang = this.translate.currentLang;
    this.langSub = this.translate.onLangChange.subscribe(data => {
      this.lang = this.translate.currentLang;
    })

    this.routed.paramMap.subscribe(async params => {
      timer(100).subscribe(async () => {
        const modalData = (this as any).childComponentData;
        if (!modalData) {
          const stack = (this.modal as any).stack;
          if (stack && stack.length > 0) {
            const lastModal = stack[stack.length - 1];
            this.id = lastModal.instance.childComponentData?.id;
          }
        } else {
          this.id = modalData.id;
        }
        this.cdRef.detectChanges();
      });
      const loaderId = this.loader.show();
      this.menuId = this.routed.snapshot.data?.menuId || '0403';
      this.permission = await this.utils.getPermission(this.menuId);
      if (this.permission.allowView) {
        this.cdRef.detectChanges();
      }
      await this.getData();
      this.loader.hide(loaderId);
      if (!this.permission?.allowView || (!this.permission?.allowNew || !this.permission?.allowEdit)) {
        setTimeout(() => {
          this.form?.control.disable();
        }, 200);
      }
      if (this.permission.allowEdit && this.id != null) {
        setTimeout(() => {
          this.form?.control.enable();
          this.form.controls['colTeamCode'].disable()
          this.form.controls['colTeamName'].disable();
          this.form.controls['teamCapacity'].disable()
          this.form.controls['collectorEmpId'].disable();
          this.form.controls['collectorEmpName'].disable();
          this.form.controls['colRoleName'].disable();
        }, 200);
      }
    })
  }

  ngOnInit(): void {
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

  async onSave() {
    this.form.onSubmit(null);
    this.form.control.markAllAsTouched();
    if (this.form.valid) {
      const loaderId = this.loader.show();
      let res = null
      let requestModel = null;
      requestModel = {
        assignmentId: this.id,
        colTeamId: this.detail.colTeamId,
        collectorId: this.detail.collectorId,
        isSupervisor: this.detail.isSupervisor,
        capacity: this.detail.capacity,
        effectiveDate: this.utils.formatDateToUTCString(this.detail.effectiveDate),
        expireDate: this.utils.formatDateToUTCString(this.detail.expireDate),
        isActive: this.detail.isActive
      }
      res = await this.collotorService.updateTeamAssignment(requestModel);
      if (res?.status) {
        this.loader.hide(loaderId);
        const message = res.message[this.translate.currentLang];
        const isConfirmed = this.utils.dialogMessageAll('success', message);
        isConfirmed.then((res) => {
          if (res) {
            this.close(true);
          }
        });
      } else {
          if (res?.isRequireConfirmation) {
          let message = res.message[this.translate.currentLang];
          let title = res.warningMessages[this.translate.currentLang];
          const isConfirmed = this.utils.dialogMessageAll('confirm', message, false, title);
          isConfirmed.then(async (confirm) => {
            if (confirm) {
              let res_confirm = null;
              res_confirm = await this.collotorService.updateTeamAssignment(requestModel, true);
              if (res_confirm?.status) {
                const message = res.message[this.translate.currentLang];
                const isConfirmed = this.utils.dialogMessageAll('success', message);
                isConfirmed.then((res) => {
                  if (res) {
                    this.loader.hideAll();
                    this.close(true);
                  }
                });
              }
            }else{
              this.loader.hideAll();
            }
          })
        } else {
          this.loader.hideAll();
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('error', message);
        }
      }
    }
  }


  doBack() {
    this.router.navigate(['..'], { relativeTo: this.routed })
  }

  async getData() {
    const res = await this.collotorService.getTeamAssignmentById(this.id);
    if (res.status == true) {
      this.detail = (res as any).data || {};
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

  async doCancel() {
    const messagebody = this.translate.instant('DialogCancel');
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then((res) => {
      if (res) {
        this.markFormPristine();
        this.maxStartDate = null;
        this.minEndDate = null;
        this.getData();
      }
    });
  }

  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
  }

}
