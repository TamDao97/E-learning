import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchService } from '../search.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration } from 'src/app/share/config/configuration';
import { DateUtils } from '../../share/common/date-utils';
import { LoginUserComponent } from '../../layout/login-user/login-user.component'
import { Constants } from '../../share/common/Constants';
import $ from "jquery";
import { MessageService } from '../../share/service/message.service';
import { DataService } from '../../layout/service/data.service';
import { ProgramService } from '../../layout/service/program.service';
import { ViewEncapsulation } from '@angular/core';
@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class SearchResultComponent implements OnInit {

  constructor(
    public searchService: SearchService,
    private data: DataService,
    private routeA: ActivatedRoute,
    public route: Router,
    public programService: ProgramService,
    private modalService: NgbModal,
    public dateUntil: DateUtils,
    private messageService: MessageService,
    public constant: Constants,
    public config: Configuration,
  ) { }
  listData: any[] = [];
  totalItems=0;
  learnerId: any = "";
  isLogin = false;
  dateNow = new Date();
  model: any = {
    learnerId: '',
    courseId: '',
  }
  searchValue:string
  ngOnInit(): void {
    this.searchValue = this.routeA.snapshot.paramMap.get('a');
    
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
        this.SearchCourse();
      }
    );
  }
  SearchCourse() {
    this.searchService.searchCourse(this.learnerId, this.searchValue).subscribe(
      (data: any) => {
        this.listData = data.data;
        this.totalItems=data.data.length;
      }
    );
  }
  search()
  {
    this.route.navigate(['/tim-kiem',{a:this.searchValue}]);
    this.SearchCourse();    
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
