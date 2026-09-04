import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons'
import { ShareDirectiveModule } from '../../Directives/share-directive.module';

@Component({
    selector: 'app-toolbar',
    imports: [TranslateModule,ButtonModule , ShareDirectiveModule],
    templateUrl: './toolbar.component.html',
    styleUrl: './toolbar.component.scss'
})
export class ToolbarComponent implements OnChanges {

  @Input() showButton : any;
  @Input() isDisable = false;

  @Input() dirty = false;

  @Output() clickBtnNew = new EventEmitter();
  @Output() clickBtnSave = new EventEmitter();
  @Output() clickBtnCancel = new EventEmitter();
  @Output() clickBtnDelete = new EventEmitter();
  @Output() clickBtnPrint = new EventEmitter();
  @Output() clickBtnBack= new EventEmitter();

  ngOnChanges(changes: SimpleChanges) {
    // changes.prop contains the old and the new value...
    this.showButton.cancel.disabled = !this.dirty;
    this.showButton.save.disabled = !this.dirty;
  }


  clickButtonNew(){
    this.clickBtnNew.emit();
  }



  clickButtonDelete(){
    this.clickBtnDelete.emit();
  }

  clickButtonPrint(){
    this.clickBtnPrint.emit();

  }

  clickButtonCancel(){
     this.clickBtnCancel.emit();
  }

  clickButtonSave(){

    this.clickBtnSave.emit();
  }

  clickButtonBack(){

    this.clickBtnBack.emit();
  }

}
