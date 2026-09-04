import {
  Component,
  EventEmitter,
  Type,
  Output,
  ViewChild,
  ViewContainerRef,
  AfterViewInit,
} from '@angular/core';
import { DialogModule, DialogComponent } from '@syncfusion/ej2-angular-popups';


export abstract class ModalChildBase {
  // injected by BaseModalComponent at runtime
  close!: (data?: any) => void;
}

@Component({
  selector: 'app-base-modal',
  imports: [DialogModule],
  standalone: true,
  template: `
    <ejs-dialog
      #dialog
      [header]="header"
      [width]="width"
      [isModal]="isModal"
      [showCloseIcon]="showCloseIcon"
      [zIndex]="zIndex"
      [visible]="false"
      (close)="onClose()"
      allowDragging="true"
    >
      <ng-template #content>
        <div class="modal-body">
          <ng-template #contentHost></ng-template>
        </div>
      </ng-template>
    </ejs-dialog>
  `,
})
export class BaseModalComponent {
  @ViewChild('dialog') dialog!: DialogComponent;
  @ViewChild('contentHost', { read: ViewContainerRef })
  contentHost!: ViewContainerRef;

  @Output() closeModal = new EventEmitter<any>();

  header = '';
  width = '400px';
  isModal = true;
  showCloseIcon = true;
  zIndex = 10000;
  childComponentType?: Type<any>;
  childComponentData: any = {};

  hasCustomFooter = false;

  compRef: any;

  ngAfterContentInit() {
    // Detect if footer exists
    const footerEl = (this as any).el?.nativeElement?.querySelector?.(
      '[modal-footer]'
    );
    this.hasCustomFooter = !!footerEl;
  }
  show() {
    // Wait for Angular to initialize ViewChild after attaching to DOM
    Promise.resolve().then(() => {
      if (!this.contentHost) {
        console.error('contentHost still undefined!');
        return;
      }

      if (this.childComponentType) {
        this.compRef = this.contentHost.createComponent(
          this.childComponentType
        );
        Object.assign(this.compRef.instance, this.childComponentData);

        (this.compRef.instance as ModalChildBase).close = (data?: any) => {
          this.closeModal.emit(data ?? null);
          this.dialog.hide();
        };
      }

      this.dialog.show();
    });
  }

  onClose() {
    this.closeModal.emit(null);

  }


}
