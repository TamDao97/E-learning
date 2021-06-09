import { Component, OnInit } from '@angular/core';

import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss']
})
export class AboutComponent implements OnInit {

  constructor(private appSetting: AppSetting) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = 'Giới thiệu';
  }

}
