import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, FileProcess, MessageService } from 'src/app/shared';
import { UploadService } from 'src/app/shared/services/upload.service';
import { TemplateCetificateCreateComponent } from '../template-cetificate-create/template-cetificate-create.component';
import { saveAs } from 'file-saver';
import { TemplateService } from '../service/template.service';

@Component({
  selector: 'app-template-cetificate-manager',
  templateUrl: './template-cetificate-manager.component.html',
  styleUrls: ['./template-cetificate-manager.component.scss']
})
export class TemplateCetificateManagerComponent implements OnInit {

  type = true;
  startIndex = 1;
  model: any = {};
  listData: any = [];
  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private uploadService: UploadService,
    private modalService: NgbModal,
    private messageService: MessageService,
    private templateService: TemplateService,
    public fileProcess: FileProcess,
    public config: Configuration

  ) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý mẫu chứng chỉ";
    this.search();
  }

  search() {
    this.templateService.search(this.type).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listData = data.data;
        } else {
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showCreateUpdate(id: string) {
    let activeModal = this.modalService.open(TemplateCetificateCreateComponent, { container: 'body', windowClass: 'template-cetificate-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    });
  }

  showConfirmDelete(model: any) {
    let obj = {
      avatar: model.filePath
    };

    this.messageService.showConfirm("Bạn có chắc muốn xoá mẫu này không?").then(
      data => {
        this.uploadService.deleteFile(obj).subscribe(res => {
          if (res.statusCode == this.constant.StatusCode.Success) {
            this.delete(model);
          } else {
            this.messageService.showListMessage(data.message);
          }
        }, error => {
          this.messageService.showError(error);
        });
      }
    );
  }

  delete(model: any) {
    this.templateService.delete(model.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.search();
        } else {
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  
  modelFile: any = {
    DataURL: '',
    Name: ''
  }
  
  DownloadFile(row) {
    this.modelFile.DataURL = row.filePath;;
    this.modelFile.Name = row.filePath.split("/").pop();
    this.fileProcess.downloadFile(this.modelFile.DataURL, this.modelFile.Name );
  }

  download(model: any) {

    let filePath = model.filePath;
    let nameFile = model.filePath.split("/").pop();
    this.templateService.download({ pathFile: filePath, nameFile: nameFile }).subscribe(
      data => {
        if (data) {
          saveAs(data, "download.docx");
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
}
