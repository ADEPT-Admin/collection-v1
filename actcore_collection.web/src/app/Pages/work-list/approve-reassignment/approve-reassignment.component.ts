import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { timer, Subscription } from 'rxjs';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { RouterModule } from '@angular/router';
import * as _ from 'lodash';
import Swal from 'sweetalert2';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { FilterService, FreezeService, GridComponent, GridModule, PageService, ResizeService, SortService } from '@syncfusion/ej2-angular-grids';
import { WorkService } from '../work.service';
import { ButtonAllModule } from '@syncfusion/ej2-angular-buttons';
import { FilterSelectComponent } from 'src/app/Shared/Components/filter-select/filter-select.component';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { LowercaseFirstPipe } from 'src/app/Shared/Pipes/lowercase-first.pipe';
import { SafeHtmlPipe } from 'src/app/Shared/Pipes/safe-html.pipe';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-approve-reassignment',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    RouterModule,
    FilterSelectComponent,
    ShareDirectiveModule,
    GridModule,
    ButtonAllModule,
    SelectComponent,
    LowercaseFirstPipe,
    SafeHtmlPipe,
    LocalizedDatePipe
  ],
  providers: [Utils, PageService, SortService, FilterService, FreezeService, ResizeService],
  templateUrl: './approve-reassignment.component.html',
  styleUrl: './approve-reassignment.component.scss'
})
export class ApproveReassignmentComponent implements OnInit {
  loading: boolean = true;
  @ViewChild('grid') public grid!: GridComponent;
  criteria: any;
  isAccessdenied: boolean = true;
  permission = {
    "allowView": null,
    "allowNew": false,
    "allowEdit": false,
    "allowDelete": false
  };
  lang = null;
  gridHeader: any = [];
  dataSource: { result: any[]; count: number } = {result : [] , count:0};
  public selectionOptions?: any = GridSelect;
  public customAttributes: Object = { class: 'customcss'};
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  public resizeSettings = { mode: 'Normal' };
  lang$: Subscription = null
  payload: any = {
    pageNumber: 1,
    pageSize: 10,
    sortColumn: null,
    sortDirection: 'desc',
    filters: {}
  }

  selectAllChecked: boolean = false;
  isIndeterminate: boolean = false;
  selectedIds = new Set<string>();
  reassignType: string = null;
  collectorList: any = [];
  collectorId: string = null;
  valueselectFilter: any = '';

  constructor(
    private loaderService: AppLoaderService,
    private translate: TranslateService,
    private utils: Utils,
    private workService: WorkService
  ) {
    this.lang$ = this.translate.onLangChange.subscribe(item => {
      if (this.grid != undefined) {
        this.utils.setDataGridFilter(this.grid);

        this.collectorList.forEach(item => {
        const role = item.isSupervisor ? 'Supervisor' : 'Collector';
        item.label =  `(${this.translate.instant(role)} , ${item.colTeamName}) ${item.collectorEmpName[this.translate.currentLang]}`
      })

      }
    })
  }

