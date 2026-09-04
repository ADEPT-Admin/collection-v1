import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { timer, Subscription } from 'rxjs';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { SharedService } from 'src/app/Shared/Services/shared.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import * as _ from 'lodash';
import Swal from 'sweetalert2';

import { Utils } from 'src/app/Shared/Utilites/utils';
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { FilterService, FreezeService, GridComponent, GridModule, PageService, ResizeService, SortService } from '@syncfusion/ej2-angular-grids';
import { WorkService } from '../work.service';
import { ButtonAllModule } from '@syncfusion/ej2-angular-buttons';
import { HeaderComponent } from 'src/app/Shared/Components/header/header.component';
import { FilterSelectComponent } from 'src/app/Shared/Components/filter-select/filter-select.component';
import { LowercaseFirstPipe } from '../../../Shared/Pipes/lowercase-first.pipe';
import { SafeHtmlPipe } from 'src/app/Shared/Pipes/safe-html.pipe';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

interface ExportColumn {
    title: string;
    dataKey: string;
}


@Component({
  selector: 'app-task-lists',
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
    HeaderComponent,
    LowercaseFirstPipe,
    SelectComponent,
    SafeHtmlPipe,
    LocalizedDatePipe
  ],
  providers : [Utils , PageService, SortService, FilterService, FreezeService,ResizeService],
  templateUrl: './unassign-lists.component.html',
  styleUrl: './unassign-lists.component.scss'
})
export class UnassignListsComponent implements OnInit  {
  data : any = null;
  tasks!: any[]
  loading: boolean = true;
  activityValues: number[] = [0, 100];

  @ViewChild('grid') public grid!: GridComponent;


  searchValue: string | undefined;
  value = '';
  public resizeSettings = { mode: 'Normal' };
  filter : string[] = []
  filterItem = null;

  isSuperVisor = false

  criteria: any;
  gridHeader: any = [];
  selectFilter: any;
  selectFilterType: any;
  searchVal = '';
  isAccessdenied : boolean = true;
  permission = {
    "allowView": null,
    "allowNew": false,
    "allowEdit": false,
    "allowDelete": false
  };
  columns = [
    {
      key : 'contractNo',
      text : 'Contract No',
      field : 'Contract No',
      type : 'text',
      width : '150px',
    },
    {
      key : 'customerName',
      text : 'Customer Name',
      field : 'Customer Name',

      type : 'text',
      width : '200px',
    },
    {
      key : 'assetType',
      text : 'Asset Type',
      field : 'Asset Type',

      type : 'text',
      width : '200px',
    },
    {
      key : 'contractDueDate',
      text : 'Contract Due Date',
      field : 'Contract Due Date',

      type : 'text',
      width : '200px',
    },
    {
      key : 'dpd',
      text : 'DPD',
      field : 'DPD',

      type : 'text',
      width : '200px',
    },
    {
      key : 'overdueAmount',
      text : 'Overdue Amount',
      field : 'Overdue Amount',
      type : 'number',
      width : '180px',
    },
    {
      key : 'outstandingAmount',
      text : 'Outstanding Amount',
      field : 'Outstanding Amount',
      type : 'number',
      width : '180px',
    },
    {
      key : 'areaCode',
      text : 'Area Code',
      field : 'Area Code',
      type : 'text',
      width : '180px',
    },
    {
      key : 'followupStatus',
      text : 'Followup Status',
      field : 'Followup Status',
      type : 'text',
      width : '180px',
    },
    {
      key : 'reAssignFrom',
      text : 'Reassign From',
      field : 'Reassign From',
      type : 'text',
      width : '180px',
    },
    {
      key : 'contractStatus',
      text : 'Contract Status',
      field : 'Contract Status',
      type : 'text',
      width : '180px',
    },

    {
      key : 'collectorName',
      text : 'Collector Name',
      field : 'Collector Name',
      type : 'text',
      width : '180px',
    },

    {
      key : 'allocateDate',
      text : 'Allocate Date',
      field : 'Allocate Date',
      type : 'text',
      width : '180px',
    },

    {
      key : 'updatedDate',
      text : 'Updated Date',
      field : 'Updated Date',
      type : 'text',
      width : '180px',
    },


  ]

