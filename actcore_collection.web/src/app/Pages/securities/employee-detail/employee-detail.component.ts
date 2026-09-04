import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { employee, SecurityService } from '../security.service';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { ToolbarComponent } from 'src/app/Shared/Components/toolbar/toolbar.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import * as _ from 'lodash';
import { ActivatedRoute, Router } from '@angular/router';
import { CanComponentDeactivate } from 'src/app/Shared/Guards/confirm-exit.guard';
import { HeaderComponent } from 'src/app/Shared/Components/header/header.component';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { DateTimePickerModule } from '@syncfusion/ej2-angular-calendars'

@Component({
  selector: 'app-employee-detail',
  imports: [
    CommonModule,
    FormsModule,
    ToolbarComponent,
    ReactiveFormsModule,
    TranslateModule,
    HeaderComponent,
    DateTimePickerModule
  ],
  templateUrl: './employee-detail.component.html',
  styleUrl: './employee-detail.component.scss',
  providers: [SecurityService, Utils]
})
export class EmployeeDetailComponent implements OnInit, AfterViewInit, CanComponentDeactivate {
  async canDeactivate(): Promise<boolean> {
    if (!this.form?.dirty) return true;
    const result = await this.utils.dialogConfirmSaveChange()
    return result;
  }

  @ViewChild('myForm') form!: NgForm;
  actionListbtn: any = {
    new: { show: false, text: null, disabled: false },
    save: { show: true, text: null, disabled: true },
    delete: { show: false, text: null, disabled: false },
    cancel: { show: true, text: null, disabled: false },
    back: { show: true, text: null, disabled: false }
  };

  detail: any = [];
  original: employee = {};
  id = null;
  menuId = null;
  header = null;
  permission = {
    "allowView": null,
    "allowNew": false,
    "allowEdit": false,
    "allowDelete": false
  };
  payload: any = {
    pageNumber: 1,
    pageSize: 10,
    sortColumn: null,
    sortDirection: 'asc',
    filters: {}
  }

  constructor(
    private routed: ActivatedRoute,
    private router: Router,
    private securityService: SecurityService,
    private cdRef: ChangeDetectorRef,
    private utils: Utils,
    private translate: TranslateService,
    private loader: AppLoaderService
  ) {
    this.actionListbtn.cancel.disabled = !this.form?.dirty;
    this.routed.paramMap.subscribe(async params => {
      this.id = params.get('id');
      this.detail = await this.securityService.getEmployeegetbyid(this.id);
      if (this.detail.status == true) {
        this.detail = (this.detail as any).data || {};
        this.original = _.cloneDeep(this.detail);
        this.menuId = this.routed.snapshot.data?.menuId || '0701';
        this.permission = await this.utils.getPermission(this.menuId);
        this.header = await this.utils.getMenu(this.menuId)
        this.header = await this.utils.getMenu(this.menuId)
        this.header.path[this.header.path.length - 1].canLink = true;
        this.header.path = [...this.header.path, ...[{ menuId: 'detail', itemName: { th: `รายละเอียด (${this.id})`, en: `Detail (${this.id})` } }]];
        if (!this.permission?.allowEdit) {
          setTimeout(() => {
            this.form?.control.disable();
          }, 200);
        }
        this.cdRef.detectChanges();
      } else {
        const message = this.detail.message[this.translate.currentLang];
        this.loader.hideAll();
        await this.utils.dialogMessageAll('error', message);
        this.doBack();
      }

    });

  }

  ngOnInit() { }

  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
  }

  doBack() {
    this.router.navigate(['..'], { relativeTo: this.routed })
  }

  async doSave() {
    // const res = await this.securityService.updateMenu(this.detail);
    // if (res.status) {
    //   const message = res.message[this.translate.currentLang];
    //   this.utils.dialogMessageAll('success', message);
    //   this.original = _.cloneDeep(this.detail);
    //   this.markFormPristine();
    // } else {
    //   const message = res.message[this.translate.currentLang];
    //   this.utils.dialogMessageAll('error', message);
    // }
  }

  async doCancel() {
    const messagebody = this.translate.instant('DialogCancel');
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then((res) => {
      if (res) {
        this.detail = _.cloneDeep(this.original);
        this.markFormPristine();
      }
    });
  }

  markFormPristine() {
    Object.values(this.form.controls).forEach(control => {
      control.markAsPristine();
      control.markAsUntouched(); // Optional: also reset "touched"
      control.updateValueAndValidity();
    });
  }


}
