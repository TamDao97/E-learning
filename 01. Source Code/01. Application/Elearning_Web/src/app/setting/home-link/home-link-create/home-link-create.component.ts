import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, FileProcess, Constants, DateUtils, Configuration } from 'src/app/shared';
import { ImageService } from 'src/app/shared/services/image.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { HomeLinkService } from '../../services/home-link.service';
@Component({
  selector: 'app-home-link-create',
  templateUrl: './home-link-create.component.html',
  styleUrls: ['./home-link-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HomeLinkCreateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constant: Constants,
    public fileProcessImage: FileProcess,
    public fileProcessDataSheet: FileProcess,
    private activeModal: NgbActiveModal,
    private config: Configuration,
    private homeLinkService: HomeLinkService,
  ) { }
  listStatus = this.constant.StatusProgram;
  filedata: any;
  fileImage: any;
  modalInfo = {
    Title: 'Thêm mới trang liên kết',
  };
 

  isAction: boolean = false;
  model: any = {
    title: '',
    status: false,
    description: '',
    pageLink:''
  }
  id: string;

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật trang liên kết';
      this.getbyid();
    } else {
      this.modalInfo.Title = 'Thêm mới trang liên kết';
    }
  }

  getbyid() {
    this.homeLinkService.getHomeLinkInfo(this.id).subscribe(
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
    this.homeLinkService.createHomeLink(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thêm mới trang liên kết thành công!');
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
    this.homeLinkService.updateHomeLink(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

          this.messageService.showSuccess('Cập nhập trang liên kết thành công!');
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
    this.fileImage = null;
    this.model = {
      title:'',
      status: false,
      description: '',
      pageLink:''
    }
  }
}
