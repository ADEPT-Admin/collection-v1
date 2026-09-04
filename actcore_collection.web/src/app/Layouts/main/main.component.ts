import { AfterViewInit, Component } from '@angular/core';
import { NavBarComponent } from '../nav-bar/nav-bar.component';
import { SideBarComponent } from '../side-bar/side-bar.component';
import { NavigationEnd, NavigationStart, Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { filter, Subscription, timer } from 'rxjs';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { WHITELIST_PATHS } from 'src/app/Shared/Enums/whitelist-url-enum';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { ApiService } from 'src/app/Shared/Services/api.service';
import { PermissionService } from 'src/app/Shared/Services/permission.service';
import { TranslateService } from '@ngx-translate/core';
import { Copyright } from 'src/app/Shared/Config/config';

@Component({
  selector: 'app-main',
  imports: [CommonModule, RouterOutlet, NavBarComponent, SideBarComponent],
  providers: [Utils ],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss',
})
export class MainComponent implements AfterViewInit {
  menu = [];
  whitelist = [];

  $lang : Subscription = null;
  config : any = `${Copyright}`;
  res = null;
  constructor(
    private utils: Utils,
    private router: Router,
    private loader: AppLoaderService,
    private apiService : ApiService,
    private permissionService : PermissionService,
    private translate : TranslateService
  ) {
    // get api permission ///
    const loaderId = this.loader.show();

    this.process(loaderId)

    this.$lang = this.translate.onLangChange.subscribe(async data => {

      if(this.res != null){
          const res = await this.utils.getMenuPermission((this.res as any).data);
          this.menu = res['menu'];
      }

    })

  }

  async process(loaderId) {


    const res = await this.genData();

    this.menu = res['menu'];
    this.whitelist = [...WHITELIST_PATHS, ...res['whiteList']];

    this.permissionService.setMenu(this.menu)


    /// whitelist url check ///

    // First Load Check
    this.checkWhiteList(this.router);


    // Url Change Check
    this.router.events
      .pipe(filter((event: any) => event instanceof NavigationEnd))
      .subscribe(async (route) => {
        this.checkWhiteList(route);
    });
    this.loader.hide(loaderId);

  }

  async genData() {
    sessionStorage.removeItem('employee')
    sessionStorage.removeItem('group')
    this.res = await this.apiService.getPermission();

    sessionStorage.setItem('employee' , JSON.stringify((this.res as any).data.employee))
    sessionStorage.setItem('group' , JSON.stringify((this.res as any).data.userGroup))
    sessionStorage.setItem('supervisor' ,((this.res as any).data.isCollectorSupervisor || false ));
    let permission = {}
    if((this.res as any).status == true || (this.res as any).data ){
      permission = await this.utils.getMenuPermission((this.res as any).data);
    }


    if (this.router.url === '/' || this.router.url === '') {

      if(permission['menu'] && permission['menu'][0]){
        const link = this.utils.getFirstChildLinkFromArray(permission['menu'] , 0);
        this.router.navigate(link)
      }
    }


    this.permissionService.setPermission(permission['permission']);
    const data = {
      menu: permission['menu'],
      whiteList: permission['whitelist'],
    };
    return data;
  }

  async ngAfterViewInit() {
    await this.utils.loadScript('assets/adminlte4/js/adminlte.js', true);
  }

  ngOnDestroy(){
    this.$lang.unsubscribe();
    this.whitelist = [];
  }

  checkWhiteList(route){
    let isAllowed = false;

    if (route.url != '/') {
      this.whitelist.forEach((item) => {
        if (route.url.includes(item)) {
          isAllowed = true;
        }
      });
    } else {
      isAllowed = true;
    }


    if (!isAllowed) {
      if(this.whitelist.length > 0){
          this.router.navigate(['404']);
      }
    }
  }
}
