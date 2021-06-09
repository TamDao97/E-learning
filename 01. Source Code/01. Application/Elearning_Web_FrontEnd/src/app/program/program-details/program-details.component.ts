import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginUserComponent } from 'src/app/layout/login-user/login-user.component';
import { Constants } from 'src/app/share/common/Constants';
import { DateUtils } from 'src/app/share/common/date-utils';
import { Configuration } from 'src/app/share/config/configuration';
import { MessageService } from 'src/app/share/service/message.service';
import { ProgramService } from '../service/program.service';
import $ from "jquery";

@Component({
  selector: 'app-program-details',
  templateUrl: './program-details.component.html',
  styleUrls: ['./program-details.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class ProgramDetailsComponent implements OnInit {
  slug: string;
  id:string;
  totalCourse: any = [];
  isLogin = false;
  model: any = {};
  dateNow = new Date();
  learnerId = "";
  listProgram = [];

  constructor(
    private service: ProgramService,
    private route: ActivatedRoute,
    public config: Configuration,
    public router: Router,
    private modalService: NgbModal,
    public dateUntil: DateUtils,
    public constant: Constants,
    private messageService: MessageService,

  ) { }

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.learnerId = localStorage.getItem('user');
    if (this.learnerId) {
      this.isLogin = true;
    }
    this.slug = this.route.snapshot.paramMap.get('slug');
    this.getProgramById(this.slug);
  }

  register() {
    this.service.createLearnerCourse(this.model).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Đăng ký khóa học thành công!');
          this.ngOnInit();
        }
        else {
          this.messageService.showListMessage(data.message);
          this.ngOnInit();
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListProgram(id){
    this.service.getListProgram(id).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.listProgram = data.data;
      }
      else {
        this.router.navigate(['/404']);
      }
    },
      error => {
        this.router.navigate(['/404']);
      });
  }

  routerLink(id){
    // this.router.navigate(['/chi-tiet-chuong-trinh'+id]);
    this.getProgramById(id);
    window.scrollTo(0, 0);

  }
  modelProgram = {
      slug:'',
      learnerId:''
  }
  getProgramById(id: string) {
    this.modelProgram.slug = id;
    this.modelProgram.learnerId = this.learnerId;
    this.service.getProgramById(this.modelProgram).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.model = data.data;
        this.totalCourse = data.data.listCourse.length;
        this.id=data.data.id;
        this.getListProgram(this.id);
      }
      else {
        this.router.navigate(['/404']);
      }
    },
      error => {
        this.router.navigate(['/404']);
      });
  }

  signIn() {
    let activeModal = this.modalService.open(LoginUserComponent, { container: 'body', windowClass: 'signIn-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        $(document).ready(function () {
          location.reload(true);
        });
      }
    });
  }

  handling(id, isLearned, finishDate,slug) {
    this.learnerId = localStorage.getItem('user');
    this.model = {
      learnerId: this.learnerId,
      courseId: id,
    }
    if (this.learnerId) {
      this.isLogin == true;
      if (isLearned == false && this.dateNow.getTime() <= this.dateUntil.getTimetoDateString(finishDate)) {
        this.register();
      }
      if (isLearned == true) {
        this.router.navigate(['chi-tiet-khoa-hoc', slug]);
      }
      if (isLearned == false && this.dateNow.getTime() > this.dateUntil.getTimetoDateString(finishDate)) {

      }
    }
    else {
      this.isLogin = false;
      this.signIn();
    }
  }
}
