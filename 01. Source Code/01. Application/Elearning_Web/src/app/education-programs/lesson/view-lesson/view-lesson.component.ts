import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Component, ElementRef, OnInit, ViewEncapsulation } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileProcess, MessageService, Constants, Configuration } from 'src/app/shared';
import { CourseStatusComponent } from '../../course/course-status/course-status.component';
import { CourseService } from '../../services/course.service';
import { LessonService } from '../../services/lesson.service';

@Component({
  selector: 'app-view-lesson',
  templateUrl: './view-lesson.component.html',
  styleUrls: ['./view-lesson.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ViewLessonComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public fileProcess: FileProcess,
    private messageService: MessageService,
    public constant: Constants,
    public config: Configuration,
    private courseService: CourseService,
    private lessonService: LessonService,
    private modalService: NgbModal,
    private elRef: ElementRef,
    private _sanitizer: DomSanitizer
  ) { }

  id: string;
  type: number;
  tabIndex = 1;
  nameCourse: '';
  totalQuestion: number;
  listLesson: any[] = [];
  listDataExam: any[] = [];
  date = new Date();
  lessonModel: any = {
    id: '',
    categoryId: null,
    name: '',
    description: '',
    content: '',
    imagePath: '',
    status: true,
    type: null,
    isExam: false,
    examTime: null,
    listQuestion: []
  }

  commnetModel: any =
    { content: '' }

  statusModel: any = {
    status: null,
    content: ''
  }

  modelExam: any = {
    id: '',
    name: '',
    isExam: false,
    examTime: 3600,
    IsTest: false,
    listQuestion: []
  }

  question = {
    PageNumber: 1,
    PageSize: 1,
    TotalItems: 0,
  }

  modalInfo = {
    title: 'Xem bài giảng',
  };
  heightType: number;
  approvalStatus: number;

  ngOnInit(): void {
    this.heightType = window.innerHeight - 200;
    if (this.id) {
      this.getLessonByIdApprovalStatus(this.id);
      this.select(this.id, this.type);
    }
  }

  select(id: string, type: number) {
    if (type == 1 || type == 4) {
      this.getLessonById(id);
    } else {
      this.exam(id);
    }
  }

  index = 0;
  clickQuestion() {
    this.index = this.question.PageNumber - 1;
  }

  getLessonByIdApprovalStatus(id: string) {
    this.lessonService.getLessonInfo(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.approvalStatus = data.data.approvalStatus;
        }
        else {
          this.closeModal(true);
          // this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getLessonById(id: string) {
    this.lessonService.getLessonInfo(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listLesson.push(data.data);
          if (data.data.listLessonFrame.length > 0) {
            this.lessonModel = data.data.listLessonFrame[0];
            if (this.lessonModel.type == 3) {
              this.type = 3;
              this.linkVideo(this.lessonModel.content);
            }
          }
          if (data.data.type == 4) {
            this.type = 4;
            if (data.data.imagePath) {
              this.linkVideo(this.config.ServerApi + data.data.imagePath);
            }
          }
        }
        else {
          this.closeModal(true);
          // this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  exam(id: string) {
    this.question = {
      PageNumber: 1,
      PageSize: 1,
      TotalItems: 0,
    }
    this.courseService.exam(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listDataExam = data.data.listQuestion;
          this.lessonModel.name = data.data.name;
          this.lessonModel.type = data.data.type;
          this.lessonModel.examTime = data.data.examTime;
          this.lessonModel.approvalStatus = data.data.approvalStatus;
          this.lessonModel.time = data.data.examTime;
          this.listLesson.push(this.lessonModel);
          this.modelExam.name = data.data.name;
          this.modelExam.examTime = data.data.examTime;
          var i = 0;
          this.listDataExam.forEach(item => {
            i = i + 1;
            var j = i - 1;
            var index = 0;
            this.listDataExam[j].listAnswer.forEach(element => {
              index = index + 1;
              if (item.type == 1) {
                if (element.isCorrect == true) {
                  element.isCorrect = index - 1;
                }
              }
              if (item.type == 3) {
                if (element.isCorrect == true) {
                  element.isCorrect = "true" + (index - 1);
                }
              }
            });
          })
          this.totalQuestion = this.listDataExam.length;
        }
        else {
          this.closeModal(true);
          //this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK);
  }

  requestLesson() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        if (result != true) {
          this.statusModel.content = result;
        }
        this.statusModel.status = 1;
        this.lessonService.requestLesson(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Yêu cầu duyệt bài giảng thành công!');
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

  approvalLesson() {
    this.messageService.showConfirm("Bạn có chắc muốn duyệt bài giảng này không?").then(
      data => {
        this.statusModel.status = 2;
        this.lessonService.approvalLesson(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Duyệt bài giảng thành công!');
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

  notApprovalLesson() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.statusModel.status = 4;
        if (result != true) {
          this.statusModel.content = result;
        }
        this.lessonService.approvalLesson(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Không duyệt bài giảng thành công!');
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

  canceLesson() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.statusModel.status = 3;
        if (result != true) {
          this.statusModel.content = result;
        }
        this.lessonService.approvalLesson(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Hủy duyệt bài giảng thành công!');
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

  clickTab(index: number) {
    this.tabIndex = index;
  }

  clickLesson(row: any, index: number) {
    this.lessonModel = {
      id: '',
      categoryId: null,
      name: '',
      description: '',
      content: '',
      imagePath: '',
      status: true,
      type: null,
      isExam: true,
      examTime: null,
      listQuestion: []
    };
    this.type = row.type;
    if (index == 1) {
      if (row.type == 2) {
        this.modelExam = row;
        this.exam(row.lessonId);
      } else if (row.type == 4 && row.imagePath) {
        this.linkVideo(this.config.ServerApi + row.imagePath);
      }
    } else if (index == 2) {
      if (row.type == 1) {
        this.lessonModel = row;
      } else if (row.type == 2) {
        this.listDataExam = row.listQuestion;
        this.lessonModel = row;
        this.modelExam.name = row.name;
        this.modelExam.examTime = row.testTime;
        var i = 0;
        this.listDataExam.forEach(item => {
          i = i + 1;
          var j = i - 1;
          var index = 0;
          this.listDataExam[j].listAnswer.forEach(element => {
            index = index + 1;
            if (item.type == 1) {
              if (element.isCorrect == true) {
                element.isCorrect = index - 1;
              }
            }
            if (item.type == 3) {
              if (element.isCorrect == true) {
                element.isCorrect = "true" + (index - 1);
              }
            }
          });
        })
        this.totalQuestion = this.listDataExam.length;
      } else if (row.type == 3) {
        this.linkVideo(row.content);
      }

    }
  }

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
    }
  }

  link: any;
  linkVideo(url: string) {
    if (!url) {
      return;
    }

    if (url.indexOf("www.youtube.com") !== -1) {
      const videoId = this.getVideoId(url);
      this.link = 'https://www.youtube.com/embed/' + videoId;
      this.link = this._sanitizer.bypassSecurityTrustResourceUrl(this.link);
    } else {
      this.link = this._sanitizer.bypassSecurityTrustResourceUrl(url);
    }

    const player = this.elRef.nativeElement.querySelector('video');
    if (player) {
      player.load();
    }
  }

  getVideoId(url: any) {
    const regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#&?]*).*/;
    const match = url?.match(regExp);

    return (match && match[2].length === 11)
      ? match[2]
      : null;
  }

  showConfirm() {
    this.messageService.showConfirm("Bạn có chắc muốn nộp bài không?").then(
      data => {
      });
  }
}
