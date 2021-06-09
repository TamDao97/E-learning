import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class NotifyService {

    private _onShowNotifyChanged: BehaviorSubject<any>;
    private _onShowThemeChanged: BehaviorSubject<any>;

    constructor() {
        this._onShowNotifyChanged = new BehaviorSubject(null);
        this._onShowThemeChanged = new BehaviorSubject(null);
    }

    get onShowNotifyChanged(): Observable<any> {
        return this._onShowNotifyChanged.asObservable();
    }

    get onShowThemeChanged(): Observable<any> {
        return this._onShowThemeChanged.asObservable();
    }

    showNotify(isShow): void {
        this._onShowNotifyChanged.next(isShow);
    }

    showTheme(isShow): void {
        this._onShowThemeChanged.next(isShow);
    }
}