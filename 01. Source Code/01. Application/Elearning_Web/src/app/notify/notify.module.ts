import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NotifyRoutingModule } from './notify-routing.module';
import { NotifyListComponent } from './notify-list/notify-list.component';


@NgModule({
  declarations: [NotifyListComponent],
  imports: [
    CommonModule,
    NotifyRoutingModule
  ],
  exports:[
    NotifyListComponent
  ]
})
export class NotifyModule { }
