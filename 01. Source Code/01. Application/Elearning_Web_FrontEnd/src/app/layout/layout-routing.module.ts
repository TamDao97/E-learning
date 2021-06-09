import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IndexComponent } from '../index/index.component';
import { MyCourseComponent } from '../my-course/my-course.component';
import { SearchResultComponent } from '../search/search-result/search-result.component';
import { LayoutComponent } from './layout.component';
import { LoginUserComponent } from './login-user/login-user.component';
import { RegistrationUserComponent } from './registration-user/registration-user.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: 'test',
        loadChildren: () => import('../sample/sample.module').then(m => m.SampleModule)
      },
      {
        path: 'my-course/lesson',
        loadChildren: () => import('../lesson/lesson.module').then(m => m.LessonModule)
      },
      {
        path: '',
        component: IndexComponent,
      },
      {
        path: 'my-course',
        component: MyCourseComponent,
      },
      {
        path: 'tim-kiem', component: SearchResultComponent,
      },
      {
        path: '',
        loadChildren: () => import('../course/course.module').then(m => m.CourseModule)
      },

    ]
  },
  {
    path: 'home',
    component: LayoutComponent,
  },
  {
    path: 'dang-nhap',
    component: LoginUserComponent,
  },
  {
    path: 'dang-ky',
    component: RegistrationUserComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
