import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SildeBarManageComponent } from './silde-bar-manage/silde-bar-manage.component';

const routes: Routes = [
  {
    path: 'silde-bar',
    component: SildeBarManageComponent,data: {animation: 'SildeBarManage'}
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SildeBarRoutingModule { }
