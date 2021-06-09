import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { RefreshPasswordComponent } from '../../refresh-password/refresh-password.component';
import { UserService } from '../../service/user.service';

@Component({
  selector: 'app-user-admin-mange',
  templateUrl: './user-admin-mange.component.html',
  styleUrls: ['./user-admin-mange.component.scss']
})
export class UserAdminMangeComponent implements OnInit {

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private userService: UserService,
    private messageService: MessageService,
    public config: Configuration,
    private router: Router,
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

    Type: 1,
    WorkUnit: '',
    UserName: '',
    PhoneNumber: '',
    Email: '',
    IsDisable: null,
    ManagerUnitId:''
  }
  userId: string;
  userName = false;
  ngOnInit(): void {
    this.userId = JSON.parse(localStorage.getItem('ElearningCurrentUser')).userId

    this.appSetting.PageTitle = "Quản lý tài khoản quản trị";
    this.searchUserAdmin();
  }

  searchUserAdmin() {
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

  showUpdate(id, type) {
    this.router.navigate(['/nguoi-dung/quan-tri-he-thong/cap-nhat/' + id + '/' + type]);
  }

  showCreate(type) {
    this.router.navigate(['/nguoi-dung/quan-tri-he-thong/them-moi/' + type]);
  }

  showViewUser(id, type) {
    this.router.navigate(['/nguoi-dung/quan-tri-he-thong/xem-thong-tin/' + id + '/' + type]);
  }

  showConfirmDelete(id) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài khoản quản trị này không?").then(
      data => {
        this.deleteUser(id);
      }
    );
  }

  deleteUser(id) {
    this.userService.deleteUser(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.searchUserAdmin();
          this.messageService.showSuccess('Xóa tài khoản quản trị thành công!');
        }
        else {
          this.messageService.showMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }


  showConfirmLockUser(id) {
    this.messageService.showConfirm("Bạn có chắc muốn khóa tài khoản quản trị này không?").then(
      data => {
        this.lockUser(id);
      }
    );
  }

  lockUser(id) {
    this.userService.userAdminLock(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.searchUserAdmin();
          this.messageService.showSuccess('Khóa tài khoản quản trị thành công!');
        }
        else {
          this.messageService.showMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmUnLockUser(id) {
    this.messageService.showConfirm("Bạn có chắc muốn mở khóa tài khoản quản trị này không?").then(
      data => {
        this.unLockUser(id);
      }
    );
  }

  unLockUser(id) {
    this.userService.userAdminUnLock(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.searchUserAdmin();
          this.messageService.showSuccess('Mở khóa tài khoản quản trị thành công!');
        }
        else {
          this.messageService.showMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showRefreshPassword(id: string) {
    let activeModal = this.modalService.open(RefreshPasswordComponent, { container: 'body', windowClass: 'refresh-password-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result: any) => {
      if (result) {
        this.searchUserAdmin();
      }
    });
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'UserName',
      OrderType: true,

      Type: 1,
      WorkUnit: '',
      UserName: '',
      PhoneNumber: '',
      Email: '',
      IsDisable: null,
      ManagerUnitId:''
    }
    this.searchUserAdmin();
  }

}
