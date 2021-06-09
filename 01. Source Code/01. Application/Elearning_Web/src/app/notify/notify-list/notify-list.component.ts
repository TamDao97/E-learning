import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { trigger, transition, state, animate, style, AnimationEvent } from '@angular/animations';

import { Subject, Subscription } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';

import { NotifyService } from '../services/notify.service';

@Component({
  selector: 'app-notify-list',
  templateUrl: './notify-list.component.html',
  styleUrls: ['./notify-list.component.scss'],
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
export class NotifyListComponent implements OnInit, OnDestroy {

  constructor(private notifyService: NotifyService) {
    this._unsubscribeAll = new Subject();
  }

  private _unsubscribeAll: Subject<any>;

  isOpen = false;

  ngOnInit(): void {
    this.notifyService.onShowNotifyChanged
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

  close() {
    this.notifyService.showNotify(false);
  }
}
