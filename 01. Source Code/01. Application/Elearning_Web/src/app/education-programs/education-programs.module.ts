import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';

import { NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { VirtualScrollerModule } from 'ngx-virtual-scroller';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxMaskModule, IConfig } from 'ngx-mask'

import { SharedModule } from "../shared/shared.module";

import { EducationProgramsRoutingModule } from './education-programs-routing.module';
import { ProgramManageComponent } from './program/program-manage/program-manage.component';
import { ProgramCreateComponent } from './program/program-create/program-create.component';
import { MentorManageComponent } from './course/mentor-manage/mentor-manage.component';
import { MentorCreateComponent } from './course/mentor-create/mentor-create.component';
import { LessonManageComponent } from './lesson/lesson-manage/lesson-manage.component';
import { LessonCreateComponent } from './lesson/lesson-create/lesson-create.component';
import { CourseManageComponent } from './course-manage/course-manage.component';
import { CourseCreateComponent } from './course-create/course-create.component';
import { ChooseQuestionComponent } from './lesson/choose-question/choose-question.component';
import { ChooseQuestionRandomComponent } from './lesson/choose-question-random/choose-question-random.component';
import { from } from 'rxjs';
import { TabContentComponent } from './tab-content/tab-content.component';
import { ChooseTabContentComponent } from './choose-tab-content/choose-tab-content.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { LearnerManageComponent } from './course/learner-manage/learner-manage.component';
import { LearnerCreateComponent } from './course/learner-create/learner-create.component';
import { CategoryManageComponent } from './category/category-manage/category-manage.component';
import { CategoryCreateComponent } from './category/category-create/category-create.component';
import { ProgressManageComponent } from './progress-manage/progress-manage.component';
import { CommentManageComponent } from './comment/comment-manage/comment-manage.component';
import { CommentCreateComponent } from './comment/comment-create/comment-create.component';
import { EmployeeSpecialistManageComponent } from './employee-specialist/employee-specialist-manage/employee-specialist-manage.component';
import { EmployeeSpecialistCreateComponent } from './employee-specialist/employee-specialist-create/employee-specialist-create.component';
import { ChooseEmployeeComponent } from './employee-specialist/choose-employee/choose-employee.component';
import { TestResultComponent } from './test-result/test-result.component';
import { ShowQuestionComponent } from './lesson/show-question/show-question.component';
//import { EditorModule } from '@tinymce/tinymce-angular';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTreeSelectModule } from 'ng-zorro-antd/tree-select';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { DetailsResultComponent } from './details-result/details-result.component';
import { CourseStatusComponent } from './course/course-status/course-status.component';
import { PreviewCourseComponent } from './course/preview-course/preview-course.component';
import { PreviewLessonComponent } from './course/preview-lesson/preview-lesson.component';
import { ViewLessonComponent } from './lesson/view-lesson/view-lesson.component';
import { LessonHistoryComponent } from './lesson/lesson-history/lesson-history.component';
import { LessonFrameTypeComponent } from './lesson/lesson-frame-type/lesson-frame-type.component';
import { LessonFrameCreateComponent } from './lesson/lesson-frame-create/lesson-frame-create.component';
export const options: Partial<IConfig> | (() => Partial<IConfig>) = null;

@NgModule({
  declarations: [
    ProgramManageComponent,
    ProgramCreateComponent,
    LessonManageComponent,
    LessonCreateComponent,
    MentorManageComponent,
    MentorCreateComponent,
    CourseCreateComponent,
    CourseManageComponent,
    ChooseQuestionComponent,
    ChooseQuestionRandomComponent,
    TabContentComponent,
    ChooseTabContentComponent,
    LearnerManageComponent,
    LearnerCreateComponent,
    CategoryManageComponent,
    CategoryCreateComponent,
    ProgressManageComponent,
    CommentManageComponent,
    CommentCreateComponent,
    EmployeeSpecialistManageComponent,
    EmployeeSpecialistCreateComponent,
    ChooseEmployeeComponent,
    TestResultComponent,
    ShowQuestionComponent,
    DetailsResultComponent,
    CourseStatusComponent,
    PreviewCourseComponent,
    PreviewLessonComponent,
    ViewLessonComponent,
    LessonHistoryComponent,
    LessonFrameTypeComponent,
    LessonFrameCreateComponent,
  ],

  imports: [
    CommonModule,
    EducationProgramsRoutingModule,
    FormsModule,
    NgbModule,
    PerfectScrollbarModule,
    VirtualScrollerModule,
    NgSelectModule,
    SharedModule,
    DragDropModule,
    NzButtonModule,
    NzTreeSelectModule,
    NzToolTipModule,
    NgbPaginationModule,
    NgxMaskModule.forRoot(),
  ],
})
export class EducationProgramsModule { }
