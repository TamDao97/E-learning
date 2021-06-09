import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CountdownComponent } from 'ngx-countdown';
import { Constants } from 'src/app/share/common/Constants';
import { Configuration } from 'src/app/share/config/configuration';
import { TestService } from '../service/test.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestComponent implements OnInit {
  @ViewChild('cd', { static: false }) private countdown: CountdownComponent;

  constructor(
    public constant: Constants,
    private testService: TestService,
    public config: Configuration,
    private route: ActivatedRoute
  ) { }

  id: string;
  courseId: string;
  lessonId: string;
  index: number = 0;
  listData: any[] = [
    { content: null }
  ];
  listQuestion: any[] = [];
  startDate = null;
  finishDate = null;
  date = new Date();
  totalCorrect: number = 0;
  totalQuestion: number = 0;
  isTest = false;

  model: any = {
    id: '',
    name: '',
    isExam: false,
    examTime: 3600,
    listQuestion: []
  }

  question: any = {
    PageNumber: 5,
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

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    this.getbyid();
  }

  getbyid() {
    this.testService.getTestInfo(this.id, null).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          this.listData = data.data.listQuestion;
          this.totalQuestion = this.listData.length;
          this.startDate = new Date();
        }
        // else {
        //   this.messageService.showListMessage(data.message);
        // }
      }, error => {
        // this.messageService.showError(error);
      }
    );
  }

  back() {
    if (this.index > 0 && this.isTest == false) {
      this.index--;
    }
  }

  next() {
    if (this.index < this.listData.length - 1 && this.isTest == false) {
      this.index++;
    }
  }

  done() {
    if (this.model.isExam) {
      this.countdown.pause();
    }
    this.finishDate = new Date();
    this.isTest = true;
    var check = true;
    this.totalCorrect = 0;
    this.listData.forEach(element => {
      check = true;
      for (var item of element.listAnswer) {
        if (element.type == 4) {
          if (item.checked != item.answerContent) {
            check = false;
            break;
          }
        } else {
          if (item.checked && item.isCorrect) {
            check = true;
          } else if (!item.checked && item.isCorrect) {
            check = false;
            break;
          }
        }
      }
      if (check) {
        this.totalCorrect++;
      }
    });
    this.create();
  }

  clickIndex(index: number) {
    this.index = index
  }

  handleEvent(event: any) {
    if (event.action == "done") {
      this.done();
    }
  }

  create() {
    this.testCreateModel = {
      CourseId: this.courseId,
      LearnerId: null,
      LessonId: this.lessonId,
      StartDate: this.startDate,
      FinishDate: this.finishDate,
      TotalQuestion: this.totalQuestion,
      TotalCorrect: this.totalCorrect,
      ListQuestion: this.listData
    }

    this.testService.createTest(this.testCreateModel).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

        }
        //else {
        //   this.messageService.showListMessage(data.message);
        // }
      },
      // error => {
      //   this.messageService.showError(error);
      // }
    );
  }

  closeModal() {

  }
}
