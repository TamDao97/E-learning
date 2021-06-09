import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../services';
import { Constants, MessageService } from '../../shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ChangePasswordComponent implements OnInit {

  returnUrl: string
  OldPassword: string;
  ConfirmOldPassword: any;
  fieldReTextType: boolean;
  fieldTextType: boolean;
  fieldTextOldType: boolean;
  loginForm: FormGroup;
  message: any

  
  model: any = {
    Id: JSON.parse(localStorage.getItem('ElearningCurrentUser')).userId,
    PasswordOld:'',
    PasswordHash: '',
    ConfirmPassword: '',
  }

  constructor(private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constant: Constants,
    public formBuilder: FormBuilder,

  ) {
    this.loginForm = this.formBuilder.group({
      password: new FormControl('',Validators.compose([
        Validators.required,
        Validators.pattern(
          /^(?=.*\d+)(?=.*\W+)(?=.*[A-Z]+)(?=.*[a-z]+)[a-zA-Z\d\W]{8,}$/
        )
      ])),
      confirmpassword: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern( /^(?=.*\d+)(?=.*\W+)(?=.*[A-Z]+)(?=.*[a-z]+)[a-zA-Z\d\W]{8,}$/)
      ])),
      passwordold: new FormControl('', Validators.compose([
        Validators.required,
      ])),
    }, { 
      validators: this.password.bind(this)
    });
   }

   error_messages = {
    'passwordnew': [
      { type: 'required', message: 'Mật khẩu không được bỏ trống!' },
      { type: 'pattern', message: 'Mật khẩu phải có ít nhất 8 ký tự, có chữ hoa, chữ thường, số, ký tự đặc biệt!' },
    ],
    'confirmpassword': [
      { type: 'required', message: 'Mật khẩu không được bỏ trống!' },
      { type: 'pattern', message: 'Mật khẩu phải có ít nhất 8 ký tự, có chữ hoa, chữ thường, số, ký tự đặc biệt!' },
    ],
    'passwordold': [
      { type: 'required', message: 'Mật khẩu không được bỏ trống!' },
    ],
  }

  password(formGroup: FormGroup) {
    const { value: password } = formGroup.get('password');
    const { value: confirmPassword } = formGroup.get('confirmpassword');
    const { value: passwordold } = formGroup.get('passwordold');
    // return password === confirmPassword ? null : "Mật khẩu không trùng nhau!";
    if(password === confirmPassword){
      return this.message = null;
    }
    else{
      return  this.message = "Mật khẩu không trùng nhau!";
    }
  }

  ngOnInit() {

  }

  closeModal(isOK: boolean) {
    this.activeModal.close(true);
  }

  ConfirmChangePassword() {
    this.messageService.showConfirm("Bạn có chắc muốn thay đổi mật khẩu không?").then(
      data => {
        this.ChangePassword();
      }
    );
  }

  toggleFieldTextOldType() {
    this.fieldTextOldType = !this.fieldTextOldType;
  }

  toggleReFieldTextType() {
    this.fieldReTextType = !this.fieldReTextType;
  }

  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }

  ChangePassword() {
    this.model.Id = JSON.parse(localStorage.getItem('ElearningCurrentUser')).userId;
    // this.model.Password = this.OldPassword;
    this.authenticationService.ChangePassword(this.model).subscribe(
      data => {
        if (data) {
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.closeModal(true);
            //this.notificationsService.success('Thông báo', 'Thay đổi mật khẩu thành công');
            this.authenticationService.logout();
            this.router.navigate(['/auth/dang-nhap']);
          }
          else {
            this.messageService.showListMessage(data.message);
          }
        }
        else {
          //this.notificationsService.error('Thông báo','Mật khẩu cũ không đúng. Vui lòng nhập lại!');
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
