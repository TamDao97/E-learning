import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TemplateCetificateCreateComponent } from './template-cetificate-create/template-cetificate-create.component';
import { TemplateCetificateManagerComponent } from './template-cetificate-manager/template-cetificate-manager.component';
import { TemplateCommonManagerComponent } from './template-common-manager/template-common-manager.component';

const routes: Routes = [
  {
    path: 'mau-chung-chi',
    component: TemplateCetificateManagerComponent, data: { animation: 'TemplateCetificateManagerComponent' }
  },
  {
    path: 'mau-van-ban',
    component: TemplateCommonManagerComponent, data: { animation: 'TemplateCommonManagerComponent' }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TemplateRoutingModule { }
