import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';



import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { LoaderComponent } from './Shared/Components/loader/loader.component';
import { Utils } from './Shared/Utilites/utils';
@Component({
    selector: 'app-root',
    imports: [RouterOutlet, LoaderComponent, TranslateModule],
    providers: [Utils],
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit   {
  title = 'poc-angular-microfrontend';
  constructor(
    private translate : TranslateService,
  ){
    this.translate.setDefaultLang('en');
    this.translate.use('en')
  }


  ngOnInit() {


  }


}
