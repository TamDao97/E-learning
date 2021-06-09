import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

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

  constructor() { }

  ngOnInit(): void {
  }

  prepareRoute(outlet: RouterOutlet) {
    return outlet && outlet.activatedRouteData && outlet.activatedRouteData.animation;
  }
}
