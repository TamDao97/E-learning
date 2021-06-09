import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./layout/layout.module').then(m => m.LayoutModule)
  },
  { path: 'silde-bar-manage', loadChildren: () => import('./silde-bar/silde-bar.module').then(m => m.SildeBarModule) },
  { path: 'silde-bar-manage', loadChildren: () => import('./silde-bar/silde-bar.module').then(m => m.SildeBarModule) },
  { path: 'silde-bar/silde-bar-manage', loadChildren: () => import('./silde-bar/silde-bar.module').then(m => m.SildeBarModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {useHash: true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
