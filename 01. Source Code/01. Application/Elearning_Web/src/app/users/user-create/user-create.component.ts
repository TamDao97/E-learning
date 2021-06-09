import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router, Routes } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, DateUtils, FileProcess, MessageService } from 'src/app/shared';
import { ComboboxService } from 'src/app/shared/services/combobox.service';
import { UploadService } from 'src/app/shared/services/upload.service';
import { UserService } from '../service/user.service';

import { TopbarComponent } from 'src/app/layout/topbar/topbar.component';
import { GroupUserService } from 'src/app/setting/services/group-user.service';

@Component({
  selector: 'app-user-create',
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserCreateComponent implements OnInit {

  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    public appSetting: AppSetting,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    public constant: Constants,
    public config: Configuration,
    public dateUtils: DateUtils,
    private userService: UserService,
    private router: Router,
    private uploadFileservice: UploadService,
    private serviceGroupUser: GroupUserService,


  ) { }

  height = 0;
  @ViewChild('scrollPracticeMaterial') scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader') scrollPracticeMaterialHeader: ElementRef;
  @ViewChild(TopbarComponent)
  private timerComponent: TopbarComponent;
  id: string;
  type: string;
  filedata = null;
  minDateNotificationV: NgbDateStruct;
  dateOfBirth = null;
  isAction: boolean = false;
  listFunction: any[] = [];
  listPermission: any[] = [];
  listUserGroup: any[] = [];
  groupFunctions: any[] = [];

  listManageUnit = [];

  isSelectAll = false;
  listFunctionIndex = 0;
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
    isChecked: false,
    managerUnitId: '',
    level: '',
  }


  modelGroupUser: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Id: '',
    Name: '',
    Status: '',
  }


  modelDelteFile: any = {
    avatar: '',
  }

  isIndeterminate = false;
  groupSelectIndex = -1;

  ngOnInit(): void {
    this.searchGroupUser();
    this.fileProcess.fileModel.DataURL = null;
    this.fileProcess.FileDataBase = null;
    this.getListGroupuser();
    this.getListManageUnit();
    this.id = this.routeA.snapshot.paramMap.get('id');
    this.type = this.routeA.snapshot.paramMap.get('type');
    this.model.type = parseInt(this.type)
    this.height = window.innerHeight - 450;
    if (this.type == '1') {
      this.appSetting.PageTitle = "Thêm mới tài khoản quản trị";
      if (this.id != null) {
        this.appSetting.PageTitle = "Cập nhật tài khoản quản trị";
        this.getUserById();
      }
    }
    if (this.type == '2') {
      this.appSetting.PageTitle = "Thêm mới tài khoản chuyên gia";
      if (this.id != null) {
        this.appSetting.PageTitle = "Cập nhật tài khoản chuyên gia";
        this.getUserById();
      }
    }
    if (this.type == '3') {
      this.appSetting.PageTitle = "Thêm mới tài khoản người hướng dẫn";
      if (this.id != null) {
        this.appSetting.PageTitle = "Cập nhật tài khoản người hướng dẫn";
        this.getUserById();
      }
    }
    this.userService.getGroupPermission(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.groupFunctions = data.data;
          this.listFunctions = this.groupFunctions[0].permissions.length;

          this.groupSelectIndex = 0;
          for (let i = 0; i < this.groupFunctions.length; i++) {
            let checkCount = this.groupFunctions[i].checkCount;
            let length = this.groupFunctions[i].permissions.length;
            this.listPermission = this.groupFunctions[i].permissions;
            if (checkCount == 0) {
              this.groupFunctions[i].isIndeterminate = false;
            }
            else {
              if (checkCount < length) {
                this.groupFunctions[i].isIndeterminate = true;
              }
              else {
                this.groupFunctions[i].isIndeterminate = false;
                this.isSelectAll = this.groupFunctions[i].checkCount == this.groupFunctions[i].permissions.length;
                this.groupFunctions[i].isChecked = this.isSelectAll;
              }
            }
          }
        }
        else {
          this.messageService.showMessage(data.message);
        }
      }
    )
  }

  listData = [];
  searchGroupUser() {
    this.serviceGroupUser.searchGroupUser(this.modelGroupUser).subscribe((data: any) => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.listData = data.data.dataResults;
        this.listData.forEach(element => {
          if (element.id == this.type) {
            this.model.groupUserId = element.id;
            this.changeGroupUser();
            return;
          }
        });
      }
      else {
        this.messageService.showError(data.message);
      }
    },
      error => {
      });
  }

  getListManageUnit() {
    this.comboboxService.getListManageUnit().subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.listManageUnit = data.data;
      }
      else {
        this.messageService.showListMessage(data.Message);
      }
    }, error => {
      this.messageService.showError(error);
    })
  }

  changeManageUnit(managerUnitId) {
    this.listManageUnit.forEach(element => {
      if (element.id == managerUnitId) {
        this.model.level = element.level;
      }
    });

  }

  pathFile: string;

  getUserById() {
    this.userService.getUserById(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          if (data.data.avatar != null && data.data.avatar != '') {
            this.filedata = this.config.ServerApi + data.data.avatar;
          }

          if (this.model.birthday != null) {
            this.dateOfBirth = this.dateUtils.convertDateToObject(this.model.birthday);
          }
          this.pathFile = this.model.avatar;
          this.groupFunctions = data.data.listGroupFunction;
          this.groupSelectIndex = 0;
          for (let i = 0; i < this.groupFunctions.length; i++) {
            let checkCount = this.groupFunctions[i].checkCount;
            let length = this.groupFunctions[i].permissions.length;
            if (checkCount == 0) {
              this.groupFunctions[i].isIndeterminate = false;
            }
            else {
              if (checkCount < length) {
                this.groupFunctions[i].isIndeterminate = true;
              }
              else {
                this.groupFunctions[i].isIndeterminate = false;
                this.isSelectAll = this.groupFunctions[i].checkCount == this.groupFunctions[i].permissions.length;
                this.groupFunctions[i].isChecked = this.isSelectAll;
              }
            }
          }
        }
        else {
          this.messageService.showListMessage(data.Message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
  listFunctions: number;
  selectGroupFunction(group, index) {
    this.groupSelectIndex = index;
    this.isSelectAll = this.groupFunctions[index].checkCount == this.groupFunctions[index].permissions.length;
    this.listFunctions = this.groupFunctions[index].permissions.length;
  }

  changeGroupFunctionCheck(group, index) {
    group.permissions.forEach(permission => {
      if (permission.isChecked && !group.isChecked) {
        group.checkCount--;
      }
      if (!permission.isChecked && group.isChecked) {
        group.checkCount++;
      }
      permission.isChecked = group.isChecked;
    });

    if (index == this.groupSelectIndex) {
      this.isSelectAll = group.isChecked;
    }
    this.groupFunctions[index].isIndeterminate = false;
  }

  selectAllPermission() {
    this.groupFunctions[this.groupSelectIndex].permissions.forEach(permission => {
      if (permission.isChecked && !this.isSelectAll) {
        this.groupFunctions[this.groupSelectIndex].checkCount--;
      }
      if (!permission.isChecked && this.isSelectAll) {
        this.groupFunctions[this.groupSelectIndex].checkCount++;
      }
      permission.isChecked = this.isSelectAll;
    });
    this.groupFunctions[this.groupSelectIndex].isChecked = this.isSelectAll;
    this.groupFunctions[this.groupSelectIndex].isIndeterminate = false;
  }

  selectPermission(permission) {
    if (!permission.isChecked) {
      this.groupFunctions[this.groupSelectIndex].checkCount--;
    }
    else {
      this.groupFunctions[this.groupSelectIndex].checkCount++;
    }

    let checkCount = this.groupFunctions[this.groupSelectIndex].checkCount;
    let length = this.groupFunctions[this.groupSelectIndex].permissions.length;
    if (checkCount == 0) {
      this.groupFunctions[this.groupSelectIndex].isChecked = false;
      this.groupFunctions[this.groupSelectIndex].isIndeterminate = false;
      this.isSelectAll = false;
    }
    else {
      if (checkCount < length) {
        this.groupFunctions[this.groupSelectIndex].isIndeterminate = true;
        this.isSelectAll = false;
      }
      else {
        this.groupFunctions[this.groupSelectIndex].isIndeterminate = false;
        this.isSelectAll = this.groupFunctions[this.groupSelectIndex].checkCount == this.groupFunctions[this.groupSelectIndex].permissions.length;
        this.groupFunctions[this.groupSelectIndex].isChecked = this.isSelectAll;
      }
    }

  }

  changeGroupUser() {
    if (this.model.groupUserId) {
      this.isSelectAll = false;
      this.userService.getGroupPermissionById(this.model.groupUserId).subscribe(
        data => {
          this.groupFunctions.forEach(group => {
            group.isChecked = false;
            group.checkCount = 0;
            group.permissions.forEach(permission => {
              permission.isChecked = false;
              data.data.forEach(groupF => {
                if (groupF.id == permission.id) {
                  permission.isChecked = true;
                  group.checkCount++;
                }
              });
            });
            let checkCount = group.checkCount;
            let length = group.permissions.length;
            group.isChecked = group.checkCount == group.permissions.length;
            if (checkCount > 0 && checkCount < length) {
              group.isIndeterminate = true;
            }
            else {
              group.isIndeterminate = false;
            }
          });

          this.isSelectAll = this.groupFunctions[this.groupSelectIndex].checkCount == this.groupFunctions[this.groupSelectIndex].permissions.length;
        }, error => {
          this.messageService.showError(error);
        });
    }
  }

  onFileChange($event) {
    this.fileProcess.onAFileChange($event);
  }

  saveAndContinue() {
    this.save(true);
  }

  getGroupPermission() {
    this.userService.getGroupPermission(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.groupFunctions = data.data;
          this.groupSelectIndex = 0;
          for (let i = 0; i < this.groupFunctions.length; i++) {
            let checkCount = this.groupFunctions[i].checkCount;
            let length = this.groupFunctions[i].permissions.length;
            if (checkCount == 0) {
              this.groupFunctions[i].isIndeterminate = false;
            }
            else {
              if (checkCount < length) {
                this.groupFunctions[i].isIndeterminate = true;
              }
              else {
                this.groupFunctions[i].isIndeterminate = false;
                this.isSelectAll = this.groupFunctions[i].checkCount == this.groupFunctions[i].permissions.length;
                this.groupFunctions[i].isChecked = this.isSelectAll;
              }
            }
          }
        }
        else {
          this.messageService.showMessage(data.message);
        }
      }
    )
  }

  getListGroupuser() {
    this.comboboxService.getListGroupuser().subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listUserGroup = data.data;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }
    );
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
    this.model.listGroupFunction = this.groupFunctions;
    if (this.id) {
      if (this.fileProcess.FileDataBase == null) {
        this.updateUser();
      }
      else {
        this.uploadFileservice.uploadFile(this.fileProcess.FileDataBase, 'Admin').subscribe(
          data => {
            this.model.avatar = data.data.fileUrl;
            this.updateUser();
          },
          error => {
            this.messageService.showError(error);
          });
      }
    }
    else {
      if (this.fileProcess.FileDataBase == null) {
        this.createUser(isContinue);
      }
      else {
        this.uploadFileservice.uploadFile(this.fileProcess.FileDataBase, 'Admin').subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.model.avatar = data.data.fileUrl;
              this.createUser(isContinue);
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
            if (this.fileProcess.FileDataBase != null) {
              this.uploadFileservice.deleteFile(this.modelDelteFile).subscribe(
                data => {
                  if (data.statusCode == this.constant.StatusCode.Success) {

                    if (this.model.type == 1) {
                      localStorage.setItem('userUpdate', JSON.stringify(this.model));
                      window.location.reload();
                    }
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
            // localStorage.setItem('ElearningCurrentUser', JSON.stringify(this.model));
            this.messageService.showSuccess('Cập nhật tài khoản thành công!');
            this.closeModal(true);
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
    else {
      this.messageService.showSuccess('Email không hợp lệ!');
    }
  }

  showComfirmDeleteFile(path) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa ảnh này không?").then(
      data => {
        this.deleteFile(path);
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

  createUser(isContinue) {
    this.model.Password = Math.random().toString(36).replace(/^(?=.*\d+)(?=.*\W+)(?=.*[A-Z]+)(?=.*[a-z]+)[a-zA-Z\d\W]{8,}$/g, '').substr(0, 9);
    var validEmail = true;
    var regex = this.constant.validEmailRegEx;
    if (this.model.email != '') {
      if (!regex.test(this.model.email)) {
        validEmail = false;
      }
    }

    if (this.dateOfBirth != null && this.dateOfBirth != '') {
      this.model.birthday = this.dateUtils.convertObjectToDate(this.dateOfBirth)
    }
    else {
      this.model.birthday = null;
    }

    if (validEmail) {
      this.userService.createUser(this.model).subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.messageService.showSuccess('Thêm mới tài khoản thành công!');
            if (this.fileProcess.fileModel.DataURL != undefined) {
              this.fileProcess.fileModel.DataURL = null;
            }
            if (isContinue) {
              this.isAction = true;
              this.model = {
                avatar: '',
                name: '',
                birthday: '',
                gender: true,
                address: '',
                email: '',
                phoneNumber: '',
                workUnit: '',
                description: '',
                userName: '',
                groupUserId: '',
                type: '',
                isDisable: false,
              };
            } else {
              this.closeModal(true);
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
    } else {
      this.messageService.showSuccess('Email không hợp lệ!');
    }
  }

  closeModal(isOK: boolean) {
    if (this.fileProcess.fileModel.DataURL != undefined) {
      this.fileProcess.fileModel.DataURL = null;
    }

    if (this.type == '1') {
      this.router.navigate(['/nguoi-dung/quan-tri-he-thong']);
    }
    if (this.type == '2') {
      this.router.navigate(['/nguoi-dung/chuyen-gia']);
    }
    if (this.type == '3') {
      this.router.navigate(['/nguoi-dung/nguoi-huong-dan']);
    }

  }

}
