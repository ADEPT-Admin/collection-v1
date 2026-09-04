import { CommonModule } from '@angular/common';
import { AfterViewInit, ChangeDetectorRef, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { Subscription, timer } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CanComponentDeactivate } from 'src/app/Shared/Guards/confirm-exit.guard';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { ModalService } from 'src/app/Shared/Services/modal.service';
import { DatePickerAllModule } from '@syncfusion/ej2-angular-calendars';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { InputNumberComponent } from 'src/app/Shared/Components/input-number/input-number.component';
import { CalendarComponent } from 'src/app/Shared/Components/calendar/calendar.component';
import { CollectorService, CollectorTeamAssignment } from 'src/app/Pages/collector/collector.service';
import { ButtonModule, CheckBoxModule } from '@syncfusion/ej2-angular-buttons';
import { ModalChildBase } from '../base-modal/base-modal.component';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { FilterSelectComponent } from 'src/app/Shared/Components/filter-select/filter-select.component';
import { LowercaseFirstPipe } from 'src/app/Shared/Pipes/lowercase-first.pipe';
import { SafeHtmlPipe } from 'src/app/Shared/Pipes/safe-html.pipe';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';
import { FilterService, FreezeService, GridComponent, GridModule, PageService, ResizeService, SortService } from '@syncfusion/ej2-angular-grids';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';

