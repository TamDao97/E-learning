import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ColorPickerModule } from 'ngx-color-picker';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgSelectModule } from '@ng-select/ng-select';

import { SharedModule } from "../shared/shared.module";


import { PageRoutingModule } from './page-routing.module';
import { AboutComponent } from './about/about.component';
import { ThemeComponent } from './theme/theme.component';


@NgModule({
  declarations: [AboutComponent, ThemeComponent],
  imports: [
    CommonModule,
    FormsModule,
    PageRoutingModule,
    ColorPickerModule,
    PerfectScrollbarModule
  ],
  exports:[
    ThemeComponent
  ]
})
export class PageModule { }
