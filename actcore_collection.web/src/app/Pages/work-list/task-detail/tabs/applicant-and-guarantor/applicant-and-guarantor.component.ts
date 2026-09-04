import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ApplicantandGuarantor } from '../../../task.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { GridAllModule, GridComponent, ResizeService } from '@syncfusion/ej2-angular-grids';
import { Subscription, timer } from 'rxjs';
import { WorkService } from '../../../work.service';
import { DateTimePickerModule } from '@syncfusion/ej2-angular-calendars';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { InputNumberComponent } from 'src/app/Shared/Components/input-number/input-number.component';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { CalendarComponent } from 'src/app/Shared/Components/calendar/calendar.component';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-applicant-and-guarantor',
  imports: [CommonModule, FormsModule, TranslateModule, GridAllModule ,DateTimePickerModule,InputNumberComponent,CalendarComponent,LocalizedDatePipe],
  providers: [ResizeService],
  templateUrl: './applicant-and-guarantor.component.html',
  styleUrl: './applicant-and-guarantor.component.scss',
})
export class ApplicantAndGuarantorComponent implements OnInit, AfterViewInit,OnDestroy{
  @Input() key: any;

  list: ApplicantandGuarantor[] = [];
  dataBorrowerdetails : any = [];
  @ViewChild('grid') public grid!: GridComponent;
  langSub : Subscription = null;
  public selectionOptions?: any = GridSelect;
  public customAttributes: Object = { class: 'customcss' };
  public resizeSettings = { mode: 'Normal' };
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  private loaded = false;
  constructor(
    private workService: WorkService, private utils: Utils, private translate: TranslateService,) {
    this.langSub = this.translate.onLangChange.subscribe(item => {
      if (this.grid != undefined) {
        this.utils.setDataGridFilter(this.grid)
      }
    })
  }

  ngOnDestroy() {
    this.langSub.unsubscribe();
  }


  dataBound(event) {
    if(this.grid){
    timer(100).subscribe(() => {
      this.utils.setDataGridFilter(this.grid);
    })
    }
  }

  ngOnInit() {
    timer(10).subscribe(() => {
      if (!this.loaded) {
        this.loaded = true;
        this.getData();
      }

    })
  }
  ngAfterViewInit(): void {

  }

  async getData() {
    if (this.key != undefined) {
      const res = await this.workService.getcontractPersonsUrl(this.key);
      this.dataBorrowerdetails = res.borrower;
      this.list = res.guarantors;
    }
  }

}
