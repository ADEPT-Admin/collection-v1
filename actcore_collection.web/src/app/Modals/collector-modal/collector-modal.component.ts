import { Component, OnInit, ViewChild } from '@angular/core';
import { ToolbarComponent } from 'src/app/Shared/Components/toolbar/toolbar.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Subscription, timer, filter } from 'rxjs';
import * as _ from 'lodash';
import {
  ActionEventArgs,
  GridComponent,
  GridModule,
  PageService,
  SortService,
  FilterService,
  FreezeService,
  ResizeService
} from '@syncfusion/ej2-angular-grids';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
import { SecurityService } from 'src/app/Pages/securities/security.service';
import { ModalChildBase } from '../base-modal/base-modal.component';
import { FilterSelectComponent } from 'src/app/Shared/Components/filter-select/filter-select.component';
import { CollectorService } from 'src/app/Pages/collector/collector.service';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { LowercaseFirstPipe } from 'src/app/Shared/Pipes/lowercase-first.pipe';
import { SafeHtmlPipe } from 'src/app/Shared/Pipes/safe-html.pipe';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-employee-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    TranslateModule,
    GridModule,
    ShareDirectiveModule,
    ButtonModule,
    FilterSelectComponent,
    LowercaseFirstPipe,
    SafeHtmlPipe,
    LocalizedDatePipe


  ],
  providers: [SecurityService, PageService, SortService, FilterService, FreezeService, ResizeService],
  templateUrl: './collector-modal.component.html',
  styleUrls: ['./collector-modal.component.scss'],
})


export class CollectorModalComponent extends ModalChildBase implements OnInit {
  collapsed = false;
  @ViewChild('grid') public grid!: GridComponent;
  columns: any = [];
  criteria: any;
  selectFilter: any;
  selectFilterType: any;
  searchVal = '';
  userGroups: any = [];
  userGroupslist: any = [];
  filter: any[] = [];
  userGroupsSub: Subscription = null;
  public customAttributes: Object = {};
  public selectionOptions?: any = GridSelect;
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  public resizeSettings = { mode: 'Normal' };
  permission = {
    "allowView": false,
    "allowNew": false,
    "allowEdit": false,
    "allowDelete": false
  };

  payload: any = {
    pageNumber: 1,
    pageSize: 10,
    sortColumn: null,
    sortDirection: 'asc',
    filters: {}
  }
  dataSysParameters: any = [];
  dataSource: { result: any[]; count: number } = {result : [] , count:0};
  rowIdClickData: any;
  valueselectFilter : any = '';
  gridHeader: any = [];
  $lang: Subscription = null;
  loaded : boolean = false;

  constructor(
    private securityService: SecurityService,
    private translate: TranslateService,
    private utils: Utils,
    private collectorService : CollectorService
  ) {

    super();



    this.$lang = this.translate.onLangChange.subscribe(item => {
      const tempSelectFilter = _.cloneDeep(this.selectFilter);
      this.selectFilter = null;
      timer(10).subscribe(() => {
        this.selectFilter = _.cloneDeep(tempSelectFilter);
      });
      if (this.grid != undefined) {
        this.utils.setDataGridFilter(this.grid)

      }

    });

  }

  async ngOnInit() {
    this.customAttributes = { class: 'customcss' };

    this.getCollectors()

  }


  async getCollectors() {
    delete this.payload.filters.selectFilter;
    this.collectorService.getCollectors(this.payload , true).then(res => {
      let result: any = res;
      const record = (res as any).data.datatables;
      const count = (res as any).data.totalRecords;
      if (record.length == 0) {
        this.payload.pageNumber = 1;
      }
      this.dataSource = { result: record, count: count };
      this.columns = result.data.displayColumns;
      this.criteria = this.columns;
      if(this.gridHeader.length == 0){
        this.gridHeader = this.criteria;
      }
      this.loaded = true;
    })
  }

  ngOnDestroy() {


    this.$lang.unsubscribe();
  }

  doubleClick(event) {
    if (event.rowData) {
      this.close({
        collectorId: event.rowData.collectorGuid,
        employeeName: event.rowData.employeeName || '',
        employeeId : event.rowData.employeeId,
        capacity : event.rowData.capacity,
        colRoleName :  event.rowData.colRoleName,
        collectorName : event.rowData.collectorName
      });
    }
  }

  async actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.payload = this.utils.handleGridAction(event, this.payload);
      await this.getCollectors();
    }
  }

  filterCallback(value: any) {
    this.searchVal = value;
  }

  getTextAlign(type: string) {
    return this.utils.getAlign(type);
  }

  rowSelectedclick(e) {
    this.rowIdClickData = e.data;
  }

  closeWithData() {
    if (this.rowIdClickData) {
      this.close({
        collectorId: this.rowIdClickData.collectorGuid,
        employeeName: this.rowIdClickData.employeeName || '',
        employeeId : this.rowIdClickData.employeeId,
        capacity : this.rowIdClickData.capacity,
        colRoleName :  this.rowIdClickData.colRoleName,
        collectorName : this.rowIdClickData.collectorName
      });
    } else {
      this.close();
    }
  }


  dataBound(event){
    if (this.grid) {
    timer(100).subscribe(() => {
        this.utils.setDataGridFilter(this.grid)
    })
    }
  }

  async onFilterSearch(filterData: any) {
    if (!filterData || Object.keys(filterData).length === 0 || (Object.keys(filterData).length === 1 && 'selectFilter' in filterData)) {
      this.payload.filters = {};
    } else {
      this.payload.filters = filterData;
    }
    let firstKey = Object.keys(filterData).find(k => k !== 'selectFilter');
    if (!firstKey && filterData.selectFilter) {
      firstKey = filterData.selectFilter;
    }
    this.valueselectFilter = firstKey || '';
    await this.getCollectors();
    this.grid.refresh();
  }

}
