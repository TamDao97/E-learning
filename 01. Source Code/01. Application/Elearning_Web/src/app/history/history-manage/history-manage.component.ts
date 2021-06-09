import { Component, OnInit } from '@angular/core';
import { AppSetting, Constants, DateUtils, MessageComponent, MessageService } from 'src/app/shared';
import { HistoryService } from '../service/history.service';

@Component({
  selector: 'app-history-manage',
  templateUrl: './history-manage.component.html',
  styleUrls: ['./history-manage.component.scss']
})
export class HistoryManageComponent implements OnInit {

  constructor(
    public constant: Constants,
    public historyService: HistoryService,
    public messageService: MessageService,
    public appSetting: AppSetting,
    public dateUtils: DateUtils
  ) { }
  startIndex = 0;
  listHistory = [];

  model: any = {
    Content: '',
    UserId: '',
    DateToV: '',
    DateFromV: '',
    Type: 0,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
  }

  searchOptions: any = {
    FieldContentName: 'Content',
    Placeholder: 'Tìm kiếm theo nội dung',
    Items: [
      {
        Name: 'Tài khoản',
        FieldName: 'UserId',
        Placeholder: 'Chọn',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.User,
        DisplayName: 'name',
        ValueName: 'id'
      },
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      }
    ]
  };


  ngOnInit(): void {
    this.appSetting.PageTitle = "Lịch sử hoạt động quản trị";
    this.searchHistory();
  }

  searchHistory() {
    if (this.model.DateFromV) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    } else {
      this.model.DateFrom = null;
    }
    if (this.model.DateToV) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.model.DateToV)
    } else {
      this.model.DateTo = null;
    }
    this.historyService.searchHistory(this.model).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.startIndex =
            (this.model.PageNumber - 1) * this.model.PageSize + 1;
          this.listHistory = data.data.dataResults;
          this.model.TotalItems = data.data.totalItems;
        } else {
          this.messageService.showListMessage(data.message);
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.model = {
      Content: '',
      UserId: '',
      DateStart: '',
      DateEnd: '',
      Type: 0,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
    }
    this.searchHistory();

  }

}
