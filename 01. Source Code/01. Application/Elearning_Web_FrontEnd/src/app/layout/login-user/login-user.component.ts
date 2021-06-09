import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/share/common/Constants';
import { Configuration } from 'src/app/share/config/configuration';
import { MessageService } from 'src/app/share/service/message.service';
import { ChangePasswordComponent } from '../change-password/change-password/change-password.component';
import { ResetPasswordComponent } from '../reset-password/reset-password/reset-password.component';
import { DataService } from '../service/data.service';
import { TopbarService } from '../service/topbar.service';
import { UpdateUserComponent } from '../update-user/update-user.component';
import { FacebookLoginProvider, GoogleLoginProvider, SocialAuthService } from "angularx-social-login";
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { JQueryStyleEventEmitter } from 'rxjs/internal/observable/fromEvent';
import * as $ from 'jquery';


@Component({
  selector: 'app-login-user',
  templateUrl: './login-user.component.html',
  styleUrls: ['./login-user.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoginUserComponent implements OnInit {

  @BlockUI() blockUI: NgBlockUI;

  constructor(
    public topbarService: TopbarService,
    private modalService: NgbModal,
    public constant: Constants,
    private activeModal: NgbActiveModal,
    public config: Configuration,
    public messageService: MessageService,
    private dataService: DataService,
    private authService: SocialAuthService

  ) { }

  type: number;
  public gapiSetup: boolean = false; // marks if the gapi library has been loaded
  public authInstance: any;
  public error: string;
  public user: any;
  password;
  passwordComfrim;

  show = false;
  showComfrim = false;

  modelGoogle: any = {
    Id: '',
    Id_Token: null
  }
  isLogin = false;

  model: any = {
    email: null,
    name: null,
    id: '',
    logo: '',
    address: '',
    phone: '',
    gmail: '',
    linkFacebook: '',
    linkGoogle: '',
    linkYoutube: '',
    copyright: '',
    passwordComfrim: ''
  }

  modelUser = {
    avatar: '',
    id: '',
    provider: ''
  }
  acsset_token: string;

  async ngOnInit() {
    this.password = 'password';
    this.passwordComfrim = 'password';
    this.type = 1;
    await this.getHomeSetting();
    this.authService.authState.subscribe((user) => {
      this.user = user;
      console.log(user);
    });
  }

  signInWithFB(): void {
    this.authService.signIn(FacebookLoginProvider.PROVIDER_ID).then(user => {
      this.user = user;
      this.modelGoogle.Id_Token = this.user.authToken;
      this.modelGoogle.Id = this.user.id;
      this.topbarService.getFacebook(this.modelGoogle).subscribe(data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          if (data.data.isDisable == false) {
            this.modelUser = data.data;
            this.isLogin = true;
            localStorage.setItem('learner', JSON.stringify(this.modelUser));
            localStorage.setItem('user', data.data.id);
            this.closeModal();
          }
          else {
            this.messageService.showMessage("Tài khoản của bạn bị khóa, vui lòng liên hệ quản trị viên theo thông tin: " + this.config.Email);
            this.signOut();
          }
        }
      })
      if (this.modelUser.id != null) {
        this.dataService.changeId(this.modelUser.id);
      }

    });
  }

  signInWithGoogle(): void {
    this.authService.signIn(GoogleLoginProvider.PROVIDER_ID).then(user => {
      this.user = user;
      this.modelGoogle.Id_Token = this.user.idToken;
      this.modelGoogle.Id = this.user.id;
      this.topbarService.getGoogle(this.modelGoogle).subscribe(data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          if (data.data.isDisable == false) {
            this.modelUser = data.data;
            this.isLogin = true;
            localStorage.setItem('learner', JSON.stringify(this.modelUser));
            localStorage.setItem('user', data.data.id);
            this.closeModal();
          }
          else {
            this.messageService.showMessage("Tài khoản của bạn bị khóa, vui lòng liên hệ quản trị viên theo thông tin: " + this.config.Email);
            this.signOut();
          }
        }
      })
      if (this.modelUser.id != null) {
        this.dataService.changeId(this.modelUser.id);
      }
    }
    );
  }

  signOut() {
    this.topbarService.logout(this.modelUser.id).subscribe(data => {
    })

    this.authService.signOut();
    this.dataService.changeId("");
    this.isLogin = false;
    localStorage.removeItem('user');
    localStorage.removeItem('learner');
  }

  getHomeSetting() {
    this.topbarService.getHomeSetting().subscribe(
      (data: any) => {
        this.model = data.data;
      }
    );
  }

  changeDangky() {
    this.type = 2;
  }

  changeDoimatkhau() {
    let activeModal = this.modalService.open(ChangePasswordComponent, { container: 'body', windowClass: 'change-password-model', backdrop: 'static' })
    activeModal.result.then((result: any) => {
      if (result) {
        this.closeModal();
      }
    });
  }

  changeDangnhap() {
    this.type = 1;
  }

  message: string;
  checkPasswordComfrim(passwordComfrim: string) {
    if (passwordComfrim != null && passwordComfrim != this.model.password) {
      this.message = 'Mật khẩu xác nhận không trùng nhau!';
    }
  }

  save() {
    var validEmail = true;
    var regex = this.constant.validEmailRegEx;
    if (this.model.email != '' && this.model.email != null) {
      if (!regex.test(this.model.email)) {
        validEmail = false;
      }
    }

    if (validEmail) {
      // this.update();
      this.createuser();
    }
    else {
      this.messageService.showMessage('Email không hợp lệ!');
    }
  }

  login() {
    this.topbarService.login(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          if (data.data.isDisable == false) {
            localStorage.setItem('learner', JSON.stringify(data.data));
            localStorage.setItem('user', data.data.id);
            this.closeModal();
            $(document).ready(function () {
              location.reload(true);
            });
          }
          else {
            this.messageService.showMessage("Tài khoản của bạn bị khóa, vui lòng liên hệ quản trị viên theo thông tin: " + this.config.Email);
            this.signOut();
          }
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeShowPassword() {
    if (this.password === 'password') {
      this.password = 'text';
      this.show = true;
    } else {
      this.password = 'password';
      this.show = false;
    }
  }

  changeShowPasswordComfrim() {
    if (this.passwordComfrim === 'password') {
      this.passwordComfrim = 'text';
      this.showComfrim = true;
    } else {
      this.passwordComfrim = 'password';
      this.showComfrim = false;
    }
  }

  createuser() {
    if (this.model.password == this.model.passwordComfrim) {
      this.blockUI.start();
      this.topbarService.createuser(this.model).subscribe(
        data => {
          setTimeout(() => {
            this.blockUI.stop();
          });
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.messageService.showSuccess('Tạo tài khoản thành công!');
            this.type = 1;
            this.model.email = null;
            this.model.name = null;
            this.model.password = null;
          }
          else {
            this.messageService.showListMessage(data.message);
          }
        },
        error => {
          setTimeout(() => {
            this.blockUI.stop();
          });
          this.messageService.showError(error);
        }
      );
    }

  }

  closeModal() {
    this.activeModal.close(true);
  }
}

