import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, AppSetting, MessageService, FileProcess } from 'src/app/shared';
import { UploadService } from 'src/app/shared/services/upload.service';
import { TemplateService } from '../service/template.service';
import { saveAs } from 'file-saver';
import { TemplateCommonCreateComponent } from '../template-common-create/template-common-create.component';

@Component({
  selector: 'app-template-common-manager',
  templateUrl: './template-common-manager.component.html',
  styleUrls: ['./template-common-manager.component.scss']
})
export class TemplateCommonManagerComponent implements OnInit {

  type = false;
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

  ) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý mẫu văn bản";
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
    let activeModal = this.modalService.open(TemplateCommonCreateComponent, { container: 'body', windowClass: 'template-common-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    });
  }

    
  modelFile: any = {
    DataURL: '',
    Name: ''
  }

  download(row: any) {

    this.modelFile.DataURL = row.filePath;;
    this.modelFile.Name = row.filePath.split("/").pop();
    this.fileProcess.downloadFile(this.modelFile.DataURL, this.modelFile.Name );
  }

}
