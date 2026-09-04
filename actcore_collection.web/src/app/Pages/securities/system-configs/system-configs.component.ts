import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SecurityService, SystemParameter } from '../security.service';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { Subscription, timer, lastValueFrom } from 'rxjs';
import * as _ from 'lodash';
import { ActionEventArgs, FilterService, FreezeService, GridComponent, GridModule, PageService, ResizeService, SortService } from '@syncfusion/ej2-angular-grids';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons'
import { Utils } from 'src/app/Shared/Utilites/utils';
import { CommonModule } from '@angular/common';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
import { HeaderComponent } from 'src/app/Shared/Components/header/header.component';
import { FilterSelectComponent } from 'src/app/Shared/Components/filter-select/filter-select.component';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { SafeHtmlPipe } from 'src/app/Shared/Pipes/safe-html.pipe';
import { LowercaseFirstPipe } from 'src/app/Shared/Pipes/lowercase-first.pipe';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-system-configs',
  imports: [
    FormsModule,
    TranslateModule,
    RouterModule,
    GridModule,
    TranslateModule,
    ButtonModule,
    CommonModule,
    ShareDirectiveModule,
    HeaderComponent,
    FilterSelectComponent,
    SafeHtmlPipe,
    LowercaseFirstPipe,
    LocalizedDatePipe

  ],
  templateUrl: './system-configs.component.html',
  styleUrl: './system-configs.component.scss',
  providers: [SecurityService,
    PageService,
    SortService,
    FilterService, FreezeService,ResizeService]
})
export class SystemConfigsComponent implements OnInit, OnDestroy {

  loading: boolean = true;
  sysParamsList: SystemParameter[] = [];
  $sysParams: Subscription = null;
  loaderId = null;
  @ViewChild('grid') public grid!: GridComponent;
  dataSource: { result: any[]; count: number } = {result : [] , count:0};
  public selectionOptions?: any = GridSelect;
  public customAttributes: Object;
  public pageSettings = { pageSizes: ['10', '20'] };
  public resizeSettings = { mode: 'Normal' };
  payload : any = {
    pageNumber: 1,
    pageSize: 10,
    sortColumn: null,
    sortDirection: 'asc',
    filters: {}
  }

  permission = {
    "allowView": null,
    "allowNew": false,
    "allowEdit": false,
    "allowDelete": false
  };

  columns: any = [];
  criteria: any;
  selectFilter: any;
  selectFilterType: any;
  searchVal = '';
  langSub: Subscription = null;
  valueselectFilter: any = '';
  gridHeader: any = [];
  menuId = null;
  header = null;
  inputType: string = 'text';
  constructor(
    private loaderService: AppLoaderService,
    private translate: TranslateService,
    private securityService: SecurityService,
    private router: Router,
    private routed: ActivatedRoute,
    private utils: Utils,
  ) {
    this.langSub = this.translate.onLangChange.subscribe(item => {
      const tempSelectFilter = _.cloneDeep(this.selectFilter);
      this.selectFilter = null;
      timer(10).subscribe(() => {
        this.selectFilter = _.cloneDeep(tempSelectFilter);
      });
      if (this.grid != undefined) {
        this.utils.setDataGridFilter(this.grid)

      }
    })

    this.$sysParams = this.securityService.getSysParameterList().subscribe(async data => {
      this.sysParamsList = data;
      this.menuId = this.routed.snapshot.data?.menuId || '9202';
      this.permission = await this.utils.getPermission(this.menuId);
      this.header = await this.utils.getMenu(this.menuId)
      this.loaderService.hide(this.loaderId);
    });
  }
  async ngOnInit() {
    this.customAttributes = { class: 'customcss' };
    this.loaderId = this.loaderService.show();
    this.getSysParameters();
  }

  async getSysParameters() {
    delete this.payload.filters.selectFilter;
    this.securityService.getSysParameters(this.payload).then(res => {
      let result: any = res;
      const record = (res as any).data.datatables;
      const count = (res as any).data.totalRecords;
      if (record.length == 0) {
        this.payload.pageNumber = 1
      }
      this.criteria = result.data.displayColumns;
      if(this.gridHeader.length == 0){
        this.gridHeader = this.criteria;
      }
      this.dataSource = { result: record, count: count };
    })
  }

  getTextAlign(type: string) {
    return this.utils.getAlign(type);
  }

  actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.payload = this.utils.handleGridAction(event, this.payload);
      this.getSysParameters();
    }
  }

  edit(item: any) {
    this.router.navigate([item.id], { relativeTo: this.routed });
  }

  ngOnDestroy() {
    this.$sysParams?.unsubscribe();
    this.langSub.unsubscribe();
  }


  doubleClick(event) {
    this.edit(event.rowData);
  }

  filterCallback(value: any) {
    this.searchVal = value;
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
    await this.getSysParameters();
    this.grid.refresh();
  }

}
