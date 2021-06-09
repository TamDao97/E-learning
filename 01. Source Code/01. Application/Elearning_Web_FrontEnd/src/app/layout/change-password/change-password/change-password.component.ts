import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/share/common/Constants';
import { MessageService } from 'src/app/share/service/message.service';
import { LoginUserComponent } from '../../login-user/login-user.component';
import { TopbarService } from '../../service/topbar.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ChangePasswordComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public topbarService: TopbarService,
    public constant: Constants,
    public messageService: MessageService,
    public router: Router,
    private modalService: NgbModal,

  ) { }

  isLogin = false;
  model: any = {
    Email: '',
  }

  ngOnInit(): void {
  }

  closeModal() {
    this.activeModal.close(true);
    this.showPopUpLoginEmail();
  }

  showPopUpLoginEmail() {
    let activeModal = this.modalService.open(LoginUserComponent, { container: 'body', windowClass: 'signIn-model', backdrop: 'static' })
    activeModal.componentInstance.type = 1;
    activeModal.result.then((result: any) => {
    });
  }

  send() {
    this.topbarService.forgotpass(this.model.email).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.closeModal();
          localStorage.removeItem('learner');
          this.router.navigate(['']);

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

}
