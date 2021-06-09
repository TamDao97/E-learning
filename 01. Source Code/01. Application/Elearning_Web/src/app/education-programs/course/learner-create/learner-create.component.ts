import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { UserService } from 'src/app/users/service/user.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-learner-create',
  templateUrl: './learner-create.component.html',
  styleUrls: ['./learner-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LearnerCreateComponent implements OnInit {

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
  //IDs các người học đang chọn
  dataSelect: any[] = [];
  //IDs các người học đã lưu
  dataSelected:any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    Name: '',
  }
  listPageSize = this.constant.ListPageSize;

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên người học',
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
    this.userService.searchLearner(this.model).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
          this.listData = data.data.dataResults;
          this.model.TotalItems = data.data.totalItems;
          // if(!this.isInit){
          this.listData.forEach(element => {
            if (this.isSelect(element.id)) {
              element.isChecked = true;
            }
            else {
              if (this.dataSelected.map(x => x.id).indexOf(element.Id) === -1) {
                element.isChecked = false;
              }
              else {
                element.isChecked = true;
              }
            }
          });
          this.isInit = true;
          // }
          // else{
          //   this.listData.forEach(element => {
          //     if (this.isSelect(element.id)) {
          //       element.isChecked = true;
          //     }
          //     else {
          //       element.isChecked = false;
          //     }
          //   });
          // }
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
    }
    this.search();
  }

  closeModal() {
    this.activeModal.close(true);
  }

  save() {
    this.dataSelected.forEach(element => {
      element.isChecked = false;
    });
    this.activeModal.close({ data: this.dataSelected, totalItems: this.dataSelected.length });
  }

  selectChange($event, row) {
    const index = this.dataSelected.map(x => x.id).indexOf(row.id);
    if(row.isChecked)
    {
      if (index===-1) {

        // kiểm tra dataselected đã có chưa, nếu chưa có thì add mới vào
          this.dataSelected.push(row);
      }
    }
    else{
      if (index!==-1) {

        // kiểm tra dataselected đã có chưa, nếu có thì remove
          this.dataSelected.splice(index,1);
      }
    }
   
  }

  isSelect(id) {
    for (var i = 0; i < this.dataSelected.length; i++) {
      if (this.dataSelected[i].id == id) {
        return true;
      }
    }
    return false;
  }
}
