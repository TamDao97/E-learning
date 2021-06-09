import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedModule } from "../shared/shared.module";
import { UsersRoutingModule } from './users-routing.module';
import { UserAdminMangeComponent } from './user-admin/user-admin-mange/user-admin-mange.component';
import { UserExpertManageComponent } from './user-expert-manage/user-expert-manage.component';
import { UserInstructorManageComponent } from './user-instructor-manage/user-instructor-manage.component';
import { UserCreateComponent } from './user-create/user-create.component';
import { RefreshPasswordComponent } from './refresh-password/refresh-password.component';
import { ReactiveFormsModule } from '@angular/forms';
import { UserStudentManagerComponent } from './user-student-manager/user-student-manager.component';
import { ShowUserComponent } from './show-user/show-user.component';
import { ShowUserStudentComponent } from './show-user-student/show-user-student.component';
import { UpdateUserComponent } from './update-user/update-user.component';

@NgModule({
  declarations: [
    UserAdminMangeComponent,
    UserExpertManageComponent,
    UserInstructorManageComponent,
    UserCreateComponent,
    RefreshPasswordComponent,
    UserStudentManagerComponent,
    ShowUserComponent,
    ShowUserStudentComponent,
    UpdateUserComponent,
  ],
  imports: [
    CommonModule,
    UsersRoutingModule,
    FormsModule,
    NgbModule,
    PerfectScrollbarModule,
    NgSelectModule,
    SharedModule,
    ReactiveFormsModule
  ],
  exports: [

  ]
})
export class UsersModule { }
