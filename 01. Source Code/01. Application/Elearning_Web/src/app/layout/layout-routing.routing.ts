import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExtraOptions, Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '../auth/guards/auth.guard';

import { LayoutComponent } from './layout.component';
import { ScreenWaitComponent } from './screen-wait/screen-wait.component';
import { DashBoardComponent } from '../dash-board/dash-board/dash-board.component';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('../auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'sample',
        canActivate: [AuthGuard],
        loadChildren: () => import('../sample/sample.module').then(m => m.SampleModule)
      },
      {
        path: 'nguoi-dung',
        canActivate: [AuthGuard],
        loadChildren: () => import('../users/users.module').then(m => m.UsersModule)
      },
      {
        path: 'dao-tao',
        canActivate: [AuthGuard],
        loadChildren: () => import('../education-programs/education-programs.module').then(m => m.EducationProgramsModule)
      },
      {
        path: 'page',
        canActivate: [AuthGuard],
        loadChildren: () => import('../page/page.module').then(m => m.PageModule)
      },
      {
        path: '',
        canActivate: [AuthGuard],
        loadChildren: () => import('../silde-bar/silde-bar.module').then(m => m.SildeBarModule)
      },
      {
        path: 'cho',
        canActivate: [AuthGuard],
        component: DashBoardComponent,data: {animation: 'DashBoard'}
      },
      {
        path: 'questions',
        canActivate: [AuthGuard],
        loadChildren: () => import('../questions/questions.module').then(m => m.QuestionsModule)
      },
      {
        path: 'setting',
        canActivate: [AuthGuard],
        loadChildren: () => import('../setting/setting.module').then(m => m.SettingModule)
      },
      {
        path: 'report',
        canActivate: [AuthGuard],
        loadChildren: () => import('../report/report.module').then(m => m.ReportModule)
      },
      {
        path: 'dash-board',
        canActivate: [AuthGuard],
        loadChildren: () => import('../dash-board/dash-board.module').then(m => m.DashBoardModule)
      },
      {
        path: 'bieu-mau',
        canActivate: [AuthGuard],
        loadChildren: () => import('../template/template.module').then(m => m.TemplateModule)
      },
      {
        path: 'lich-su',
        canActivate: [AuthGuard],
        loadChildren: () => import('../history/history.module').then(m => m.HistoryModule)
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class LayoutRoutingModule { }