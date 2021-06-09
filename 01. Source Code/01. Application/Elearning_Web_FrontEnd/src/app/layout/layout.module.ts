import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LayoutRoutingModule } from './layout-routing.module';
import { LayoutComponent } from './layout.component';
import { CarouselModule } from 'ngx-owl-carousel-o';
import { NgbDateParserFormatter, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FooterComponent } from './footer/footer.component';
import { TopbarComponent } from './topbar/topbar.component';
import { HomeProgramComponent } from '../home-program/home-program.component';
import { MyCourseComponent } from '../my-course/my-course.component';
import { HomeServiceComponent } from '../home-service/home-service.component';
import { HomeSilderComponent } from '../home-silder/home-silder/home-silder.component';
import { HomeExpertComponent } from '../home-expert/home-expert.component';
import { LoginUserComponent } from './login-user/login-user.component';
import { RegistrationUserComponent } from './registration-user/registration-user.component';
import { IndexComponent } from '../index/index.component';
import { UpdateUserComponent } from './update-user/update-user.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { TruncatePipe } from '../share/pipe/truncate.pipe';
import { NgbDateVNParserFormatter } from '../share/common/ngb-date-vn-parser-formatter';
import { RegisterCourseComponent } from './register-course/register-course.component';
import { ShowErroUserComponent } from './show-erro-user/show-erro-user.component';
import { MessageComponent } from '../share/component/message/message.component';
import { SafeHtmlPipe } from '../share/pipe/safe-html.pipe';
import { LockUserComponent } from '../lock-user/lock-user.component';
import { NtsNumberIntDirective } from '../share/directives/nts-number-int.directive';
import { ChangePasswordComponent } from './change-password/change-password/change-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password/reset-password.component';
import { SearchComponent } from '../search/search/search.component';
import { SearchResultComponent } from '../search/search-result/search-result.component';
@NgModule({
  declarations: [
    LayoutComponent,
    HomeSilderComponent,
    HomeServiceComponent,
    FooterComponent,
    TopbarComponent,
    HomeProgramComponent,
    LoginUserComponent,
    RegistrationUserComponent,
    HomeExpertComponent,
    IndexComponent,
    UpdateUserComponent,
    TruncatePipe,
    MyCourseComponent,
    RegisterCourseComponent,
    ShowErroUserComponent,
    MessageComponent,
    SafeHtmlPipe,
    LockUserComponent,
    NtsNumberIntDirective,
    ChangePasswordComponent,
    ResetPasswordComponent,
    SearchComponent,
    SearchResultComponent
  ],
  imports: [
    CommonModule,
    LayoutRoutingModule,
    CarouselModule,
    NgbModule,
    FormsModule,
    NgSelectModule,
    ReactiveFormsModule,
  ],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateVNParserFormatter },
  ],
  entryComponents: [
    UpdateUserComponent,
    LockUserComponent
  ],
  exports: [
    TruncatePipe,
    SafeHtmlPipe,
    NtsNumberIntDirective
  ]
})
export class LayoutModule { }
