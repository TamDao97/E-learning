import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { CourseService } from '../services/course.service';

@Component({
  selector: 'app-details-result',
  templateUrl: './details-result.component.html',
  styleUrls: ['./details-result.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DetailsResultComponent implements OnInit {

  constructor(
    public constant: Constants,
    public config: Configuration,
    public appSetting: AppSetting,
    private activeModal: NgbActiveModal,
    private courseService: CourseService,
    private messageService: MessageService,
  ) { }

  testId:string;
  ngOnInit(): void {
    this.getAllLesson();
  }
  listData: any[] = [];
  getAllLesson() {
    this.courseService.getQuestion(this.testId).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listData = data.data;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
  closeModal() {
    this.activeModal.close(true);
  }
}
