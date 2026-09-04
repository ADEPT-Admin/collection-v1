import { Routes } from '@angular/router';
import { LoginComponent } from './Pages/login/login.component';
import { DashboardComponent } from './Pages/dashboard/dashboard.component';
import { MainComponent } from './Layouts/main/main.component';
import { authGuard } from './Shared/Guards/auth.guard';
import { PageComponent } from './Pages/page.component';
import { NotFoundComponent } from './Pages/not-found/not-found.component';
import { ChangePassComponent } from './Pages/change-pass/change-pass.component';
import { SecuritiesComponent } from './Pages/securities/securities.component';
import { SystemComponent } from './Pages/system/system.component';


export const routes: Routes = [
  {
    path : 'login',
    component : LoginComponent,
    pathMatch : 'full'
  },

   {
    path : 'change-password',
    canActivate : [authGuard],
    component : ChangePassComponent,
    pathMatch : 'full'
  },
  {
    path : '',
    component : MainComponent,
    canActivate : [authGuard],
    children : [
      {
        path : 'user',
        component : PageComponent,
        children: [
          {
            path: '',
            loadChildren: () =>
              import('./Pages/page-routing.module').then(m => m.PageRoutingModule),
          }
        ]
      },
      {
        path : 'dashboard',
        component : PageComponent,
        children: [
          { path: '', component: DashboardComponent },

          {
            path: '',
            loadChildren: () =>
              import('./Pages/page-routing.module').then(m => m.PageRoutingModule),
          }
        ]
      },
      {
        path : 'worklist',
        component : PageComponent,
        children: [
          { path: '', component: DashboardComponent },

          {
            path: '',
            loadChildren: () =>
              import('./Pages/page-routing.module').then(m => m.PageRoutingModule),
          }
        ]
      },
      {
        path : 'reports',
        component : PageComponent,
        children: [
          { path: '', component: DashboardComponent },

          {
            path: '',
            loadChildren: () =>
              import('./Pages/page-routing.module').then(m => m.PageRoutingModule),
          }
        ]
      },
      {
        path : 'collector',
        component : PageComponent,
        children: [
          { path: '', component: DashboardComponent },

          {
            path: '',
            loadChildren: () =>
              import('./Pages/page-routing.module').then(m => m.PageRoutingModule),
          }
        ]
      },
      {
        path : 'collection-master',
        component : PageComponent,
        children: [
          { path: '', component: DashboardComponent },

          {
            path: '',
            loadChildren: () =>
              import('./Pages/page-routing.module').then(m => m.PageRoutingModule),
          }
        ]
      },


      {
        path : 'security',
        component : PageComponent,
        children: [
          { path: '', component: SecuritiesComponent },

          {
            path: '',
            loadChildren: () =>
              import('./Pages/page-routing.module').then(m => m.PageRoutingModule),
          }
        ]
      },{
        path : 'system',
        component : PageComponent,
        children: [
          { path: '', component: SystemComponent },

          {
            path: '',
            loadChildren: () =>
              import('./Pages/page-routing.module').then(m => m.PageRoutingModule),
          }
        ]
      },
    ]
  },
  {
    path: '404',
    component: NotFoundComponent,
  },
  {
    path: '**',
    redirectTo: '/404',
  }

];
