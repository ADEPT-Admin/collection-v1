import { TeamAssignmentDetailComponent } from './collector/team-assignment-detail/team-assignment-detail.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './securities/users/users.component';
import { UserDetailComponent } from './securities/user-detail/user-detail.component';
import { GroupsComponent } from './securities/groups/groups.component';
import { GroupDetailComponent } from './securities/group-detail/group-detail.component';
import { ConfirmExitGuard } from '../Shared/Guards/confirm-exit.guard';
import { SystemConfigsComponent } from './securities/system-configs/system-configs.component';
import { SystemConfigDetailComponent } from './securities/system-config-detail/system-config-detail.component';
import { ChangePasswordComponent } from './securities/change-password/change-password.component';
import { TaskListsComponent } from './work-list/task-lists/task-lists.component';
import { TaskDetailComponent } from './work-list/task-detail/task-detail.component';
import { CollectorProfileComponent } from './collector/collector-profile/collector-profile.component';
import { CollectorTeamComponent } from './collector/collector-team/collector-team.component';
import { CollectorTeamDetailComponent } from './collector/collector-team-detail/collector-team-detail.component';
import { CollectorProfileDetailComponent } from './collector/collector-profile-detail/collector-profile-detail.component';
import { ApproveReassignmentComponent } from './work-list/approve-reassignment/approve-reassignment.component';
import { ChangeTranslateValueComponent } from './securities/change-translate-value/change-translate-value.component';
import { ChangeTranslateValueDetailComponent } from './securities/change-translate-value-detail/change-translate-value-detail.component';
import { MasterMenuComponent } from './securities/master-menu/master-menu.component';
import { MasterMenuDetailComponent } from './securities/master-menu-detail/master-menu-detail.component';
import { TeamAssignmentComponent } from './collector/team-assignment/team-assignment.component';
import { UnassignListsComponent } from './work-list/unassign-lists/unassign-lists.component';
import { EmployeeListComponent } from './securities/employee-list/employee-list.component';
import { EmployeeDetailComponent } from './securities/employee-detail/employee-detail.component';
import { AddMultipleUserComponent } from './securities/add-multiple-user/add-multiple-user.component';
import { AddMultipleCollectorProflieComponent } from './collector/add-multiple-collector-proflie/add-multiple-collector-proflie.component';

