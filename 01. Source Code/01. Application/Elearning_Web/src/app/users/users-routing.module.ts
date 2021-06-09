import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ShowUserStudentComponent } from './show-user-student/show-user-student.component';
import { ShowUserComponent } from './show-user/show-user.component';
import { UserAdminMangeComponent } from './user-admin/user-admin-mange/user-admin-mange.component';
import { UserCreateComponent } from './user-create/user-create.component';
import { UserExpertManageComponent } from './user-expert-manage/user-expert-manage.component';
import { UserInstructorManageComponent } from './user-instructor-manage/user-instructor-manage.component';
import { UserStudentManagerComponent } from './user-student-manager/user-student-manager.component';

const routes: Routes = [
  {
    path: 'quan-tri-he-thong',
    component: UserAdminMangeComponent,data: {animation: 'UserAdminMange'}
  },
  {
    path: 'chuyen-gia',
    component: UserExpertManageComponent,data: {animation: 'UserExpertManage'}
  },
  {
    path: 'nguoi-huong-dan',
    component: UserInstructorManageComponent,data: {animation: 'UserInstructorManage'}
  },
  {
    path: 'nguoi-dung',
    component: UserStudentManagerComponent,data: {animation: 'UserStudentManager'}
  },
  {
    path: 'quan-tri-he-thong/them-moi/:type',
    component: UserCreateComponent,data: {animation: 'UserCreate'}
  },
  {
    path: 'quan-tri-he-thong/cap-nhat/:id/:type',
    component: UserCreateComponent,data: {animation: 'UserCreate'}
  },

  {
    path: 'chuyen-gia/them-moi/:type',
    component: UserCreateComponent,data: {animation: 'UserCreate'}
  },
  {
    path: 'chuyen-gia/cap-nhat/:id/:type',
    component: UserCreateComponent,data: {animation: 'UserCreate'}
  },

  {
    path: 'nguoi-huong-dan/them-moi/:type',
    component: UserCreateComponent,data: {animation: 'UserCreate'}
  },
  {
    path: 'nguoi-huong-dan/cap-nhat/:id/:type',
    component: UserCreateComponent,data: {animation: 'UserCreate'}
  },

  {
    path: 'quan-tri-he-thong/xem-thong-tin/:id/:type',
    component: ShowUserComponent,data: {animation: 'ShowUser'}
  },
  {
    path: 'chuyen-gia/xem-thong-tin/:id/:type',
    component: ShowUserComponent,data: {animation: 'ShowUser'}
  },
  {
    path: 'nguoi-huong-dan/xem-thong-tin/:id/:type',
    component: ShowUserComponent,data: {animation: 'ShowUser'}
  },
  {
    path: 'nguoi-dung/xem-thong-tin/:id',
    component: ShowUserStudentComponent,data: {animation: 'ShowUserStuden'}
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersRoutingModule { }
