import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SildeBarRoutingModule } from './silde-bar-routing.module';
import { SildeBarManageComponent } from './silde-bar-manage/silde-bar-manage.component';
import { SildeBarCreateComponent } from './silde-bar-create/silde-bar-create.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { SharedModule } from '../shared/shared.module';
import { DragDropModule } from '@angular/cdk/drag-drop';


@NgModule({
  declarations: [SildeBarManageComponent, SildeBarCreateComponent],
  imports: [
    CommonModule,
    SildeBarRoutingModule,
    CommonModule,
    FormsModule,
    NgbModule,
    PerfectScrollbarModule,
    NgSelectModule,
    SharedModule,
    ReactiveFormsModule,
    DragDropModule

  ]
})
export class SildeBarModule { }
