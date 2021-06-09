import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import {  AppSetting } from '../shared';
import { slideInAnimation } from './animations/router.animations';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
  animations: [
    slideInAnimation
    // animation triggers go here
  ]
})
export class LayoutComponent implements OnInit {

  constructor(
    public appSetting:AppSetting) {

  }

  ngOnInit() {
  }

  prepareRoute(outlet: RouterOutlet) {
    return outlet && outlet.activatedRouteData && outlet.activatedRouteData.animation;
  }

}
