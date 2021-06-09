import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { FileProcess, AppSetting, MessageService, Constants, Configuration, DateUtils } from 'src/app/shared';
import { ComboboxService } from 'src/app/shared/services/combobox.service';
import { UploadService } from 'src/app/shared/services/upload.service';
import { UserService } from '../service/user.service';

@Component({
  selector: 'app-update-user',
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class UpdateUserComponent implements OnInit {

  constructor(
    public fileProcess: FileProcess,
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    public config: Configuration,
    public dateUtils: DateUtils,
    private userService: UserService,
    private router: Router,
    private uploadFileservice: UploadService,
    private activeModal: NgbActiveModal,

  ) { }

  filedata = null;
  id: string;
  dateOfBirth = null;
  pathFile: string;
  isAction = false;
  minDateNotificationV: NgbDateStruct;

  modalInfo: any = {
    Title: 'Cập nhật tài khoản'
  }

  model: any = {
    avatar: '',
    name: '',
    birthday: '',
    gender: true,
    address: '',
    email: '',
    phoneNumber: null,
    workUnit: '',
    description: '',
    userName: '',
    groupUserId: '',
    type: '',
    isDisable: false,
    Password: '',
    isChecked: false
  }

  userModel: any = {

  }

  modelDelteFile: any = {
    avatar: '',
  }


  ngOnInit(): void {
    this.fileProcess.fileModel = [];
    this.modalInfo.Title = "Cập nhật tài khoản";
    this.getUserById();
  }

  getUserById() {
    this.userService.getUserById(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          if (data.data.avatar != null) {
            this.filedata = this.config.ServerApi + data.data.avatar;
          }

          if (this.model.birthday != null) {
            this.dateOfBirth = this.dateUtils.convertDateToObject(this.model.birthday);
          }
          this.pathFile = this.model.avatar;
        }
        else {
          this.messageService.showListMessage(data.Message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  onFileChange($event) {
    this.fileProcess.onAFileChange($event);
  }

  showComfirmDeleteFile(path) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa ảnh này không?").then(
      data => {
        this.deleteFile(path);
      }
    );
  }

  deleteFile(avatar) {
    this.modelDelteFile.avatar = avatar;
    if (this.modelDelteFile.avatar != null && this.modelDelteFile.avatar != '') {
      this.uploadFileservice.deleteFile(this.modelDelteFile).subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.model.avatar = null;
            this.updateUser();
          }
          else {
            this.messageService.showMessage(data.message);
          }
        }
      );
    }
    if (this.fileProcess.FileDataBase != null && avatar == null) {
      this.filedata = null;
      this.fileProcess.fileModel.DataURL = null;
    }

  }

  save(isContinue: boolean) {
    let dateNow = new Date();
    if (this.dateOfBirth != null) {
      let date = new Date(this.dateUtils.convertObjectToDate(this.dateOfBirth));
      if (date > dateNow) {
        this.messageService.showWarning("Ngày sinh không được lớn hơn ngày hiện tại!");
        return;
      }
    }

    if (this.fileProcess.FileDataBase == null) {
      this.updateUser();

    }
    else {
      this.uploadFileservice.uploadFile(this.fileProcess.FileDataBase, 'Admin').subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.model.avatar = data.data.fileUrl;
            this.updateUser();
          }
          else {
            this.messageService.showMessage(data.message);
          }
        },
        error => {
          this.messageService.showError(error);
        });
    }


  }

  updateUser() {
    if (this.dateOfBirth != null && this.dateOfBirth != "") {
      this.model.birthday = this.dateUtils.convertObjectToDate(this.dateOfBirth);
    }
    else {
      this.model.birthday = null;
    }
    var validEmail = true;
    var regex = this.constant.validEmailRegEx;
    if (this.model.email != '') {
      if (!regex.test(this.model.email)) {
        validEmail = false;
      }
    }

    if (validEmail) {
      this.userService.updateUser(this.id, this.model).subscribe(
        data => {
          if (this.fileProcess.fileModel.DataURL != undefined) {
            this.fileProcess.fileModel.DataURL = null;
          }
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.modelDelteFile.avatar = this.pathFile;
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
            this.userModel = JSON.parse(localStorage.getItem('ElearningCurrentUser'));
            localStorage.removeItem('ElearningCurrentUser');
            this.userModel.name = this.model.name;
            this.userModel.avatar = this.model.avatar;
            localStorage.setItem('ElearningCurrentUser', JSON.stringify(this.userModel));
            this.messageService.showSuccess('Cập nhật tài khoản thành công!');
            this.closeModal(true);
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
    else {
      this.messageService.showSuccess('Email không hợp lệ!');
    }
  }

  closeModal(isOK: boolean) {
    this.fileProcess.FileDataBase = null;
    this.fileProcess.fileModel = [];
    if (this.fileProcess.fileModel.DataURL != undefined) {
      this.fileProcess.fileModel.DataURL = null;
    }
    this.activeModal.close(isOK ? isOK : this.isAction);

  }

}
