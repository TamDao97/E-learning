import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HistoryFontendComponent } from './history-fontend/history-fontend.component';
import { HistoryManageComponent } from './history-manage/history-manage.component';

const routes: Routes = [
  {
    path: 'quan-tri',
    component: HistoryManageComponent, data: { animation: 'HistoryManage' }
  },
  {
    path: 'nguoi-hoc',
    component: HistoryFontendComponent, data: { animation: 'HistoryFontend' }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HistoryRoutingModule { }
