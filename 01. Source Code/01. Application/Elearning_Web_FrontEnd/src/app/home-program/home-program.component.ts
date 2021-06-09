import { Component, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { from } from 'rxjs';
import { Configuration } from 'src/app/share/config/configuration';
import { ProgramService } from '../layout/service/program.service';
import { LoginUserComponent } from '../layout/login-user/login-user.component'
import { DateUtils } from '../share/common/date-utils';
import { MessageService } from '../share/service/message.service';
import $ from "jquery";
import { Constants } from '../share/common/Constants';
import { DataService } from '../layout/service/data.service';
@Component({
  selector: 'app-home-program',
  templateUrl: './home-program.component.html',
  styleUrls: ['./home-program.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class HomeProgramComponent implements OnInit {

  constructor(
    public programService: ProgramService,
    public config: Configuration,
    private modalService: NgbModal,
    public dateUntil: DateUtils,
    private messageService: MessageService,
    public route: Router,
    public constant: Constants,
    private data: DataService
  ) { }

  dayDiff:any[];
  learnerId: any="";
  isLogin = false;
  dateNow = new Date();
  listData: any[] = [];
  model: any = {
    learnerId: '',
    courseId: '',
  }
  ngOnInit(): void {
    this.data.currentLearnerId.subscribe(
      (id:any)=>{
        if(id&&id!="")
        {
          this.learnerId=id;
          this.isLogin = true;
        }
        if(id=="")
        {
          this.learnerId=id;
          this.isLogin = false;
        }
        else
        {
          this.learnerId = localStorage.getItem('user');
          if(this.learnerId!="")
          this.isLogin = true;
        }
        this.GetAllProgram();
       
      }
    );
  }

  register() {
    this.programService.createLearnerCourse(this.model).subscribe(
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

  GetAllProgram() {
    this.programService.searchCourse(this.learnerId).subscribe(
      (data: any) => {
        this.listData = data.data;
      }
    );
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
        this.route.navigate(['chi-tiet-khoa-hoc', slug]);
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
