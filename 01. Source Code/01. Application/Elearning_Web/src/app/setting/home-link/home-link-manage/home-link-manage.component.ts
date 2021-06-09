import { Component, OnInit } from '@angular/core';

import { MessageService, AppSetting, Constants, Configuration } from 'src/app/shared';
import { HomeLinkCreateComponent } from '../home-link-create/home-link-create.component';
import { HomeLinkService } from '../../services/home-link.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-home-link-manage',
  templateUrl: './home-link-manage.component.html',
  styleUrls: ['./home-link-manage.component.scss']
})
export class HomeLinkManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private modalService: NgbModal,
    private messageService: MessageService,
    public config: Configuration,
    private homeLinkService: HomeLinkService,
  ) { }
  startIndex = 1;
  listData: any[] = [];
  listPageSize = this.constant.ListPageSize;
  model: any = {
    Title: '',
    Description: '',
    PageLink: '',
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
  }
  searchOptions: any = {
    FieldContentName: 'Title',
    Placeholder: 'Tìm kiếm theo tiêu để',
    Items: [
      {
        Name: 'Mô tả',
        FieldName: 'Description',
        Placeholder: 'Nhập mô tả',
        Type: 'text'
      },
    ]
  };

  ngOnInit(): void {
    this.search();
    this.appSetting.PageTitle = "Quản lý trang liên kết";
  }

  search() {
    this.homeLinkService.searchHomeLink(this.model).subscribe(
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

  UpdateStatus(id) {
    this.homeLinkService.updateStatusHomeLink(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.search();
          this.messageService.showSuccess('Cập nhập trạng thái home link thành công!');
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
      Title: '',
      Description: '',
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
    }
    this.search();
  }

  showConfirmDelete(id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá home link này không?").then(
      data => {
        this.deleteProgram(id);
      });
  }

  deleteProgram(id) {
    this.homeLinkService.deleteHomeLink(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.search();
          this.messageService.showSuccess('Xóa home link thành công!');
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
    let activeModal = this.modalService.open(HomeLinkCreateComponent, { container: 'body', windowClass: 'home-link-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    });
  }

}
