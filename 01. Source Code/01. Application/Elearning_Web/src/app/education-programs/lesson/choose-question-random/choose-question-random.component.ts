import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants } from 'src/app/shared';
import { ComboboxService } from 'src/app/shared/services/combobox.service';

@Component({
  selector: 'app-choose-question-random',
  templateUrl: './choose-question-random.component.html',
  styleUrls: ['./choose-question-random.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseQuestionRandomComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    public constant: Constants,
  ) { }

  modalInfo = {
    Title: 'Cấu hình chọn ngẫu nhiên',
  };
  listData: any[] = [];

  model: any = {
    number: 0,
    listData: []
  }

  ngOnInit() {
    this.getTopic();
  }

  getTopic() {
    this.comboboxService.getTopicFull().subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listData = data.data;
        } else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  chooseQuestion() {
    if (this.model.number <= 0) {
      this.messageService.showMessage("Số lượng câu hỏi phải lớn hơn 0");
      return;
    }
    this.activeModal.close(this.model);
  }

  closeModal() {
    this.activeModal.close(false);
  }
}
