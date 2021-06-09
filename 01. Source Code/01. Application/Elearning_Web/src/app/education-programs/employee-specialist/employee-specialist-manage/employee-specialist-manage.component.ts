import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { elementAt } from 'rxjs/operators';
import { Constants, Configuration, AppSetting, MessageService, FileProcess } from 'src/app/shared';
import { UploadService } from 'src/app/shared/services/upload.service';
import { EmployeeSpecialistService } from '../../services/employee-specialist.service';
import { ChooseEmployeeComponent } from '../choose-employee/choose-employee.component';

@Component({
  selector: 'app-employee-specialist-manage',
  templateUrl: './employee-specialist-manage.component.html',
  styleUrls: ['./employee-specialist-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EmployeeSpecialistManageComponent implements OnInit {

  constructor(
    public constant: Constants,
    public config: Configuration,
    public appSetting: AppSetting,
    private modalService: NgbModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    private employeeSpecialistService: EmployeeSpecialistService,
    private uploadFileservice: UploadService,

  ) { }

  startIndex = 1;
  listData: any[] = [];

  model: any = {
    title:'Chuyên gia',
    description:'',
    ListEmployeeSpeciallist: []
  }

  modelDelteFile: any = {
    avatar: '',
  }

  ngOnInit(): void {
    this.fileProcess.fileModel.DataURL = null;
    this.appSetting.PageTitle = "Quản lý cấu hình chuyên gia";
    this.search();
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

  onFileChange($event, row) {
    this.model.avatar = '';
    this.fileProcess.onAFileChange($event);
    this.uploadFileservice.uploadFile(this.fileProcess.FileDataBase, 'EmployeeSpecialist').subscribe(
      data => {
        this.listData.forEach(element => {
          if (element.id == row.id) {
            element.avartar = data.data.fileUrl;
          }
        })
      },
      error => {
        this.deleteFileError();
        this.messageService.showError(error);
      });

  }

  selectLesson() {
    let activeModal = this.modalService.open(ChooseEmployeeComponent, { container: 'body', windowClass: 'choose-employee', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];

    this.listData.forEach(element => {
      ListIdSelect.push(element.id);
    });

    activeModal.componentInstance.listIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listData.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDelete(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chuyên gia này không?").then(
      data => {
        this.removeLesson(row);
      }
    );
  }

  removeLesson(row) {
    var index = this.listData.indexOf(row);
    if (index > -1) {
      this.listData.splice(index, 1);
    }
  }

  save() {
    var isSave = false;
    this.model.ListEmployeeSpeciallist = this.listData;
    this.listData.forEach(element => {

      if(element.facebook != null&& element.facebook != '' ){
        var isfacebook = element.facebook.includes("https://www.facebook.com/");
        if(isfacebook == false){
          this.messageService.showMessage('Đường dẫn facebook sai ở '+element.name+'!');
          isSave =true;
          return;
        }
      }

      if(element.lotus != null&& element.lotus != '' ){
        var islotus = element.lotus.includes("https://lotus.vn/");
        if(islotus == false){
          this.messageService.showMessage('Đường dẫn lotus sai ở '+element.name+'!');
          isSave =true;
          return;

        }
      }

      if(element.twitter != null&& element.twitter != '' ){
        var istwitter = element.twitter.includes("https://twitter.com/");
        if(istwitter == false){
          this.messageService.showMessage('Đường dẫn twitter sai ở '+element.name+'!');
          isSave =true;
          return;

        }
      }

      if(element.instagram != null&& element.instagram != '' ){
        var instagram = element.instagram.includes("https://www.instagram.com/");
        if(instagram == false){
          this.messageService.showMessage('Đường dẫn instagram sai'+element.name+'!');
          isSave =true;
          return;
        }
      }
    });
    
    if(isSave == false){
      if (this.model.id) {
        this.update();
      } else {
        this.create();
      }
    }
  }

  search() {
    this.employeeSpecialistService.searchEmployeeSpecialist().subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          this.listData = data.data.employeeSpecialists;
          this.model.TotalItems = data.data.employeeSpecialists.length;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.employeeSpecialistService.updateEmployeeSpecialist(this.model.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Cập nhật chuyên gia thành công!');
          if (this.fileProcess.fileModel.DataURL != undefined) {
            this.fileProcess.fileModel.DataURL = null;
            this.search();
          }
        }
        else {
          this.deleteFileError();
          this.messageService.showMessage(data.message);
        }
      },
      error => {
        this.deleteFileError();
        this.messageService.showError(error);
      }
    );
  }

  create() {
    this.employeeSpecialistService.createEmployeeSpecialist(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thêm mới chuyên gia thành công!');
          if (this.fileProcess.fileModel.DataURL != undefined) {
            this.fileProcess.fileModel.DataURL = null;
          }
          this.search();
        }
        else {
          this.deleteFileError();
          this.messageService.showMessage(data.message);
        }
      },
      error => {
        this.deleteFileError();
        this.messageService.showError(error);
      }
    );
  }


}
