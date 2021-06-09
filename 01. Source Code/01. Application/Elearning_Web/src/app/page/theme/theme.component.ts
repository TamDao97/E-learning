import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { trigger, transition, state, animate, style, AnimationEvent } from '@angular/animations';

import { Subject, Subscription } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';
import { NotifyService } from 'src/app/notify/services/notify.service';
import { Constants } from 'src/app/shared';

@Component({
  selector: 'app-theme',
  templateUrl: './theme.component.html',
  styleUrls: ['./theme.component.scss'],
  animations: [
    trigger('openClose', [
      // ...
      state('open', style({
        width: '300px',

      })),
      state('closed', style({
        width: '0px'
      })),
      transition('open => closed', [
        animate('0.5s')
      ]),
      transition('closed => open', [
        animate('0.5s')
      ]),
    ]),
  ],
  encapsulation: ViewEncapsulation.None
})
export class ThemeComponent implements OnInit, OnDestroy {

  constructor(private notifyService: NotifyService,
    public constant: Constants,) {
    this._unsubscribeAll = new Subject();
  }

  private _unsubscribeAll: Subject<any>;

  themes: any = [
    {
      Name: 'Mặc định',
      BodyBackground: '#f5f5f5',
      BodyFontsize: 14,
      TopbarAboveBackground: '#0056ac',
      TopbarAboveColor: '#fff',
      TopbarAboveSize: 20,
      TopbarUnderBackground: '#fff',
      TopbarUnderColor: '#0056ac',
      TopbarUnderSize: 18,
      LogoBackground: '#fff',
      LeftbarBackground: '#0056ac',
      LeftbarColor: '#fff',
      LeftbarBackgroundHover: '#263238',
      LeftbarBackgroundActive: '#263238',
      LeftbarBackgroundMenuActive: '#ed1b24',
      LeftbarBackgroundMenuHover: '#161e22',
      LeftbarFontsize: 16,
      InfoFooterBackground: '#fff',
      InfoFooterColor: '#000',
      InfoFooterSize: 12,
      ContentTableHeaderBackground: '#0056ac',
      ContentTableHeaderColor: '#fff',
    },
    {
      Name: "Logo NTS",
      BodyBackground: "#f5f5f5",
      BodyFontsize: "14",
      TopbarAboveBackground: "#ca1800",
      TopbarAboveColor: "#fdfdfd",
      TopbarAboveSize: 20,
      TopbarUnderBackground: "#ffffff",
      TopbarUnderColor: "#ca1800",
      TopbarUnderSize: 18,
      LogoBackground: "#ffffff",
      LeftbarBackground: "#ca1800",
      LeftbarColor: "#fff",
      LeftbarBackgroundHover: "#0056ac",
      LeftbarBackgroundActive: "#263238",
      LeftbarBackgroundMenuActive: "#0056ac",
      LeftbarBackgroundMenuHover: "#0056ac",
      LeftbarFontsize: 16,
      InfoFooterBackground: "#fff",
      InfoFooterColor: "#000",
      InfoFooterSize: 12,
      ContentTableHeaderBackground: "#ca1800",
      ContentTableHeaderColor: "#fff"
    }
  ];

  isOpen = false;
  themeSelect: any = null;
  height = 400;

