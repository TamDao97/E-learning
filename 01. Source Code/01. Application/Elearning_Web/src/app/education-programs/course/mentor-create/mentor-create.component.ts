import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, Configuration, Constants, MessageService } from '../../../shared';
import { UserService } from '../../../users/service/user.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-mentor-create',
  templateUrl: './mentor-create.component.html',
  styleUrls: ['./mentor-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class MentorCreateComponent implements OnInit {

  constructor(
    public constant: Constants,
    public config: Configuration,
    public appSetting: AppSetting,
    private userService: UserService,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
  ) { }

  startIndex = 1;
  listData: any[] = [];
  //IDs các giảng viên đang chọn
  dataSelect: any[] = [];
  //IDs các giảng viên đã lưu
  dataSelected = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    Name: '',
    PhoneNumber: '',
    Email: '',
    Address: ''
  }
  listPageSize = this.constant.ListPageSize;

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên giảng viên',
    Items: [
      {
        Name: 'Địa chỉ',
        FieldName: 'Address',
        Placeholder: 'Nhập địa chỉ',
        Type: 'text'
      },
      {
        Name: 'Email',
        FieldName: 'Email',
        Placeholder: 'Nhập địa chỉ',
        Type: 'text'
      },
      {
        Name: 'Số điện thoại',
        FieldName: 'PhoneNumber',
        Placeholder: 'Nhập số điện thoại',
        Type: 'text'
      },
    ]
  };

  isInit = false;

  ngOnInit(): void {
    this.search();
  }

  search() {
    this.userService.searchMentor(this.model).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
          this.listData = data.data.dataResults;
          this.model.TotalItems = data.data.totalItems;
          if (!this.isInit) {
            this.listData.forEach(element => {
              if (this.isSelect(element.id)) {
                element.isChecked = true;
              }
              else {
                if (this.dataSelected.indexOf(element.id) === -1) {
                  element.isChecked = false;
                }
                else {
                  element.isChecked = true;
                  this.dataSelect.push(element);
                }
              }
            });
            this.isInit = true;
          }
          else {
            this.listData.forEach(element => {
              if (this.isSelect(element.id)) {
                element.isChecked = true;
              }
              else {
                element.isChecked = false;
              }
            });
          }

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
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      Name: '',
      PhoneNumber: '',
      Email: '',
      Address: ''
    }
    this.search();
  }

  closeModal() {
    this.activeModal.close(true);
  }

  save() {
    this.activeModal.close({ data: this.dataSelect, totalItems: this.dataSelect.length });
  }

  selectChange($event, row) {
    const index = this.dataSelect.indexOf(row);
    if (row.isChecked) {
      if (index === -1) {
        this.dataSelect.push(row);
      }
    }
    else {
      this.dataSelect.splice(index, 1);
    }
  }

  isSelect(id) {
    for (var i = 0; i < this.dataSelect.length; i++) {
      if (this.dataSelect[i].id == id) {
        return true;
      }
    }
    return false;
  }
}
