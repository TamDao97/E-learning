import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgSelectModule } from '@ng-select/ng-select';

import { SharedModule } from "../shared/shared.module";

import { SampleRoutingModule } from './sample-routing.module';
import { SampleManageComponent } from './sample-manage/sample-manage.component';
import { SampleCreateComponent } from './sample-create/sample-create.component';


@NgModule({
  declarations: [SampleManageComponent, SampleCreateComponent],
  imports: [
    CommonModule,
    FormsModule,
    SampleRoutingModule,
    NgbModule,
    PerfectScrollbarModule,
    NgSelectModule,
    SharedModule
  ]
})
export class SampleModule { }
