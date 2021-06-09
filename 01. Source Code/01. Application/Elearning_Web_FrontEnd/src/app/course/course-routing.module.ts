import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from '../not-found/not-found.component';
import { ProgramDetailsComponent } from '../program/program-details/program-details.component';
import { CourseDetailsComponent } from './course-details/course-details.component';

const routes: Routes = [
  {
    path: 'chi-tiet-khoa-hoc/:slug',
    component: CourseDetailsComponent,
  },
  {
    path: 'chi-tiet-chuong-trinh/:slug',
    component: ProgramDetailsComponent,
  },
  {
    path: '404',
    component: NotFoundComponent,
  },
  {
    path: '**',redirectTo:'404'
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CourseRoutingModule { }
