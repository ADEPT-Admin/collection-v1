import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { SecurityService, SystemParameter } from '../security.service';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { ToolbarComponent } from 'src/app/Shared/Components/toolbar/toolbar.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import * as _ from 'lodash';
import { ActivatedRoute, Router } from '@angular/router';
import { CanComponentDeactivate } from 'src/app/Shared/Guards/confirm-exit.guard';
import { Observable } from 'rxjs';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';

@Component({
  selector: 'app-change-password',
  imports: [CommonModule,
    FormsModule,
    ToolbarComponent,
    ReactiveFormsModule,
    TranslateModule,
    ShareDirectiveModule
  ],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss',
  providers: [SecurityService, Utils]
})

export class ChangePasswordComponent implements OnInit, AfterViewInit, CanComponentDeactivate {
  async canDeactivate(): Promise<boolean> {
    if (!this.form.dirty) return true;
    const result = await this.utils.dialogConfirmSaveChange()
    return result;
  }

  @ViewChild('myForm', { static: false }) form!: NgForm;
  @ViewChild("Username") public userTextBox: ElementRef;
  @ViewChild('pwInput') pwTextBox: ElementRef;
  actionListbtn: any = {
    new: { show: false, text: null, disabled: false },
    save: { show: true, text: null, disabled: true },
    delete: { show: false, text: null, disabled: false },
    cancel: { show: true, text: null, disabled: false },
    back: { show: false, text: null, disabled: false }
  };

  currentPassword: string | null = null;
  newPassword: string | null = null;
  confirmPassword: string | null = null;
  constructor(

    private securityService: SecurityService,
    private cdRef: ChangeDetectorRef,
    private utils: Utils,
    private translate: TranslateService,
    private loader : AppLoaderService
  ) {
    this.actionListbtn.cancel.disabled = !this.form?.dirty;
  }

  ngOnInit() { }

  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
  }

  async changepassword() {
    this.form.onSubmit(null);          // จะ trigger submitted = true
    this.form.control.markAllAsTouched();
    if (this.form.valid) {
      const requestModel: any = {
        currentPassword: this.currentPassword,
        newPassword: this.newPassword,
        confirmPassword: this.confirmPassword
      };

      const loaderId = this.loader.show();

      const res = await this.securityService.changepassword(requestModel);
      if (res.status) {
        const message = res.message[this.translate.currentLang]
        this.utils.dialogMessageAll('success', message);
        this.currentPassword = '';
        this.newPassword = '';
        this.confirmPassword = '';
        this.markFormPristine();
      } else {
        // let msg = '';
        // res.errorMessages.forEach(item => {
        //   msg = msg + `<p>${item}</p>`;
        // });
        const message = res.message[this.translate.currentLang];

        this.utils.dialogMessageAll('error', message);
      }
      this.loader.hide(loaderId)

    }
  }

  async doCancel() {
    const messagebody = this.translate.instant('DialogCancel');
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then((res) => {
      if (res) {
        this.currentPassword = '';
        this.newPassword = '';
        this.confirmPassword = '';
        this.markFormPristine();
      }
    });
  }

  markFormPristine() {
  if (this.form) {
    this.form.resetForm({
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    });
    this.cdRef.detectChanges(); // เคลียร์สีแดงทันที
  }
}

}
