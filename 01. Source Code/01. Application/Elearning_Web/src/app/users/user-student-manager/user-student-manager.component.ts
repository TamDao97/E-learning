import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { Constants, AppSetting, MessageService, Configuration } from 'src/app/shared';
import { UserService } from '../service/user.service';

@Component({
  selector: 'app-user-student-manager',
  templateUrl: './user-student-manager.component.html',
  styleUrls: ['./user-student-manager.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserStudentManagerComponent implements OnInit {

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private userService: UserService,
    private messageService: MessageService,
    public config: Configuration,
    private router: Router,    

  ) { }

  listData = [];
  startIndex = 1;

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    WardId: null,
    DistrictId: null,
    ProvinceId: null,
    NationId: null,
    Name: '',
    Provider: '',
    Email: '',
    IsDisable: null,
    Old: ''
  }

  listOld = [
    {Id: 1, Name:'Từ 18 đến 24 tuổi'},
    {Id: 2, Name:'Từ 25 đến 34 tuổi'},
    {Id: 3, Name:'Từ 35 đến 44 tuổi'},
    {Id: 4, Name:'Từ 45 đến 54 tuổi'},
    {Id: 5, Name:'Từ 55 đến 64 tuổi'},
    {Id: 6, Name:'Từ 65 tuổi trở lên'},
    {Id: 7, Name:'Khác'},
  ];

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
      {
        Name: 'Email',
        FieldName: 'Email',
        Placeholder: 'Email',
        Type: 'text'
      },
      {
        Name: 'Tỉnh',
        FieldName: 'ProvinceId',
        Placeholder: 'Tỉnh',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.Province,
        DisplayName: 'name',
        ValueName: 'id',
        IsRelation: true,
        RelationIndexTo: 2
      },
      {
        Name: 'Huyện',
        FieldName: 'DistrictId',
        Placeholder: 'Huyện',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.Districti,
        DisplayName: 'name',
        ValueName: 'id',
        IsRelation: true,
        RelationIndexFrom: 1,
        RelationIndexTo: 3
      },
      {
        Name: 'Xã',
        FieldName: 'WardId',
        Placeholder: 'Xã',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.Ward,
        DisplayName: 'name',
        ValueName: 'id',
        RelationIndexFrom: 2
      },
      {
        Name: 'Hình thức đăng ký',
        FieldName: 'Provider',
        Type: 'select',
        Data: this.constant.provinceStudent,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Dân tộc',
        FieldName: 'DepartmentId',
        Placeholder: 'Dân tộc',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.Nation,
        DisplayName: 'name',
        ValueName: 'code',
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
        Name: 'Tuổi',
        FieldName: 'Old',
        Placeholder: 'Tuổi',
        Type: 'select',
        Data: this.listOld,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  }


  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý tài khoản người dùng";
    this.searchUserStudent();
  }

  searchUserStudent() {
    this.userService.search(this.model).subscribe(
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

  showConfirmLockUser(id) {
    this.messageService.showConfirm("Bạn có chắc muốn khóa tài khoản người dùng này không?").then(
      data => {
        this.lockUser(id);
      }
    );
  }

  lockUser(id) {
    this.userService.userAdminLock(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.searchUserStudent();
          this.messageService.showSuccess('Khóa tài khoản người dùng thành công!');
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
    this.messageService.showConfirm("Bạn có chắc muốn mở khóa tài khoản người dùng này không?").then(
      data => {
        this.unLockUser(id);
      }
    );
  }

  unLockUser(id) {
    this.userService.userAdminUnLock(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.searchUserStudent();
          this.messageService.showSuccess('Mở khóa tài khoản người dùng thành công!');
        }
        else {
          this.messageService.showMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDelete(id) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài khoản người dùng này không?").then(
      data => {
        this.deleteUser(id);
      }
    );
  }

  deleteUser(id) {
    this.userService.deleteUser(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.searchUserStudent();
          this.messageService.showSuccess('Xóa tài khoản người dùng thành công!');
        }
        else {
          this.messageService.showMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showViewUser(id) {
    this.router.navigate(['/nguoi-dung/nguoi-dung/xem-thong-tin/' + id]);
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Name',
      OrderType: true,

      WardId: null,
      DistrictId: null,
      ProvinceId: null,
      NationId: null,
      Name: '',
      Provider: '',
      Email: '',
      IsDisable: null,
    }
    this.searchUserStudent();
  }

}
