import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, MessageService, FileProcess, Constants } from 'src/app/shared';
import { UploadService } from 'src/app/shared/services/upload.service';
import { TemplateService } from '../service/template.service';

@Component({
  selector: 'app-template-common-create',
  templateUrl: './template-common-create.component.html',
  styleUrls: ['./template-common-create.component.scss']
})
export class TemplateCommonCreateComponent implements OnInit {

  modalInfo = {
    Title: 'Cập nhập mẫu văn bản',
  };
  folder: string = "TemplateCommon";
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
    this.getById(this.id);
  }

  getById(id: string) {
    this.templateService.getById(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          this.fileOld = this.model.filePath;
          let arr = this.model.filePath.split("/");
          this.model.filePath = arr.pop();
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

  update(filePath: string) {
    this.model.filePath = filePath;
    this.templateService.update(this.model).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.messageService.showSuccess('Cập nhập mẫu văn bản thành công!');
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
      this.uploadService.deleteFile({ avatar: this.fileOld }).subscribe(res => {
        if (res.statusCode == this.constant.StatusCode.Success) {
          var endFile = this.file.File.name.split(".").pop();
          if (endFile == 'doc' || endFile == 'docx' || endFile == 'xls' || endFile == 'xlsx') {
            this.uploadService.uploadFileTemplate(this.file.File, this.folder).subscribe(data => {
              if (data.statusCode == this.constant.StatusCode.Success) {
                this.update(data.data.fileUrl);
              } else {
                this.messageService.showListMessage(data.message);
              }
            });
          }
          else {
            this.messageService.showWarning('Bạn không được upload file này!');
          }
        } else {
          this.messageService.showListMessage(res.message)
        }
      });
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
