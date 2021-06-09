import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeSettingManageComponent } from './home-setting/home-setting-manage/home-setting-manage.component';
import { HomeServiceManageComponent } from './home-service/home-service-manage/home-service-manage.component';
import { HomeLinkManageComponent } from './home-link/home-link-manage/home-link-manage.component';
import { ManageUnitManageComponent } from './manage-unit/manage-unit-manage/manage-unit-manage.component';
import { GroupUserPermissionComponent } from './group-user-permision/group-user-permission/group-user-permission.component';
import { SystemParamManageComponent } from './system-param/system-param-manage/system-param-manage.component';
const routes: Routes = [
  {
    path: 'cau-hinh-thong-so-he-thong',
    component: SystemParamManageComponent, data: { animation: 'SystemParamManage' }
  },
  {
    path: 'cai-dat-trang-chu',
    component: HomeSettingManageComponent, data: { animation: 'HomeSetting' }
  },
  {
    path: 'quan-ly-loi-tua',
    component: HomeServiceManageComponent, data: { animation: 'HomeService' }
  },
  {
    path: 'quan-ly-trang-lien-ket',
    component: HomeLinkManageComponent, data: { animation: 'HomeLinkService' }
  },
  {
    path: 'quan-ly-don-vi-chu-quan',
    component: ManageUnitManageComponent, data: { animation: 'ManageUnitService' }
  },
  {
    path: 'cau-hinh-nhom-quyen',
    component: GroupUserPermissionComponent, data: { animation: 'ManageUnitService' }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingRoutingModule { }
