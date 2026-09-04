import { interval, lastValueFrom, Subscription, timer } from 'rxjs';
import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, EventEmitter, inject, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/Shared/Services/authentication/auth.service';
import Swal from 'sweetalert2';
import { ApiService } from 'src/app/Shared/Services/api.service';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
import { FormsModule } from '@angular/forms';
import { ShareService } from 'src/app/Shared/Services/share.service';



@Component({
    selector: 'app-nav-bar',
    imports: [CommonModule, TranslateModule , ShareDirectiveModule , SelectComponent , FormsModule],
    templateUrl: './nav-bar.component.html',
    styleUrl: './nav-bar.component.scss',
    providers: [
        TranslateService,
    ]
})
export class NavBarComponent implements OnInit , AfterViewInit {

  router = inject(Router);

  lang = 'en';

  user = '';

  employee : any = {employeeId : '' , nameLanguage : {},email : '' ,position : '' , department : ''};
  textDepartmentandPosition  = {};
  group = "";
  $lang : Subscription = null;

  avatars = [
    '1758610358912.jpg',
    '1758610927040.jpg',
    '1758612190586.jpg'
  ]

  avatar :string = '';

  constructor(
    private translate : TranslateService,
    private authService : AuthService,
    private apiService : ApiService,
    private share : ShareService
  ){



    this.$lang = this.translate.onLangChange.subscribe(item => {
        this.lang = item.lang;
        localStorage.setItem('lang' , this.lang)
    })

    this.avatar = this.avatars[Math.floor(Math.random() * this.avatars.length)];
    this.lang = localStorage.getItem('lang') || this.translate.currentLang  || 'en';
    this.translate.use(this.lang)




  }




  @Output() toggleSideBar = new EventEmitter();

  readonly modelLanguageDropdown = [];
  dataLanguage : any;
  dropdownLanguage = [];



  async ngOnInit() {


    const multiangs = await this.apiService.getAvailableLang();
    this.dropdownLanguage = (multiangs as any).data;
    this.share.setAvailableLang((multiangs as any).data)
    this.dropdownLanguage.map(item => {
      item.label = item.text
      item.value = item.value.toLowerCase()
    })

  }

  ngAfterViewInit(): void {

    const check$ = interval(100).subscribe(() => {
    const employeeStr = sessionStorage.getItem('employee');
    const groupStr = sessionStorage.getItem('group');

    if (employeeStr && groupStr) {
      this.user = sessionStorage.getItem('username')
      this.employee = JSON.parse(employeeStr) || {};

      this.textDepartmentandPosition = {};
      ['department', 'position'].forEach(key => {
        const langs = this.employee?.[key]?.nameLanguage || {};
        Object.keys(langs).forEach(lang => {
          if (!this.textDepartmentandPosition[lang]) {
            this.textDepartmentandPosition[lang] = [];
          }
          this.textDepartmentandPosition[lang].push(langs[lang]);
        });
      });

      // Convert arrays → comma separated strings
      Object.keys(this.textDepartmentandPosition).forEach(lang => {
        this.textDepartmentandPosition[lang] = this.textDepartmentandPosition[lang].filter(x => !!x).join(', ');
      });


      const group2 = JSON.parse(groupStr) || [];
      this.group = group2.map(item => item.userGroupName).join(', ');
      check$.unsubscribe(); // stop checking once loaded ✅
    }
  });


  }

  async logout() {
    const result = await Swal.fire({
      title: this.translate.instant('Confirmation'),
      text: this.translate.instant('Are you sure you want to log out?'),
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: this.translate.instant('OK'),
      cancelButtonText: this.translate.instant('Cancel'),
    didOpen: () => {
      const confirmBtn = document.querySelector('.swal2-confirm') as HTMLElement;
      const cancelBtn = document.querySelector('.swal2-cancel') as HTMLElement;
      if (confirmBtn) confirmBtn.style.minWidth = '70px';
      if (cancelBtn) cancelBtn.style.minWidth = '70px';
    }
    });
    if (result.isConfirmed) {
      const res = await lastValueFrom(this.authService.logout());
      if((res as any).status == true){
        sessionStorage.clear();
        this.router.navigate(['login'])
      }


    }
  }


  changeLang(args){
    if(this.dataLanguage == args) {
      return
    }
    if (args) {
      this.dataLanguage = args.toLowerCase();
      sessionStorage.setItem("language", this.dataLanguage);
      this.translate.use(this.dataLanguage);
    }
  }


  changePassword(){
    this.router.navigate(['user','change-password'])
  }

  ngOnDestroy(){
    this.$lang.unsubscribe();
  }


}
