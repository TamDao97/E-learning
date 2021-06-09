import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Elearning web';
  location: Location;
  ngOnInit() {
    if (location.hostname === 'www.nhantinsoft.vn') {
      window.location.href = location.href.replace('https://www.nhantinsoft.vn:9508/', 'https://nhantinsoft.vn:9508/');
    }
  }
}
