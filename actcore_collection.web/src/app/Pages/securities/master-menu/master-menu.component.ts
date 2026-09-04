import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SecurityService } from '../security.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import * as _ from 'lodash';
import {
  GridComponent,
  GridModule,
  PageService,
  SortService,
  FilterService,
  FreezeService,
  ResizeService
} from '@syncfusion/ej2-angular-grids';
import { ButtonModule, CheckBoxModule } from '@syncfusion/ej2-angular-buttons';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { HeaderComponent } from 'src/app/Shared/Components/header/header.component';
import { FilterSelectComponent } from 'src/app/Shared/Components/filter-select/filter-select.component';
import { ShareService } from 'src/app/Shared/Services/share.service';
import { Subscription, timer, filter, lastValueFrom, firstValueFrom } from 'rxjs';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { LowercaseFirstPipe } from 'src/app/Shared/Pipes/lowercase-first.pipe';
import { SafeHtmlPipe } from 'src/app/Shared/Pipes/safe-html.pipe';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-master-menu',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    TranslateModule,
    GridModule,
    ButtonModule,
    ShareDirectiveModule,
    CheckBoxModule,
    HeaderComponent,
    FilterSelectComponent,
    LowercaseFirstPipe,
    SafeHtmlPipe,
    LocalizedDatePipe
  ],
  templateUrl: './master-menu.component.html',
  styleUrl: './master-menu.component.scss',
  providers: [SecurityService,PageService,SortService,FilterService, FreezeService,ResizeService]
})

export class MasterMenuComponent  implements OnInit, OnDestroy {
  collapsed = false;
  @ViewChild('grid') public grid!: GridComponent;
  columns: any = [];
  criteria: any;
  selectFilter: any;
  selectFilterType: any;
  searchVal = '';
  valueselectFilter : any = '';
  filter: any[] = [];
  public selectionOptions?: any = GridSelect;
  public customAttributes: Object = {};
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  public resizeSettings = { mode: 'Normal' };
  actionListbtn: any = {
    new: { show: true, text: null, disabled: false },
    save: { show: false, text: null, disabled: true },
    delete: { show: true, text: null, disabled: false },
    cancel: { show: false, text: null, disabled: true },
    back: { show: false, text: 'ScorecardList', disabled: true }
  };
  permission = {
    "allowView": null,
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

  dataSource: { result: any[]; count: number } = {result : [] , count:0};
  langSub : Subscription = null;
  menuId = null;
  header = null;
  inputType: string = 'text';
  gridHeader: any = [];
  constructor(
    private securityService: SecurityService,
    private router: Router,
    private routed: ActivatedRoute,
    private translate: TranslateService,
    private utils: Utils,
    private share : ShareService
  ) {
    this.langSub = this.translate.onLangChange.subscribe(async item => {
      const tempSelectFilter = _.cloneDeep(this.selectFilter);
      this.selectFilter = null;
      timer(10).subscribe(() => {
        this.selectFilter = _.cloneDeep(tempSelectFilter);
      });
      if(this.grid != undefined){
        this.utils.setDataGridFilter(this.grid);
      }
    });
  }

  async ngOnInit() {
    this.customAttributes = { class: 'customcss' };
    await this.getMenuListUrl();
    this.menuId = this.routed.snapshot.data?.menuId || '0906';
    this.permission = await this.utils.getPermission(this.menuId);
    this.header = await this.utils.getMenu(this.menuId)
    this.actionListbtn.new.disabled = !this.permission.allowNew;
    this.actionListbtn.delete.disabled = !this.permission.allowDelete;
  }

  async getMenuListUrl() {
    delete this.payload.filters.selectFilter;
    this.securityService.getMenuListPageUrl(this.payload).then(res => {
      let result : any = res;
      const record = (res as any).data.datatables;
      const count = (res as any).data.totalRecords;
      if (record.length == 0) {
        this.payload.pageNumber = 1
      }
      this.dataSource = { result: record, count: count };
      this.criteria = result.data.displayColumns;
      if(this.gridHeader.length == 0){
        this.gridHeader = this.criteria;
      }
    })
  }

  getTextAlign(type: string) {
    return this.utils.getAlign(type);
  }

  ngOnDestroy() {
    this.langSub.unsubscribe();
  }

  doubleClick(event) {
    this.edit(event.rowData);
  }

  edit(data) {
    this.router.navigate([data.itemId], { relativeTo: this.routed });
  }


  async actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.payload = this.utils.handleGridAction(event, this.payload);
      await this.getMenuListUrl();
    }
  }

  dataBound(event) {
    if(this.grid){
    timer(100).subscribe(() => {
      this.utils.setDataGridFilter(this.grid);
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
    await this.getMenuListUrl();
    this.grid.refresh();
  }
}
