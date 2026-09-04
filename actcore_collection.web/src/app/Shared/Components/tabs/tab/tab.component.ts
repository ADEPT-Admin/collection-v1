import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, ContentChild, Input, TemplateRef } from '@angular/core';

@Component({
  selector: 'app-tab',
  imports: [CommonModule],
  templateUrl: './tab.component.html',
  styleUrl: './tab.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TabComponent {
 @Input() title?: string; // fallback if no custom header
  @Input() lazy = true;
  active = false;

  loaded = false;

  @ContentChild('tabHeader') headerTpl!: TemplateRef<any>;
  @ContentChild('tabContent') content!: TemplateRef<any>;
}
