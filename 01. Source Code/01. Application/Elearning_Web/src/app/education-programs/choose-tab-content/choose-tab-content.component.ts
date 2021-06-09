import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, Constants, MessageService } from 'src/app/shared';
import { CourseService } from '../services/course.service';

@Component({
  selector: 'app-choose-tab-content',
  templateUrl: './choose-tab-content.component.html',
  styleUrls: ['./choose-tab-content.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseTabContentComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public courseService: CourseService,
    public constant: Constants,
    public config: Configuration
  ) { }


  height = 0;
  @ViewChild('scrollPracticeMaterial') scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader') scrollPracticeMaterialHeader: ElementRef;

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  isAction: boolean = false;
  listSelect: any = [];
  listIdSelect: any = [];
  listIdSelectRequest: any = [];
  listData: any = [];
  listItem = []
  listItemData = []
  listIdSelectSearch: any = [];
  isDelete = false;
  isRequest: boolean;

  searchOptions: any = {
    FieldContentName: 'name',
    Placeholder: 'Tìm kiếm theo tên bài giảng ...',
    Items: [
      {
        Name: 'Danh mục',
        FieldName: 'CategoryId',
        Placeholder: 'Chọn',
        Type: 'treeSelect',
        DataType: this.constant.SearchDataType.Category,
        DisplayName: 'title',
        ValueName: 'key'
      },
    ]
  };

  modelSearch: any = {
    TotalItem: 0,

    name: '',
    ListIdSelect: [],
    ListIdChecked:[],
    IsRequest: '',
    CategoryId: ''
  }

  ngOnInit(): void {
    this.height = window.innerHeight - 450;
    this.listIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchContent();
  }

  searchContent() {

    this.listIdSelectSearch = [];
    this.listIdSelect.forEach(element => {
      this.listIdSelectSearch.push(element);
    });
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.lessonId);
    });


    if(this.listSelect.length == 0 && this.isDelete == true){
      this.modelSearch.ListIdSelect = [];
    }

    if(this.listIdSelectSearch.length != 0 && this.listSelect.length == 0){
      this.modelSearch.ListIdSelect = this.listIdSelectSearch;
    }

    if(this.listSelect.length > 0){
      this.listSelect.forEach(element => {
        this.listIdSelectSearch.push(element.id);

      });
      this.modelSearch.ListIdSelect = this.listIdSelectSearch;
    }


    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.lessonId);
      }
    });

    this.courseService.getListLesson(this.modelSearch).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.listData = data.data.dataResults;
        this.modelSearch.TotalItem = data.data.totalItems;

      } else {
        this.messageService.showListMessage(data.message);
      }
    }, error => {
      this.messageService.showError(error);
    })
  }

  checkAll(isCheck: any) {
    if (isCheck) {
      this.listData.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
          this.listItemData.push(element)
        } else {
          element.Checked = false;
          this.listItemData = [];
        }
      }
      );
      this.listSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
          this.listItem.push(element)
        } else {
          element.Checked = false;
          this.listItem = [];

        }
      });
    }
  }

  addRow() {
    this.listData.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });

    this.checkedTop = false;

    this.listSelect.forEach(element => {
      var index = this.listData.indexOf(element);
      if (index > -1) {
        this.listData.splice(index, 1);
      }
      element.Checked = false;
    });

    this.listItemData = [];
    this.listItem = [];
  }

  removeRow() {

    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.listData.push(element);
      }
    });

    this.checkedBot = false;

    this.listData.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
      element.Checked = false;
    });
    this.listItemData = [];
    this.listItem = [];
  }

  checkItem(row) {
    if (row.Checked == true) {
      this.listItem.push(row);
    }
    else {
      var index = this.listItem.indexOf(row);
      if (index > -1) {
        this.listItem.splice(index, 1);
      }
    }
    if (this.listItem.length == this.listSelect.length) {
      this.checkedBot = true;
    }
    else {
      this.checkedBot = false;

    }
  }

  checkItemData(row) {
    if (row.Checked == true) {
      this.listItemData.push(row);
    }
    else {
      var index = this.listItemData.indexOf(row);
      if (index > -1) {
        this.listItemData.splice(index, 1);
      }
    }
    if (this.listItemData.length == this.listData.length) {
      this.checkedTop = true;
    }
    else {
      this.checkedTop = false;

    }
  }

  clear() {
    this.modelSearch = {
      TotalItem: 0,

      name: '',
      ListIdSelect: [],
      IsRequest: '',
    }

    this.modelSearch.IsRequest = this.isRequest;
    if (this.isRequest) {
      this.listIdSelectRequest.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    } else {
      this.listIdSelect.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    }
    this.searchContent();
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
