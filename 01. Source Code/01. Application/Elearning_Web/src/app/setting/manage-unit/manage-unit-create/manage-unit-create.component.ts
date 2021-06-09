import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, FileProcess, Constants, DateUtils, Configuration } from 'src/app/shared';
import { ImageService } from 'src/app/shared/services/image.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ManageUnitService } from '../../services/manage-unit.service';
import { UploadService } from 'src/app/shared/services/upload.service';

@Component({
  selector: 'app-manage-unit-create',
  templateUrl: './manage-unit-create.component.html',
  styleUrls: ['./manage-unit-create.component.scss']
})
export class ManageUnitCreateComponent implements OnInit {

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
    private manageUnitService: ManageUnitService,
    private uploadFileservice: UploadService,
  ) { }

  
  listOrder: any[] = [];
  listStatus = this.constant.StatusHomeService;
  filedata: any;
  fileImage: any;
  modalInfo = {
    Title: 'Thêm mới đơn vị chủ quản',
  };
  modelDelteFile: any = {
    avatar: '',
  }

  isAction: boolean = false;
  model: any = {
    name: '',
    logo: '',
  }
  id: string;

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật đơn vị chủ quản';
      this.getbyid();
    } else {
      this.modalInfo.Title = 'Thêm mới đơn vị chủ quản';
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
    this.manageUnitService.getManageUnitInfo(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          if(data.data.logo != null && data.data.logo != ''){
            this.filedata = this.config.ServerApi + data.data.logo;
          }
          else{
            this.filedata = null;
          }
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
    this.manageUnitService.createManageUnit(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thêm mới đơn vị chủ quản thành công!');
          if (isContinue) {
            this.isAction = true;
            this.clear();
            this.fileImage=null;
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
    this.manageUnitService.updateManageUnit(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

          this.messageService.showSuccess('Cập nhập đơn vị chủ quản thành công!');
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
      name: '',
      logo:'',
    }
  }
}
