import { Directive, ElementRef, HostListener, Input, Renderer2, OnDestroy } from '@angular/core';

@Directive({
  selector: '[appTooltip]',
  standalone : false
})
export class TooltipDirective implements OnDestroy {
  @Input('appTooltip') tooltipContent : string | HTMLElement = '';
  tooltipElement?: HTMLElement;

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  private isEllipsisActive(): boolean {

    const element = this.el.nativeElement as HTMLElement;

    /// check if not from select ////
    if (!(element.classList.contains('select-option') || element.classList.contains('select-display'))) {
      return true;
    }

    const ellipsisEl = element.querySelector('.ellipsis') as HTMLElement | null;
    if (ellipsisEl) {
      return ellipsisEl.scrollWidth > ellipsisEl.clientWidth;
    }


    return element.scrollWidth > element.clientWidth;
  }




  @HostListener('mouseenter')
  onMouseEnter() {
    if (!this.tooltipContent) return;
    if (!this.isEllipsisActive()) return;

    this.showTooltip();
  }

  @HostListener('mouseleave')
  onMouseLeave() {
    this.hideTooltip();
  }

  @HostListener('click')
  onClick() {
    this.hideTooltip();
  }

  @HostListener('window:scroll')
  @HostListener('window:resize')
  onWindowChange() {
    this.hideTooltip();
  }

  private showTooltip() {
    this.tooltipElement = this.renderer.createElement('div');
    this.renderer.addClass(this.tooltipElement, 'custom-tooltip');
    // this.renderer.appendChild(document.body, this.tooltipElement);

    if (typeof this.tooltipContent === 'string') {
      this.tooltipElement.innerHTML = this.tooltipContent; // allow HTML
    } else {
      this.renderer.appendChild(this.tooltipElement, this.tooltipContent);
    }

    this.renderer.appendChild(document.body, this.tooltipElement);

    const rect = this.el.nativeElement.getBoundingClientRect();
    const tooltipMaxWidth = 300;
    const padding = 8; // safe margin

    // style base
    this.renderer.setStyle(this.tooltipElement, 'position', 'fixed'); // ✅ use fixed, not absolute
    this.renderer.setStyle(this.tooltipElement, 'max-width', `${tooltipMaxWidth}px`);
    this.renderer.setStyle(this.tooltipElement, 'white-space', 'normal');
    this.renderer.setStyle(this.tooltipElement, 'word-wrap', 'break-word');
    this.renderer.setStyle(this.tooltipElement, 'z-index', '1000');

    // measure tooltip size after attaching
    const ttRect = this.tooltipElement.getBoundingClientRect();

    let top = rect.bottom + 4;
    let left = rect.left;

    // ✅ Flip vertically if tooltip goes off bottom
    if (top + ttRect.height > window.innerHeight - padding) {
      top = rect.top - ttRect.height - 4;
    }

    // ✅ Clamp horizontally
    if (left + ttRect.width > window.innerWidth - padding) {
      left = window.innerWidth - ttRect.width - padding - 10;
    }
    if (left < padding) {

      left = padding;
    }

    // apply
    this.renderer.setStyle(this.tooltipElement, 'top', `${top}px`);
    this.renderer.setStyle(this.tooltipElement, 'left', `${left}px`);
  }

  hideTooltip() {
    if (this.tooltipElement) {
      this.renderer.removeChild(document.body, this.tooltipElement);
      this.tooltipElement = undefined;
    }
  }

  ngOnDestroy() {
    this.hideTooltip();
  }
}
