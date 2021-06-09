import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { UploadService } from 'src/app/shared/services/upload.service';
import { TemplateService } from 'src/app/template/service/template.service';
import { CourseCreateComponent } from '../course-create/course-create.component';
import { TemplateCreateComponent } from '../template-create/template-create.component';

@Component({
  selector: 'app-template-manager',
  templateUrl: './template-manager.component.html',
  styleUrls: ['./template-manager.component.scss']
})
export class TemplateManagerComponent implements OnInit {

  startIndex = 1;
  model: any = {};
  listData: any = [];
  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private uploadService: UploadService,
    private modalService: NgbModal,
    private messageService: MessageService,
    private templateService: TemplateService
  ) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý mẫu văn bằng";
    // this.search();
  }

  // search() {
  //   this.templateService.search().subscribe(
  //     data => {
  //       if (data.statusCode == this.constant.StatusCode.Success) {
  //         this.listData = data.data;
  //       } else {
  //         this.messageService.showListMessage(data.message);
  //       }
  //     },
  //     error => {
  //       this.messageService.showError(error);
  //     }
  //   );
  // }

  showCreateUpdate(id: string) {
    let activeModal = this.modalService.open(TemplateCreateComponent, { container: 'body', windowClass: 'template-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        // this.search();
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
          // this.search();
        } else {
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  download(model: any) {
    let filePath = model.filePath;
    let nameFile = model.filePath.split("/").pop();
    this.templateService.download({ pathFile: filePath, nameFile: nameFile }).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

        } else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {

      }
    );
  }
}
