import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ToolbarComponent } from 'src/app/Shared/Components/toolbar/toolbar.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SecurityService, User } from '../security.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Subscription, timer, filter, lastValueFrom, firstValueFrom } from 'rxjs';
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
import { ButtonModule, CheckBoxModule } from '@syncfusion/ej2-angular-buttons';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { HeaderComponent } from 'src/app/Shared/Components/header/header.component';
import { FilterSelectComponent } from 'src/app/Shared/Components/filter-select/filter-select.component';
import { ShareService } from 'src/app/Shared/Services/share.service';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { SafeHtmlPipe } from 'src/app/Shared/Pipes/safe-html.pipe';
import { LowercaseFirstPipe } from 'src/app/Shared/Pipes/lowercase-first.pipe';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-users',
  standalone: true,
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
    SafeHtmlPipe,
    LowercaseFirstPipe,
    LocalizedDatePipe

  ],
    templateUrl: './users.component.html',
    styleUrl: './users.component.scss',
  providers: [SecurityService,PageService,SortService,FilterService, FreezeService,ResizeService]
})



export class UsersComponent  implements OnInit, OnDestroy {
  @ViewChild('grid') public grid!: GridComponent;
  criteria: any;
  selectFilter: any = null;
  userGroups: any = [];
  valueselectFilter : any = '';
  filter: any[] = [];
  public customAttributes: Object = {};
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  public selectionOptions?: any = GridSelect;
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




  userListSub : Subscription = null;
  users : User[] = [];

  langSub : Subscription = null;
  gridHeader: any = [];
  menuId = null;
  header = null;
  inputType: string = 'text';
  public resizeSettings = { mode: 'Normal' };
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
    this.menuId = this.routed.snapshot.data?.menuId || '9104';
    this.permission = await this.utils.getPermission(this.menuId);
    this.header = await this.utils.getMenu(this.menuId)
    this.actionListbtn.new.disabled = !this.permission.allowNew;
    this.actionListbtn.delete.disabled = !this.permission.allowDelete;
    await this.getUsers();


  }

  async getUsers() {
    delete this.payload.filters.selectFilter;
    this.securityService.getUsers(this.payload).then(async res => {
      let result : any = res;
      const record = await (res as any).data.datatables;
      const count = await (res as any).data.totalRecords;

      if (record.length == 0) {
        this.payload.pageNumber = 1
      }
      this.criteria = result.data.displayColumns;


      if(this.gridHeader.length == 0){
        this.gridHeader = this.criteria;
      }

      this.dataSource = { result: record, count: count };
      this.dataSource.result.forEach(element => {
        element.checked = false;
      });

    })
  }


  ngOnDestroy() {
    this.userListSub?.unsubscribe();
    this.langSub.unsubscribe();
  }


  getTextAlign(type: string) {
    return this.utils.getAlign(type);
  }


toggleSelectAll(event: any) {
  const checked = event.target.checked;
  const records: any[] = this.grid.getCurrentViewRecords();
  if (checked) {
    records.forEach(r => this.selectedIds.add(r.userId));
  } else {
    records.forEach(r => this.selectedIds.delete(r.userId));
  }

  this.dataSource.result.forEach(element => {
    element.checked = this.selectedIds.has(element.userId);
  });

  this.updateHeaderCheckboxState();
  this.grid.refresh();
  this.grid.hideSpinner();
}

onRowCheckboxChange(data: any, event: any) {
  if (event.target.checked) {
    this.selectedIds.add(data.userId);
  } else {
    this.selectedIds.delete(data.userId);
  }
  this.dataSource.result.forEach(element => {
    element.checked = this.selectedIds.has(element.userId);
  });

  this.updateHeaderCheckboxState();
}


updateHeaderCheckboxState() {
  const records: any[] = this.grid.getCurrentViewRecords();
  const total = records.length;
  const checkedCount = records.filter(r => this.selectedIds.has(r.userId)).length;
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
        const res: any = await this.securityService.deleteUser(targets);
        if (res.status) {
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('success', message);
          this.selectedIds.clear();
          this.selectAllChecked = false;
          this.isIndeterminate = false;
          this.dataSource.result.forEach(element => element.checked = false);
          await this.getUsers();
          this.grid.refresh();
          this.grid.clearSelection();
        } else {
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('error', message);
        }
      } catch (err) {
        this.utils.dialogMessageAll('error', this.translate.instant('Delete Failed'));
      }
    }
  }

  detail(item: any) {
    this.router.navigate([item.userId], { relativeTo: this.routed });
  }

  doNew() {
    this.router.navigate(['new'], { relativeTo: this.routed });
  }

  edit(data: any) {
    this.router.navigate([data.userId], { relativeTo: this.routed });
  }

  async actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.clearSelectionState();
      this.payload = this.utils.handleGridAction(event, this.payload);
      await this.getUsers();
    }
  }

  dataBound(event){
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
    await this.getUsers();
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
