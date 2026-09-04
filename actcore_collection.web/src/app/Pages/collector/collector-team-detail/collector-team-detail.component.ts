import { CommonModule } from '@angular/common';
import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { ToolbarComponent } from 'src/app/Shared/Components/toolbar/toolbar.component';
import { Subscription, timer } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CanComponentDeactivate } from 'src/app/Shared/Guards/confirm-exit.guard';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { ModalService } from 'src/app/Shared/Services/modal.service';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { SecurityService } from '../../securities/security.service';
import { CollectorService, CollectorTeam } from '../collector.service';
import { InputNumberComponent } from 'src/app/Shared/Components/input-number/input-number.component';
import { HeaderComponent } from "src/app/Shared/Components/header/header.component";
import { FilterService, FreezeService, GridComponent, GridModule, PageService, ResizeService, SortService } from '@syncfusion/ej2-angular-grids';
import { LowercaseFirstPipe } from 'src/app/Shared/Pipes/lowercase-first.pipe';
import { SafeHtmlPipe } from 'src/app/Shared/Pipes/safe-html.pipe';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { FilterSelectComponent } from 'src/app/Shared/Components/filter-select/filter-select.component';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { TeamAssignmentModalComponent } from 'src/app/Modals/team-assignment-modal/team-assignment-modal.component';
import { TeamAssignmentDetailModalComponent } from 'src/app/Modals/team-assignment-detail-modal/team-assignment-detail-modal.component';

@Component({
  selector: 'app-collector-team-detail',
  imports: [
    CommonModule,
    FormsModule,
    ToolbarComponent,
    ReactiveFormsModule,
    TranslateModule,
    InputNumberComponent,
    HeaderComponent,
    GridModule,
    LowercaseFirstPipe,
    SafeHtmlPipe,
    FilterSelectComponent,
    ButtonModule
  ],
  templateUrl: './collector-team-detail.component.html',
  styleUrl: './collector-team-detail.component.scss',
  providers: [PageService, SortService, FilterService, FreezeService, ResizeService, CollectorService]
})
export class CollectorTeamDetailComponent implements OnInit, AfterViewInit, CanComponentDeactivate, OnDestroy {
  async canDeactivate(): Promise<boolean> {
    if (!this.form.dirty) return true;
    const result = await this.utils.dialogConfirmSaveChange()
    return result;
  }
  @ViewChild('myForm') form: NgForm;


  @ViewChild('grid') public grid!: GridComponent;
  columns: any = [];
  criteria: any;
  selectFilter: any;
  selectFilterType: any;
  searchVal = '';
  userGroups: any = [];
  userGroupslist: any = [];
  filter: any[] = [];
  inputType: string = 'text';
  valueselectFilter: any = '';
  actionListbtn: any = {
    new: { show: true, text: null, disabled: false },
    save: { show: true, text: null, disabled: true },
    delete: { show: true, text: null, disabled: false },
    cancel: { show: true, text: null, disabled: true },
    back: { show: true, text: null, disabled: false }
  }

  detail: CollectorTeam = { supervisorNameLang: { 'en': '', 'th': '' }, capacity: 0 };
  public customAttributes: Object = {};
  userSub: Subscription = null;
  id: any = '';
  previousId: string = null;
  original: any = {};
  employees: any = [];
  permission = {
    "allowView": null,
    "allowNew": false,
    "allowEdit": false,
    "allowDelete": false
  };
  menuId = null;
  header = null;
  gridHeader: any = [];
  lang = 'en';
  payload: any = {
    pageNumber: 1,
    pageSize: 10,
    sortColumn: null,
    sortDirection: 'asc',
    filters: {}
  }
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  public selectionOptions?: any = GridSelect;
  dataSource: { result: any[]; count: number } = { result: [], count: 0 };
  selectAllChecked: boolean = false;
  isIndeterminate: boolean = false;
  selectedIds = new Set<string>();
  public resizeSettings = { mode: 'Normal' };
  langSub: Subscription = null;
  constructor(
    private securityService: SecurityService,
    private collotorService: CollectorService,
    private routed: ActivatedRoute,
    private router: Router,
    private utils: Utils,
    private cdRef: ChangeDetectorRef,
    private translate: TranslateService,
    private modal: ModalService,
    private loader: AppLoaderService,
  ) {


    this.lang = this.translate.currentLang;


    this.langSub = this.translate.onLangChange.subscribe(data => {
      this.lang = this.translate.currentLang;
    })


    this.routed.paramMap.subscribe(async params => {
      this.id = params.get('id');
      const loaderId = this.loader.show();
      this.menuId = this.routed.snapshot.data?.menuId || '0402';
      this.permission = await this.utils.getPermission(this.menuId);
      if (this.permission.allowView) {
        this.cdRef.detectChanges();
      }
      this.header = await this.utils.getMenu(this.menuId)
      this.header.path[this.header.path.length - 1].canLink = true;
      if (this.id != 'new') {
        await this.getData();
        await this.getList();
        this.header.path = [...this.header.path, ...[{ menuId: 'detail', itemName: { th: `รายละเอียด (${this.detail.colTeamCode})`, en: `Detail (${this.detail.colTeamCode})` } }]]
        this.loader.hide(loaderId);
      } else {

        this.id = null;
        this.original = _.cloneDeep(this.detail);
        this.actionListbtn.delete.disabled = true;
        this.actionListbtn.new.disabled = true;
        this.header.path = [...this.header.path, ...[{ itemName: { th: 'สร้าง', en: 'New' } }]]
        this.loader.hide(loaderId);
      }
      this.previousId = this.id;
      if (!this.permission?.allowView || (!this.permission?.allowNew || !this.permission?.allowEdit)) {

        setTimeout(() => {
          this.form?.control.disable();
        }, 200);
      }
      if (this.permission.allowNew && this.id == null) {
        setTimeout(() => {
          this.form?.control.enable();
        }, 200);
      }


      if (this.permission.allowEdit && this.id != null) {
        setTimeout(() => {
          this.form?.control.enable();
          this.form?.controls['colTeamCode']?.disable();
        }, 200);
      }



    })


  }
  ngOnInit(): void {
    //await this.getList();
    this.customAttributes = { class: 'customcss' };
  }

