import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';
import { CourseService } from 'src/app/course/service/course.service';
import { LoginUserComponent } from 'src/app/layout/login-user/login-user.component';
import { Constants } from 'src/app/share/common/Constants';
import { DateUtils } from 'src/app/share/common/date-utils';
import { Configuration } from 'src/app/share/config/configuration';
import { MessageService } from 'src/app/share/service/message.service';
import { CommentService } from '../service/comment.service';
import { TestService } from '../service/test.service';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import * as moment from 'moment';
import { DataService } from 'src/app/layout/service/data.service';
import { CountdownComponent } from 'ngx-countdown';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-comment-font-end',
  templateUrl: './comment-font-end.component.html',
  styleUrls: ['./comment-font-end.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CommentFontEndComponent implements OnInit {

  constructor(
    private commentService: CommentService,
    public constant: Constants,
    public config: Configuration,
    private route: ActivatedRoute,
    private testService: TestService,
    private messageService: MessageService,
    private service: CourseService,
    private modalService: NgbModal,
    private dateUntils: DateUtils,
    private dataService: DataService,
    public router: Router,
    private elRef: ElementRef,
    private _sanitizer: DomSanitizer
  ) { }


  @ViewChild('cd', { static: false }) private countdown: CountdownComponent;
  @Output() myEvent = new EventEmitter();

  id: string;
  types: string;
  courseId: string;
  lessonId: string;
  lessonSlug: string;
  slug: string;
  listPageSize = [5, 10, 15, 20, 25, 30];
  startIndex = 1;
  learnerId: string;
  type: number;
  typeLessonFrame: number;
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

  model: any = {
    PageNumber: 1,
    PageSize: 10,
    TotalItems: 0,

    courseId: '',
    lessonId: '',
    lessonFrameId: '',
    userId: null
  }

  commnetModel: any = {
    id: '',
    parentCommentId: null,
    courseId: null,
    lessonId: null,
    lessonFrameId: null,
    objectId: null,
    type: 1,
    content: '',
    status: 0
  }

  courseModel: any = {
    id: '',
    name: '',
    listLessonCourse: []
  }

  listData: any = []

  modelGoogle: any = {
    Id: '',
    Id_Token: null
  }

  // Bài giảng trắc nghiệm

  index: number = 0;
  listDataExam: any[] = [
    { content: '' }
  ];
  listQuestion: any[] = [];
  startDate = null;
  dateStart = null;
  finishDate = null;
  date = new Date();
  totalCorrect: number = 0;
  totalQuestion: number = 0;
  isTest = false;
  lessionType: number;

  modelExam: any = {
    id: '',
    name: '',
    isExam: false,
    examTime: 3600,
    IsTest: false,
    listQuestion: []
  }

  question: any = {
    PageNumber: 1,
    PageSize: 1,
    TotalItems: 0,
  }

  testCreateModel: any = {
    CourseId: null,
    LearnerId: null,
    LessonId: null,
    StartDate: null,
    FinishDate: null,
    TotalQuestion: 0,
    TotalCorrect: 0,
    ListQuestion: []
  }
  testId: string;
  modelQuestionAnswer: any = {
    LearnerId: '',
    LessonId: '',
    CourseId: '',
    Type: null,
  }

  isDrackDrop = false;

  savetempmodel: any = {
    ListQuestion: []
  }

  finishtestmodel: any = {
    ListQuestion: [],
  }

  listQuestionAnswer = [];
  typeOld: number;
  row: any;
  name: string;
  heightType: number;
  ngOnInit() {
    window.scrollTo(0, 0);
    this.heightType = window.innerHeight - 90;
    this.dataService.currentLessonId.subscribe(
      (id: any) => {
        this.lessonId = id;
      }
    );
    this.lessonSlug = this.route.snapshot.paramMap.get('lesson');
    this.types = this.route.snapshot.paramMap.get('types');
    this.slug = this.route.snapshot.paramMap.get('slug');
    this.row = this.service.tripDetailsarray;
    this.learnerId = localStorage.getItem('user');
    if (this.learnerId) {
      this.commnetModel.objectId = this.learnerId;
      this.model.userId = this.learnerId;
    }
    forkJoin(
      this.commentService.getLessonCourse(this.slug, this.learnerId, this.courseModel.listLessonCourse),
    ).subscribe(([res1]) => {
      if (this.slug) {
        this.courseModel = res1.data;
        this.courseId = this.courseModel.id
        this.commnetModel.courseId = this.courseModel.id;
        this.modelQuestionAnswer.CourseId = this.courseModel.id;
        this.modelQuestionAnswer.LearnerId = this.learnerId;
        this.getLessonId();
      }
    });
  }

  getLessonCourse() {
    this.commentService.getLessonCourse(this.slug, this.learnerId, this.courseModel.listLessonCourse).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.courseModel = data.data;
          this.commnetModel.courseId = this.courseModel.id;
          this.courseId = this.courseModel.id;
        }
      });
  }

  getLessonId() {
    this.commentService.getLessonIdCourseSlug(this.lessonSlug).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.lessonId = data.data;
          if (this.lessonId) {
            this.courseModel.listLessonCourse.forEach(async element => {
              if (element.lessonId == this.lessonId) {
                if (element.type == 1 && element.listLessonFrame.length > 0) {
                  this.getLessonById(element.listLessonFrame[0].id, element.listLessonFrame[0].type, element.slug, this.lessonId, 2);
                } else if (element.type == 2) {
                  this.getLessonById(this.lessonId, element.type, element.slug, this.lessonId, 1);
                } else if (element.type == 4) {

                }
              }
            });
          }
          else {
            this.lessonId = this.courseModel.listLessonCourse[0].lessonId;
            this.modelQuestionAnswer.LessonId = this.courseModel.listLessonCourse[0].lessonId;
            if (this.courseModel.listLessonCourse[0].type == 1 && this.courseModel.listLessonCourse[0].listLessonFrame.length > 0) {
              this.getLessonById(this.courseModel.listLessonCourse[0].listLessonFrame[0].id, this.courseModel.listLessonCourse[0].listLessonFrame[0].type, this.courseModel.listLessonCourse[0].slug, this.lessonId, 2);
            } else if (this.courseModel.listLessonCourse[0].type == 2) {
              this.getLessonById(this.lessonId, this.courseModel.listLessonCourse[0].type, this.courseModel.listLessonCourse[0].slug, this.lessonId, 1);
            }
          }
        }
      });
  }

  totalTime: number;
  lessonFrameId: string;
  indexTest: number;
  getLessonById(id: string, type: number, slug: string, lessonId: string, indexTest: number) {
    if (type == 2 && !this.learnerId) {
      this.messageService.showMessage("Bạn phải đăng nhập trước khi tiến hành kiểm tra!");
      return;
    }

    this.lessonFrameId = id;
    this.indexTest = indexTest;
    this.router.navigate(['/my-course/lesson/bai-giang-ly-thuyet/' + this.slug, { lesson: slug }]);
    if (this.typeOld == 2 && !this.modelExam.finishDate && this.learnerId) {
      this.messageService.showConfirm("Bạn có chắc chắn muốn nộp bài không?").then(
        data => {
          this.finishtest();
          this.lessionById(id, type, lessonId);
        },
      );
    }
    else {
      this.typeOld = type;
      this.lessionById(id, type, lessonId)
    }
  }

  lessionById(id: string, type: number, lessonId: string) {
    this.listDataExam = [];
    this.listAnser = [];
    this.index = 0;
    this.modelQuestionAnswer.LessonId = lessonId;
    this.type = type;
    this.lessionType = type;
    this.lessonId = lessonId;
    this.commnetModel.lessonId = lessonId;
    this.testId = null;
    if (type == 1 || type == 3) {
      this.commnetModel.courseId = this.courseId;
      this.commentService.getLessonFrameInfo(id, this.commnetModel).subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.lessonModel = data.data;
            if (this.lessonModel.type == 3) {
              this.linkVideo(this.lessonModel.content);
            }
            this.typeOld = this.lessonModel.type;
            if (this.learnerId) {
              this.searchComment();
              this.getLessonCourse();
            }
          }
        });
    } else if (type == 2 && this.learnerId) {
      this.modelQuestionAnswer.Type = type;
      if (this.indexTest == 2) {
        this.getTestLessonFrameInfo(id);
      } else {
        forkJoin(
          this.testService.getListQuestionAnswer(this.modelQuestionAnswer)
        ).subscribe(([ress1]) => {
          this.listQuestionAnswer = ress1.data.dataResults
          this.isTime = ress1.data.isTime;
          this.dateStart = ress1.data.stateDate;
          this.totalTime = ress1.data.totalTime
          if (ress1.data.dataResults.length > 0) {
            this.testId = ress1.data.dataResults[0].testId;
          }
          this.getbyid();
        })
      }
    } else if (type == 4) {
      this.commentService.getLessonInfo(id, this.commnetModel).subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.lessonModel = data.data;
            if (this.lessonModel.type == 4 && this.lessonModel.imagePath) {
              this.linkVideo(this.config.ServerApi + this.lessonModel.imagePath);
            }
            this.typeOld = this.lessonModel.type;
            if (this.learnerId) {
              this.searchComment();
              this.getLessonCourse();
            }
          }
        });
    }
  }

  isTime: any;
  getListQuestionAnswer() {
    this.testService.getListQuestionAnswer(this.modelQuestionAnswer).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listQuestionAnswer = data.data.dataResults;
          this.isTime = data.data.isTime;
        }
      }, error => {
      }
    );
  }


  getbyid() {
    this.question = {
      PageNumber: 1,
      PageSize: 1,
      TotalItems: 0,
    }
    this.commnetModel.LearnerId = this.commnetModel.objectId;
    if (this.modelQuestionAnswer.Type == 3 && this.dateStart == null) {
      this.messageService.showConfirm("Bạn có muốn làm bài thi không?").then(
        data => {
          this.getInfoById();
        }
      );
    } else {
      this.getInfoById();
    }

  }
  isLessionQuession = true;
  getInfoById() {
    this.testService.getTestInfo(this.lessonId, this.commnetModel).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          var i = 0;
          var index = 0;
          this.modelExam = data.data;
          if (data.data.finishDate != null) {
            const duration = moment.duration(this.modelExam.examTime, 'seconds');
            const resultstring = moment.utc(duration.asMilliseconds()).format('HH:mm:ss');
            this.modelExam.examTime = resultstring
            this.isDrackDrop = true;
          }
          this.listDataExam = data.data.listQuestion;

          this.listDataExam.forEach(item => {
            index++;
            if (item.type == 1 || item.type == 2 || item.type == 3) {
              for (var element = 0; element < this.listDataExam[index - 1].listAnswer.length; element++) {
                if (this.listDataExam[index - 1].listAnswer[element].isCorrect != this.listDataExam[index - 1].listAnswer[element].isCorrectQuestion && this.listDataExam[index - 1].listAnswer[element].id == this.listDataExam[index - 1].listAnswer[element].answerLearnerId) {
                  this.listDataExam[index - 1].isLessionQuession = false;
                  break;
                }
              }
            }

            else if (item.type == 4) {
              for (var element = 0; element < this.listDataExam[index - 1].listAnswer.length; element++) {
                if (this.listDataExam[index - 1].listAnswer[element].answerContent != this.listDataExam[index - 1].listAnswer[element].answerContentQuestion && this.listDataExam[index - 1].listAnswer[element].id == this.listDataExam[index - 1].listAnswer[element].answerLearnerId) {
                  this.listDataExam[index - 1].isLessionQuession = false;
                  break;
                }
              }
            }

            else {
              for (var element = 0; element < this.listDataExam[index - 1].listAnswer.length; element++) {
                if (this.listDataExam[index - 1].listAnswer[element].displayIndex != this.listDataExam[index - 1].listAnswer[element].displayIndexQuestion && this.listDataExam[index - 1].listAnswer[element].id == this.listDataExam[index - 1].listAnswer[element].answerLearnerId) {
                  this.listDataExam[index - 1].isLessionQuession = false;
                  break;
                }
              }
            }
          })

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
            this.listDataExam[j].listAnswerQuession.forEach(element => {


              index = index + 1;
              if (item.type == 1) {
                if (element.isCorrectQuestion == true) {
                  element.isCorrectQuestion = 0;
                }
              }

              if (item.type == 3) {
                if (element.isCorrectQuestion == true) {
                  element.isCorrectQuestion = "true0";
                }
              }



            });
          })
          this.totalQuestion = this.listDataExam.length;
          this.startDate = new Date();
          this.getLessonCourse();
        }
      }, error => {
      }
    );
  }

  getTestLessonFrameInfo(id: string) {
    this.testService.getTestLessonFrameInfo(id, this.commnetModel).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          var i = 0;
          var index = 0;
          this.modelExam = data.data;
          this.listQuestionAnswer = this.modelExam.listAnswerQuession;
          if (data.data.finishDate != null) {
            const duration = moment.duration(this.modelExam.examTime, 'seconds');
            const resultstring = moment.utc(duration.asMilliseconds()).format('HH:mm:ss');
            this.modelExam.examTime = resultstring
            this.isDrackDrop = true;
          }
          this.listDataExam = data.data.listQuestion;

          this.listDataExam.forEach(item => {
            index++;
            if (item.type == 1 || item.type == 2 || item.type == 3) {
              for (var element = 0; element < this.listDataExam[index - 1].listAnswer.length; element++) {
                if (this.listDataExam[index - 1].listAnswer[element].isCorrect != this.listDataExam[index - 1].listAnswer[element].isCorrectQuestion && this.listDataExam[index - 1].listAnswer[element].id == this.listDataExam[index - 1].listAnswer[element].answerLearnerId) {
                  this.listDataExam[index - 1].isLessionQuession = false;
                  break;
                }
              }
            }

            else if (item.type == 4) {
              for (var element = 0; element < this.listDataExam[index - 1].listAnswer.length; element++) {
                if (this.listDataExam[index - 1].listAnswer[element].answerContent != this.listDataExam[index - 1].listAnswer[element].answerContentQuestion && this.listDataExam[index - 1].listAnswer[element].id == this.listDataExam[index - 1].listAnswer[element].answerLearnerId) {
                  this.listDataExam[index - 1].isLessionQuession = false;
                  break;
                }
              }
            }

            else {
              for (var element = 0; element < this.listDataExam[index - 1].listAnswer.length; element++) {
                if (this.listDataExam[index - 1].listAnswer[element].displayIndex != this.listDataExam[index - 1].listAnswer[element].displayIndexQuestion && this.listDataExam[index - 1].listAnswer[element].id == this.listDataExam[index - 1].listAnswer[element].answerLearnerId) {
                  this.listDataExam[index - 1].isLessionQuession = false;
                  break;
                }
              }
            }
          })

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
            this.listDataExam[j].listAnswerQuession.forEach(element => {


              index = index + 1;
              if (item.type == 1) {
                if (element.isCorrectQuestion == true) {
                  element.isCorrectQuestion = 0;
                }
              }

              if (item.type == 3) {
                if (element.isCorrectQuestion == true) {
                  element.isCorrectQuestion = "true0";
                }
              }



            });
          })
          this.totalQuestion = this.listDataExam.length;
          this.startDate = new Date();
          this.getLessonCourse();
        }
      }, error => {
      }
    );
  }

  reply(row: any) {
    row.repply = true;
  }

  searchComment() {
    this.model.courseId = this.courseModel.id;
    this.model.lessonId = this.lessonId;
    this.model.lessonFrameId = this.lessonFrameId;
    this.commentService.searchCommentFontEndCourse(this.model).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
          this.listData = data.data.dataResults;
          this.model.TotalItems = data.data.totalItems;
        }
      });
  }

  savetemp(value) {
    var i = 0;
    this.currentPage = this.question.PageNumber;
    this.savetempmodel.CourseId = this.courseId;
    this.savetempmodel.LearnerId = this.learnerId;
    this.savetempmodel.LessonId = this.lessonId;
    this.savetempmodel.Type = this.lessionType;
    this.savetempmodel.ListAnswer = this.listDataExam[value].listAnswer;
    this.savetempmodel.ListAnswer.forEach(item => {
      i = i + 1;
      item.displayIndex = i;
    })
    var listAnswerOld = this.listDataExam[value].listAnswer;
    var questionType = this.listDataExam[value].type;
    this.savetempmodel.QuestionId = this.listDataExam[value].id;
    if (this.modelExam.finishDate == null) {
      this.savetempmodel.ListAnswer.forEach(element => {
        if (element.isCorrect != false && (element.type == 1 || element.type == 3)) {
          element.isCorrect = true;
        }
      });
      this.commentService.savetemp(this.savetempmodel).subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.testId = data.data;

            var i = 0;
            this.savetempmodel.ListAnswer.forEach(element => {
              i++;
              listAnswerOld.forEach(item => {
                if (item.id == element.id && item.isCorrect == true) {
                  if (questionType == 1) {
                    element.isCorrect = i - 1;
                  }
                  if (questionType == 3) {
                    element.isCorrect = 'true' + (i - 1);
                  }
                }
              });
            });
          }
        },
      );
    }
  }



  finishtest() {
    // this.countdown = new CountdownComponent(null,null,null,null,null);
    this.countdown.pause();
    this.finishtestmodel.ListQuestion = this.listDataExam;
    this.finishtestmodel.CourseId = this.courseId;
    this.finishtestmodel.LearnerId = this.learnerId;
    this.finishtestmodel.LessonId = this.lessonId;
    this.finishtestmodel.Type = this.lessionType;
    this.finishtestmodel.ListQuestion.forEach(q => {
      var i = 0
      q.listAnswer.forEach(a => {
        if (q.type == 5) {
          i = i + 1;
          a.displayIndex = i;
        }
        if (q.type == 1 || q.type == 3) {
          if (a.isCorrect != false) {
            a.isCorrect = true;
          }
        }
      });
    });
    if (this.modelExam.finishDate == null && this.indexTest == 2) {
      this.testService.finishTestLessonFrame(this.lessonFrameId, this.finishtestmodel).subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            // this.messageService.showMessage("Bạn đã hoàn thành bài giảng trắc nghiệm với kết quả: " + this.totalCorrect + "/" + this.totalQuestion);
            this.messageService.showMessage("Bạn đã hoàn thành bài giảng trắc nghiệm với kết quả: " + data.data.totalRightAnswer + "/" + data.data.totalQuestion);
            // this.modelExam.testId = data.data.testId
            this.modelExam.finishDate = data.data.finishDate;
            //this.getTestLessonFrameInfo(this.lessonFrameId);
          }
        },
      );
    } else if (this.modelExam.finishDate == null && this.indexTest == 1) {
      this.commentService.finishtest(this.finishtestmodel).subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            // this.messageService.showMessage("Bạn đã hoàn thành bài giảng trắc nghiệm với kết quả: " + this.totalCorrect + "/" + this.totalQuestion);
            this.messageService.showMessage("Bạn đã hoàn thành bài giảng trắc nghiệm với kết quả: " + data.data.totalRightAnswer + "/" + data.data.totalQuestion);
            // this.modelExam.testId = data.data.testId
            this.modelExam.finishDate = data.data.finishDate;
            //this.getInfoById();
          }
        },
      );
    }

  }

  create(id: string, content: string, row: any) {
    this.commnetModel.parentCommentId = id;
    this.commnetModel.lessonFrameId = this.lessonFrameId;
    if (this.learnerId == null) {
      let activeModal = this.modalService.open(LoginUserComponent, { container: 'body', windowClass: 'signIn-model', backdrop: 'static' })
      activeModal.result.then((result) => {
        if (result) {
        }
      });
    }
    if (content != null) {
      this.commnetModel.content = content;
    }
    if (this.commnetModel.content && this.commnetModel.content != '') {
      this.commentService.createCommentFontEnd(this.commnetModel).subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            if (id) {
              row.repply = false;
            }
            this.commnetModel = {
              id: '',
              parentCommentId: null,
              courseId: this.courseModel.id,
              lessonId: this.lessonId,
              lessonFrameId: this.lessonFrameId,
              objectId: this.learnerId,
              type: 1,
              content: '',
              status: 0
            }
            this.searchComment();
          }
        });
    }
    else {
      this.messageService.showMessage("Bạn chưa nhập thông tin câu hỏi!");
    }

  }

  currentPage = 1;
  clickQuestion() {
    this.index = this.question.PageNumber - 1;
    if (this.indexTest == 1) {
      this.savetemp(this.currentPage - 1);
    }
  }

  showConfirm() {
    this.messageService.showConfirm("Bạn có chắc chắn muốn nộp bài không?").then(
      data => {
        this.finishtest();
      });
  }

  clickIndex(index: number) {
    this.index = index
  }

  handleEvent(event: any) {
    if (event.action == "done") {
      this.finishtest();
    }
  }

  listAnser = [];

  onChangeRadio(row: any, list: any) {
    list.forEach(element => {
      if (row.id != element.id) {
        element.isCorrect = row.isCorrect;
      }
      else {
        element.isCorrect = false;
      }
    });
  }

  // drack and drop

  drop(event: CdkDragDrop<string[]>) {
    this.listAnser = [];
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
    }
    var i = 0;
    this.listAnser = event.container.data;
    this.listAnser.forEach(item => {
      i = i + 1;
      item.displayIndex = i;
    })
  }

  clickTab(tab: number) {
    this.tabIndex = tab;
  }

  selectIndex = -1;
  loadValue(row: any, data: any, index: number) {
    if (index == 2) {
      this.selectIndex = row.id;
      if (!this.learnerId) {
        row.status = true;
        var a = data.listLessonFrame.filter((i: { status: boolean; }) => i.status == true);
        if (a.length > 0) {
          let d = (a.length) / (data.listLessonFrame.length) * 100;
          data.percent = Math.round(d);
        }
      }
    } else if (index == 1) {
      this.selectIndex = row.lessonId;
      if (this.learnerId) {
        row.percent = 100;
      }
    }
  }

  collapse(row: any) {
    row.col = !row.col ? row.col = true : row.col = false;
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
