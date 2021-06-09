import {
  Component,
  OnInit,
  ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginUserComponent } from 'src/app/layout/login-user/login-user.component';
import { DateUtils } from 'src/app/share/common/date-utils';
import { Configuration } from 'src/app/share/config/configuration';
import { MessageService } from 'src/app/share/service/message.service';
import { CourseService } from '../service/course.service';
import $ from "jquery";

import { Constants } from 'src/app/share/common/Constants';
import { ProgramService } from 'src/app/layout/service/program.service';
import { DataService } from 'src/app/layout/service/data.service';
import { CommentService } from 'src/app/lesson/service/comment.service';
@Component({
  selector: 'app-course-details',
  templateUrl: './course-details.component.html',
  styleUrls: ['./course-details.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class CourseDetailsComponent implements OnInit {
  constructor(
    public config: Configuration,
    private service: CourseService,
    private route: ActivatedRoute,
    public router: Router,
    public dateUntil: DateUtils,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    public programService: ProgramService,
    private data: DataService,
    private commentService: CommentService
  ) { }

  learnerId = "";
  objectId = "";
  isLogin = false;
  dateNow = new Date();
  slug: string;
  id:string;
  model: any = {};
  totalRelatedCourse = 0;
  startIndex = 1;
  listData: any[] = [];
  modelLearnerCourse: any = {
    learnerId: '',
    slug: '',
  }

  commnetModel: any = {
    id: '',
    parentCommentId: null,
    courseId: null,
    lessonId: null,
    objectId: null,
    type: 1,
    content: '',
    status: 0
  }

  searchModel: any = {
    PageNumber: 1,
    PageSize: 10,
    TotalItems: 0,

    courseId: '',
    userId: ''
  }

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.slug = this.route.snapshot.paramMap.get('slug');
    this.data.currentLearnerId.subscribe(
      (id: any) => {
        if (id && id != "") {
          this.learnerId = id;
          this.isLogin = true;
        }
        if (id == "") {
          this.learnerId = id;
          this.isLogin = false;
        }
        else {
          this.learnerId = localStorage.getItem('user');
          if (this.learnerId != "")
            this.isLogin = true;
        }
        this.getCourseById();
      }
    );
  }

  getCourseById() {
    window.scrollTo(0, 0);
    this.modelLearnerCourse.slug = this.slug;
    this.modelLearnerCourse.learnerId = this.learnerId;
    this.service.getCourseById(this.modelLearnerCourse).subscribe((data) => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.model = data.data;
        this.totalRelatedCourse = data.data.listRelatedCourse.length;
        this.id=data.data.id;
        this.objectId = localStorage.getItem('user');
        this.commnetModel.courseId = this.id;
        this.commnetModel.lessonId = this.id;
        this.commnetModel.objectId = this.objectId;
        this.searchModel.courseId = this.id;
        this.searchModel.userId = this.objectId;
        this.searchComment();
      }
      else {
        this.router.navigate(['/404']);
      }
    },
      error => {
        this.router.navigate(['/404']);
      });
  }

  redirectToLink(slug: string) {
    // this.router.navigate(['/chi-tiet-khoa-hoc', id]);
    this.slug = slug;
    this.getCourseById();
  }

  showCourseDetail(item, i) {
    this.router.navigate(['/my-course/lesson/bai-giang-ly-thuyet/'+this.slug, {lesson:item.slug}]);
    this.data.changeLessonIdId(item.id);
    this.learnerId = localStorage.getItem('user');
   
  }

  endCourse() {
    this.messageService.showMessage("Khóa học đã kết thúc. Bạn không thể đăng ký tham gia khóa học này được nữa!");
  }

  register(slug) {
    this.programService.createLearnerCourse(this.modelLearnerCourse).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Đăng ký khóa học thành công!');
          this.router.navigate(['chi-tiet-khoa-hoc', slug]);
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
        this.objectId = localStorage.getItem('user');
        $(document).ready(function () {
          location.reload(true);
        });
      }
    });
  }

  handling(id, isLearned, finishDate,slug) {
    this.learnerId = localStorage.getItem('user');
    this.modelLearnerCourse = {
      learnerId: this.learnerId,
      courseId: id,
    }
    if (this.learnerId) {
      this.isLogin == true;
      if (isLearned == false && this.dateNow.getTime() <= this.dateUntil.getTimetoDateString(finishDate)) {
        this.register(slug);
      }
      if (isLearned == true) {
        this.router.navigate(['chi-tiet-khoa-hoc', slug]);
      }
      if (isLearned == false && this.dateNow.getTime() > this.dateUntil.getTimetoDateString(finishDate)) {
        this.ngOnInit();
      }
    }
    else {
      this.isLogin = false;
      this.signIn();
    }
  }

  reply(row: any) {
    row.repply = true;
  }

  searchComment() {
    this.commentService.searchCommentFontEndCourse(this.searchModel).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
          this.listData = data.data.dataResults;
          this.searchModel.TotalItems = data.data.totalItems;
        }
      });
  }

  create(id: string, content: string, row: any) {
    this.commnetModel.parentCommentId = id;
    if (!this.objectId) {
      this.signIn();
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
              courseId: this.id,
              lessonId: this.id,
              objectId: this.objectId,
              type: 1,
              content: '',
              status: 0
            }
            this.searchComment();
          }
          //else {
          //     this.messageService.showListMessage(data.message);
          //   }
          // },
          // error => {
          //   this.messageService.showError(error);
        });
    }
    else {
      this.messageService.showMessage("Bạn chưa nhập thông tin câu hỏi!");
    }

  }

}