  getTextAlign(type: string) {
    switch (type) {
      case 'Number': return 'Right';
      case 'DateTime': return 'Center';
      default: return 'Left';
    }
  }

  ngOnDestroy() {
    this.langSub.unsubscribe();
  }

  async onSave() {
    this.form.onSubmit(null);
    this.form.control.markAllAsTouched();
    if (this.form.valid) {
      let res = null;
      if (this.id != null) {
        const requestModel: any = {
          colTeamCode: this.detail.colTeamCode,
          colTeamName: this.detail.colTeamName,
          capacity: this.detail.capacity,
          description: this.detail.description,
          isActive: this.detail.isActive,
          colTeamId: this.detail.colTeamId,
        }
        res = await this.collotorService.updateTeam(requestModel);
      } else {
        const requestModel: any = {
          colTeamCode: this.detail.colTeamCode,
          colTeamName: this.detail.colTeamName,
          capacity: this.detail.capacity,
          description: this.detail.description,
          isActive: this.detail.isActive,
        };
        res = await this.collotorService.createTeam(requestModel);
      }
      if (res?.status) {
        const message = res.message[this.translate.currentLang]
        this.utils.dialogMessageAll('success', message);
        this.markFormPristine();
        if (this.id == null) {
          this.router.navigate(['../', res.data.colTeamId], { relativeTo: this.routed })
          this.id = res.data.userId;
        } else {
          this.getData();
        }
      } else {
        const message = res.message[this.translate.currentLang];

        this.utils.dialogMessageAll('error', message);
      }
    }
  }

  setNew() {
    this.id = 'new';
    this.detail = { supervisorNameLang: { 'en': '', 'th': '' }, capacity: 0 };
    this.original = _.cloneDeep(this.detail);
    this.dataSource = { result: [], count: 0 };
    this.gridHeader = [];
  }

  async doNew() {
    let res = false
    if (this.form.dirty) {
      const result = await this.utils.dialogConfirmSaveChange()
      res = result;
      this.setNew();
    } else {
      res = true;
      this.setNew();
    }
    if (res == true) this.router.navigate(['../new'], { relativeTo: this.routed });
  }

  doBack() {
    this.router.navigate(['..'], { relativeTo: this.routed })
  }

  async getData() {
    this.actionListbtn.new.disabled = !this.permission.allowNew;
    this.actionListbtn.delete.disabled = !this.permission.allowDelete;
    const res = await this.collotorService.getTeamById(this.id);
    if (res.status == true) {
      this.detail = (res as any).data || {};
      this.original = _.cloneDeep(this.detail);
    } else {
      const message = res.message[this.translate.currentLang];
      this.loader.hideAll();
      await this.utils.dialogMessageAll('error', message);
      this.doBack();
    }
  }

  markFormPristine() {
    Object.values(this.form.controls).forEach(control => {
      control.markAsPristine();
      control.markAsUntouched(); // Optional: also reset "touched"
      control.updateValueAndValidity();
    });
  }

