import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProgramRoutingModule } from './program-routing.module';
import { ProgramDetailsComponent } from './program-details/program-details.component';
import { LayoutModule } from '../layout/layout.module';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';

@NgModule({
  declarations: [
    // ProgramDetailsComponent
  ],
  imports: [
    CommonModule,
    // ProgramRoutingModule,
    FormsModule,
    NgbModule,
    NgSelectModule,
    LayoutModule,

  ]
})
export class ProgramModule { }
 