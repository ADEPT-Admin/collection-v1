import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { ModalChildBase } from '../base-modal/base-modal.component';
@Component({
  selector: 'app-search-modal',
  imports: [FormsModule, CommonModule, ButtonModule],
  templateUrl: './search-modal.component.html',
  styleUrl: './search-modal.component.scss',
  providers: [],
})
export class SearchModalComponent extends ModalChildBase {
  // modalClose!: (data?: any) => void;

  columns = [];
  data = [];
  filter = [];
  searchVal = '';
  selectFilter = '';
  key = null;
  constructor() {
    super()
  }

  selectedRow = null;

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

  async settitle(item) {}

  closeWithData() {
    this.close({ id: 'xxx' });
  }
}