  collector = '';
  teams = []
  team: string = '';
  empId : string = '';
  empList = [];
  selectedTasks: any[] = []
  filterField = null;
  placeholderText = null;
  dropdownKey = 0;
  dropdownVisible = true;
  totalTask = null;
  totalNew = null;
  totalinProgress = null;
  totalSuccess = null;
  newStatus = [6];
  successStatus = [9 , 10];
  notInProgressStatus = [...this.newStatus , ...this.successStatus]
  exportColumns!: ExportColumn[];
  contractStatusList = [];
  followupStatusList = [];
  lang = null;
  dataSource: { result: any[]; count: number } = {result : [] , count:0};

  public customAttributes: Object = { class: 'customcss'};
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };

  lang$ : Subscription = null

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

  reassignType : string = null;
  collectorId : string = null;
  taemSelect : any = [{label: "IsMyTeam",value: "1"},{label: "OtherTeam",value: "0"}];

  collectorList = [];

  menuId = null;
  header = null;
  inputType: string = 'text';
  valueselectFilter: any = '';

  isSupervisor = null;

  constructor(
    private share : SharedService,
    private loaderService : AppLoaderService,
    private router: Router,
    private routed : ActivatedRoute,
    private translate : TranslateService,
    private utils : Utils,
    private workService : WorkService
  ){

    const employee = JSON.parse(sessionStorage.getItem('employee') || '{}');
    this.empId = employee?.employeeId


    this.lang$ = this.translate.onLangChange.subscribe(item => {
      if(this.grid != undefined){
        this.utils.setDataGridFilter(this.grid);
      }


      this.collectorList.forEach(item => {
        const role = item.isSupervisor ? 'Supervisor' : 'Collector';
        item.label =  `(${this.translate.instant(role)} , ${item.colTeamName}) ${item.collectorEmpName[this.translate.currentLang]}`
      })
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

    this.menuId = this.routed.snapshot.data?.menuId || '0201';
    this.permission = await this.utils.getPermission(this.menuId);
    this.header = await this.utils.getMenu(this.menuId)


    await this.getList();

    await this.getCollectorList();
    this.isSupervisor = await sessionStorage.getItem('supervisor') === 'true';


    if(this.isSupervisor === false){
      this.reassignType = '1';
      this.getCollectorList();
    }


    this.loaderService.hide(loaderId);
  }

  ngOnDestroy(){
    this.lang$.unsubscribe();
  }

  async getList() {
    const loadId = this.loaderService.show();
    delete this.payload.filters.selectFilter;
    if(this.payload.sortColumn == null){
      this.payload.sortColumn = null;
      this.payload.sortDirection = 'desc';
    }
    const res = await this.workService.getUnAssignWorkListData(this.payload);

    if (res?.status) {
      const record = (res as any).data.datatables;
      const count = (res as any).data.totalRecords;
      if (record.length == 0) {
        this.payload.pageNumber = 1;
      }
      this.dataSource = { result: record, count: count };
      this.criteria = res.data.displayColumns;

      this.dataSource.result.forEach(element => {
        element.checked = false;
      });

      if(this.gridHeader.length == 0){
        this.gridHeader = this.criteria;
      }
      this.isAccessdenied = false;
      this.loaderService.hide(loadId);
    } else {
      this.isAccessdenied = true;
      const message = res.message[this.translate.currentLang];

      this.utils.dialogMessageAll('error', message);
      this.loaderService.hide(loadId);
    }

  }

  getTextAlign(type: string) {
    return this.utils.getAlign(type);
  }

  doubleClick(event) {
    this.view(event.rowData);
  }

  view(data: any) {
    this.router.navigate([data.worklistId], { relativeTo: this.routed });
  }

  clear(table) {
    table.clear();
    this.searchValue = ''
  }

  toggleSelectAll(event: any) {
    const checked = event.target.checked;
    const records: any[] = this.grid.getCurrentViewRecords();
    if (checked) {
      records.forEach(r => this.selectedIds.add(r.worklistId));
    } else {
      records.forEach(r => this.selectedIds.delete(r.worklistId));
    }

    this.dataSource.result.forEach(element => {
      element.checked = this.selectedIds.has(element.worklistId);
    });

    this.updateHeaderCheckboxState();
    this.grid.refresh();
    this.grid.hideSpinner();
  }

  onRowCheckboxChange(data: any, event: any) {
    if (event.target.checked) {
      this.selectedIds.add(data.worklistId);
    } else {
      this.selectedIds.delete(data.worklistId);
    }
    this.dataSource.result.forEach(element => {
      element.checked = this.selectedIds.has(element.worklistId);
    });

    this.updateHeaderCheckboxState();
  }


  updateHeaderCheckboxState() {
    const records: any[] = this.grid.getCurrentViewRecords();
    const total = records.length;
    const checkedCount = records.filter(r => this.selectedIds.has(r.worklistId)).length;
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

  getSeverity(status: string) {
      switch (status) {
          case 'Paid':
              return 'success';
          case  'Promise to Pay':
              return 'success';
          case 'Followed Success':
              return 'warn';
          case 'Followed Not Success':
              return 'warn';
          case 'WaitForApprove':
              return 'warn';
          case 'Reassign_New':
              return 'warn';
          case 'Reassign_New(Reject)':
              return 'warn';
          case 'New':

              return null;
      }
  }

  detail(item){
    this.router.navigate(['detail' , item.id] , {relativeTo : this.routed })
  }


  async reassign() {


    const hasChecked = this.dataSource.result.some((item: any) => item.checked);
    if (!hasChecked) {
      Swal.fire({
        title: this.translate.instant('SelectTask'),
        text : this.translate.instant('PleaseSelectTask'),
        icon: 'warning'
      });
      return;
    }

    if(!this.collectorId){
      Swal.fire({
        title: this.translate.instant('SelectCollector'),
        text : this.translate.instant('PleaseSelectCollector'),
        icon: 'warning'
      });
      return;
    }

    const result = await Swal.fire({
      title: this.translate.instant('ConfirmReassignment'),
      input: "text",
      html:  `${this.translate.instant('Specifyreassign')} <br> ${this.translate.instant('Pleaseprovidereason')}`,
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
            payload.push({worklistId : item.worklistId ,collectorId : this.collectorId , reassignReason : result.value})
        })

        const loaderId = this.loaderService.show();


        const res = await this.workService.manualRessign(payload , true)

        if (res.status) {
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('success', message);
          await this.getList();
          this.clearSelection();
          this.loaderService.hide(loaderId);

        }else{
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('error', message);
          this.loaderService.hide(loaderId);

        }





    }
  }

clearSelection(){


  this.selectedIds.clear();
  this.updateHeaderCheckboxState();
  this.grid.refresh();
}




 conCatText(text1 , text2){
  return `${text1}.${text2}`
 }

  dataBound(event) {
    if(this.grid){
    timer(100).subscribe(() => {
      this.utils.setDataGridFilter(this.grid);
    })
    }
  }

  async getCollectorList(){
    this.collectorList = []
    this.reassignType = '1';
    if(this.reassignType != null){
      const loaderId = this.loaderService.show();
      const res = await this.workService.getCollectorInTeamUrl(this.reassignType);
      if(res.status == true){
        this.collectorList = res.data.map(item => {
          item.value = item.collectorId
          const role = item.isSupervisor ? 'Supervisor' : 'Collector'

          item.label =  `(${this.translate.instant(role)} , ${item.colTeamName}) ${item.collectorEmpName[this.translate.currentLang]}`
          return item
        });
        this.loaderService.hide(loaderId);
      }else{
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('error', message);
        this.loaderService.hide(loaderId);
      }
    }
  }

  selectReAssignType(event){

      this.getCollectorList();


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
