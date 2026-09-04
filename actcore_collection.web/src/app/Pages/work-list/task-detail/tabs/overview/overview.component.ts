import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { GridAllModule, GridComponent, GridModule, ResizeService } from '@syncfusion/ej2-angular-grids';
import { Subscription, timer } from 'rxjs';
import { WorkService } from '../../../work.service';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { CalendarComponent } from 'src/app/Shared/Components/calendar/calendar.component';
import { ButtonAllModule } from '@syncfusion/ej2-angular-buttons';
import { InputNumberComponent } from 'src/app/Shared/Components/input-number/input-number.component';
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
import { DateTimePickerModule } from '@syncfusion/ej2-angular-calendars'
import { RouterModule } from '@angular/router';
import { Task } from '../../../task.model';
import { SafeHtmlPipe } from 'src/app/Shared/Pipes/safe-html.pipe';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-overview',
  imports: [
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
    SafeHtmlPipe,
    LocalizedDatePipe,
    GridModule,
  ],
  providers: [ResizeService],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.scss'
})
export class OverviewComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('grid') public grid!: GridComponent;
  @Input() key: any;
  contractHeader: Task = {};
  langSub: Subscription = null;
  dataSource: any = [];
  gridHeader: any = [];
  criteria: any;
  public selectionOptions?: any = GridSelect;
  public customAttributes: Object = { class: 'customcss' };
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  public resizeSettings = { mode: 'Normal' };
  constructor(
    private workService: WorkService, private utils: Utils, public translate: TranslateService,) {
    this.langSub = this.translate.onLangChange.subscribe(item => { })
  }

  ngOnDestroy() {
    this.langSub.unsubscribe();
  }

  async ngOnInit() {
    await this.getDetail();
  }
  ngAfterViewInit(): void {

  }

  async getDataOverDueDetail() {
    this.workService.getOverDueDetail(this.contractHeader.contractNo).then(async res => {
      let result: any = res;
      this.dataSource = result.data.datatables;
      this.gridHeader = result.data.displayColumns;
    })
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

  async getDetail() {
    const res = await this.workService.getWorkDetail(this.key);
    if (res.status == true) {
      this.contractHeader = res.data;
      if (this.contractHeader.contractNo) {
        await this.getDataOverDueDetail();
      }
    } else {
      const message = res.message[this.translate.currentLang];
      await this.utils.dialogMessageAll('error', message);
    }

  }

}
