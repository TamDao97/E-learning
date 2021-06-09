import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommentFontEndComponent } from './comment-font-end/comment-font-end.component';
import { TestComponent } from './test/test.component';

const routes: Routes = [
  {
    path: 'bai-giang-trac-nghiem/:id',
    component: TestComponent, data: { animation: 'TestManage' }
  },
  {
    path: 'bai-giang-ly-thuyet/:slug',
    component: CommentFontEndComponent, data: { index:'index', animation: 'CommentFontEnd' }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LessonRoutingModule { }
