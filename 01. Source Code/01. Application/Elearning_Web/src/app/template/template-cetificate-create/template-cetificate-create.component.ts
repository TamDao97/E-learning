import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, MessageService, FileProcess, Constants, DateUtils } from 'src/app/shared';
import { UploadService } from 'src/app/shared/services/upload.service';
import { TemplateService } from '../service/template.service';

@Component({
  selector: 'app-template-cetificate-create',
  templateUrl: './template-cetificate-create.component.html',
  styleUrls: ['./template-cetificate-create.component.scss']
})
export class TemplateCetificateCreateComponent implements OnInit {
  modalInfo = {
    Title: 'Thêm mới mẫu chứng chỉ',
  };
  folder: string = "TemplateCetificate";
  model: any = {};
  file: any;
  fileOld: string;

  @Input() id: string;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private uploadService: UploadService,
    public fileProcess: FileProcess,
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private templateService: TemplateService,
  ) { }

  ngOnInit(): void {
    this.modalInfo.Title = 'Thêm mới mẫu chứng chỉ';
    if (this.id) {
      this.modalInfo.Title = 'Cập nhập mẫu chứng chỉ';
      this.getById(this.id);
    }
  }

  getById(id: string) {
    this.templateService.getById(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          this.fileOld = this.model.filePath;
          let arr = this.model.filePath.split("/");
          this.model.filePath = arr[arr.length - 1];
        } else {
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  deleteFileError(filePath: string) {
    let model = { avatar: filePath }
    this.uploadService.deleteFile(model).subscribe(
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

  create(filePath: string) {

    this.model.filePath = filePath;
    this.templateService.create(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thêm mới mẫu chứng chỉ thành công!');
          this.clear();
        } else {
          this.deleteFileError(filePath);
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.deleteFileError(filePath);
        this.messageService.showError(error);
      }
    );
  }

  update(filePath: string) {
    this.model.filePath = filePath;
    this.templateService.update(this.model).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.messageService.showSuccess('Cập nhập mẫu chứng chỉ thành công!');
        this.closeModal(true);
      } else {
        this.deleteFileError(filePath);
        this.messageService.showListMessage(data.message);
      }
    },
      error => {
        this.deleteFileError(filePath);
        this.messageService.showError(error);
      }
    );
  }

  save() {
    if (this.file) {

      // trường hợp update thì xóa file 
      if (this.id) {
        this.uploadService.deleteFile({ avatar: this.fileOld }).subscribe(res => {
          if (res.statusCode == this.constant.StatusCode.Success) {
          } else {
            this.messageService.showListMessage(res.message)
          }
        },
          err => {
            this.messageService.showListMessage(err.message)
          });
      }

      // Upload file lên
      var endFile = this.file.File.name.split(".").pop();
      if (endFile == 'doc' || endFile == 'docx' || endFile == 'xls' || endFile == 'xlsx') {
        this.uploadService.uploadFileTemplate(this.file.File, this.folder).subscribe(data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            if (this.id) {
              this.update(data.data.fileUrl);
            } else
              this.create(data.data.fileUrl);
          } else {
            this.messageService.showListMessage(data.message);
          }
        });
      }
      else {
        this.messageService.showWarning('Bạn không được upload file này!');
      }
    } else {
      var endFile = this.model.filePath.split(".").pop();
      if (endFile == 'doc' || endFile == 'docx' || endFile == 'xls' || endFile == 'xlsx') {
        this.update(this.model.filePath);
      }
      else {
        this.messageService.showWarning('Bạn không được upload file này!');
      }
    }

  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK);
  }

  clear() {
    this.model = {};
  }

  onFileChange($event: any) {
    this.fileProcess.readDataFileIOnUploadSize($event).subscribe(
      data => {
        this.file = data;
        this.model.filePath = data.Name;
      }
    );
  }
}