const routes: Routes = [
  {
    path: 'user',
    children: [
      {
        path: '',
        component: UsersComponent,
        data : {
          menuId : '9104'
        }
      },
      {
        path: 'new',
        component: AddMultipleUserComponent,
        canDeactivate: [ConfirmExitGuard],
        data : {
          menuId : '9104'
        }
      },
      {
        path: ':id',
        component: UserDetailComponent,
        canDeactivate: [ConfirmExitGuard],
        data : {
          menuId : '9104'
        }
      },


      {
        path: '',
        loadChildren: () =>
          import('./page-routing.module').then((m) => m.PageRoutingModule),
      },
    ],
  },
  {
    path: 'usergroup',
    children: [
      {
        path: '',
        component: GroupsComponent,
        data : {
          menuId : '9103'
        }
      },
      {
        path: ':id',
        component: GroupDetailComponent,
        data : {
          menuId : '9103'
        },
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'employee',
    children: [
      {
        path: '',
        component: EmployeeListComponent,
        data : {
          menuId : '0701'
        }
      },
      {
        path: ':id',
        component: EmployeeDetailComponent,
        data : {
          menuId : '0701'
        },
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'system-parameter',
    children: [
      {
        path: '',
        component: SystemConfigsComponent,
        data : {
          menuId : '9202'
        }
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        canDeactivate: [ConfirmExitGuard],
        data : {
          menuId : '9202'
        }
      },
    ],
  },
    {
    path: 'language',
    children: [
      {
        path: '',
        component: ChangeTranslateValueComponent,
        data : {
          menuId : '9203'
        }
      },
      {
        path: ':id',
        component: ChangeTranslateValueDetailComponent,
        canDeactivate: [ConfirmExitGuard],
        data : {
          menuId : '9203'
        }
      },
    ],
  },
      {
    path: 'master-menu',
    children: [
      {
        path: '',
        component: MasterMenuComponent,
        data : {
          menuId : '9201'
        }
      },
      {
        path: ':id',
        component: MasterMenuDetailComponent,
        canDeactivate: [ConfirmExitGuard],
        data : {
          menuId : '9201'
        }
      },
    ],
  },
  {
    path: 'my-worklist',
    children: [
      {
        path: '',
        component: TaskListsComponent,
        data : {
          menuId : '0201'
        }
      },
      {
        path: ':id',
        component: TaskDetailComponent,
        canDeactivate: [ConfirmExitGuard],
        data : {
          menuId : '0201'
        }
      },
    ],
  },
  {
    path: 'approve-reassign',
    children: [
      {
        path: '',
        component: ApproveReassignmentComponent,
        data : {
          menuId : '0202'
        }
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        canDeactivate: [ConfirmExitGuard],
        data : {
          menuId : '0202'
        }
      },
    ],
  },
  {
    path: 'unassigned-worklist',
    children: [
      {
        path: '',
        component: UnassignListsComponent,
        data : {
          menuId : '0203'
        }
      },
      {
        path: ':id',
        component: TaskDetailComponent,
        canDeactivate: [ConfirmExitGuard],
        data : {
          menuId : '0203'
        }
      },
    ],
  },

  {
    path: 'do-not-call',
    children: [
      {
        path: '',
        component: SystemConfigsComponent,
        data : {
          menuId : '0204'
        }
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        canDeactivate: [ConfirmExitGuard],
        data : {
          menuId : '0204'
        }
      },
    ],
  },
  {
    path: 'unassigned-worklist',
    children: [
      {
        path: '',
        component: SystemConfigsComponent,
        data : {
          menuId : '0203'
        }
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        canDeactivate: [ConfirmExitGuard],
        data : {
          menuId : '0203'
        }
      },
    ],
  },
  {
    path: 'rp1-assignment-summary',
    children: [
      {
        path: '',
        component: SystemConfigsComponent,
        data : {
          menuId : '0301'
        }
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        data : {
          menuId : '0301'
        },
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'rp2-reassign-collectors',
    children: [
      {
        path: '',
        component: SystemConfigsComponent,
        data : {
          menuId : '0302'
        }
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        data : {
          menuId : '0302'
        },
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'collector-profile',
    children: [
      {
        path: '',
        component: CollectorProfileComponent,
        data : {
          menuId : '0401'
        }
      },
      {
        path: 'new',
        component: AddMultipleCollectorProflieComponent,
        canDeactivate: [ConfirmExitGuard],
        data : {
          menuId : '0401'
        }
      },
      {
        path: ':id',
        component: CollectorProfileDetailComponent,
         data : {
          menuId : '0401'
        },
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'collector-team',
    children: [
      {
        path: '',
        component: CollectorTeamComponent,
         data : {
          menuId : '0402'
        }
      },
      {
        path: ':id',
        component: CollectorTeamDetailComponent,
         data : {
          menuId : '0402'
        },
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'team-assignment',
    children: [
      {
        path: '',
        component: TeamAssignmentComponent,
         data : {
          menuId : '0403'
        }
      },
      {
        path: ':id',
        component: TeamAssignmentDetailComponent,
         data : {
          menuId : '0403'
        },
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'collector-mapping-area',
    children: [
      {
        path: '',
        component: SystemConfigsComponent,
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'sla',
    children: [
      {
        path: '',
        component: SystemConfigsComponent,
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'approval',
    children: [
      {
        path: '',
        component: SystemConfigsComponent,
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'collection-role',
    children: [
      {
        path: '',
        component: SystemConfigsComponent,
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'collection-team',
    children: [
      {
        path: '',
        component: SystemConfigsComponent,
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'collection-area-code',
    children: [
      {
        path: '',
        component: SystemConfigsComponent,
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },
  {
    path: 'adjacent-area-mapping',
    children: [
      {
        path: '',
        component: SystemConfigsComponent,
      },
      {
        path: ':id',
        component: SystemConfigDetailComponent,
        canDeactivate: [ConfirmExitGuard],
      },
    ],
  },

  {
    path: 'change-password',
    children: [
      {
        path: '',
        component: ChangePasswordComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PageRoutingModule {}
