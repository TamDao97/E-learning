import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appUipermission]'
})
export class UipermissionDirective {

  constructor(private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef) {

  }

  @Input() set appUipermission(permission: any) {

    var isAuthorize = false;
    var user = localStorage.getItem('ElearningCurrentUser');
    if (user) {
      var listPermission = JSON.parse(localStorage.getItem('ElearningCurrentUser')).permissions;
      if (listPermission != null && listPermission.length > 0 && permission) {
        permission.forEach(function (item) {
          if (!isAuthorize && listPermission.indexOf(item) != -1) {
            isAuthorize = true;
          }
        });
      }
    }

    if (!permission || permission.length == 0) {
      isAuthorize = true;
    }

    if (isAuthorize) {
      this.viewContainer.createEmbeddedView(this.templateRef);

    } else {
      this.viewContainer.clear();
    }
  }

}
