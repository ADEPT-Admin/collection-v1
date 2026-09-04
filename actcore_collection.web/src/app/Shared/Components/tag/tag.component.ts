import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-tag',
  standalone: true,
  templateUrl: './tag.component.html',
  styleUrls: ['./tag.component.scss'],
  imports : [CommonModule]
})
export class TagComponent {
  @Input() value: string = '';
  @Input() severity: 'success' | 'info' | 'warning' | 'danger' | 'default' = 'default';
  @Input() rounded: boolean = false;
}
