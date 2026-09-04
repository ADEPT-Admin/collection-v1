import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { CollectionNote } from '../../../task.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { GridAllModule, GridComponent, ResizeService } from '@syncfusion/ej2-angular-grids';
import { Subscription, timer } from 'rxjs';
import { WorkService } from '../../../work.service';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';
import { LocalizedDatePipe } from 'src/app/Shared/Pipes/localized-date.pipe';

@Component({
  selector: 'app-collection-note',
  imports: [CommonModule , FormsModule , TranslateModule , GridAllModule,LocalizedDatePipe],
  providers: [ResizeService],
  templateUrl: './collection-note.component.html',
  styleUrl: './collection-note.component.scss'
})
export class CollectionNoteComponent implements OnInit ,  AfterViewInit , OnDestroy{
  @Input() key : any;

  list : any = [];
  @ViewChild('grid') public grid!: GridComponent;
  langSub : Subscription = null;
  payload = {
    pageNumber: 1,
    pageSize: 10,
    sortColumn: null,
    sortDirection: 'asc',
    filters: {}
  }
  public selectionOptions?: any = GridSelect;
  public customAttributes: Object = { class: 'customcss'};
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  public resizeSettings = { mode: 'Normal' };
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


  async getData(){
      if(this.key != undefined){
        const res = await this.workService.getCollectionNoteDetail(this.key , this.payload)
        const list = res.data.datatables;

        list.forEach(item => {
          item.followupDate = this.utils.transformToLocalDateTime(item.followupDate)
          item.nextFollowupDate = this.utils.transformToLocalDateTime(item.nextFollowupDate)
          item.promiseToPayDate = this.utils.transformToLocalDateTime(item.promiseToPayDate)
        })

        this.list = {result: list, count: res.data.totalRecords };


      }

  }


  actionBegin(event) {
    if (["paging", "sorting", "filtering"].includes(event.requestType)) {
      this.payload = this.utils.handleGridAction(event, this.payload);
      this.getData();
    }
  }

}
