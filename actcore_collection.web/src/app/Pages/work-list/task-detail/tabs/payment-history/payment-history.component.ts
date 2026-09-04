import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ContractPayment } from '../../../task.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { GridAllModule, GridComponent, ResizeService } from '@syncfusion/ej2-angular-grids';
import { Subscription, timer } from 'rxjs';
import { WorkService } from '../../../work.service';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { InputNumberComponent } from 'src/app/Shared/Components/input-number/input-number.component';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-payment-history',
  imports: [CommonModule, FormsModule, TranslateModule, GridAllModule,InputNumberComponent,LocalizedDatePipe],
  providers: [ResizeService],
  templateUrl: './payment-history.component.html',
  styleUrl: './payment-history.component.scss'
})
export class PaymentHistoryComponent implements OnInit, AfterViewInit ,OnDestroy{
  @Input() key: any;

  list: any = [];
  payload = {
    pageNumber: 1,
    pageSize: 10,
    sortColumn: null,
    sortDirection: 'asc',
    filters: {}
  }
  detailPayment: any = {
    contractTerm: '',
    paidTerm: '',
    paidAmount: ''
  };
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
      const res = await this.workService.getcontractPaymentlistUrl(this.key , this.payload);
      // this.list = { res.data.datatables;
      this.list = {result: res.data.datatables, count: res.data.totalRecords };

      this.detailPayment.contractTerm = res.data.contractTerm;
      this.detailPayment.paidTerm = res.data.paidTerm;
      this.detailPayment.paidAmount = res.data.paidAmount;
    }
  }

  dataBound(event) {
    if(this.grid){
    timer(100).subscribe(() => {
      this.utils.setDataGridFilter(this.grid);
    })
    }
  }

}
