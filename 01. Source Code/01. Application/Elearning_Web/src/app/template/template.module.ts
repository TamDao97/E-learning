import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TemplateRoutingModule } from './template-routing.module';
import { TemplateCetificateManagerComponent } from './template-cetificate-manager/template-cetificate-manager.component';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { TemplateCetificateCreateComponent } from './template-cetificate-create/template-cetificate-create.component';
import { FormsModule } from '@angular/forms';
import { TemplateCommonManagerComponent } from './template-common-manager/template-common-manager.component';
import { TemplateCommonCreateComponent } from './template-common-create/template-common-create.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [TemplateCetificateManagerComponent, TemplateCetificateCreateComponent, TemplateCommonManagerComponent, TemplateCommonCreateComponent],
  imports: [
    CommonModule,
    TemplateRoutingModule,
    PerfectScrollbarModule,
    NgbModule,
    FormsModule,
    SharedModule,
  ]
})
export class TemplateModule { }
