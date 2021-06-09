import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Validators, FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/auth/services';
import { Constants, MessageService } from 'src/app/shared';
import { UserService } from '../service/user.service';

@Component({
  selector: 'app-refresh-password',
  templateUrl: './refresh-password.component.html',
  styleUrls: ['./refresh-password.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class RefreshPasswordComponent implements OnInit {

  loginForm: FormGroup;
  id: string;
  constructor(
    private activeModal: NgbActiveModal,
    public formBuilder: FormBuilder,
    private messageService: MessageService,
    public constant: Constants,
    private userService: UserService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,

  ) {
    this.loginForm = this.formBuilder.group({
      password: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern(
          /^(?=.*\d+)(?=.*\W+)(?=.*[A-Z]+)(?=.*[a-z]+)[a-zA-Z\d\W]{8,}$/
        )
      ])),
      confirmpassword: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern(/^(?=.*\d+)(?=.*\W+)(?=.*[A-Z]+)(?=.*[a-z]+)[a-zA-Z\d\W]{8,}$/)
      ])),
    }, {
      validators: this.password.bind(this)
    });
  }

  fieldTextType: boolean;
  fieldReTextType: boolean;
  modalInfo = {
    Title: 'Thay đổi mật khẩu',
  };

  changePassword: string;
  isPassword = true;
  message: any


  isAction: boolean = false;


  model: any = {
    password: '',
    isPassword: ''
  }
  user: any;
  ngOnInit(): void {
  }

  error_messages = {
    'password': [
      { type: 'required', message: 'Mật khẩu không được bỏ trống!' },
      { type: 'pattern', message: 'Mật khẩu phải có ít nhất 8 ký tự, có chữ hoa, chữ thường, số, ký tự đặc biệt!' },
    ],
    'confirmpassword': [
      { type: 'required', message: 'Mật khẩu không được bỏ trống!' },
      { type: 'pattern', message: 'Mật khẩu phải có ít nhất 8 ký tự, có chữ hoa, chữ thường, số, ký tự đặc biệt!' },
    ],
  }

  password(formGroup: FormGroup) {
    const { value: password } = formGroup.get('password');
    const { value: confirmPassword } = formGroup.get('confirmpassword');
    // return password === confirmPassword ? null : "Mật khẩu không trùng nhau!";
    if (password === confirmPassword) {
      return this.message = null;
    }
    else {
      return this.message = "Mật khẩu không trùng nhau!";
    }
  }


  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }

  toggleReFieldTextType() {
    this.fieldReTextType = !this.fieldReTextType;
  }

  save() {
    this.user = JSON.parse(localStorage.getItem('ElearningCurrentUser'));
    if (this.isPassword == true) {
      this.model.passwordHash = Math.random().toString(36).replace(/^(?=.*\d+)(?=.*\W+)(?=.*[A-Z]+)(?=.*[a-z]+)[a-zA-Z\d\W]{8,}$/g, '').substr(0, 9);
    }
    this.model.isPassword = this.isPassword;
    this.userService.changePassword(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thay đổi mật khẩu thành công!');
          this.closeModal(true);
          if (this.user.userId == this.id) {
            this.authenticationService.logout();
            this.router.navigate(['/auth/dang-nhap']);
          }
        }
        else {
          this.messageService.showMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  // checkPasswords(group: FormGroup) {
  //   let pass = group.controls.password.value;
  //   let confirmPass = group.controls.confirmPassword.value;

  //   return pass === confirmPass ? "Mật khẩu không trùng nhau." : ""
  // }

}
