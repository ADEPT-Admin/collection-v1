import { CommonModule } from '@angular/common';
import { FilterService, FreezeService, GridAllModule, GridComponent, GridModule, PageService, ResizeService, SortService } from '@syncfusion/ej2-angular-grids';
import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { ToolbarComponent } from 'src/app/Shared/Components/toolbar/toolbar.component';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CanComponentDeactivate } from 'src/app/Shared/Guards/confirm-exit.guard';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { CollectorProfile, CollectorService, Employee } from '../collector.service';
import { HeaderComponent } from 'src/app/Shared/Components/header/header.component';
import { LowercaseFirstPipe } from 'src/app/Shared/Pipes/lowercase-first.pipe';
import { SafeHtmlPipe } from 'src/app/Shared/Pipes/safe-html.pipe';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { timer } from 'rxjs';
import { FilterSelectComponent } from 'src/app/Shared/Components/filter-select/filter-select.component';

@Component({
  selector: 'app-collector-profile-detail',
  imports: [
    GridModule,
    CommonModule,
    FormsModule,
    ToolbarComponent,
    ReactiveFormsModule,
    TranslateModule,
    SelectComponent,
    HeaderComponent,
    LowercaseFirstPipe,
    SafeHtmlPipe,
    LocalizedDatePipe,
    GridAllModule,
    FilterSelectComponent

  ],
  templateUrl: './collector-profile-detail.component.html',
  styleUrl: './collector-profile-detail.component.scss',
  providers: [CollectorService,PageService,SortService,FilterService, FreezeService,ResizeService]
})
export class CollectorProfileDetailComponent implements OnInit, AfterViewInit, CanComponentDeactivate {
  async canDeactivate(): Promise<boolean> {
    if (!this.form.dirty) return true;
    const result = await this.utils.dialogConfirmSaveChange()
    return result;
  }
  @ViewChild('grid') public grid!: GridComponent;
  @ViewChild('myForm') form: NgForm;

  public selectionOptions?: any = GridSelect;
  columns: any = [];
  criteria: any;
  actionListbtn: any = {
    new: { show: false, text: null, disabled: false },
    save: { show: true, text: null, disabled: true },
    delete: { show: true, text: null, disabled: false },
    cancel: { show: true, text: null, disabled: true },
    back: { show: true, text: null, disabled: false }
  }

  public customAttributes: Object = {};
  detail: CollectorProfile = {};
  detailEmployee: Employee = {};
  datadetailEmployee: any = {};
  id: any = '';
  previousId: string = null;
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
    "allowNew": true,
    "allowEdit": true,
    "allowDelete": false
  };
  dataSource: { result: any[]; count: number } = {result : [] , count:0};
  menuId = null;
  header = null;
  dataCollectorRole: any = [];
  selectCollectorRole: string = '';
  gridHeader: any = [];
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  public resizeSettings = { mode: 'Normal' };
  valueselectFilter : any = '';
  constructor(
    private collotorService: CollectorService,
    private routed: ActivatedRoute,
    private router: Router,
    private utils: Utils,
    private cdRef: ChangeDetectorRef,
    private translate: TranslateService,
    private loader: AppLoaderService,
  ) {

    this.routed.paramMap.subscribe(async params => {
      this.id = params.get('id');
      const loaderId = this.loader.show();
      this.menuId = this.routed.snapshot.data?.menuId || '0401';
      this.permission = await this.utils.getPermission(this.menuId);
      if (this.permission.allowView) {
        this.cdRef.detectChanges();
      }
      this.header = await this.utils.getMenu(this.menuId)
      this.header.path[this.header.path.length - 1].canLink = true;
        await this.getData();
        this.loader.hide(loaderId);
        this.header.path = [...this.header.path, ...[{ menuId: 'detail', itemName: { th: `รายละเอียด (${this.detail.employeeId})`, en: `Detail (${this.detail.employeeId})` } }]];
      this.previousId = this.id;
      if (!this.permission?.allowView || (!this.permission?.allowNew || !this.permission?.allowEdit)) {
        setTimeout(() => {
          this.form?.control.disable();
        }, 200);
      }

      if (this.permission.allowEdit && this.id != null) {
        setTimeout(() => {
          this.form?.control.enable();
        }, 200);
      }
      await this.getListpagedBycollector();
      await this.getcollectorRole();

    })
  }

  async ngOnInit() {
    this.customAttributes = { class: 'customcss' };
  }

  async getcollectorRole() {
    let dataCollectorRole = await this.collotorService.getcollectorRole();
    this.dataCollectorRole = (dataCollectorRole as any).data.map(element => {
      return {
        label: element.colRoleName,
        value: element.colRoleId
      };
    });
    const role = this.dataCollectorRole.find(r => r.value == this.detail.colRole?.colRoleId);
    this.selectCollectorRole = role ? role.value : '';
  }

  async getListpagedBycollector() {
    delete this.payload.filters.selectFilter;
    const result = await this.collotorService.getTeamAssignmentlistpagedbycollectorUrl(this.payload,this.id);
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
  }

  async onSave() {
    this.form.onSubmit(null);
    this.form.control.markAllAsTouched();
    if (this.form.valid) {
      let res = null
      const requestModel: any = {
        collectorId: this.detail.collectorGuid,
        colRoleId: this.selectCollectorRole,
        isActive: this.detail.isActive
      };

      res = await this.collotorService.updateCollectors(requestModel);
      if (res?.status) {
        const message = res.message[this.translate.currentLang]
        this.utils.dialogMessageAll('success', message);
        this.markFormPristine();
        this.getData();
      } else {
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('error', message);
      }
    }
  }

  doBack() {
    this.router.navigate(['..'], { relativeTo: this.routed })
  }

  async actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.payload = this.utils.handleGridAction(event, this.payload);
      await this.getListpagedBycollector();
    }
  }

  dataBound(event){
    if(this.grid){
      timer(100).subscribe(() => {
          this.utils.setDataGridFilter(this.grid);
      })
    }
  }

  async getData() {
    this.actionListbtn.new.disabled = !this.permission.allowNew;
    this.actionListbtn.delete.disabled = !this.permission.allowDelete;
    let dataDetail = await this.collotorService.getCollectorDetail(this.id);
    if(dataDetail.status == true){
      this.detail = (dataDetail as any).data || {};
      this.original = _.cloneDeep(this.detail);
    }else{
      const message = dataDetail.message[this.translate.currentLang];
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
        const payload = [{ collectorId: this.detail.collectorGuid }];
        const res: any = await this.collotorService.deleteCollectors(payload);
        this.loader.hide(loaderId)

        if (res.status == true) {
          const message = res.message[this.translate.currentLang]
          const res_msg = await this.utils.dialogMessageAll('success', message);
          if (res_msg) {
            this.markFormPristine();
            this.router.navigate(['..'], { relativeTo: this.routed });
          }
        }else{
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
        await this.getData();
        await this.getcollectorRole();
        this.markFormPristine()
      }
    });
  }

  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
  }

  getTextAlign(type: string) {
    return this.utils.getAlign(type);
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
    await this.getListpagedBycollector();
    this.grid.refresh();
  }

}
