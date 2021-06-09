import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppSetting, Configuration, Constants, DateUtils, MessageService } from 'src/app/shared';
import { UserService } from '../service/user.service';

@Component({
  selector: 'app-show-user',
  templateUrl: './show-user.component.html',
  styleUrls: ['./show-user.component.scss']
})
export class ShowUserComponent implements OnInit {

  constructor(
    private router: Router,
    public appSetting: AppSetting,
    private routeA: ActivatedRoute,
    private userService: UserService,
    public constant: Constants,
    public config: Configuration,
    public dateUtils: DateUtils,
    private messageService: MessageService,

  ) { }

  id: string;
  height: number
  model:any;
  filedata: any;
  dateOfBirth: any;
  type: string;
  pathFile: string;
  groupFunctions: any[] = [];
  isIndeterminate = false;
  groupSelectIndex = -1;
  isSelectAll = false;
  listPermission: any[] = [];
  listFunctions: number;

  ngOnInit(): void {
    this.id = this.routeA.snapshot.paramMap.get('id');
    this.type = this.routeA.snapshot.paramMap.get('type');
    this.height = window.innerHeight - 450;
    if (this.type == '1') {
        this.appSetting.PageTitle = "Thông tin tài khoản quản trị";
        this.getUserById();
    }
    if (this.type == '2') {
        this.appSetting.PageTitle = "Thông tin tài khoản chuyên gia";
        this.getUserById();
    }
    if (this.type == '3') {
        this.appSetting.PageTitle = "Thông tin tài khoản người hướng dẫn";
        this.getUserById();
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

  selectGroupFunction(group, index) {
    this.groupSelectIndex = index;
    this.isSelectAll = this.groupFunctions[index].checkCount == this.groupFunctions[index].permissions.length;
    this.listFunctions = this.groupFunctions[index].permissions.length;
  }

  getUserById() {
    this.userService.getUserById(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          if(data.data.avatar != null){
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

  closeModal(isOK: boolean) {
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
