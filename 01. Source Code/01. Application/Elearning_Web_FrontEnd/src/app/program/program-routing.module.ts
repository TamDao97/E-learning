import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProgramDetailsComponent } from './program-details/program-details.component';

const routes: Routes = [

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProgramRoutingModule { }
