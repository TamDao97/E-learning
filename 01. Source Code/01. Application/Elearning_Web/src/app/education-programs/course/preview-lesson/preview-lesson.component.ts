import { Component, ElementRef, OnInit, ViewEncapsulation } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileProcess, MessageService, Constants, Configuration } from 'src/app/shared';
import { CourseService } from '../../services/course.service';
import { LessonService } from '../../services/lesson.service';

@Component({
  selector: 'app-preview-lesson',
  templateUrl: './preview-lesson.component.html',
  styleUrls: ['./preview-lesson.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PreviewLessonComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public fileProcess: FileProcess,
    private messageService: MessageService,
    public constant: Constants,
    public config: Configuration,
    private courseService: CourseService,
    private lessonService: LessonService,
    private elRef: ElementRef,
    private _sanitizer: DomSanitizer
  ) { }

  id: string;
  nameCourse: string;
  courseId: string;
  type: number;
  totalQuestion: number;
  listLesson: any[] = [];
  listDataExam: any[] = [];
  date = new Date();
  tabIndex = 1;
  lessonModel: any = {
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
  }

  question = {
    PageNumber: 1,
    PageSize: 1,
    TotalItems: 0,
  }

  modelExam: any = {
    id: '',
    name: '',
    isExam: false,
    examTime: 3600,
    IsTest: false,
    listQuestion: []
  }

  modalInfo = {
    title: 'Xem bài giảng',
  };

  heightType: number;
  ngOnInit(): void {
    this.heightType = window.innerHeight - 200;
    if (this.courseId) {
      this.searchLessonByCourseId();
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

  getLessonById(id: string) {
    var data = this.listLesson.filter(i => i.lessonId == id);
    if (data.length > 0 && data[0].listLessonFrame.length > 0) {
      this.lessonModel = data[0].listLessonFrame[0];
      if (this.lessonModel.type == 3) {
        this.type = 3;
        this.linkVideo(this.lessonModel.content);
      } else if (this.lessonModel.type == 4) {
        this.type = 4;
        if (this.lessonModel.imagePath) {
          this.linkVideo(this.config.ServerApi + this.lessonModel.imagePath);
        }
      }
    }
    // this.lessonService.getLessonInfo(id).subscribe(
    //   data => {
    //     if (data.statusCode == this.constant.StatusCode.Success) {
    //       this.lessonModel = data.data;
    //     }
    //     else {
    //       this.messageService.showListMessage(data.message);
    //     }
    //   }, error => {
    //     this.messageService.showError(error);
    //   }
    // );
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
          this.modelExam.name = data.data.name;
          this.modelExam.examTime = data.data.examTime;
          //this.lessonModel.name = data.data.name;
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
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  searchLessonByCourseId() {
    this.courseService.searchLessonByCourseId(this.courseId).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listLesson = data.data.dataResults;
          if (this.id) {
            this.select(this.id, this.type)
          }
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK);
  }

  clickTab(index) {
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
}
