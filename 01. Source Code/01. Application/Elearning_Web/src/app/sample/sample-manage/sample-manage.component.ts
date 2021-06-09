import { Component, OnInit } from '@angular/core';

import { AppSetting, Constants } from 'src/app/shared';

@Component({
  selector: 'app-sample-manage',
  templateUrl: './sample-manage.component.html',
  styleUrls: ['./sample-manage.component.scss']
})
export class SampleManageComponent implements OnInit {

  constructor(public appSetting: AppSetting,
    private constant: Constants) { }

  selectedId: any = null;
  items: any[] = [
    {
      Id: 1,
      Name: 'Name 11',
    },
    {
      Id: 2,
      Name: 'Name 22',
    }
  ];

  searchModel: any = {
    SampleName: '',
    SampleCode: '',
    Type: null,
    SampleId: null,
    example:null
  };

  searchOptions: any = {
    FieldContentName: 'SampleName',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
      {
        Name: 'Mã',
        FieldName: 'SampleCode',
        Placeholder: 'Nhập mã',
        Type: 'text'
      },
      {
        Name: 'Loại',
        FieldName: 'Type',
        Placeholder: 'Chọn loại',
        Type: 'select',
        Data: this.items,        
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'SampleId',
        FieldName: 'SampleId',
        Placeholder: 'Chọn',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.Sample,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Thời gian',
        FieldName: 'TimeType',
        Placeholder: 'Nhóm',
        Type: 'select',
        Data: this.constant.SearchTimeTypes,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  ngOnInit(): void {
    this.appSetting.PageTitle = "Ví dụ theme";
  }

  saerchSample() {

  }

  clear() {
    this.searchModel = {
      SampleName: '',
      SampleCode: '',
      Type: null,
      SampleId: null
    };
  }
}
