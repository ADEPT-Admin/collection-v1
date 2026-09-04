import { AfterViewInit, ChangeDetectorRef, Component, Input, OnDestroy, ViewChild } from '@angular/core';
import { OutstandingBalance, OutstandingBalanceList } from '../../../task.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { InputNumberComponent } from 'src/app/Shared/Components/input-number/input-number.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AggregateService, GridAllModule, GridComponent, ResizeService } from '@syncfusion/ej2-angular-grids';
import { Subscription, timer } from 'rxjs';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { WorkService } from '../../../work.service';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';

@Component({
  selector: 'app-outstanding-balance',
  imports: [CommonModule , FormsModule , TranslateModule , GridAllModule , InputNumberComponent],
  templateUrl: './outstanding-balance.component.html',
  styleUrl: './outstanding-balance.component.scss',
  providers : [AggregateService,ResizeService]

})
export class OutstandingBalanceComponent implements AfterViewInit , OnDestroy{
  @Input() key : any;

  @ViewChild('grid') public grid!: GridComponent;
  public selectionOptions?: any = GridSelect;
  public resizeSettings = { mode: 'Normal' };
  lang$ : Subscription = null;
  constructor(
    private cdr : ChangeDetectorRef,
    private translate : TranslateService,
    private utils : Utils,
    private workService : WorkService
  ){
        this.lang$ = this.translate.onLangChange.subscribe(item => {
          if(this.grid != undefined){
            this.utils.setDataGridFilter(this.grid)
          }
        })
  }

  ngOnDestroy() {
    this.lang$.unsubscribe();
  }

  dataBound(event) {
    if(this.grid){
    timer(100).subscribe(() => {
      this.utils.setDataGridFilter(this.grid);
    })
    }
  }

  detail : OutstandingBalance = {loanAmount : 0 , outstandingAmount : 0 , totalOverdues :0};
  list : OutstandingBalanceList[] = [];
  public customAttributes: Object = { class: 'customcss'};
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };

  ngAfterViewInit(): void {
    timer(10).subscribe(() => {
        this.getData();
    })

  }



  async getData(){

    if(this.key != undefined){

      const res = await this.workService.getOverDueDetail(this.key);
      this.detail.loanAmount = res.data.loanAmount;
      this.detail.outstandingAmount = res.data.outstandingAmount;
      this.detail.totalOverdues = res.data.totalOverdues;
      this.list = res.data.overdues;

      this.list = this.list.map((item , index) => {
        item.rowIndex = index+1;
        return item
      })

    }


  }

}
