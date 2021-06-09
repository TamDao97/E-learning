import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HistoryRoutingModule } from './history-routing.module';
import { HistoryManageComponent } from './history-manage/history-manage.component';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { SharedModule } from '../shared/shared.module';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { VirtualScrollerModule } from 'ngx-virtual-scroller';
import { HistoryFontendComponent } from './history-fontend/history-fontend.component';

@NgModule({
  declarations: [HistoryManageComponent, HistoryFontendComponent],
  imports: [
    CommonModule,
    HistoryRoutingModule,
    FormsModule,
    NgbModule,
    PerfectScrollbarModule,
    VirtualScrollerModule,
    NgSelectModule,
    SharedModule,
  ]
})
export class HistoryModule { }
