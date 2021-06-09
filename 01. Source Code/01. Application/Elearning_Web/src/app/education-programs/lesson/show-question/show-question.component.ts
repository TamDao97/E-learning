import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { QuestionsService } from 'src/app/questions/service/questions.service';
import { Constants, AppSetting, MessageService } from 'src/app/shared';
import { CourseStatusComponent } from '../../course/course-status/course-status.component';

@Component({
  selector: 'app-show-question',
  templateUrl: './show-question.component.html',
  styleUrls: ['./show-question.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowQuestionComponent implements OnInit {

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private questionsService: QuestionsService,
    private modalService: NgbModal,
  ) { }

  id: string;
  type: number;
  model: any = {
    id: '',
    topicId: null,
    type: 1,
    status: false,
    content: '',
    listAnswer: []
  }

  statusModel: any = {
    status: null,
    content: ''
  }

  ngOnInit(): void {
    if (this.id) {
      this.getbyid();
    }
  }

  getbyid() {
    this.questionsService.getQuestionById(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  requestQuestion() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        if (result != true) {
          this.statusModel.content = result;
        }
        this.statusModel.status = 1;
        this.questionsService.requestQuestion(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Yêu cầu duyệt câu hỏi thành công!');
              this.activeModal.close(true);
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

  approvalQuestion() {
    this.messageService.showConfirm("Bạn có chắc muốn duyệt câu hỏi này không?").then(
      data => {
        this.statusModel.status = 2;
        this.questionsService.approvalQuestion(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Duyệt câu hỏi thành công!');
              this.activeModal.close(true);
            }
            else {
              this.messageService.showListMessage(data.message);
            }
          }, error => {
            this.messageService.showError(error);
          });
      });
  }

  notApprovalQuestion() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.statusModel.status = 4;
        if (result != true) {
          this.statusModel.content = result;
        }
        this.questionsService.approvalQuestion(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Không duyệt câu hỏi thành công!');
              this.activeModal.close(true);
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

  canceQuestion() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.statusModel.status = 3;
        if (result != true) {
          this.statusModel.content = result;
        }
        this.questionsService.approvalQuestion(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Hủy duyệt câu hỏi thành công!');
              this.activeModal.close(true);
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

  closeModal() {
    this.activeModal.close();
  }

}
