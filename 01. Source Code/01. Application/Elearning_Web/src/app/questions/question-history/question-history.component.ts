import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FileProcess, MessageService, Constants, Configuration } from 'src/app/shared';
import { QuestionsService } from '../service/questions.service';

@Component({
  selector: 'app-question-history',
  templateUrl: './question-history.component.html',
  styleUrls: ['./question-history.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class QuestionHistoryComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public fileProcess: FileProcess,
    private messageService: MessageService,
    public constant: Constants,
    public config: Configuration,
    private questionsService: QuestionsService,
  ) { }

  id: string;
  listQuestion: any[] = [];
  modalInfo = {
    title: 'Xem lịch sử',
  };
  @ViewChild('scrollHeaderQuestion') scrollHeaderQuestion: ElementRef;

  ngOnInit(): void {
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderQuestion.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.approvalHistory();
  }

  approvalHistory() {
    this.questionsService.approvalHistory(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listQuestion = data.data;
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

}
