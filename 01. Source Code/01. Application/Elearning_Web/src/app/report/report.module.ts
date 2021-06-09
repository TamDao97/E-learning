import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportRoutingModule } from './report-routing.module';
import { ReportLearnerComponent } from './report-learner/report-learner.component';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { SharedModule } from '../shared/shared.module';
import { ChartsModule } from 'ng2-charts';
import { ReportLearnerProvinceComponent } from './report-learner-province/report-learner-province.component';
import { StatisticalCompleteCourseComponent } from './statistical-complete-course/statistical-complete-course.component';
import { StatisticalNumberSubscribersComponent } from './statistical-number-subscribers/statistical-number-subscribers.component';


@NgModule({
  declarations: [
    ReportLearnerComponent, 
    ReportLearnerProvinceComponent,
    StatisticalCompleteCourseComponent,
    StatisticalNumberSubscribersComponent,
  ],
  imports: [
    CommonModule,
    ReportRoutingModule,
    FormsModule,
    NgbModule,
    PerfectScrollbarModule,
    NgSelectModule,
    SharedModule,
    ChartsModule
  ]
})
export class ReportModule { }
