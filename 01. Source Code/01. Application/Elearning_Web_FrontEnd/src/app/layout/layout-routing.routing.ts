import { NgModule } from '@angular/core';
import { ExtraOptions, Routes, RouterModule } from '@angular/router';

//import { AuthGuard } from '../auth/guards/auth.guard';
import { LayoutComponent } from './layout.component';
import { IndexComponent } from '../index/index.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    //canActivate: [AuthGuard],
    children: [
      {
        path: 'test',
        //canActivate: [AuthGuard],
        loadChildren: () => import('../sample/sample.module').then(m => m.SampleModule)
      },
      {
        path: '',
        component: IndexComponent,
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class LayoutRoutingModule { }