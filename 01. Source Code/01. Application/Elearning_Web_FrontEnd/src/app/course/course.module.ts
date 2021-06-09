import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CourseRoutingModule } from './course-routing.module';
import { CourseDetailsComponent } from './course-details/course-details.component';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { CountdownModule } from 'ngx-countdown';
import { LayoutModule } from '../layout/layout.module';
import { NotFoundComponent } from '../not-found/not-found.component';
import { ProgramDetailsComponent } from '../program/program-details/program-details.component';

@NgModule({
  declarations: [
    CourseDetailsComponent,
    NotFoundComponent,
    ProgramDetailsComponent
  ],
  imports: [
    CommonModule,
    CourseRoutingModule,
    FormsModule,
    CountdownModule,
    NgbModule,
    NgSelectModule,
    LayoutModule
  ],
})
export class CourseModule { }
