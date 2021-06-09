import { Component, OnInit } from '@angular/core';

import { MessageService, AppSetting, Constants, Configuration } from 'src/app/shared';
import { HomeServiceCreateComponent } from '../home-service-create/home-service-create.component';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HomeServiceService } from '../../services/home-service.service';

import { CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
@Component({
  selector: 'app-home-service-manage',
  templateUrl: './home-service-manage.component.html',
  styleUrls: ['./home-service-manage.component.scss']
})
export class HomeServiceManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private modalService: NgbModal,
    private messageService: MessageService,
    private homeServiceService: HomeServiceService,
    public config: Configuration,
    public dragDropModule: DragDropModule,
  ) { }

  startIndex = 1;
  listData: any[] = [];
  listPageSize = this.constant.ListPageSize;

  model: any = {
    Title: '',
    Status: null,
    TotalItems: 0,
  }

  listIndex: any[] = [];

  searchOptions: any = {
    FieldContentName: 'Title',
    Placeholder: 'Tìm kiếm theo tiêu đề',
    Items: [
      {
        Name: 'Tình trạng',
        FieldName: 'Status',
        Placeholder: 'Chọn tình trạng',
        Type: 'select',
        Data: this.constant.StatusHomeService,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  ngOnInit(): void {
    this.search();
    this.appSetting.PageTitle = "Quản lý lời tựa";
  }

  search() {
    this.homeServiceService.searchHomeService(this.model).subscribe(
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

  drop(event: CdkDragDrop<string[]>) {
    this.listData = [];
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
    }

    this.listData = event.container.data;
    var i = 1;
    this.listData.forEach(element => {
      element.displayIndex = i++;
      this.listIndex.push({ id: element.id, displayIndex: element.displayIndex });
      this.update(this.listIndex);
    })
  }

  update(model) {
    this.homeServiceService.updateIndexHomeService(model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          
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
      Status: null,
      TotalItems: 0,
    }
    this.search();
  }

  showConfirmDelete(id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá lời tựa này không?").then(
      data => {
        this.deleteHomeService(id);
      });
  }

  deleteHomeService(id) {
    this.homeServiceService.deleteHomeService(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.search();
          this.messageService.showSuccess('Xóa lời tựa thành công!');
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
    this.homeServiceService.updateStatusHomeService(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.search();
          this.messageService.showSuccess('Cập nhập trạng thái lời tựa thành công!');
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
    let activeModal = this.modalService.open(HomeServiceCreateComponent, { container: 'body', windowClass: 'home-service-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    });
  }
}
