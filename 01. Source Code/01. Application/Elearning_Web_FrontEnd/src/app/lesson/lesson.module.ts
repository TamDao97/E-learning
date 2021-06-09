import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LessonRoutingModule } from './lesson-routing.module';
import { TestComponent } from './test/test.component';
import { FormsModule } from '@angular/forms';
import { CommentFontEndComponent } from './comment-font-end/comment-font-end.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LayoutModule } from '../layout/layout.module';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { from } from 'rxjs';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CarouselModule } from 'ngx-owl-carousel-o';
import { EstimatedPipe } from '../share/pipe/estimated.pipe';
import { CountdownComponent, CountdownModule } from 'ngx-countdown';
import { TimePipe } from '../share/pipe/time.pipe';


@NgModule({
  declarations: [
    TestComponent,
    CommentFontEndComponent,
    EstimatedPipe,
    TimePipe,
  ],
  imports: [
    CommonModule,
    LessonRoutingModule,
    FormsModule,
    CountdownModule,
    NgbModule,
    NgSelectModule,
    LayoutModule,
    DragDropModule,
    CarouselModule,
  ],
  exports:[
    CountdownComponent,
    EstimatedPipe,
    TimePipe,
  ]
})
export class LessonModule { }
