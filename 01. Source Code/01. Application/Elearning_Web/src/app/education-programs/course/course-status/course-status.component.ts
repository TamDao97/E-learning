import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, AppSetting, MessageService } from 'src/app/shared';

@Component({
  selector: 'app-course-status',
  templateUrl: './course-status.component.html',
  styleUrls: ['./course-status.component.scss']
})
export class CourseStatusComponent implements OnInit {

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
  ) { }

  content: string = '';
  modalInfo = {
    Title: 'Nội dung',
  };

  ngOnInit(): void {
  }

  save() {
    if (this.content) {
      this.activeModal.close(this.content);
    } else {
      this.activeModal.close(true);
    }
  }

  closeModal() {
    this.activeModal.close(false);
  }

}
