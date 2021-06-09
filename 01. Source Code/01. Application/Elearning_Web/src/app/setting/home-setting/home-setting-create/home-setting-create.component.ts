import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, FileProcess, Constants, DateUtils, Configuration } from 'src/app/shared';
import { ImageService } from 'src/app/shared/services/image.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { HomeSettingService } from '../../services/home-setting.service';
import { UploadService } from 'src/app/shared/services/upload.service';
@Component({
  selector: 'app-home-setting-create',
  templateUrl: './home-setting-create.component.html',
  styleUrls: ['./home-setting-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HomeSettingCreateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constant: Constants,
    public fileProcessImage: FileProcess,
    public fileProcessDataSheet: FileProcess,
    public dateUtils: DateUtils,
    private activeModal: NgbActiveModal,
    private serviceImg: ImageService,
    private config: Configuration,
    private homeSettingService: HomeSettingService,
    private uploadFileservice: UploadService,
  ) { }

  listStatus = this.constant.StatusProgram;
  filedata: any;
  fileImage: any;
  modalInfo = {
    Title: 'Thêm mới thiết lập trang chủ',
  };
  modelDelteFile: any = {
    avatar: '',
  }

  isAction: boolean = false;
  model: any = {
    logo: '',
    address: '',
    phone: '',
    gmail: '',
    website:'',
    linkFacebook: '',
    linkGoogle: '',
    linkYoutube: '',
    copyright: '',
  }
  id: number;

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật thiết lập trang chủ';
      this.getbyid();
    } else {
      this.modalInfo.Title = 'Thêm mới thiết lập trang chủ';
    }
  }

  onFileChange($event: any) {
    this.fileProcess.readDataFileIOnUploadSize($event).subscribe(
      data => {
        this.fileImage = data;
      }
    );
  }

  getbyid() {
    this.homeSettingService.getHomeSettingInfo(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          this.filedata = this.config.ServerApi + data.data.logo;
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
    this.homeSettingService.createHomeSetting(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thêm mới thiết lập trang chủ thành công!');
          if (isContinue) {
            this.isAction = true;
            this.clear();
          } else {
            this.closeModal(true);
          }
        } else {
          this.deleteFileError();
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.deleteFileError();
        this.messageService.showError(error);
      }
    );
  }
  save(isContinue: boolean) {
    if (this.fileImage == null) {
      if (this.id) {
        this.update();
      } else {
        this.create(isContinue);
      }
    }
    else {
      this.serviceImg.uploadFile(this.fileImage.File, 'setting').subscribe(
        data => {
          this.model.logo = data.data.fileUrl;
          if (this.id) {
            this.update();
          } else {
            this.create(isContinue);
          }
        },
        error => {
          this.messageService.showError(error);
        });
    }

  }

  update() {
    this.homeSettingService.updateHomeSetting(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

          this.messageService.showSuccess('Cập nhập thiết lập trang chủ thành công!');
          this.closeModal(true);
        }
        else {
          this.deleteFileError();
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.deleteFileError();
        this.messageService.showError(error);
      }
    );
  }

  deleteFileError() {
    this.modelDelteFile.avatar = this.model.avatar;
    this.uploadFileservice.deleteFile(this.modelDelteFile).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
        }
        else {
          this.messageService.showMessage(data.message);
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
      logo: '',
      address: '',
      phone: '',
      gmail: '',
      website:'',
      linkFacebook: '',
      linkGoogle: '',
      linkYoutube: '',
      copyright: '',
    }
  }

}
