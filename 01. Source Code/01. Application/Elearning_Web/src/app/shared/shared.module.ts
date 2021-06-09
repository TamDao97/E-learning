import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { NgbDateParserFormatter, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';

import {
    TruncatePipe, FilterInListPipe, FilterBadgeClassInListPipe, FilterTextClassInListPipe,
    NtsNumberIntDirective, NtsLocationDirective, DateUtils, AppSetting, UisectionboxDirective, SafeHtmlPipe
} from '../shared';
import { UipermissionDirective, DiablePermissionDirective } from '../shared';
import { NtscurrencyDirective } from './directives/ntscurrency.directive';
import { NtscurrencyPipe } from './pipe/ntscurrency.pipe';
import { NtsNumberDirective } from './directives/ntsnumber.directive';
import { PagingComponent } from './paging/paging.component';
import {
    NgbDateVNParserFormatter, MessageconfirmComponent, MessageComponent,
} from '../shared';

import { MessageconfirmcodeComponent } from './component/messageconfirmcode/messageconfirmcode.component';
import { NtsStatusBadgeComponent } from './component/nts-status-badge/nts-status-badge.component';
import { NtsStatusComponent } from './component/nts-status/nts-status.component';
import { ImportExcelComponent } from './component/import-excel/import-excel.component';
import { NtsCheckboxComponent } from './component/nts-checkbox/nts-checkbox.component';
import { SwitchLanguageComponent } from './component/switch-language/switch-language.component';
import { NTSSearchBarComponent } from './component/nts-search-bar/nts-search-bar.component';
import { NtsTinymceComponent } from './tinymce/nts-tinymce.component';
import { NzTreeSelectModule } from 'ng-zorro-antd/tree-select';
import { SplitPipe } from './pipe/split.pipe';
import { TimePipe } from './pipe/time.pipe';

@NgModule({
    declarations: [
        TruncatePipe,
        FilterInListPipe,
        FilterBadgeClassInListPipe,
        FilterTextClassInListPipe,
        UipermissionDirective,
        DiablePermissionDirective,
        NtscurrencyDirective,
        NtscurrencyPipe,
        NtsNumberDirective,
        PagingComponent,
        MessageconfirmComponent,
        MessageComponent,
        NtsNumberIntDirective,
        NtsLocationDirective,
        UisectionboxDirective,
        MessageconfirmcodeComponent,
        NtsStatusBadgeComponent,
        NtsStatusComponent,
        ImportExcelComponent,
        SafeHtmlPipe,
        NtsCheckboxComponent,
        SwitchLanguageComponent,
        NTSSearchBarComponent,
        NtsTinymceComponent,
        SplitPipe,
        TimePipe,
    ],
    imports: [
        FormsModule,
        CommonModule,
        NgbModule,
        NgSelectModule,
        NzTreeSelectModule
    ],
    providers: [
        { provide: NgbDateParserFormatter, useClass: NgbDateVNParserFormatter }
    ],
    entryComponents: [
        MessageconfirmComponent,
        MessageComponent,
        MessageconfirmcodeComponent,
        ImportExcelComponent,
    ],
    exports: [
        TruncatePipe,
        FilterInListPipe,
        FilterBadgeClassInListPipe,
        FilterTextClassInListPipe,
        NtscurrencyPipe,
        UipermissionDirective,
        DiablePermissionDirective,
        NtscurrencyDirective,
        NtsNumberDirective,
        PagingComponent,
        MessageconfirmComponent,
        MessageComponent,
        NtsNumberIntDirective,
        NtsLocationDirective,
        UisectionboxDirective,
        DiablePermissionDirective,
        NtsStatusBadgeComponent,
        NtsStatusComponent,
        SafeHtmlPipe,
        NtsCheckboxComponent,
        SwitchLanguageComponent,
        NTSSearchBarComponent,
        NtsTinymceComponent,
        SplitPipe,
        TimePipe
    ]
})

export class SharedModule {
}