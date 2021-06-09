import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, MessageService, FileProcess, Constants, DateUtils } from 'src/app/shared';
import { UploadService } from 'src/app/shared/services/upload.service';
import { TemplateService } from 'src/app/template/service/template.service';

@Component({
  selector: 'app-template-create',
  templateUrl: './template-create.component.html',
  styleUrls: ['./template-create.component.scss']
})
export class TemplateCreateComponent implements OnInit {
  modalInfo = {
    Title: 'Thêm mới mẫu chứng chỉ',
  };
  folder: string = "Template";
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
      this.uploadService.uploadFileTemplate(this.file.File, this.folder).subscribe(data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          if (this.id) {
            this.uploadService.deleteFile({ avatar: this.fileOld }).subscribe(res => {
              if (data.statusCode == this.constant.StatusCode.Success) {
                this.update(data.data.fileUrl);
              } else {
                this.messageService.showListMessage(data.message)
              }
            },
              err => {
                this.messageService.showListMessage(data.message)
              });
          } else
            this.create(data.data.fileUrl);
        } else {
          this.messageService.showListMessage(data.message);
        }
      });
    } else {
      this.update(this.model.filePath);
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
