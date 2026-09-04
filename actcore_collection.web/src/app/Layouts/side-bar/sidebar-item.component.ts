import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

export interface MenuItem {
  name: string;
  link?: string;
  icon?: string;
  isShow: boolean;
  children?: MenuItem[];
}

@Component({
  selector: 'app-sidebar-item',
  templateUrl: './sidebar-item.component.html',
  styleUrl: './side-bar.component.scss',

  imports : [CommonModule , TranslateModule , RouterModule , RouterLink , RouterLinkActive]
})
export class SidebarItemComponent {
  @Input() item!: MenuItem;
}
