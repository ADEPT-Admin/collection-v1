import {
  ApplicationRef,
  ComponentRef,
  Injectable,
  Type,
  createComponent,
  EnvironmentInjector,
} from '@angular/core';
import { timer } from 'rxjs';
import { BaseModalComponent } from 'src/app/Modals/base-modal/base-modal.component';

@Injectable({ providedIn: 'root' })
export class ModalService {
  private stack: ComponentRef<BaseModalComponent>[] = [];

  constructor(
    private appRef: ApplicationRef,
    private injector: EnvironmentInjector
  ) {}

  async open<T>(
    component: Type<T>,
    config: { header?: string; data?: Partial<T> | Record<string, any>; width?: string } = {}
  ): Promise<T | undefined> {
    return new Promise<T | null>((resolve) => {
      const modalRef = createComponent(BaseModalComponent, {
        environmentInjector: this.injector,
      });
      modalRef.instance.header = config.header ?? '';
      modalRef.instance.width = config.width ?? '400px';
      modalRef.instance.childComponentType = component;
      modalRef.instance.childComponentData = config.data ?? {};

      // modalRef.instance.close.subscribe((result) => {
      //   this.close(modalRef);
      //   resolve(result);
      // });

       modalRef.instance.closeModal.subscribe((data: T | null) => {
        this.close(modalRef);
        resolve(data ?? null);   // ✅ always return at least null
        modalRef.destroy();
      });

      this.appRef.attachView(modalRef.hostView);
      document.body.appendChild(modalRef.location.nativeElement);
      timer(30).subscribe(() => {
        modalRef.instance.show();

        this.stack.push(modalRef);
      });
    });
  }

  private close(modalRef: ComponentRef<BaseModalComponent>) {
    const index = this.stack.indexOf(modalRef);
    if (index > -1) this.stack.splice(index, 1);
    this.appRef.detachView(modalRef.hostView);
    modalRef.destroy();
  }
}
