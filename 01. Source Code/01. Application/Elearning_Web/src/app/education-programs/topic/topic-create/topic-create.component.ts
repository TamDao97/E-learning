import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, FileProcess, Constants, DateUtils } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TopicService } from '../../services/topic.service';
import { ComboboxService } from 'src/app/shared/services/combobox.service';
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
    public fileProcess: FileProcess,
    public constant: Constants,
    public fileProcessImage: FileProcess,
    public fileProcessDataSheet: FileProcess,
    public dateUtils: DateUtils,
    private activeModal: NgbActiveModal,
    private topicService: TopicService,
    private comboboxService: ComboboxService,
  ) { }
  listCategory: any[] = [];
  modalInfo = {
    Title: 'Thêm mới chủ đề',
  };
  isAction: boolean = false;
  model: any = {
    id: '',
    name: '',
    patentTopicId:null,
  }
  id: string;

  ngOnInit(): void {
    this.getCategory();
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật chủ đề';
      this.getbyid();
    } else {
      this.modalInfo.Title = 'Thêm mới chủ đề';
    }
  }
  getCategory() {
    this.comboboxService.getTopic().subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listCategory = data.data;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
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
      patentTopicId:null,
    }
  }

}
