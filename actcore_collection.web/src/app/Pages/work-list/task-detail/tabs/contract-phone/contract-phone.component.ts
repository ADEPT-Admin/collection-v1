import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ContractPhone } from '../../../task.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { GridAllModule, GridComponent } from '@syncfusion/ej2-angular-grids';
import { Subscription, timer } from 'rxjs';
import { WorkService } from '../../../work.service';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-contract-phone',
  imports: [CommonModule, FormsModule, TranslateModule, GridAllModule,LocalizedDatePipe],
  templateUrl: './contract-phone.component.html',
  styleUrl: './contract-phone.component.scss',
})
export class ContractPhoneComponent implements OnInit, AfterViewInit,OnDestroy {
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
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
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


  dataBound(event) {
    if(this.grid){
    timer(100).subscribe(() => {
      this.utils.setDataGridFilter(this.grid);
    })
    }
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

  actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.payload = this.utils.handleGridAction(event, this.payload);
      this.getData();
    }
  }


  async getData() {
    if (this.key != undefined) {
      const res = await this.workService.getcontractphonelistUrl(this.key,this.payload)

      const list = res.data.datatables;
      list.forEach(element => {
        element.isVerifiedPhone = element.isVerifiedPhone == "True" ? true : false;
      });

      console.log();


      this.list = list;
    }
  }

}
