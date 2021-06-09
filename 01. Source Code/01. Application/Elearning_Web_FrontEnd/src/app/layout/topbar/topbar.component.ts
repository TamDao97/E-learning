import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { config } from 'rxjs';
import { LockUserComponent } from 'src/app/lock-user/lock-user.component';
import { Constants } from 'src/app/share/common/Constants';
import { Configuration } from 'src/app/share/config/configuration';
import { MessageService } from 'src/app/share/service/message.service';
import { LoginUserComponent } from '../login-user/login-user.component';
import { ResetPasswordComponent } from '../reset-password/reset-password/reset-password.component';
import { DataService } from '../service/data.service';

import { TopbarService } from '../service/topbar.service'
import { UpdateUserComponent } from '../update-user/update-user.component';
import { SocialAuthService, GoogleLoginProvider, SocialUser } from 'angularx-social-login';

socialUser: SocialUser;
declare var FB: any;
declare var gapi: any;

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class TopbarComponent implements OnInit {

  public gapiSetup: boolean = false; // marks if the gapi library has been loaded
  public authInstance: any;
  public error: string;
  public user: any;

  constructor(
    public topbarService: TopbarService,
    private modalService: NgbModal,
    public constant: Constants,
    public router: Router,
    public messageService: MessageService,
    public config: Configuration,
    private dataService: DataService,
    private socialAuthService: SocialAuthService
  ) { }

  modelGoogle: any = {
    Id: '',
    Id_Token: null
  }

  modelFacebook: any = {
    Id_Token: ''
  }
  isLogin = false;

  model: any = {
    id: '',
    logo: '',
    address: '',
    phone: '',
    gmail: '',
    linkFacebook: '',
    linkGoogle: '',
    linkYoutube: '',
    copyright: '',
  }

  modelUser: any = {
    avatar: ''
  };
  acsset_token: string;

  ngOnInit() {
    this.getHomeSetting();

    var userLogin = JSON.parse(localStorage.getItem('learner'));
    if (userLogin != null) {
      this.isLogin = true;
      this.modelUser = userLogin;
      this.dataService.changeId(userLogin.id);
      if (this.modelUser.avatar != null && this.modelUser.avatar != '') {
        this.modelUser.avatar = this.config.ServerApi + this.modelUser.avatar;
      }
      else {
        this.modelUser.avatar = '/assets/img/noavatar.png';
      }
    }
  }


  async signOut() {
    this.socialAuthService.signOut();

    await this.topbarService.logout(this.modelUser.id).subscribe(data => {
    })
    this.isLogin = false;

    await localStorage.removeItem('user');
    this.router.navigate(['']);

    await localStorage.removeItem('learner');
    this.router.navigate(['']);
    await this.dataService.changeId('');
  }

  showUpdateUser(id) {
    let activeModal = this.modalService.open(UpdateUserComponent, { container: 'body', windowClass: 'update-user-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.email = 1;
    activeModal.result.then((result: any) => {
      if (result) {
        this.modelUser = JSON.parse(localStorage.getItem('learner'));
        if (this.modelUser != null) {
          if (this.modelUser.avatar != null && this.modelUser.avatar != '') {
            this.modelUser.avatar = this.config.ServerApi + this.modelUser.avatar;
          }
          else {
            this.modelUser.avatar = '/assets/img/noavatar.png';
          }
        }
        else {
          this.modelUser = {};
          this.isLogin = false;
        }

      }
    });
  }

  showLockUser() {
    let activeModal = this.modalService.open(LockUserComponent, { container: 'body', windowClass: 'lock-user-model', backdrop: 'static' })
    activeModal.result.then((result: any) => {
      if (result) {
        this.ngOnInit();
      }
    });
  }

  showPopUpLoginEmail() {
    let activeModal = this.modalService.open(LoginUserComponent, { container: 'body', windowClass: 'signIn-model', backdrop: 'static' })
    activeModal.componentInstance.type = 1;
    activeModal.result.then((result: any) => {
      if (result) {
        this.modelUser = JSON.parse(localStorage.getItem('learner'));
        if (this.modelUser != null) {
          if (this.modelUser.isLogin == true) {
            this.showFromResetPassword(this.modelUser);
          }
          this.isLogin = true;
          localStorage.setItem('user', this.modelUser.id);
          this.dataService.changeId(this.modelUser.id);
          if (this.modelUser.avatar != null && this.modelUser.avatar != '') {
            this.modelUser.avatar = this.config.ServerApi + this.modelUser.avatar;
          }
          else {
            this.modelUser.avatar = '/assets/img/noavatar.png';
          }
        }
        else {
          this.modelUser = {};
          this.isLogin = false;
        }
      }
    });
  }

  showFromResetPassword(data) {
    if (data == null) {
      data = JSON.parse(localStorage.getItem('learner'));
    }
    let activeModal = this.modalService.open(ResetPasswordComponent, { container: 'body', windowClass: 'reset-password-model', backdrop: 'static' })
    activeModal.componentInstance.data = data;
    activeModal.result.then((result: any) => {
      if (result) {
        this.modelUser = JSON.parse(localStorage.getItem('learner'));
        localStorage.setItem('user', this.modelUser.id);
        if (this.modelUser != null) {
          this.isLogin = true;
        }
        else {
          this.isLogin = false;
        }

        if (this.modelUser.avatar != null && this.modelUser.avatar != '') {
          this.modelUser.avatar = this.config.ServerApi + this.modelUser.avatar;
        }
        else {
          this.modelUser.avatar = '/assets/img/noavatar.png';
        }
      }
    });
  }

  getHomeSetting() {
    this.topbarService.getHomeSetting().subscribe(
      (data: any) => {
        this.model = data.data;
      }
    );
  }
}

