import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ButtonModule, CheckBoxModule } from '@syncfusion/ej2-angular-buttons';
import { FilterService, FreezeService, GridComponent, GridModule, PageService, ResizeService, SortService } from '@syncfusion/ej2-angular-grids';
import { ToolbarComponent } from 'src/app/Shared/Components/toolbar/toolbar.component';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { CollectorService } from '../collector.service';
import { Subscription, timer } from 'rxjs';
import { Utils } from 'src/app/Shared/Utilites/utils';
import * as _ from 'lodash';
import { HeaderComponent } from 'src/app/Shared/Components/header/header.component';
import { FilterSelectComponent } from 'src/app/Shared/Components/filter-select/filter-select.component';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { LowercaseFirstPipe } from 'src/app/Shared/Pipes/lowercase-first.pipe';
import { SafeHtmlPipe } from 'src/app/Shared/Pipes/safe-html.pipe';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-collector-profile',
  imports: [
    ToolbarComponent,
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
  templateUrl: './collector-profile.component.html',
  styleUrl: './collector-profile.component.scss',
  providers : [PageService,SortService,FilterService, FreezeService, ResizeService , CollectorService]
})
export class CollectorProfileComponent implements OnInit {
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
  selectAllChecked: boolean = false;
  isIndeterminate: boolean = false;
  selectedIds = new Set<string>();
  valueselectFilter : any = '';
  langSub : Subscription = null;
  menuId = null;
  header = null;
  inputType: string = 'text';
  gridHeader: any = [];
  constructor(
    private CollectorService: CollectorService,
    private router: Router,
    private routed: ActivatedRoute,
    private translate: TranslateService,
    private utils: Utils,
  ) {
    this.langSub = this.translate.onLangChange.subscribe(item => {
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
      this.menuId = this.routed.snapshot.data?.menuId || '0401';
      this.permission = await this.utils.getPermission(this.menuId);
      this.header = await this.utils.getMenu(this.menuId)
      this.actionListbtn.new.disabled = !this.permission.allowNew;
      this.actionListbtn.delete.disabled = !this.permission.allowDelete;
      await this.getCollectors();
  }

  async getCollectors() {
    delete this.payload.filters.selectFilter;
    this.CollectorService.getCollectors(this.payload).then(res => {
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
      this.dataSource.result.forEach(element => element.checked = false);
    })
  }

  getTextAlign(type: string) {
    return this.utils.getAlign(type);
  }

  ngOnDestroy() {
    this.langSub.unsubscribe();
  }


toggleSelectAll(event: any) {
  const checked = event.target.checked;
  const records: any[] = this.grid.getCurrentViewRecords();
  if (checked) {
    records.forEach(r => this.selectedIds.add(r.collectorId));
  } else {
    records.forEach(r => this.selectedIds.delete(r.collectorId));
  }

  this.dataSource.result.forEach(element => {
    element.checked = this.selectedIds.has(element.collectorId);
  });

  this.updateHeaderCheckboxState();
  this.grid.refresh();
  this.grid.hideSpinner();
}

onRowCheckboxChange(data: any, event: any) {
  if (event.target.checked) {
    this.selectedIds.add(data.collectorId);
  } else {
    this.selectedIds.delete(data.collectorId);
  }
  this.dataSource.result.forEach(element => {
    element.checked = this.selectedIds.has(element.collectorId);
  });

  this.updateHeaderCheckboxState();
}


updateHeaderCheckboxState() {
  const records: any[] = this.grid.getCurrentViewRecords();
  const total = records.length;
  const checkedCount = records.filter(r => this.selectedIds.has(r.collectorId)).length;
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

  doubleClick(event) {
    this.edit(event.rowData);
  }

  async delete(data: any = null) {
    let targets = [];
    data != null ? targets = [...targets , data] : targets = this.dataSource.result.filter(item => item.checked == true);
    if (!targets.length) return;
    const messagebody = `${this.translate.instant('DialogDelete')}`;
    const isConfirmed = await this.utils.dialogMessageAll('confirm', messagebody);
    if (isConfirmed) {
      try {
        const payload = targets.map(item => ({
          collectorId: item.collectorGuid
        }));
        const res: any = await this.CollectorService.deleteCollectors(payload);
        if (res.status) {
          const message = res.message[this.translate.currentLang]
          this.utils.dialogMessageAll('success', message);
          this.selectedIds.clear();
          this.selectAllChecked = false;
          this.isIndeterminate = false;
          this.dataSource.result.forEach(element => element.checked = false);
          await this.getCollectors();
          this.grid.refresh();
          this.grid.clearSelection();
        } else {
          // const msg = res.errorMessages.map((item: string) => `<p>${item}</p>`).join('');
          const message = res.message[this.translate.currentLang]
          this.utils.dialogMessageAll('error', message);
        }
      } catch (err) {
        this.utils.dialogMessageAll('error', this.translate.instant('Delete Failed'));
      }
    }
  }


  doNew() {
    this.router.navigate(['new'], { relativeTo: this.routed });
  }

  edit(data: any) {
    this.router.navigate([data.collectorId], { relativeTo: this.routed });
  }

  async actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.clearSelectionState();
      this.payload = this.utils.handleGridAction(event, this.payload);
      await this.getCollectors();
    }
  }

  filterCallback(value: any) {
    this.searchVal = value;
  }

  dataBound(event) {
    if (this.grid) {
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
    this.clearSelectionState();
    await this.getCollectors();
    this.grid.refresh();
  }

  getColWidth(col: any): number {
    if (col.width) return col.width;
    switch (col.type) {
        case 'DateTime': return 180;
        case 'Number': return 120;
        case 'Enum': return 60;
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
