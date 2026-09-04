// dynamic-host.directive.ts
import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[dynamicHost]',
  standalone: true
})
export class DynamicHostDirective {
  constructor(public viewContainerRef: ViewContainerRef) {}
}
