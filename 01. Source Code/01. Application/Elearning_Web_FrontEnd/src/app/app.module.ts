import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { BlockUIModule } from 'ng-block-ui';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { Configuration } from './share/config/configuration';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { MessageComponent } from './share/component/message/message.component';
import { LockUserComponent } from './lock-user/lock-user.component';
// import { DragDropModule } from '@angular/cdk/drag-drop';

import { SocialLoginModule, SocialAuthServiceConfig, FacebookLoginProvider } from 'angularx-social-login';
import { GoogleLoginProvider } from 'angularx-social-login';
import { CountdownModule } from 'ngx-countdown';

export function initializeApp(appConfig: Configuration) {
  return () => appConfig.load();
}




@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    CountdownModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    SocialLoginModule,
    BlockUIModule.forRoot({
      delayStart: 1000,
    }),
    ToastrModule.forRoot(
      {
        timeOut: 5000,
        positionClass: 'toast-top-right',
        preventDuplicates: true,
      }
    ),

  ],
  providers: [
    {
      deps: [Configuration],
      multi: true,
      useFactory: initializeApp,
      provide: APP_INITIALIZER,
    },
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(
              '322459588507-dchbdrd4maak9mssu8uuopllv0cbvj3b.apps.googleusercontent.com'
              // '322459588507-7q565hn86apcp63lcm2g9qdkbni6m8lc.apps.googleusercontent.com'
            )
          },
          {
            id: FacebookLoginProvider.PROVIDER_ID,
            provider: new FacebookLoginProvider(
              // '322459588507-dchbdrd4maak9mssu8uuopllv0cbvj3b.apps.googleusercontent.com'
              '503214120847401'
            )
          }
        ]
      } as SocialAuthServiceConfig,
    } 
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    // MessageComponent
  ]
})
export class AppModule { }
