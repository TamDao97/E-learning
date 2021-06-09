import { Directive, Input, ElementRef, TemplateRef, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[disUipermission]'
})
export class DiablePermissionDirective {

  private _el: HTMLElement;

  constructor(el: ElementRef) {
    this._el = el.nativeElement;
  }


  @Input() set disUipermission(permission: any) {

    var isAuthorize = false;
    var listPermission = JSON.parse(localStorage.getItem('ElearningCurrentUser')).permissions;
    if (listPermission != null && listPermission.length > 0 && permission) {
      permission.forEach(function (item) {
        if (!isAuthorize && listPermission.indexOf(item) != -1) {
          isAuthorize = true;
        }
      });
    }

    if (!permission || permission.length == 0) {
      isAuthorize = true;
    }

    if (!isAuthorize) {
      this._el.classList.add('disabled');
    }
  }

}
