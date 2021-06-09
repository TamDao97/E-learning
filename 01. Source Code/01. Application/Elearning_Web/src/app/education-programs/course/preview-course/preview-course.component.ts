import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileProcess, MessageService, Constants, Configuration } from 'src/app/shared';
import { LessonCreateComponent } from '../../lesson/lesson-create/lesson-create.component';
import { CourseService } from '../../services/course.service';
import { CourseStatusComponent } from '../course-status/course-status.component';
import { PreviewLessonComponent } from '../preview-lesson/preview-lesson.component';

@Component({
  selector: 'app-preview-course',
  templateUrl: './preview-course.component.html',
  styleUrls: ['./preview-course.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PreviewCourseComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public fileProcess: FileProcess,
    private messageService: MessageService,
    public constant: Constants,
    public config: Configuration,
    private courseService: CourseService,
    private modalService: NgbModal,
  ) { }

  id: string;
  nameCourse: string;
  listLesson: any[] = [];
  listMentor: any[] = [];
  model: any = {
    id: '',
    name: '',
    status: false,
    description: '',
    content: '',
    imagePath: '',
    startDate: null,
    finishDate: null,
    programId: null,
    employeeCourses: [],
    learnerCourses: [],
    lessonCourses: [],
    isDelete: false,
    displayIndex: null,
    certificateTemplateId: null
  };

  statusModel: any = {
    status: null,
    content: ''
  }

  modalInfo = {
    title: 'Xem khóa học',
  };

  ngOnInit(): void {
    this.getInfo();
    this.searchLessonByCourseId();
    this.getMentorById();
  }

  getInfo() {
    this.courseService.getCourseInfo(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
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

  searchLessonByCourseId() {
    this.courseService.searchLessonByCourseId(this.id).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listLesson = data.data.dataResults;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getMentorById() {
    this.courseService.searchMentor(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listMentor = data.data.dataResults;
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

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK);
  }

  requestCourse() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        if (result != true) {
          this.statusModel.content = result;
        }
        this.statusModel.status = 1;
        this.courseService.requestCourse(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Yêu cầu duyệt khóa học thành công!');
              this.closeModal(true);
            }
            else {
              this.messageService.showListMessage(data.message);
            }
          }, error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  approvalCourse() {
    this.messageService.showConfirm("Bạn có chắc muốn duyệt khóa học này không?").then(
      data => {
        this.statusModel.status = 2;
        this.courseService.approvalCourse(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Duyệt khóa học thành công!');
              this.closeModal(true);
            }
            else {
              this.messageService.showListMessage(data.message);
            }
          }, error => {
            this.messageService.showError(error);
          });
      });
  }

  notApprovalCourse() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.statusModel.status = 4;
        if (result != true) {
          this.statusModel.content = result;
        }
        this.courseService.approvalCourse(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Không duyệt khóa học thành công!');
              this.closeModal(true);
            }
            else {
              this.messageService.showListMessage(data.message);
            }
          }, error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  canceCourse() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.statusModel.status = 3;
        if (result != true) {
          this.statusModel.content = result;
        }
        this.courseService.approvalCourse(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Hủy duyệt khóa học thành công!');
              this.closeModal(true);
            }
            else {
              this.messageService.showListMessage(data.message);
            }
          }, error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  previewLesson(id: string, type: number) {
    let activeModal = this.modalService.open(PreviewLessonComponent, { container: 'body', windowClass: 'preview-lesson-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.courseId = this.id;
    activeModal.componentInstance.type = type;
    activeModal.componentInstance.nameCourse = this.model.name;
    activeModal.result.then((result) => {
      if (result) {

      }
    });
  }
}
