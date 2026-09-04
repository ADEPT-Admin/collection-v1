import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ContractDetail, Task } from '../../../task.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { GridAllModule, GridComponent, ResizeService, GridModule } from '@syncfusion/ej2-angular-grids';
import { Subscription, timer } from 'rxjs';
import { WorkService } from '../../../work.service';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { CalendarComponent } from 'src/app/Shared/Components/calendar/calendar.component';
import { ButtonAllModule } from '@syncfusion/ej2-angular-buttons';
import { InputNumberComponent } from 'src/app/Shared/Components/input-number/input-number.component';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { DateTimePickerModule } from '@syncfusion/ej2-angular-calendars'
import { ActivatedRoute, RouterModule } from '@angular/router';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';

@Component({
  selector: 'app-contract-information',
  imports: [
    GridModule,
    CommonModule,
    FormsModule,
    TranslateModule,
    GridAllModule,
    RouterModule,
    DateTimePickerModule,
    ShareDirectiveModule,
    InputNumberComponent,
    ButtonAllModule,
    CalendarComponent,
  ],
  providers: [ResizeService],
  templateUrl: './contract-information.component.html',
  styleUrl: './contract-information.component.scss'
})
export class ContractInformationComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('grid') public grid!: GridComponent;
  @Input() contractID: any;
  payload = {
    pageNumber: 1,
    pageSize: 10,
    sortColumn: null,
    sortDirection: 'asc',
    filters: {}
  }
  langSub: Subscription = null;
  detailContract: ContractDetail = {};
  dataBorrowerdetail: any = [];
  dataGuarantordetail: any = [];
  listBorrowerAddress: any = [];
  listGuarantorAddress: any = [];
  listBorrowerPhone: any = [];
  listGuarantorPhone: any = [];
  public selectionOptions?: any = GridSelect;
  public customAttributes: Object = { class: 'customcss' };
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  public resizeSettings = { mode: 'Normal' };
  constructor(
    private routed: ActivatedRoute,
    private loaderService: AppLoaderService,
    private workService: WorkService,
    private utils: Utils,
    private translate: TranslateService,) {
    this.langSub = this.translate.onLangChange.subscribe(item => {
      if (this.grid != undefined) {
        this.utils.setDataGridFilter(this.grid)
      }
    })
  }

  ngOnDestroy() {
    this.langSub.unsubscribe();
  }

  async ngOnInit() {
    const loaderId = this.loaderService.show();
    await this.getDataContractAddress();
    await this.getDataContractPhone();
    await this.getDataContract();
    await this.getDataContractPersons();
    setTimeout(() => {
      this.loaderService.hide(loaderId);
    }, 200);
  }

  ngAfterViewInit(): void {
  }


  dataBound(event) {
    if (this.grid) {
      timer(100).subscribe(() => {
        this.utils.setDataGridFilter(this.grid);
      })
    }
  }

  async actionBegin(event, type) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.payload = this.utils.handleGridAction(event, this.payload);
    }
  }

  async getDataContract() {
    if (this.contractID != undefined) {
      const res = await this.workService.getContractDetail(this.contractID);
      this.detailContract = res.data;
    }
  }

  async getDataContractPersons() {
    if (this.contractID != undefined) {
      const res = await this.workService.getcontractPersonsUrl(this.contractID);
      this.dataBorrowerdetail = [];
      this.dataGuarantordetail = [];
      const borrower = res.find(item => item.personType === 'Borrower');
      const guarantor = res.find(item => item.personType === 'Guarantor');
      if (borrower) {
        this.dataBorrowerdetail.push(borrower);
        this.dataBorrowerdetail = this.dataBorrowerdetail[0];
      }
      if (guarantor) {
        this.dataGuarantordetail.push(guarantor);
        this.dataGuarantordetail = this.dataGuarantordetail[0];
      }
    }
  }

  async getDataContractAddress() {
    if (!this.contractID) return;
    const res = await this.workService.getcontractAddresslistUrl(this.contractID, this.payload)
    const list = res.data.datatables;
    this.listBorrowerAddress = list.filter(x => x.personType === 'Borrower');
    this.listGuarantorAddress = list.filter(x => x.personType === 'Guarantor');
  }

  async getDataContractPhone() {
    if (!this.contractID) return;
    const res = await this.workService.getcontractphonelistUrl(this.contractID, this.payload);
    const list = res.data.datatables;
    this.listBorrowerPhone = list.filter(x => x.personType === 'Borrower');
    this.listGuarantorPhone = list.filter(x => x.personType === 'Guarantor');
  }

}
