import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Address } from '../../../task.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { GridAllModule, GridComponent, ResizeService } from '@syncfusion/ej2-angular-grids';
import { Subscription, timer } from 'rxjs';
import { WorkService } from '../../../work.service';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-address',
  imports: [CommonModule, FormsModule, TranslateModule, GridAllModule,LocalizedDatePipe],
  providers: [ResizeService],
  templateUrl: './address.component.html',
  styleUrl: './address.component.scss',
})
export class AddressComponent implements OnInit, AfterViewInit ,OnDestroy {
  @Input() key: any;

  list: any = [];
  payload = {
    pageNumber: 1,
    pageSize: 10,
    sortColumn: null,
    sortDirection: 'asc',
    filters: {}
  }
  @ViewChild('grid') public grid!: GridComponent;
  langSub : Subscription = null;
  public selectionOptions?: any = GridSelect;
  public customAttributes: Object = { class: 'customcss' };
  public pageSettings = { pageSize: 10, pageSizes: [ 10, 20, 50, 100] };
  public resizeSettings = { mode: 'Normal' };
  private loaded = false;
  constructor(
    private workService: WorkService, private utils: Utils, private translate: TranslateService,) {
    this.langSub = this.translate.onLangChange.subscribe(item => {
      if (this.grid != undefined) {
        this.utils.setDataGridFilter(this.grid)
      }
    })
  }

  ngOnDestroy() {
    this.langSub.unsubscribe();
  }

  ngOnInit() {
    timer(10).subscribe(() => {
      if (!this.loaded) {
        this.loaded = true;
        this.getData();
      }

    })
  }
  ngAfterViewInit(): void {

  }


  dataBound(event) {
    if(this.grid){
    timer(100).subscribe(() => {
      this.utils.setDataGridFilter(this.grid);
    })
    }
  }

  actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.payload = this.utils.handleGridAction(event, this.payload);
      this.getData()
    }
  }


  async getData() {
    if (this.key != undefined) {
      const res = await this.workService.getcontractAddresslistUrl(this.key , this.payload)

      const list = res.data.datatables;
      list.forEach(element => {
        element.isVerifiedAddress = element.isVerifiedAddress == 'Y' ? true : false;
      });


      this.list = {result: list, count: res.data.totalRecords };
    }
  }

}
