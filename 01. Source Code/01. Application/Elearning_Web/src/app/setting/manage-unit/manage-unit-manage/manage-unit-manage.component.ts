import { Component, OnInit } from '@angular/core';

import { MessageService, AppSetting, Constants, Configuration } from 'src/app/shared';
import { ManageUnitCreateComponent } from '../manage-unit-create/manage-unit-create.component';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ManageUnitService } from '../../services/manage-unit.service';

@Component({
  selector: 'app-manage-unit-manage',
  templateUrl: './manage-unit-manage.component.html',
  styleUrls: ['./manage-unit-manage.component.scss']
})
export class ManageUnitManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private modalService: NgbModal,
    private messageService: MessageService,
    private manageUnitService: ManageUnitService,
    public config: Configuration,
  ) { }
  startIndex = 1;
  listData: any[] = [];
  listPageSize = this.constant.ListPageSize;

  model: any = {
    Name: '',
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
  }

  listIndex: any[] = [];

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
      {

      },
    ]
  };

  ngOnInit(): void {
    this.search();
    this.appSetting.PageTitle = "Quản lý đơn vị chủ quản";
  }

  search() {
    this.manageUnitService.searchManageUnit(this.model).subscribe(
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



  clear() {
    this.model = {
      Name: '',
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
    }
    this.search();
  }

  showConfirmDelete(id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá đơn vị chủ quản này không?").then(
      data => {
        this.deleteManageUnit(id);
      });
  }

  deleteManageUnit(id) {
    this.manageUnitService.deleteManageUnit(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.search();
          this.messageService.showSuccess('Xóa đơn vị chủ quản thành công!');
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
  showCreateUpdate(id: string) {
    let activeModal = this.modalService.open(ManageUnitCreateComponent, { container: 'body', windowClass: 'manage-unit-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    });
  }
}
