import { ApiService } from 'src/app/Shared/Services/api.service';
import { AfterViewInit, Component, ElementRef, HostListener, inject, NgZone, OnInit, ViewChild } from '@angular/core';
import { SharedService } from '../../Shared/Services/shared.service';
import { Router } from '@angular/router';
import { ConfigService } from '../../Shared/Services/config.service';

import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { FormsModule, NgForm, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { AuthService } from 'src/app/Shared/Services/authentication/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { lastValueFrom, timer } from 'rxjs';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';


declare function calcMD5(password): any;
declare function calSHA256(password): any;

import {TextBoxModule} from '@syncfusion/ej2-angular-inputs'
import { ButtonModule } from '@syncfusion/ej2-angular-buttons'
import { SearchModalComponent } from 'src/app/Modals/search-modal/search-modal.component';
import { ModalService } from 'src/app/Shared/Services/modal.service';
@Component({
    selector: 'app-change-pass',
    imports: [CommonModule, FormsModule, TranslateModule, ShareDirectiveModule , TextBoxModule , ButtonModule  ],
    providers : [ModalService],
    templateUrl: './change-pass.component.html',
    styleUrl: './change-pass.component.scss'
})
export class ChangePassComponent implements OnInit , AfterViewInit{

  @ViewChild('myForm', { static: false }) form! : NgForm ;
  @ViewChild("Username") public userTextBox: ElementRef;
  @ViewChild('pwInput' ) pwTextBox : ElementRef;

  @HostListener('document:keydown.enter', ['$event'])  onKeydownHandler(event: KeyboardEvent) {
    const keyEvent = (event as any)
      if (keyEvent.key == 'Enter' || keyEvent.keyCode == 13) {
        this.changePass()
      }
  }


  data : any = null;
  config : any = 'ADEPT Collection Management 0.1 © 2025';



  dropdownLanguage : any;
  dataLanguage : any;

  MD5Ver: any = [];
  MD5User: any = [];
  dataLoginJson: any =  {};

  loginMessage : any = null;

  router = inject(Router);
  currentLang = 'TH';

  userName: string ='';
  password: string ='';
  confirmPassword: string ='';

  constructor(
    private share : SharedService,
    private translate : TranslateService,
    private auth: AuthService,
    private utils : Utils,
    private loader : AppLoaderService,
    private apiService : ApiService

  ){
    this.translate.use('en');
  }




  async ngOnInit() {
    const username = sessionStorage.getItem('username');
    const userInfo = await lastValueFrom(this.auth.getUserInfo());
    this.userName = username

    const multiangs = await this.apiService.getAvailableLang();
    this.dropdownLanguage = (multiangs as any).data;
  }

  async ngAfterViewInit() {

    timer(4000).subscribe(() => {
      this.pwTextBox.nativeElement.focus();

    })
  }

  async getData(){
    this.share.getData().subscribe(data => {
      this.data = data;
    })
  }

  dropdownLanguage_OnChange(args: any): void {
    if (args) {
      this.dataLanguage = args.target.value?.toLowerCase();
      sessionStorage.setItem("language", this.dataLanguage);
      this.translate.use(this.dataLanguage);
     }
  }

  validateInput(userName: string, password: string) : boolean {
    if (userName !== '' && userName.length > 0 && password !== '' && password.length > 0) {
      return true;
    }
    return false;
  }


  async changePass() {

    this.form.onSubmit(null);          // จะ trigger submitted = true
    this.form.control.markAllAsTouched();

    if(this.form.valid){
      const password  = this.password.replace(/ /g, "");
      const confirmPassword  = this.confirmPassword.replace(/ /g, "");
      // this.dataLoginJson.userName = this.userName;
      this.dataLoginJson.newPassword = password;
      this.dataLoginJson.confirmPassword = confirmPassword;
      const loaderId = this.loader.show();
      const res = await lastValueFrom(this.auth.changePasswordNewUser(this.dataLoginJson));

      if (res instanceof HttpErrorResponse) {
        const messagebody = res.error.errorMessages.join("<br/>");
        this.utils.dialogMessageAll('error', `${messagebody}`);
        this.loader.hide(loaderId)
      }else{


        if(res.status){
            this.loader.hide(loaderId)
            const message = res.message[this.translate.currentLang]
            const res_msg = await this.utils.dialogMessageAll('success', `${message}`)
            if(res_msg){
              this.router.navigate(['/']);
            }
        }else{


          const messagebody = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('error', `${messagebody}`);
          this.loader.hide(loaderId)
          this.form.reset();

        }
      }


    }


  }


}
