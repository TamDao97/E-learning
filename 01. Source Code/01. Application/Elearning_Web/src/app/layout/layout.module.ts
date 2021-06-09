import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgSelectModule } from '@ng-select/ng-select';


import { LayoutComponent } from './layout.component';
import { LayoutRoutingModule } from './layout-routing.routing';
import { SharedModule } from '../shared/shared.module';
import { TopbarComponent } from './topbar/topbar.component';
import { LeftbarComponent } from './leftbar/leftbar.component';
import { NavbarComponent } from './navbar/navbar.component';
import { NavCollapsableComponent } from './collapsable/collapsable.component';
import { NavItemComponent } from './item/item.component';
import { NtsNavigationService } from './navigation/navigation.service';
import { ScreenWaitComponent } from './screen-wait/screen-wait.component';
import { NotifyModule } from '../notify/notify.module';
import { FooterComponent } from './footer/footer.component';
import { PageModule } from '../page/page.module';

@NgModule({
    declarations: [
        LayoutComponent,
        TopbarComponent,
        LeftbarComponent,
        NavbarComponent,
        NavCollapsableComponent,
        NavItemComponent,
        ScreenWaitComponent,
        FooterComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        LayoutRoutingModule,
        SharedModule,
        NgbModule,
        PerfectScrollbarModule,
        NotifyModule,
        PageModule
    ],
    providers: [
    ],
    entryComponents: [],
})

export class LayoutModule {
}