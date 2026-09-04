import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, Input } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { PageRoutingModule } from 'src/app/Pages/page-routing.module';

@Component({
  selector: 'app-header',
  imports: [CommonModule , RouterModule ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

  @Input() header : any;

  lang = null;

  $lang : Subscription = null;

  constructor(private translate : TranslateService , private router : Router){
    this.lang = this.translate.currentLang;
    this.$lang = this.translate.onLangChange.subscribe(async item => {
        this.lang = this.translate.currentLang;
    })
  }


  ngOnDestroy(){
    this.$lang.unsubscribe();
  }


  navigate(item){
    this.router.navigate([...['.'],...item.link])
  }






}
