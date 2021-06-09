import { Configuration, AppSetting, Constants } from '../../shared';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChangePasswordComponent } from './../../auth/change-password/change-password.component';
import { NtsNavigationService } from '../navigation/navigation.service'
import { NotifyService } from 'src/app/notify/services/notify.service';
import { UpdateUserComponent } from 'src/app/users/update-user/update-user.component';
import { AuthenticationService } from 'src/app/auth/services';

declare var $: any;

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TopbarComponent implements OnInit {

  fullName: string;
  constructor(public appSetting: AppSetting,
    private modalService: NgbModal,
    private route: ActivatedRoute,
    private router: Router,
    public config: Configuration,
    private notifyService: NotifyService,
    public authService: AuthenticationService,
    public constant: Constants
  ) {

  }

  linkManual: string;
  linkImage: string;
  currentUsers = JSON.parse(localStorage.getItem('ElearningCurrentUser'));
  ngOnInit() {


    let currentUser = JSON.parse(localStorage.getItem('ElearningCurrentUser'));
    if (currentUser) {
      if (currentUser.imageLink) {
        this.linkImage = this.config.ServerApi + currentUser.imageLink;
      }

      this.fullName = currentUser.name;
    }

  }

  menuChatToggle(type) {
    if (type == 'menu') {
      this.appSetting.MenuFolded = !this.appSetting.MenuFolded;
      this.appSetting.chatFolded = false;
    }
  }

  updateUser() {
    var userId = JSON.parse(localStorage.getItem('ElearningCurrentUser')).userId;

    let activeModal = this.modalService.open(UpdateUserComponent, { container: 'body', windowClass: 'update-user-model', backdrop: 'static' })
    activeModal.componentInstance.id = userId;
    activeModal.result.then((result) => {
      if (result) {
        this.currentUsers = JSON.parse(localStorage.getItem('ElearningCurrentUser'));
        this.linkImage = this.currentUsers.avatar;
        this.fullName = this.currentUsers.name;
        if (this.linkImage != null && this.linkImage != '') {
          this.linkImage = this.config.ServerApi + this.linkImage;
        }
        else {
          this.linkImage = '/assets/img/noavatar.png';
        }
      }
    }, (reason) => {
    });
  }

  logout() {

    this.authService.logout().subscribe(
      result => {
        if (result.statusCode == this.constant.StatusCode.Success) {
          localStorage.removeItem('ElearningCurrentUser');
          this.router.navigate(['/auth/dang-nhap']);
        }
      },
      error => {

      }
    );
  }

  routerLink() {
    this.router.navigate(['/main']);
  }

  fnChangePassword() {
    let activeModal = this.modalService.open(ChangePasswordComponent, { container: 'body' });
    activeModal.result.then((result) => {

    }, (reason) => {

    });
  }

  showManual() {

  }

  errorImage(event) {
    this.linkImage = '/assets/img/noavatar.png';
  }

  showNotify() {
    this.notifyService.showNotify(true);
  }

  showTheme() {
    this.notifyService.showTheme(true);
  }
}
