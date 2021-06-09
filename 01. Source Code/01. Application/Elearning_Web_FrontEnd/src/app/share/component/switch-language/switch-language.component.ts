import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { AppSetting } from '../../config/appsetting';

@Component({
  selector: 'nts-switch-language',
  templateUrl: './switch-language.component.html',
  styleUrls: ['./switch-language.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SwitchLanguageComponent implements OnInit {

  constructor(public appSetting: AppSetting) { }

  ngOnInit(): void {
  }

  language:any = 'vn';

  changeLanguage() {
    // this.appSetting.Language = this.appSetting.Language == 'vn' ? 'en' : 'vn';
  }
}