  async doDel() {
    let messagebodyDialogDelete = this.translate.instant('DialogDelete');
    let messagebody = `${messagebodyDialogDelete}`

    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then(async (confirm) => {
      if (confirm) {
        const loaderId = this.loader.show();
        const payload = [{ colTeamId: this.id }];
        const res: any = await this.collotorService.deleteTeam(payload);
        this.loader.hide(loaderId)

        if (res.status == true) {
          const message = res.message[this.translate.currentLang]
          const res_msg = await this.utils.dialogMessageAll('success', message);
          if (res_msg) {
            this.markFormPristine();
            this.router.navigate(['..'], { relativeTo: this.routed });
          }
        } else {
          const message = res.message[this.translate.currentLang];
          this.loader.hideAll();
          await this.utils.dialogMessageAll('error', message);
        }
      }
    })
  }

  async doCancel() {
    const messagebody = this.translate.instant('DialogCancel');
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then(async (res) => {
      if (res) {
        if (this.id == 'new' || this.id == null) {
          this.markFormPristine();
          if (this.previousId != null) {
            this.setNew();
            this.router.navigate(['..', this.previousId], { relativeTo: this.routed });
          } else {
            this.doBack();
          }
        } else {
          await this.getData();
          await this.getList();
          this.markFormPristine();
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
            this.router.navigate(['..', this.previousId], { relativeTo: this.routed });
          });
        }

      }
    });
  }


  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
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


  async getList() {
    delete this.payload.filters.selectFilter;
    const result = await this.collotorService.getTeamAssignmentlistpagedbyteamUrlPaged(this.payload, this.id);
    const record = (result as any).datatables;
    const count = (result as any).totalRecords;
    if (record.length == 0) {
      this.payload.pageNumber = 1
    }
    this.dataSource = { result: record, count: count };
    this.criteria = result.displayColumns;
    if (this.gridHeader.length == 0) {
      this.gridHeader = this.criteria;
    }
    this.dataSource.result.forEach(element => {
      element.checked = false;
    });
  }

  async edit(data: any) {
    const res: any = await this.modal.open(TeamAssignmentDetailModalComponent, { header: this.translate.instant('CollectorTeamAssignment'), width: '75%', data: { id: data.assignmentId } });
    if (res) {
      this.selectedIds.clear();
      this.selectAllChecked = false;
      this.isIndeterminate = false;
      this.updateHeaderCheckboxState();
      await this.getList();
      this.grid.refresh();
      this.markFormPristine();
    }

  }

  doubleClick(event) {
    this.edit(event.rowData);
  }

  async Add() {
    const res: any = await this.modal.open(TeamAssignmentModalComponent, {
      header: this.translate.instant('CollectorTeamAssignment'),
      width: '75%',
      data: { id: this.id }
    });
    if (res) {
      this.selectedIds.clear();
      this.selectAllChecked = false;
      this.isIndeterminate = false;
      this.updateHeaderCheckboxState();
      await this.getList();
      this.grid.refresh();
      this.markFormPristine();
    }
  }

  async delete(data: any = null) {
    let targets = [];
    data != null ? targets = [...targets, data] : targets = this.dataSource.result.filter(item => item.checked == true);
    if (!targets.length) return;
    const messagebody = `${this.translate.instant('DialogDelete')}`;
    const isConfirmed = await this.utils.dialogMessageAll('confirm', messagebody);
    if (isConfirmed) {
      try {
        const payload = targets.map(item => ({
          assignmentId: item.assignmentId
        }));
        const res: any = await this.collotorService.deleteTeamAssignment(payload);
        if (res.status) {
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('success', message);
          this.selectedIds.clear();
          this.selectAllChecked = false;
          this.isIndeterminate = false;
          this.dataSource.result.forEach(element => element.checked = false);
          await this.getList();
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


  toggleSelectAll(event: any) {
    const checked = event.target.checked;
    const records: any[] = this.grid.getCurrentViewRecords();
    if (checked) {
      records.forEach(r => this.selectedIds.add(r.assignmentId));
    } else {
      records.forEach(r => this.selectedIds.delete(r.assignmentId));
    }

    this.dataSource.result.forEach(element => {
      element.checked = this.selectedIds.has(element.assignmentId);
    });

    this.updateHeaderCheckboxState();
    this.grid.refresh();
    this.grid.hideSpinner();
  }

  onRowCheckboxChange(data: any, event: any) {
    if (event.target.checked) {
      this.selectedIds.add(data.assignmentId);
    } else {
      this.selectedIds.delete(data.assignmentId);
    }
    this.dataSource.result.forEach(element => {
      element.checked = this.selectedIds.has(element.assignmentId);
    });

    this.updateHeaderCheckboxState();
  }


  updateHeaderCheckboxState() {
    const records: any[] = this.grid.getCurrentViewRecords();
    const total = records.length;
    const checkedCount = records.filter(r => this.selectedIds.has(r.assignmentId)).length;
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

    if (this.grid) {
      timer(30).subscribe(() => {
        this.utils.setDataGridFilter(this.grid);
      })
    }
  }

  async actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.clearSelectionState();
      this.payload = this.utils.handleGridAction(event, this.payload);
      await this.getList();
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
