import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { GroupUserService } from '../../services/group-user.service';
import { GroupUserPermissionCreateComponent } from '../group-user-permission-create/group-user-permission-create.component';

@Component({
  selector: 'app-group-user-permission',
  templateUrl: './group-user-permission.component.html',
  styleUrls: ['./group-user-permission.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class GroupUserPermissionComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private serviceGroupUser: GroupUserService,
    public constant: Constants
  ) { }

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Id: '',
    Name: '',
    Status: '',
  }

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý nhóm quyền";
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    this.searchGroupUser();
  }

  listData = [];
  startIndex = 0;
  searchGroupUser() {
    this.serviceGroupUser.searchGroupUser(this.model).subscribe((data: any) => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.data.dataResults;
        this.model.TotalItems = data.data.totalItems;
      }
      else {
        this.messageService.showError(data.message);
      }
    },
      error => {
      });
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Name',
      OrderType: true,

      Id: '',
      Name: '',
      Status: '',
    }
    this.searchGroupUser();
  }

  // showConfirmDeleteGroupUser(Id: string) {
  //   this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm quyền này không?").then(
  //     data => {
  //       this.deleteGroupUser(Id);
  //     }
  //   );
  // }

  // deleteGroupUser(Id: string) {
  //   this.serviceGroupUser.deleteGroupUser(Id).subscribe(
  //     data => {
  //       if (data.statusCode == this.constant.StatusCode.Success) {
  //         this.searchGroupUser();
  //         this.messageService.showSuccess('Xóa nhóm quyền thành công!');
  //       }
  //       else {
  //         this.messageService.showMessage(data.message);
  //       }
  //     },
  //     error => {
  //       this.messageService.showError(error);
  //     });
  // }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(GroupUserPermissionCreateComponent, { container: 'body', windowClass: 'group-user-create', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchGroupUser();
      }
    }, (reason) => {
    });
  }

}