  ngOnInit(): void {
    this.height = window.innerHeight - 60;

    var saveTheme = localStorage.getItem('ElearningThemeDefault');

    if (saveTheme) {
      this.themes.push(JSON.parse(saveTheme));
      this.themeSelect = this.themes[this.themes.length - 1];
    } else {
      this.themeSelect = this.themes[0];
    }

    this.changeTheme();

    this.notifyService.onShowThemeChanged
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe(data => {
        this.isOpen = data;
      });
  }

  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next();
    this._unsubscribeAll.complete();
  }

  changeBodyBackground() {
    this.changeValueCss('--body-background', this.themeSelect.BodyBackground);
  }

  changeBodyFontsize() {
    this.changeValueCss('--body-fontsize', this.themeSelect.BodyFontsize + 'px');
  }

  changeTopbarAboveBackground() {
    this.changeValueCss('--topbar-above-background', this.themeSelect.TopbarAboveBackground);
  }

  changeTopbarAboveColor() {
    this.changeValueCss('--topbar-above-color', this.themeSelect.TopbarAboveColor);
  }

  changeTopbarAboveSize() {
    this.changeValueCss('--topbar-above-size', this.themeSelect.TopbarAboveSize + 'px');
  }

  changeTopbarUnderBackground() {
    this.changeValueCss('--topbar-under-background', this.themeSelect.TopbarUnderBackground);
  }

  changeTopbarUnderColor() {
    this.changeValueCss('--topbar-under-color', this.themeSelect.TopbarUnderColor);
  }

  changeTopbarUnderSize() {
    this.changeValueCss('--topbar-under-size', this.themeSelect.TopbarUnderSize + 'px');
  }

  changeLogoBackground() {
    this.changeValueCss('--logo-background', this.themeSelect.LogoBackground);
  }

  changeLeftbarBackground() {
    this.changeValueCss('--leftbar-background', this.themeSelect.LeftbarBackground);
  }

  changeLeftbarColor() {
    this.changeValueCss('--leftbar-color', this.themeSelect.LeftbarColor);
  }

  changeLeftbarBackgroundHover() {
    this.changeValueCss('--leftbar-background-hover', this.themeSelect.LeftbarBackgroundHover);
  }

  changeLeftbarBackgroundActive() {
    this.changeValueCss('--leftbar-background-active', this.themeSelect.LeftbarBackgroundActive);
  }

  changeLeftbarBackgroundMenuActive() {
    this.changeValueCss('--leftbar-background-menu-active', this.themeSelect.LeftbarBackgroundMenuActive);
  }

  changeLeftbarBackgroundMenuHover() {
    this.changeValueCss('--leftbar-background-menu-hover', this.themeSelect.LeftbarBackgroundMenuHover);
  }

  changeLeftbarFontsize() {
    this.changeValueCss('--leftbar-fontsize', this.themeSelect.LeftbarFontsize + 'px');
  }

  changeContentTableHeaderBackground() {
    this.changeValueCss('--content-table-header-background', this.themeSelect.ContentTableHeaderBackground);
  }

  changeContentTableHeaderColor() {
    this.changeValueCss('--content-table-header-color', this.themeSelect.ContentTableHeaderColor);
  }

  changeTheme() {

    this.changeBodyBackground();

    this.changeBodyFontsize();

    this.changeTopbarAboveBackground();

    this.changeTopbarAboveColor();

    this.changeTopbarAboveSize();

    this.changeTopbarUnderBackground();

    this.changeTopbarUnderColor();

    this.changeTopbarUnderSize();

    this.changeLogoBackground();

    this.changeLeftbarBackground();

    this.changeLeftbarColor();

    this.changeLeftbarBackgroundHover();

    this.changeLeftbarBackgroundActive();

    this.changeLeftbarBackgroundMenuActive();

    this.changeLeftbarBackgroundMenuHover();

    this.changeLeftbarFontsize();

    this.changeContentTableHeaderBackground();

    this.changeContentTableHeaderColor();
  }

  downloadTheme() {
    var link = document.createElement("a");
    link.href = "data:application/json;charset=UTF-8," + encodeURIComponent(JSON.stringify(this.themeSelect));;

    link.setAttribute("download", "theme.json");
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  changeValueCss(key: string, value: string) {
    document.documentElement.style.setProperty(key, value);
  }

  saveTheme() {
    var saveTheme = Object.assign({}, this.themeSelect);
    saveTheme.Name = "Save theme";
    localStorage.setItem('ElearningThemeDefault', JSON.stringify(saveTheme));
  }

  close() {
    this.notifyService.showTheme(false);
  }
}
