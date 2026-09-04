import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { ModalChildBase } from '../base-modal/base-modal.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { GridComponent, GridModule, PageService, ResizeService } from '@syncfusion/ej2-angular-grids';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { Subscription, timer } from 'rxjs';
import * as _ from 'lodash';
import { GridSelect } from 'src/app/Shared/Enums/grid-select';

@Component({
  selector: 'app-icon-modal',
  imports: [FormsModule, CommonModule, ButtonModule , TranslateModule , GridModule],
  providers : [PageService,ResizeService],
  templateUrl: './icon-modal.component.html',
  styleUrl: './icon-modal.component.scss'
})
export class IconModalComponent extends ModalChildBase implements OnInit {
  // modalClose!: (data?: any) => void;

  @ViewChild('grid') public grid!: GridComponent;

  icons = [
    {name : 'fa-house' , code : 'fa-solid fa-house' } ,
    {name : 'fa-file-contract' , code : 'fa-solid fa-file-contract' } ,
    {name : 'fa-gear' , code : 'fa-solid fa-gear' },
    {name : 'fa-user' , code : 'fa-solid fa-user' },
    {name : 'fa-users' , code : 'fa-solid fa-users' },
    {name : 'fa-chart-pie' , code : 'fa-solid fa-chart-pie' },

  ];
  columns = [];
  data = [];
  filter = [];
  searchVal = '';
  selectFilter = '';
  key = null;
  public selectionOptions?: any = GridSelect;
  public pageSettings = { pageSize: 10, pageSizes: [10, 20, 50, 100] };
  public customAttributes: Object = {};
  public resizeSettings = { mode: 'Normal' };
  rowIdClickData: any;
  $lang: Subscription = null;


  constructor(
    private translate: TranslateService,
      private utils: Utils,) {
    super();

    this.$lang = this.translate.onLangChange.subscribe(item => {
                  const tempSelectFilter = _.cloneDeep(this.selectFilter);
                  this.selectFilter = null;
                  timer(10).subscribe(() => {
                    this.selectFilter = _.cloneDeep(tempSelectFilter);
                  });
                  if (this.grid != undefined) {
                    this.utils.setDataGridFilter(this.grid)

                  }

                });


  }

  selectedRow = null;

  ngOnInit(): void {
    this.customAttributes = { class: 'customcss' };
  }

  ngOnDestroy() {

    this.$lang.unsubscribe();
  }


  onClose() {}

  setFilter(event) {
    this.filter[0] = event.value;
    this.selectFilter = event.value;
  }

  filterCallback(event) {
    this.searchVal = event;
  }

  rowSelection(event) {
    if (this.selectedRow) {
    }
  }

  async onRowDoubleClick(data) {
    const rowData = data;
    this.selectedRow = rowData;
    this.rowSelection({});
  }



  doubleClick(event) {
    if (event.rowData) {
      this.close(this.rowIdClickData);
    }
  }


  rowSelectedclick(e) {
    this.rowIdClickData = e.data;
  }


  closeWithData() {
    this.close(this.rowIdClickData);
  }


  dataBound(event) {
    if (this.grid) {
      timer(100).subscribe(() => {
        this.utils.setDataGridFilter(this.grid)
      })
    }
  }
}

