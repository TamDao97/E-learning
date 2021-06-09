import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, AppSetting, MessageService, FileProcess, Configuration } from 'src/app/shared';
import { ComboboxService } from 'src/app/shared/services/combobox.service';
import { UploadService } from 'src/app/shared/services/upload.service';
import { SildeBarService } from '../service/silde-bar.service';

@Component({
  selector: 'app-silde-bar-create',
  templateUrl: './silde-bar-create.component.html',
  styleUrls: ['./silde-bar-create.component.scss']
})
export class SildeBarCreateComponent implements OnInit {

  constructor(
    public fileProcess: FileProcess,
    public constant: Constants,
    public appSetting: AppSetting,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private sildeBarService: SildeBarService,
    private uploadFileservice: UploadService,
    public config: Configuration

  ) { }

  filedata = null;
  listSildeBar = [];
  id: number;
  isAction: boolean = false;
  modalInfo = {
    Title: 'Thêm mới silde bar',
  };

  modelDelteFile: any = {
    imagePath: '',
  }

  model: any = {
    id: '',
    displayIndex: 1,
    name: '',
    status: true
  }

  ngOnInit(): void {
    this.fileProcess.fileModel.DataURL = null;
    this.getListSilderBar();
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật silde bar';
      this.getbyid();
    } else {
      this.modalInfo.Title = 'Thêm mới silde bar';
    }
  }

  getbyid() {
    this.sildeBarService.getByIdSildeBar(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          this.filedata = this.config.ServerApi + data.data.imagePath;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  onFileChange($event) {
    this.fileProcess.onAFileChange($event);
  }

  saveAndContinue() {
    this.save(true);
  }

  create(isContinue) {
    this.sildeBarService.createSildeBar(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          if (this.fileProcess.fileModel.DataURL != undefined) {
            this.fileProcess.fileModel.DataURL = null;
          }
          this.messageService.showSuccess('Thêm mới silde bar thành công!');
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

  deleteFileError() {
    this.modelDelteFile.imagePath = this.model.imagePath;
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

  save(isContinue: boolean) {

    if (this.fileProcess.FileDataBase == null && this.model.imagePath == null) {
      this.messageService.showMessage("Bạn không được để trống ảnh!");
      return;
    }
    if (this.id) {

      if(this.fileProcess.FileDataBase == null){
        this.update();
      }
      else{
        this.uploadFileservice.uploadFile(this.fileProcess.FileDataBase, 'sildebar').subscribe(
          data => {
            this.model.imagePath = data.data.fileUrl;
            this.update();
          },
          error => {
            this.messageService.showError(error);
          });
      }
    } else {
      this.uploadFileservice.uploadFile(this.fileProcess.FileDataBase, 'sildebar').subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.model.imagePath = data.data.fileUrl;
            this.create(isContinue);
          }
          else {
            this.deleteFileError();

            this.messageService.showMessage(data.message);
          }
        },
        error => {
          this.deleteFileError();

          this.messageService.showError(error);
        });
    }
  }

  update() {
    this.sildeBarService.updateSildeBar(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

          this.messageService.showSuccess('Cập nhập silde bar thành công!');
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

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  getListSilderBar() {
    this.sildeBarService.getListDisplayIndex().subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listSildeBar = data.data;
          if (this.id == 0) {
            this.model.displayIndex = data.data[data.data.length - 1].order;
          } else {
            this.listSildeBar.splice(this.listSildeBar.length - 1, 1);
          }

        }
        else {
          this.messageService.showListMessage(data.message);
        } 
      }
    );
  }

  clear() {
    this.model = {
      id: '',
      parentCategoryId: null,
      name: '',
    }
  }



}
