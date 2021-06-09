import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedModule } from "../shared/shared.module";
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { VirtualScrollerModule } from 'ngx-virtual-scroller';
import { NgSelectModule } from '@ng-select/ng-select';
import { SettingRoutingModule } from './setting-routing.module';
import { HomeSettingManageComponent } from './home-setting/home-setting-manage/home-setting-manage.component';
import { HomeSettingCreateComponent } from './home-setting/home-setting-create/home-setting-create.component';
import { HomeServiceManageComponent } from './home-service/home-service-manage/home-service-manage.component';
import { HomeServiceCreateComponent } from './home-service/home-service-create/home-service-create.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { HomeLinkManageComponent } from './home-link/home-link-manage/home-link-manage.component';
import { HomeLinkCreateComponent } from './home-link/home-link-create/home-link-create.component';
import { ManageUnitManageComponent } from './manage-unit/manage-unit-manage/manage-unit-manage.component';
import { ManageUnitCreateComponent } from './manage-unit/manage-unit-create/manage-unit-create.component';
import { GroupUserPermissionComponent } from './group-user-permision/group-user-permission/group-user-permission.component';
import { GroupUserPermissionCreateComponent } from './group-user-permision/group-user-permission-create/group-user-permission-create.component';
import { SystemParamManageComponent } from './system-param/system-param-manage/system-param-manage.component';
import { SystemParamCreateComponent } from './system-param/system-param-create/system-param-create.component';

@NgModule({
  declarations: [
    HomeSettingManageComponent,
    HomeSettingCreateComponent,
    HomeServiceManageComponent,
    HomeServiceCreateComponent,
    HomeLinkManageComponent,
    HomeLinkCreateComponent,
    ManageUnitManageComponent,
    ManageUnitCreateComponent,
    GroupUserPermissionComponent,
    GroupUserPermissionCreateComponent,
    SystemParamManageComponent,
    SystemParamCreateComponent
  ],
  imports: [
    CommonModule,
    SettingRoutingModule,
    FormsModule,
    NgbModule,
    PerfectScrollbarModule,
    VirtualScrollerModule,
    NgSelectModule,
    SharedModule,
    DragDropModule
  ]
})
export class SettingModule { }
