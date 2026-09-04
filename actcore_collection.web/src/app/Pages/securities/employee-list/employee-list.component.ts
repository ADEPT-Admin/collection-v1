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
import { Subscription, timer} from 'rxjs';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';

@Component({
  selector: 'app-employee-list',
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
    FilterSelectComponent
  ],
  templateUrl: './employee-list.component.html',
  styleUrl: './employee-list.component.scss',
  providers: [SecurityService, PageService, SortService, FilterService, FreezeService,ResizeService]
})

export class EmployeeListComponent implements OnInit, OnDestroy {
  collapsed = false;
  @ViewChild('grid') public grid!: GridComponent;
  columns: any = [];
  criteria: any;
  selectFilter: any;
  valueselectFilter: any = '';
  public customAttributes: Object = {};
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  public selectionOptions?: any = GridSelect;
  public resizeSettings = { mode: 'Normal' };
  actionListbtn: any = {
    new: { show: true, text: null, disabled: false },
    save: { show: false, text: null, disabled: true },
    delete: { show: true, text: null, disabled: false },
    cancel: { show: false, text: null, disabled: true },
    back: { show: false, text: 'null', disabled: true }
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
  langSub: Subscription = null;
  menuId = null;
  header = null;

  constructor(
    private securityService: SecurityService,
    private router: Router,
    private routed: ActivatedRoute,
    private translate: TranslateService,
    private utils: Utils,
    private share: ShareService
  ) {
    this.langSub = this.translate.onLangChange.subscribe(async item => {
      const tempSelectFilter = _.cloneDeep(this.selectFilter);
      this.selectFilter = null;
      timer(10).subscribe(() => {
        this.selectFilter = _.cloneDeep(tempSelectFilter);
      });
      if (this.grid != undefined) {
        this.utils.setDataGridFilter(this.grid);
      }
    });
  }

  async ngOnInit() {
    this.customAttributes = { class: 'customcss' };
    this.menuId = this.routed.snapshot.data?.menuId || '0701';
    this.permission = await this.utils.getPermission(this.menuId);
    this.header = await this.utils.getMenu(this.menuId)
    this.actionListbtn.new.disabled = !this.permission.allowNew;
    this.actionListbtn.delete.disabled = !this.permission.allowDelete;
    await this.getEmployees();
  }

  async getEmployees() {
    delete this.payload.filters.selectFilter;
    this.securityService.getEmployees(this.payload).then(res => {
      let result: any = res;
      const record = (res as any).data.datatables;
      const count = (res as any).data.totalRecords;
      if (record.length == 0) {
        this.payload.pageNumber = 1;
      }
      this.dataSource = { result: record, count: count };
      this.columns = result.data.displayColumns;
      this.criteria = this.columns;
    })
  }

  ngOnDestroy() {
    this.langSub.unsubscribe();
  }

  doubleClick(event) {
    this.edit(event.rowData);
  }

  edit(data) {
    this.router.navigate([data.employeeId], { relativeTo: this.routed });
  }

  async actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.payload = this.utils.handleGridAction(event, this.payload);
      await this.getEmployees();
    }
  }

  dataBound(event) {
    timer(30).subscribe(() => {
      this.utils.setDataGridFilter(this.grid);
    })
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
    await this.getEmployees();
    this.grid.refresh();
  }
}
