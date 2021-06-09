import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, FileProcess, Constants, DateUtils, Configuration } from 'src/app/shared';
import { ImageService } from 'src/app/shared/services/image.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { HomeServiceService } from '../../services/home-service.service';
import { UploadService } from 'src/app/shared/services/upload.service';
import { NullVisitor } from '@angular/compiler/src/render3/r3_ast';
@Component({
  selector: 'app-home-service-create',
  templateUrl: './home-service-create.component.html',
  styleUrls: ['./home-service-create.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class HomeServiceCreateComponent implements OnInit {

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
    private homeServiceService: HomeServiceService,
    private uploadFileservice: UploadService,
  ) { }
  listOrder: any[] = [];
  listStatus = this.constant.StatusHomeService;
  filedata: any;
  fileImage: any;
  modalInfo = {
    Title: 'Thêm mới lời tựa',
  };
  modelDelteFile: any = {
    avatar: '',
  }

  isAction: boolean = false;
  model: any = {
    title: '',
    status: true,
    description: '',
    imagePath: '',
    displayIndex: null,
  }
  id: number;

  getListOrder() {
    this.homeServiceService.getListOrder().subscribe((data: any) => {
      this.listOrder = data.data;
      if (this.id == 0) {
        this.model.displayIndex = data.data[data.data.length - 1].order;
      } else {
        this.listOrder.splice(this.listOrder.length - 1, 1);
      }

    });
  }

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật lời tựa';
      this.getbyid();
    } else {
      this.modalInfo.Title = 'Thêm mới lời tựa';
    }
    this.getListOrder();
  }

  onFileChange($event: any) {
    this.fileProcess.readDataFileIOnUploadSize($event).subscribe(
      data => {
        this.fileImage = data;
      }
    );
  }

  getbyid() {
    this.homeServiceService.getHomeServiceInfo(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          if(data.data.imagePath != null && data.data.imagePath != ''){
            this.filedata = this.config.ServerApi + data.data.imagePath;
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
    this.homeServiceService.createHomeService(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thêm mới lời tựa thành công!');
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
          this.model.imagePath = data.data.fileUrl;
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
    this.homeServiceService.updateHomeService(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

          this.messageService.showSuccess('Cập nhập lời tựa thành công!');
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
      title: '',
      status: true,
      description: '',
      imagePath: '',
      displayIndex: null,
    }
    this.getListOrder();
  }
}
