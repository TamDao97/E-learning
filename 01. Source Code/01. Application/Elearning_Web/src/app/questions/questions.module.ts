import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuestionsRoutingModule } from './questions-routing.module';
import { QuestionsManagerComponent } from './questions-manager/questions-manager.component';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { SharedModule } from '../shared/shared.module';
import { NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { QuestionsCreateComponent } from './questions-create/questions-create.component';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTreeSelectModule } from 'ng-zorro-antd/tree-select';
import { TopicCreateComponent } from './topic-create/topic-create.component';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { QuestionHistoryComponent } from './question-history/question-history.component';

@NgModule({
  declarations: [
    QuestionsManagerComponent,
    QuestionsCreateComponent,
    TopicCreateComponent,
    QuestionHistoryComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgbModule,
    QuestionsRoutingModule,
    PerfectScrollbarModule,
    SharedModule,
    NgbPaginationModule,
    NzButtonModule,
    NzTreeSelectModule,
    NzToolTipModule
  ],
})
export class QuestionsModule {}
