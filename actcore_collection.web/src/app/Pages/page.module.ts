import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { PageRoutingModule } from './page-routing.module';
import { PageComponent } from './page.component';
import { DashboardComponent } from './dashboard/dashboard.component';

@NgModule({
  imports: [CommonModule, RouterModule, PageRoutingModule],
})
export class PageModule {}
