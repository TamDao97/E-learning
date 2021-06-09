import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuestionsCreateComponent } from './questions-create/questions-create.component';
import { QuestionsManagerComponent } from './questions-manager/questions-manager.component';

const routes: Routes = [
  {
    path: 'questions-manager',
    component: QuestionsManagerComponent
  },
  {
    path: 'questions-create',
    component: QuestionsCreateComponent
  },
  {
    path: 'questions-edit/:id',
    component: QuestionsCreateComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class QuestionsRoutingModule { }