@Component({
  selector: 'app-team-assignment-modal',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule,
    InputNumberComponent,
    DatePickerAllModule,
    CalendarComponent,
    ButtonModule,
    ShareDirectiveModule,
    FilterSelectComponent,
    CheckBoxModule,
    LowercaseFirstPipe,
    SafeHtmlPipe,
    LocalizedDatePipe,
    GridModule

  ],
  templateUrl: './team-assignment-modal.component.html',
  styleUrl: './team-assignment-modal.component.scss',
  providers: [CollectorService, PageService, SortService, FilterService, FreezeService, ResizeService]
})
export class TeamAssignmentModalComponent extends ModalChildBase implements OnInit, AfterViewInit, CanComponentDeactivate, OnDestroy {
  async canDeactivate(): Promise<boolean> {
    if (!this.form.dirty) return true;
    const result = await this.utils.dialogConfirmSaveChange()
    return result;
  }
  @ViewChild('myForm') form: NgForm;
  @ViewChild('grid') public grid!: GridComponent;
  detail: CollectorTeamAssignment = { collectorEmpNameLang: { 'en': '', 'th': '' }, capacity: 0 };
  original: any = {};
  payload: any = {
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
  menuId = null;
  lang = 'en';
  public customAttributes: Object = {};
  langSub: Subscription = null;
  id: any = null;
  valueselectFilter : any = '';
  dataSource: { result: any[]; count: number } = {result : [] , count:0};
  selectAllChecked: boolean = false;
  isIndeterminate: boolean = false;
  selectedIds = new Set<string>();
  gridHeader: any = [];
  public resizeSettings = { mode: 'Normal' };
  public selectionOptions?: any = GridSelect;
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  columns: any = [];
  criteria: any;
  capacity: number = 0;
  public maxStartDate: Date | null = null;
  public minEndDate: Date | null = null;
  constructor(
    private collotorService: CollectorService,
    private routed: ActivatedRoute,
    private router: Router,
    private utils: Utils,
    private cdRef: ChangeDetectorRef,
    private translate: TranslateService,
    private modal: ModalService,
    private loader: AppLoaderService,
  ) {
    super();
    this.lang = this.translate.currentLang;
    this.langSub = this.translate.onLangChange.subscribe(data => {
      this.lang = this.translate.currentLang;
    })

    this.routed.paramMap.subscribe(async params => {
      const loaderId = this.loader.show();
      timer(100).subscribe(async () => {
      const modalData = (this as any).childComponentData;
      if (!modalData) {
        const stack = (this.modal as any).stack;
        if (stack && stack.length > 0) {
          const lastModal = stack[stack.length - 1];
          this.id = lastModal.instance.childComponentData?.id;
        }
      } else {
        this.id = modalData.id;
      }
      if (this.id) {
        await this.getDatabyid();
        await this.getData();
        this.loader.hide(loaderId);
      }
      this.cdRef.detectChanges();
    });
      this.menuId = this.routed.snapshot.data?.menuId || '0403';
      this.permission = await this.utils.getPermission(this.menuId);
      if (this.permission.allowView) {
        this.cdRef.detectChanges();
      }
      if (!this.permission?.allowView || (!this.permission?.allowNew || !this.permission?.allowEdit)) {
        setTimeout(() => {
          this.form?.control.disable();
        }, 200);
      }
      if (this.permission.allowEdit && this.id != null) {
        setTimeout(() => {
          this.form?.control.enable();
          this.form.controls['colTeamCode'].disable()
          this.form.controls['colTeamName'].disable()
        }, 200);
      }
    })
  }

  async ngOnInit(): Promise<void> {
    this.customAttributes = { class: 'customcss' };
  }

  ngOnDestroy() {
    this.langSub.unsubscribe();
  }

  async onSave() {
    this.form.onSubmit(null);
    this.form.control.markAllAsTouched();
    if (this.form.valid) {
      const loaderId = this.loader.show();
      let res = null
      let requestModel = null;
      const selectedCollectorIds = this.dataSource.result.filter(item => item.checked === true).map(item => item.collectorGuid);
      requestModel = {
        colTeamId: this.id,
        collectorIds: selectedCollectorIds,
        capacity: this.capacity,
        effectiveDate: this.utils.formatDateToUTCString(this.detail.effectiveDate),
        expireDate: this.utils.formatDateToUTCString(this.detail.expireDate),
        isActive: this.detail.isActive,
      };
      res = await this.collotorService.bulkassignTeamAssignmentUrl(requestModel);
      if (res?.status) {
        this.loader.hide(loaderId);
        const message = res.message[this.translate.currentLang];
        const isConfirmed = this.utils.dialogMessageAll('success', message);
        isConfirmed.then((res) => {
          if (res) {
            this.close(true);
          }
        });
      } else {
        this.loader.hideAll();
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('error', message);
      }
    }
  }

  doBack() {
    this.router.navigate(['..'], { relativeTo: this.routed })
  }

  onStartDateChange(value: Date | null) {
    this.detail.effectiveDate = value;
    this.minEndDate = value ? new Date(value) : null;
  }

  onEndDateChange(value: Date | null) {
    this.detail.expireDate = value;
    this.maxStartDate = value ? new Date(value) : null;
  }

  async getData() {
    delete this.payload.filters.selectFilter;
    const result = await this.collotorService.getlistassigncollectorUrlUrlPaged(this.payload);
    const record = (result as any).datatables;
    const count = (result as any).totalRecords;
    if (record.length == 0) {
      this.payload.pageNumber = 1
    }
    this.dataSource = { result: record, count: count };
    this.criteria = result.displayColumns;
    if(this.gridHeader.length == 0){
      this.gridHeader = this.criteria;
    }
    this.dataSource.result.forEach(element => {
      element.checked = false;
    });
  }

  async getDatabyid() {
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

  async doCancel() {
    const messagebody = this.translate.instant('DialogCancel');
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then(async (res) => {
      if (res) {
        await this.getDatabyid();
        this.selectedIds.clear();
        this.selectAllChecked = false;
        this.isIndeterminate = false;
        await this.getData();
        this.updateHeaderCheckboxState();
        this.grid.refresh();
        this.grid.hideSpinner();
        this.maxStartDate = null;
        this.minEndDate = null;
        this.capacity = 0;
        this.markFormPristine();
      }
    });
  }

  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
    if (document.activeElement instanceof HTMLElement) {
    document.activeElement.blur();
  }
  }

  toggleSelectAll(event: any) {
    const checked = event.target.checked;
    const records: any[] = this.grid.getCurrentViewRecords();
    if (checked) {
      records.forEach(r => this.selectedIds.add(r.collectorId));
    } else {
      records.forEach(r => this.selectedIds.delete(r.collectorId));
    }
    this.form.control.markAsDirty();
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
    this.form.control.markAsDirty();
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

  getTextAlign(type: string) {
    return this.utils.getAlign(type);
  }

  dataBound(event) {
    if (this.grid) {
      timer(100).subscribe(() => {
        this.utils.setDataGridFilter(this.grid);
      })
    }
  }

  async actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.clearSelectionState();
      this.payload = this.utils.handleGridAction(event, this.payload);
      await this.getData();
    }
  }

  async onFilterSearch(filterData: any) {
    this.payload.pageNumber = 1;
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
    await this.getData();
    if (this.grid) {
      this.grid.pageSettings.totalRecordsCount = this.dataSource.count;
      this.grid.refresh();
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
