import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ReportLearnerProvinceComponent } from './report-learner-province/report-learner-province.component';
import { ReportLearnerComponent } from './report-learner/report-learner.component';
import { StatisticalCompleteCourseComponent } from './statistical-complete-course/statistical-complete-course.component';
import { StatisticalNumberSubscribersComponent } from './statistical-number-subscribers/statistical-number-subscribers.component';

const routes: Routes = [
  {
    path: 'thong-ke-nguoi-hoc',
    component: ReportLearnerComponent, data: { animation: 'ReportLearner' }
  },
  {
    path: 'thong-ke-thong-tin-nguoi-hoc',
    component: ReportLearnerProvinceComponent, data: { animation: 'ReportLearnerProvince' }
  },
  {
    path: 'thong-ke-so-luong-hoan-thanh-khoa-hoc',
    component: StatisticalCompleteCourseComponent, data: { animation: 'CompleteCourse' }
  },
  {
    path: 'thong-ke-so-dang-ky-khoa-hoc',
    component: StatisticalNumberSubscribersComponent, data: { animation: 'NumberSubscribers' }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