  async actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.clearSelectionState();
      this.payload = this.utils.handleGridAction(event, this.payload);
      await this.getList();
    }
  }

  async ngOnInit() {
    this.lang = this.translate.currentLang;
    const loaderId = this.loaderService.show();
    await this.getList();
    this.permission = await this.utils.getPermission('0202');
    this.loaderService.hide(loaderId);
  }

  ngOnDestroy() {
    this.lang$.unsubscribe();
  }

  async getList() {
    const loadId = this.loaderService.show();
    delete this.payload.filters.selectFilter;
    if (this.payload.sortColumn == null) {
      this.payload.sortDirection = 'desc';
    }
    const res = await this.workService.getApprovereassignListData(this.payload);
    if (res?.status) {
      const record = (res as any).data.datatables;
      const count = (res as any).data.totalRecords;
      if (record.length == 0) {
        this.payload.pageNumber = 1;
      }
      this.dataSource = { result: record, count: count };
      this.criteria = res.data.displayColumns;
      if(this.gridHeader.length == 0){
        this.gridHeader = this.criteria;
      }

      this.dataSource.result.forEach(element => {
        element.checked = false;
      });
      this.isAccessdenied = false;
      this.getCollectorList();
      this.loaderService.hide(loadId);
    } else {
      this.isAccessdenied = true;
      const message = res.message[this.translate.currentLang];
      this.utils.dialogMessageAll('error', message);
      this.loaderService.hide(loadId);
    }

  }

  async approveandreject(data: any, type: string) {
    const result = await Swal.fire({
      title: this.translate.instant(type === 'r' ? 'RejectReassignment' : 'ApproveReassignment'),
      icon: 'warning',
      input: 'text',
      showCancelButton: true,
      confirmButtonText: this.translate.instant('Confirm'),
      cancelButtonText: this.translate.instant('Cancel'),
      inputValidator: (value) => (type === 'r' && !value) ? this.translate.instant('SpecifyReason') : null
    });
    if (result.isConfirmed) {
      const payload: any = {
        worklistId: data.worklistId,
        approveReason: result.value,
        type: type
      };
      const loaderId = this.loaderService.show();
      const res = await this.workService.approvereassignUrl([payload])
      if (res.status) {
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('success', message);
        this.selectedIds.clear();
        this.selectAllChecked = false;
        this.isIndeterminate = false;
        await this.getList();
        this.loaderService.hide(loaderId);
      } else {
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('error', message);
        this.loaderService.hide(loaderId);
      }
    }
  }

  async reassign() {
    const hasChecked = this.dataSource.result.some((item: any) => item.checked);
    if (!hasChecked) {
      Swal.fire({
        title: this.translate.instant('SelectTask'),
        text: this.translate.instant('PleaseSelectTask'),
        icon: 'warning'
      });
      return;
    }

    if (!this.collectorId) {
      Swal.fire({
        title: this.translate.instant('SelectCollector'),
        text: this.translate.instant('PleaseSelectCollector'),
        icon: 'warning'
      });
      return;
    }

    const result = await Swal.fire({
      title: this.translate.instant('ConfirmReassignment'),
      input: "text",
      html: `${this.translate.instant('Specifyreassign')} <br> ${this.translate.instant('Pleaseprovidereason')}`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: this.translate.instant('ConfirmReassign'),
      cancelButtonText: this.translate.instant('Cancel'),
      inputValidator: (value) => {
        if (!value) {
          return this.translate.instant('SpecifyReason');
        }
        return null;
      }
    });


    if (result.isConfirmed) {
      const workListIds = this.dataSource.result.filter(item => item.checked);
      let payload = [];
      workListIds.forEach(item => {
        payload.push({
          worklistId: item.worklistId,
          collectorId: this.collectorId,
          reassignReason: result.value
        })
      })
      const loaderId = this.loaderService.show();
      const res = await this.workService.manualRessign(payload)
      if (res.status) {
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('success', message);
        this.selectedIds.clear();
        this.selectAllChecked = false;
        this.isIndeterminate = false;
        await this.getList();
        this.loaderService.hide(loaderId);
      } else {
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('error', message);
        this.loaderService.hide(loaderId);
      }
    }
  }


  async getCollectorList() {
    this.collectorList = []
      const loaderId = this.loaderService.show();
      const res = await this.workService.getCollectorInTeamUrl(1);
      if (res.status == true) {
        this.collectorList = res.data.map(item => {
          item.value = item.collectorId
          const role = item.isSupervisor ? 'Supervisor' : 'Collector'
          item.label = `(${this.translate.instant(role)} , ${item.colTeamName}) ${item.collectorEmpName[this.translate.currentLang]}`
          return item
        });
        this.loaderService.hide(loaderId);
      } else {
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('error', message);
        this.loaderService.hide(loaderId);
      }
  }

  getTextAlign(type: string) {
    return this.utils.getAlign(type);
  }

  toggleSelectAll(event: any) {
    const checked = event.target.checked;
    const records: any[] = this.grid.getCurrentViewRecords();
    if (checked) {
      records.forEach(r => this.selectedIds.add(r.contractNo));
    } else {
      records.forEach(r => this.selectedIds.delete(r.contractNo));
    }

    this.dataSource.result.forEach(element => {
      if(element.isMyTeam == true){
      element.checked = this.selectedIds.has(element.contractNo);
      }
    });

    this.updateHeaderCheckboxState();
    this.grid.refresh();
    this.grid.hideSpinner();
  }

  onRowCheckboxChange(data: any, event: any) {
    if (event.target.checked) {
      this.selectedIds.add(data.contractNo);
    } else {
      this.selectedIds.delete(data.contractNo);
    }
    this.dataSource.result.forEach(element => {
      element.checked = this.selectedIds.has(element.contractNo);
    });

    this.updateHeaderCheckboxState();
  }


  updateHeaderCheckboxState() {
    const records: any[] = this.grid.getCurrentViewRecords();
    const total = records.length;
    const checkedCount = records.filter(r => this.selectedIds.has(r.contractNo)).length;
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
    this.clearSelectionState();
    await this.getList();
    this.grid.refresh();
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
