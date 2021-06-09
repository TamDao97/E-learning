import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { MessageService } from './../../shared/index';

@Injectable({
    providedIn: 'root',
})
export class AuthGuard implements CanActivate {

    constructor(private router: Router, private messageService: MessageService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        let user = JSON.parse(localStorage.getItem('ElearningCurrentUser'));
        if (user && user.LoginDate) {
            let dateNow = new Date().getTime();

            let loginDate = new Date(user.LoginDate).getTime();
            let msec = dateNow - loginDate;
            if (Math.floor((msec / 60000) / 60) < user.expireDateAfter*24) {
                // logged in so return true
                return true;
            } else {
                localStorage.removeItem('ElearningCurrentUser');
                ///this.messageService.showMessage("Bạn đã hết phiên làm việc. Bạn hãy đăng nhập lại để tiếp tục.", 1);
            }
        }

        // not logged in so redirect to login page with the return url
        this.router.navigate(['/auth/dang-nhap'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}