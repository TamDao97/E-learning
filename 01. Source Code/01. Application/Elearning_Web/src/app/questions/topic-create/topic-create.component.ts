import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, FileProcess, Constants, DateUtils } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService } from 'src/app/shared/services/combobox.service';
import { QuestionsService } from '../service/questions.service';
import { TopicService } from 'src/app/education-programs/services/topic.service';
@Component({
  selector: 'app-topic-create',
  templateUrl: './topic-create.component.html',
  styleUrls: ['./topic-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TopicCreateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private comboboxService: ComboboxService,
    private questionService: QuestionsService,
    private topicService: TopicService
  ) { }

  modalInfo = {
    Title: 'Thêm mới chủ đề',
  };

  id: string;
  isAction: boolean = false;
  topics: any = [];
  model: any = {
    name: '',
    parentTopicId: null,
  }

  ngOnInit(): void {
    this.getTopicFull();
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật chủ đề';
      this.getbyid();
    } else {
      this.modalInfo.Title = 'Thêm mới chủ đề';
    }
  }

  getTopicFull() {
    this.questionService.getTopicFull().subscribe(data => {
      this.topics = data.data;
    });
  }

  getbyid() {
    this.topicService.gettopicInfo(this.id).subscribe(
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

  saveAndContinue() {
    this.save(true);
  }

  create(isContinue) {
    this.topicService.createtopic(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thêm mới chủ đề thành công!');
          if (isContinue) {
            this.isAction = true;
            this.clear();
            this.getTopicFull();
          } else {
            this.closeModal(true);
          }
        } else {
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  save(isContinue: boolean) {

    if (this.id) {
      this.update();
    } else {
      this.create(isContinue);
    }
  }

  update() {
    this.topicService.updatetopic(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

          this.messageService.showSuccess('Cập nhập chủ đề thành công!');
          this.closeModal(true);
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  clear() {
    this.model = {
      id: '',
      name: '',
      patentTopicId: null,
    }
  }
}
