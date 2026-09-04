import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule, LowerCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Subscription, timer } from 'rxjs';
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
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { SecurityService } from 'src/app/Pages/securities/security.service';
import { ModalChildBase } from '../base-modal/base-modal.component';
import { FilterSelectComponent } from 'src/app/Shared/Components/filter-select/filter-select.component';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { LowercaseFirstPipe } from 'src/app/Shared/Pipes/lowercase-first.pipe';
import { SafeHtmlPipe } from 'src/app/Shared/Pipes/safe-html.pipe';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-user-modal',
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
  templateUrl: './user-modal.component.html',
  styleUrl: './user-modal.component.scss'
})


export class UserModalComponent extends ModalChildBase implements OnInit {
  @ViewChild('grid') public grid!: GridComponent;
  columns: any = [];
  criteria: any;
  selectFilter: any;
  searchVal = '';
  filter: any[] = [];
  public selectionOptions?: any = GridSelect;
  public customAttributes: Object = {};
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  public resizeSettings = { mode: 'Normal' };
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
    filters: {},
    mode: "dialog"
  }
  dataSource: { result: any[]; count: number } = { result: [], count: 0 };
  rowIdClickData: any;
  valueselectFilter: any = '';
  loaded: boolean = false;
  $lang: Subscription = null;
  gridHeader: any = [];
  selectAllChecked: boolean = false;
  isIndeterminate: boolean = false;
  selectedIds = new Set<string>();
  constructor(
    private securityService: SecurityService,
    private translate: TranslateService,
    private utils: Utils,
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
    await this.getUsers();
  }


  async getUsers() {
    delete this.payload.filters.selectFilter;
    this.payload.mode = 'dialog';
    this.securityService.getUsers(this.payload, true).then(res => {
      let result: any = res;
      const record = (res as any).data.datatables;
      const count = (res as any).data.totalRecords;
      if (record.length == 0) {
        this.payload.pageNumber = 1;
      }
      this.dataSource = { result: record, count: count };
      this.columns = result.data.displayColumns;
      this.criteria = this.columns;

      if (this.gridHeader.length == 0) {
        this.gridHeader = this.criteria;
      }
      this.loaded = true;
    })
  }

  ngOnDestroy() {
    this.$lang.unsubscribe();
  }

  async actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.clearSelectionState();
      this.payload = this.utils.handleGridAction(event, this.payload);
      await this.getUsers();
    }
  }

  filterCallback(value: any) {
    this.searchVal = value;
  }

  rowSelectedclick(e) {
    this.rowIdClickData = e.data;
  }

  closeWithData() {
    let data = this.dataSource.result.filter(item => item.checked == true);
    if (data.length > 0) {
      this.close(data);
    } else {
      this.close();
    }
  }


  dataBound(event) {
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
    this.clearSelectionState();
    await this.getUsers();
    this.grid.refresh();
  }


  getTextAlign(type: string) {
    return this.utils.getAlign(type);
  }

  toggleSelectAll(event: any) {
    const checked = event.target.checked;
    const records: any[] = this.grid.getCurrentViewRecords();
    if (checked) {
      records.forEach(r => this.selectedIds.add(r.employeeId));
    } else {
      records.forEach(r => this.selectedIds.delete(r.employeeId));
    }

    this.dataSource.result.forEach(element => {
      element.checked = this.selectedIds.has(element.employeeId);
    });

    this.updateHeaderCheckboxState();
    this.grid.refresh();
    this.grid.hideSpinner();
  }

  onRowCheckboxChange(data: any, event: any) {
    if (event.target.checked) {
      this.selectedIds.add(data.employeeId);
    } else {
      this.selectedIds.delete(data.employeeId);
    }
    this.dataSource.result.forEach(element => {
      element.checked = this.selectedIds.has(element.employeeId);
    });

    this.updateHeaderCheckboxState();
  }


  updateHeaderCheckboxState() {
    const records: any[] = this.grid.getCurrentViewRecords();
    const total = records.length;
    const checkedCount = records.filter(r => this.selectedIds.has(r.employeeId)).length;
    if (checkedCount === 0) {
      this.selectAllChecked = false;
      this.isIndeterminate = false;
    } else if (checkedCount === total) {
      this.selectAllChecked = true;
      this.isIndeterminate = false;
    } else {
      this.selectAllChecked = false;
      this.isIndeterminate = true;
    }
  }

  clearSelectionState() {
    this.selectedIds.clear();
    this.selectAllChecked = false;
    this.isIndeterminate = false;
    if (this.dataSource?.result) {
      this.dataSource.result.forEach(x => x.checked = false);
    }
  }

}
