import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FileProcess, MessageService, Constants, Configuration } from 'src/app/shared';
import { LessonService } from '../../services/lesson.service';

@Component({
  selector: 'app-lesson-history',
  templateUrl: './lesson-history.component.html',
  styleUrls: ['./lesson-history.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LessonHistoryComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public fileProcess: FileProcess,
    private messageService: MessageService,
    public constant: Constants,
    public config: Configuration,
    private lessonService: LessonService,
  ) { }

  id: string;
  listHistory: any[] = [];
  modalInfo = {
    title: 'Xem lịch sử',
  };
  
  @ViewChild('scrollHeaderOne') scrollHeaderOne: ElementRef;

  ngOnInit(): void {
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.approvalHistory();
  }

  approvalHistory() {
    this.lessonService.approvalHistory(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listHistory = data.data;
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
