import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { ComboboxService } from 'src/app/shared/services/combobox.service';
import { CourseService } from '../services/course.service';
import { TestResultComponent } from '../test-result/test-result.component';

@Component({
  selector: 'app-progress-manage',
  templateUrl: './progress-manage.component.html',
  styleUrls: ['./progress-manage.component.scss']
})
export class ProgressManageComponent implements OnInit {

  constructor(
    public constant: Constants,
    private appSetting: AppSetting,
    private courseService: CourseService,
    private messageService: MessageService,
    private modalService: NgbModal,
    public comboboxService: ComboboxService,
  ) { }

  startIndex = 1;
  listData: any[] = [];
  listEmployeeCourse: any[] = [];
  listProgram:any[]=[];
  listPageSize = this.constant.ListPageSize;
  address = "";
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    id: null,
    programId:null,
  }

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý tiến độ";
    this.getAllProgram();
  }

  getAllProgram() {
    this.comboboxService.getProgram().subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listProgram = data.data
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

  getListCourseByProgramId(programId) {
    this.listData=[];
    this.model.id=null;
    this.courseService.getEmployeeCourse(programId).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listEmployeeCourse = data.data;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  search() {
    this.courseService.getProgress(this.model).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
          this.listData = data.data.dataResults;
          this.model.TotalItems = data.data.totalItems;
          this.listData.forEach(element => {
            element.progress = (element.studiedCourse / element.availableCourse) * 100;
            if (element.ward && element.address && element.district && element.province) {
              element.address = element.address + '/' + element.ward + '/' + element.district + '/' + element.province
            }
            else {
              element.address = "";
            }
          });
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      id: null,
      programId:null,
    }
    this.search();
  }


  testResult = [];
  popup(courseId, learnerId) {
    var resquest = {
      courseId: courseId,
      learnerId: learnerId
    };
    this.courseService.getTestResult(resquest).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.testResult = data.data;
          this.showTestResult();
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  showTestResult() {
    let activeModal = this.modalService.open(TestResultComponent, { container: 'body', windowClass: 'test-result-model', backdrop: 'static' })
    activeModal.componentInstance.data = this.testResult;
    activeModal.result.then((result) => {
    }, (reason) => {
    });
  }
}
