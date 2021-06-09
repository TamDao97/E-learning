import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LessonManageComponent } from './lesson/lesson-manage/lesson-manage.component';
import { ProgramManageComponent } from './program/program-manage/program-manage.component';
import { CourseManageComponent } from '../education-programs/course-manage/course-manage.component';
import { MentorManageComponent } from './course/mentor-manage/mentor-manage.component';
import { TabContentComponent } from './tab-content/tab-content.component';

import { LearnerManageComponent } from './course/learner-manage/learner-manage.component';
import { CategoryManageComponent } from './category/category-manage/category-manage.component';
import { ProgressManageComponent } from './progress-manage/progress-manage.component';
import { CommentManageComponent } from './comment/comment-manage/comment-manage.component';
import { EmployeeSpecialistManageComponent } from './employee-specialist/employee-specialist-manage/employee-specialist-manage.component';
import { EmployeeSpecialistCreateComponent } from './employee-specialist/employee-specialist-create/employee-specialist-create.component';
import { LessonCreateComponent } from './lesson/lesson-create/lesson-create.component';
const routes: Routes = [
  {
    path: 'chuong-trinh-dao-tao',
    component: ProgramManageComponent, data: { animation: 'ProgramManage' }
  },
  {
    path: 'khoa-hoc',
    component: CourseManageComponent, data: { animation: 'CourseManage' }
  },
  {
    path: 'huong-dan-vien',
    component: MentorManageComponent, data: { animation: 'MentorManage' }
  },
  {
    path: 'quan-ly-bai-giang',
    component: LessonManageComponent, data: { animation: 'LessonManage' }
  },
  {
    path: 'noi-dung',
    component: TabContentComponent, data: { animation: 'TabContent' }
  },
  {
    path: 'nguoi-hoc',
    component: LearnerManageComponent, data: { animation: 'LearnerManage' }
  },
  {
    path: 'quan-ly-danh-muc',
    component: CategoryManageComponent, data: { animation: 'CategoryManage' }
  },
  {
    path: 'quan-ly-tien-do',
    component: ProgressManageComponent, data: { animation: 'ProgressManage' }
  },
  {
    path: 'quan-ly-binh-luan',
    component: CommentManageComponent, data: { animation: 'CommentManage' }
  },
  {
    path: 'cau-hinh-chuyen-gia',
    component: EmployeeSpecialistManageComponent, data: { animation: 'EmployeeSpecialist' }
  },
  {
    path: 'quan-ly-bai-giang/them-moi',
    component: LessonCreateComponent, data: { animation: 'LessonCreate' }
  },
  {
    path: 'quan-ly-bai-giang/chinh-sua/:id',
    component: LessonCreateComponent, data: { animation: 'LessonCreate' }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EducationProgramsRoutingModule { }
