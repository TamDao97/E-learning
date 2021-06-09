import { Component, OnInit } from '@angular/core';

import { MessageService, AppSetting, Constants, Configuration } from 'src/app/shared';
import { ProgramCreateComponent } from '../program-create/program-create.component';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EducationProgramService } from '../../services/education-program.service'
@Component({
  selector: 'app-program-manage',
  templateUrl: './program-manage.component.html',
  styleUrls: ['./program-manage.component.scss']
})
export class ProgramManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private modalService: NgbModal,
    private messageService: MessageService,
    private programService: EducationProgramService,
    public config: Configuration,
  ) { }
  startIndex = 1;
  listData: any[] = [];
  listPageSize = this.constant.ListPageSize;
  model: any = {
    Name: '',
    Description:'',
    Code:'',
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
  }
  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
      {
        Name: 'Mã chương trình',
        FieldName: 'Code',
        Placeholder: 'Nhập mã chương trình',
        Type: 'text'
      },
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
    this.appSetting.PageTitle = "Quản lý chương trình đào tạo";
  }

  search() {
    this.programService.searchProgram(this.model).subscribe(
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

  UpdateStatus(id){
    this.programService.updateStatusProgram(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.search();
          this.messageService.showSuccess('Cập nhập trạng thái chương trình đào tạo thành công!');
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
      Description:'',
      Code:'',
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
    }
    this.search();
  }

  showConfirmDelete(id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chương trình đào tạo này không?").then(
      data => {
        this.deleteProgram(id);
      });
  }

  deleteProgram(id) {
    this.programService.deleteProgram(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.search();
          this.messageService.showSuccess('Xóa chương trình đào tạo thành công!');
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
    let activeModal = this.modalService.open(ProgramCreateComponent, { container: 'body', windowClass: 'program-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    });
  }
}
