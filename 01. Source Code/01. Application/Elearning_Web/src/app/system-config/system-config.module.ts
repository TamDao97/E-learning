import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SystemConfigRoutingModule } from './system-config-routing.module';
import { ProgramComponent } from './program/program.component';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [ProgramComponent],
  imports: [
    CommonModule,
    FormsModule,
    PerfectScrollbarModule,
    SystemConfigRoutingModule
  ]
})
export class SystemConfigModule { }
