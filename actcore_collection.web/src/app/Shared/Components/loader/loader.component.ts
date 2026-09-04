import { Component } from '@angular/core';
import { AppLoaderService } from './loader.service';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-loader',
    imports: [CommonModule],
    templateUrl: './loader.component.html',
    styleUrls: ['./loader.component.scss']
})
export class LoaderComponent {

  isShow : boolean = false;
  constructor(private loaderService : AppLoaderService) {
    this.loaderService.getLoader().subscribe(data => {
      this.isShow = data;
    })
  }
}
