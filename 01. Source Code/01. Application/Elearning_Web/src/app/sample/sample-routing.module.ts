import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SampleManageComponent } from './sample-manage/sample-manage.component';

const routes: Routes = [
  {
    path: 'manage',
    component: SampleManageComponent,data: {animation: 'SampleManage'}
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SampleRoutingModule { }
