import { AfterViewInit, Component, ElementRef, HostListener, inject, NgZone, OnInit, ViewChild } from '@angular/core';
import { SharedService } from '../../Shared/Services/shared.service';
import { Router } from '@angular/router';
import { ConfigService } from '../../Shared/Services/config.service';

import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { AuthService } from 'src/app/Shared/Services/authentication/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { lastValueFrom, Subscription, timer } from 'rxjs';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';


import {TextBoxModule} from '@syncfusion/ej2-angular-inputs'
import { ButtonModule } from '@syncfusion/ej2-angular-buttons'
import { ModalService } from 'src/app/Shared/Services/modal.service';
import { ApiService } from 'src/app/Shared/Services/api.service';
import { ADEPT_ProductName, Build, Copyright, Version } from 'src/app/Shared/Config/config';
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
@Component({
    selector: 'app-login',
    imports: [CommonModule, FormsModule, TranslateModule, ShareDirectiveModule , TextBoxModule , ButtonModule , SelectComponent ],
    providers : [ModalService],
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit , AfterViewInit{

  @ViewChild('myForm', { static: false }) form! : NgForm ;
  @ViewChild("Username") public userTextBox: ElementRef;
  @ViewChild('pw' ) pwTextBox : ElementRef;
  @ViewChild('btn', { static: true }) btn!: ElementRef<HTMLButtonElement>;
  @HostListener('document:keydown.enter', ['$event'])  onKeydownHandler(event: KeyboardEvent) {
    const keyEvent = (event as any)
      if (keyEvent.key == 'Enter' || keyEvent.keyCode == 13) {
        this.login()
      }
  }


  data : any = null;
  config : any = `${ADEPT_ProductName} V ${Version} Build ${Build} ${Copyright}`;



  dropdownLanguage : any;
  readonly modelLanguageDropdown = [];
  dataLanguage : any;

  MD5Ver: any = [];
  MD5User: any = [];
  dataLoginJson: any =  {};

  loginMessage : any = null;

  router = inject(Router);
  currentLang = 'en';

  userName: string ='';
  password: string ='';


  $lang : Subscription = null;

  constructor(
    private share : SharedService,
    private translate : TranslateService,
    private auth: AuthService,
    private utils : Utils,
    private loader : AppLoaderService,
    private apiService : ApiService

  ){

    this.$lang = this.translate.onLangChange.subscribe(item => {
        this.dataLanguage = item.lang;
    })

    this.dataLanguage = this.translate.currentLang || 'en';
    this.translate.use('en');
    sessionStorage.clear();
  }




  async ngOnInit() {


    const loaderId = this.loader.show();

    const multiangs = await this.apiService.getAvailableLang();
    this.dropdownLanguage = (multiangs as any).data;

    this.dropdownLanguage.map(item => {
      item.label = item.text
      item.value = item.value.toLowerCase()
    })


    this.loader.hide(loaderId)


  }

  async ngAfterViewInit() {

    timer(0).subscribe(() => {

      this.userTextBox.nativeElement.focus();

    })

  }

  ngOnDestroy(){
    this.$lang.unsubscribe();
  }

  async getData(){
    this.share.getData().subscribe(data => {
      this.data = data;
    })
  }

  dropdownLanguage_OnChange(args: any): void {
    if (args) {
      this.dataLanguage = args.toLowerCase();
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


  async login() {

    const userName = this.userName ?? '';
    const password = this.password ?? '';


    if (this.validateInput(userName.replace(/ /g, ""), password.replace(/ /g, ""))) {

      this.dataLoginJson.userName = userName.replace(/\s/g, "");
      this.dataLoginJson.password = password;
      try {

        const loaderId = this.loader.show();
        const res = await lastValueFrom(this.auth.AuthenticateloginUser(this.dataLoginJson));

        if (res instanceof HttpErrorResponse) {
          const messagebody = res.error.errorMessages.join("<br/>");
          this.utils.dialogMessageAll('error', `${messagebody}`);
          this.loader.hide(loaderId)
        }else{


          if(res.status){
              sessionStorage.setItem('token', res.data.accessToken);
              sessionStorage.setItem('re', res.data.refreshToken);
              const userInfo = await lastValueFrom(this.auth.getUserInfo());
              sessionStorage.setItem("user", userInfo.data.userId);
              sessionStorage.setItem("username" ,this.dataLoginJson.userName )
              // const userObj = {
              //   firstName : userInfo.data.employee.firstName ,
              //   lastName : userInfo.data.employee.lastName ,
              //   isCollector: userInfo.data.isCollector,
              //   isSupervisorCollector: userInfo.data.isSupervisorCollector,
              //   employeeId : userInfo.data.employee.employeeId
              // }
              // sessionStorage.setItem("employee", JSON.stringify(userObj));
              if(!userInfo.data.isNewUser){
                this.router.navigate(['/']);
              }else{
                this.router.navigate(['/change-password']);
              }
              this.loader.hide(loaderId)
          }else{
            const message = res.message[this.translate.currentLang];
            this.utils.dialogMessageAll('error', message);
            this.loader.hide(loaderId)
            this.form.reset();

          }

        }

      }
      catch (e) {
        this.loginMessage = e;
      }
    } else {
      let messagebody = this.utils.listMessageLanguage(this.translate.currentLang,'InvalidUserIDorPasswordPleasetryagain');
      this.utils.dialogMessageAll('error', messagebody);
      this.userTextBox.nativeElement.focus();
      this.form.reset();
    }
  }


}
