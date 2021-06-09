import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, AppSetting, MessageService, Configuration } from 'src/app/shared';
import { RefreshPasswordComponent } from '../refresh-password/refresh-password.component';
import { UserService } from '../service/user.service';

@Component({
  selector: 'app-user-expert-manage',
  templateUrl: './user-expert-manage.component.html',
  styleUrls: ['./user-expert-manage.component.scss']
})
export class UserExpertManageComponent implements OnInit {

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private userService: UserService,
    private messageService: MessageService,
    private router: Router,    
    public config: Configuration,
    private modalService: NgbModal,

  ) { }

  listData = [];
  startIndex = 1;

  searchOptions: any = {
    FieldContentName: 'UserName',
    Placeholder: 'Tìm kiếm theo tên tài khoản',
    Items: [
      {
        Name: 'Số điện thoại',
        FieldName: 'PhoneNumber',
        Placeholder: 'Số điện thoại',
        Type: 'text'
      },
      {
        Name: 'Nơi công tác',
        FieldName: 'WorkUnit',
        Placeholder: 'Nơi công tác',
        Type: 'text'
      },
      {
        Name: 'Email',
        FieldName: 'Email',
        Placeholder: 'Email',
        Type: 'text'
      },
      {
        Name: 'Tình trạng',
        FieldName: 'IsDisable',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.statusUserAdmin,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Đơn vị',
        FieldName: 'ManagerUnitId',
        Placeholder: 'Chọn',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.ManageUnit,
        DisplayName: 'name',
        ValueName: 'id'
      },
    ]
  }

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'UserName',
    OrderType: true,

    Type: 2,
    WorkUnit: '',
    UserName: '',
    Email: '',
    PhoneNumber:'',
    IsDisable: null,
    ManagerUnitId:''
  }

  userId: string;


  ngOnInit(): void {
    this.userId = JSON.parse(localStorage.getItem('ElearningCurrentUser')).userId

    this.appSetting.PageTitle = "Quản lý tài khoản chuyên gia";
    this.searchUserExpert();
  }

  searchUserExpert() {
    this.userService.searchUser(this.model).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
          this.listData = data.data.dataResults;
          this.model.TotalItems = data.data.totalItems;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmLockUser(id){
    this.messageService.showConfirm("Bạn có chắc muốn khóa tài khoản chuyên gia này không?").then(
      data => {
        this.lockUser(id);
      }
    );
  }

  lockUser(id){
    this.userService.userAdminLock(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.searchUserExpert();
          this.messageService.showSuccess('Khóa tài khoản chuyên gia thành công!');
        }
        else {
          this.messageService.showMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmUnLockUser(id){
    this.messageService.showConfirm("Bạn có chắc muốn mở khóa tài khoản chuyên gia này không?").then(
      data => {
        this.unLockUser(id);
      }
    );
  }

  unLockUser(id){
    this.userService.userAdminUnLock(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.searchUserExpert();
          this.messageService.showSuccess('Mở khóa tài khoản chuyên gia thành công!');
        }
        else {
          this.messageService.showMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreate(type){
    this.router.navigate(['/nguoi-dung/chuyen-gia/them-moi/'+type]);
  }

  showUpdate(id, type){
    this.router.navigate(['/nguoi-dung/chuyen-gia/cap-nhat/'+id+'/'+type]);
  }

  showViewUser(id, type) {
    this.router.navigate(['/nguoi-dung/chuyen-gia/xem-thong-tin/' + id + '/' + type]);
  }

  showConfirmDelete(id){
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài khoản chuyên gia này không?").then(
      data => {
        this.deleteUser(id);
      }
    );
  }

  deleteUser(id){
    this.userService.deleteUser(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.searchUserExpert();
          this.messageService.showSuccess('Xóa tài khoản chuyên gia thành công!');
        }
        else {
          this.messageService.showMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showRefreshPassword(id: string){
    let activeModal = this.modalService.open(RefreshPasswordComponent, { container: 'body', windowClass: 'refresh-password-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result: any) => {
      if (result) {
        this.searchUserExpert();
      }
    });
  }

  clear() {
    this.model={
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'UserName',
      OrderType: true,
  
      Type:2,
      WorkUnit:'',
      UserName:'',
      PhoneNumber:'',
      Email:'',
      IsDisable:null,
      ManagerUnitId:''
    }
    this.searchUserExpert();
  }

}
