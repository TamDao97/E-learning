import { Component, OnInit } from '@angular/core';

import { MessageService, AppSetting, Constants, Configuration } from 'src/app/shared';
import { TopicCreateComponent } from '../topic-create/topic-create.component';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TopicService } from '../../services/topic.service'
@Component({
  selector: 'app-topic-manage',
  templateUrl: './topic-manage.component.html',
  styleUrls: ['./topic-manage.component.scss']
})
export class TopicManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private modalService: NgbModal,
    private messageService: MessageService,
    private topicService: TopicService,
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
    this.appSetting.PageTitle = "Quản lý chủ đề";
  }

  search() {
    this.topicService.searchtopic(this.model).subscribe(
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
    this.messageService.showConfirm("Bạn có chắc muốn xoá chủ đề này không?").then(
      data => {
        this.deletetopic(id);
      });
  }

  deletetopic(id) {
    this.topicService.deletetopic(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.search();
          this.messageService.showSuccess('Xóa chủ đề thành công!');
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
    let activeModal = this.modalService.open(TopicCreateComponent, { container: 'body', windowClass: 'topic-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    });
  }
}
