import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/share/common/Constants';
import { MessageService } from 'src/app/share/service/message.service';
import { DataService } from '../../service/data.service';
import { TopbarService } from '../../service/topbar.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ResetPasswordComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public formBuilder: FormBuilder,
    private messageService: MessageService,
    public constant: Constants,
    public topbarService: TopbarService,
    private route: ActivatedRoute,
    private router: Router,
    public dataService: DataService

  ) {
    this.loginForm = this.formBuilder.group({
      passwordOld: new FormControl('', Validators.compose([
        Validators.required,
      ])),
      passwordNew: new FormControl('', Validators.compose([
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

  fieldTextTypeOld: boolean;
  fieldTextTypeNew: boolean;
  fieldReTextType: boolean;
  modalInfo = {
    Title: 'ĐỔI MẬT KHẨU',
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
  
  loginForm: FormGroup;
  data: any;
  ngOnInit(): void {

  }

  error_messages = {
    'passwordOld': [
      { type: 'required', message: 'Mật khẩu cũ không được bỏ trống!' },
    ],
    'passwordNew': [
      { type: 'required', message: 'Mật khẩu cũ không được bỏ trống!' },
      { type: 'pattern', message: 'Mật khẩu phải có ít nhất 8 ký tự, có chữ hoa, chữ thường, số, ký tự đặc biệt!' },
    ],
    'confirmpassword': [
      { type: 'required', message: 'Mật khẩu không được bỏ trống!' },
      { type: 'pattern', message: 'Mật khẩu phải có ít nhất 8 ký tự, có chữ hoa, chữ thường, số, ký tự đặc biệt!' },
    ],
  }

  password(formGroup: FormGroup) {
    const { value: passwordOld } = formGroup.get('passwordOld');
    const { value: passwordNew } = formGroup.get('passwordNew');
    const { value: confirmPassword } = formGroup.get('confirmpassword');
    // return password === confirmPassword ? null : "Mật khẩu không trùng nhau!";
    if (passwordNew === confirmPassword) {
      return this.message = null;
    }
    else {
      return this.message = "Mật khẩu không trùng nhau!";
    }
  }


  
  toggleFieldTextTypeOld() {
    this.fieldTextTypeOld = !this.fieldTextTypeOld;
  }

  toggleFieldTextType() {
    this.fieldTextTypeNew = !this.fieldTextTypeNew;
  }

  toggleReFieldTextType() {
    this.fieldReTextType = !this.fieldReTextType;
  }

  save() {
    this.topbarService.resetpassword(this.model, this.data.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thay đổi mật khẩu thành công!');
          this.closeModal(true);
          localStorage.setItem('learner', JSON.stringify(this.data));
          this.dataService.changeUserLogin(this.data);

        }
        else {
          this.messageService.showMessage(data.message);
          this.closeModal(false);

        }
      },
      error => {
        this.closeModal(false);
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }


}
