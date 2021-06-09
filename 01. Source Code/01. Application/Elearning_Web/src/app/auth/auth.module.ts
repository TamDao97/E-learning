import { ChangePasswordComponent } from './change-password/change-password.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthRoutingModule } from './auth-routing.routing';
import { LoginComponent } from './login/login.component';
import { BlockUIModule } from 'ng-block-ui';
import { SharedModule } from '../shared/shared.module';
import { NgxCaptchaModule } from 'ngx-captcha';
import { DeviceDetectorService } from 'ngx-device-detector';
import { AuthenticationService } from './services';

@NgModule({
    declarations: [
        LoginComponent,
        ChangePasswordComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        AuthRoutingModule,
        BlockUIModule,
        SharedModule,
        ReactiveFormsModule,
    ],
    providers: [
    ],
    entryComponents: [ChangePasswordComponent],
})

export class AuthModule {
}