import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


import { ToastrModule } from 'ngx-toastr';
import { BlockUIModule } from 'ng-block-ui';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { JwtInterceptor } from './auth/helpers';
import { Configuration, SafeHtmlPipe } from './shared';
import { SildeBarModule } from './silde-bar/silde-bar.module';
import { registerLocaleData } from '@angular/common';
import vi from '@angular/common/locales/vi';
registerLocaleData(vi);
import { NZ_I18N, vi_VN } from 'ng-zorro-antd/i18n';

export function initializeApp(appConfig: Configuration) {
  return () => appConfig.load();
}

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BlockUIModule.forRoot({
      delayStart: 1000,
    }),
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 2000,
      closeButton: true
    }),

    SildeBarModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    },
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [Configuration],
      multi: true
    },
    { provide: NZ_I18N, useValue: vi_VN }
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
