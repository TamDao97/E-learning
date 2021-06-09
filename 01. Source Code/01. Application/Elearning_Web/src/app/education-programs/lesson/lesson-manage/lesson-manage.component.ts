import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, Configuration, AppSetting, MessageService } from 'src/app/shared';
import { LessonService } from '../../services/lesson.service';
import { LessonCreateComponent } from '../lesson-create/lesson-create.component';
import { LessonHistoryComponent } from '../lesson-history/lesson-history.component';
import { ViewLessonComponent } from '../view-lesson/view-lesson.component';

@Component({
  selector: 'app-lesson-manage',
  templateUrl: './lesson-manage.component.html',
  styleUrls: ['./lesson-manage.component.scss']
})
export class LessonManageComponent implements OnInit {

  constructor(
    public constant: Constants,
    public config: Configuration,
    public appSetting: AppSetting,
    private modalService: NgbModal,
    private lessonService: LessonService,
    private messageService: MessageService,
    private route: Router,
  ) { }

  startIndex = 1;
  listData: any[] = [];
  totalItem = 0;
  lessonModel: any = {
    PageNumber: 1,
    PageSize: 10,
    TotalItems: 0,
    TotalCreating: 0,
    TotalRequest: 0,
    TotalApproval: 0,
    TotalNotApproval: 0,
    TotalNotBrowse: 0,

    Name: '',
    CategoryId: '',
    Status: null,
    Type: null,
    ManageUnitId: null,
    CreateBy: '',
    ApprovalStatus: null
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tên bài giảng',
    Items: [
      {
        Name: 'Tình trạng',
        FieldName: 'ApprovalStatus',
        Placeholder: 'Chọn tình trạng',
        Type: 'select',
        Data: this.constant.approvalStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Danh mục',
        FieldName: 'CategoryId',
        Placeholder: 'Chọn',
        Type: 'treeSelect',
        DataType: this.constant.SearchDataType.Category,
        DisplayName: 'title',
        ValueName: 'key'
      },
      {
        Name: 'Loại bài giảng',
        FieldName: 'Type',
        Placeholder: 'Chọn loại bài giảng',
        Type: 'select',
        Data: this.constant.LessonType,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Người tạo',
        FieldName: 'CreateBy',
        Placeholder: 'Người tạo',
        Type: 'text',
      },

      {
        Name: 'Đơn vị chủ quản',
        FieldName: 'ManageUnitId',
        Placeholder: 'Chọn',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.ManageUnit,
        DisplayName: 'name',
        ValueName: 'id'
      },
    ]
  };

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý bài giảng";
    this.searchLesson();
  }

  searchStatus(status: number) {
    this.lessonModel = {
      PageNumber: 1,
      PageSize: 10,
      TotalItems: 0,
      TotalCreating: 0,
      TotalRequest: 0,
      TotalApproval: 0,
      TotalNotApproval: 0,
      TotalNotBrowse: 0,

      Name: '',
      CategoryId: '',
      Status: null,
      Type: null,
      ManageUnitId: null,
      CreateBy: '',
      ApprovalStatus: status
    }

    this.searchLesson();
  }

  searchLesson() {
    this.lessonService.searchLesson(this.lessonModel).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.startIndex = ((this.lessonModel.PageNumber - 1) * this.lessonModel.PageSize + 1);
          this.listData = data.data.dataResults;
          this.lessonModel.TotalItems = data.data.totalItems;
          this.lessonModel.TotalCreating = data.data.totalCreating;
          this.lessonModel.TotalRequest = data.data.totalRequest;
          this.lessonModel.TotalApproval = data.data.totalApproval;
          this.lessonModel.TotalNotApproval = data.data.totalNotApproval;
          this.lessonModel.TotalNotBrowse = data.data.totalNotBrowse;
          if (this.lessonModel.ApprovalStatus == null) {
            this.totalItem = 0;
            this.totalItem = data.data.totalItems;
          } else if (this.lessonModel.ApprovalStatus == this.constant.approval_status.Course_Approval_Creating) {
            this.totalItem = 0;
            this.totalItem = data.data.totalCreating;
          } else if (this.lessonModel.ApprovalStatus == this.constant.approval_status.Course_Approval_Request) {
            this.totalItem = 0;
            this.totalItem = data.data.totalRequest;
          } else if (this.lessonModel.ApprovalStatus == this.constant.approval_status.Course_Approval_Approved) {
            this.totalItem = 0;
            this.totalItem = data.data.totalApproval;
          } else if (this.lessonModel.ApprovalStatus == this.constant.approval_status.Course_Approval_NotApproved) {
            this.totalItem = 0;
            this.totalItem = data.data.totalNotApproval;
          } else if (this.lessonModel.ApprovalStatus == this.constant.approval_status.Course_Approval_NotBrowse) {
            this.totalItem = 0;
            this.totalItem = data.data.totalNotBrowse;
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

  update(id: string) {
    this.lessonService.updateStatus(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Cập nhật trạng thái bài giảng thành công!');
          this.searchLesson();
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDelete(id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá bài giảng này không?").then(
      data => {
        this.deleteLesson(id);
      }
    );
  }

  deleteLesson(id: string) {
    this.lessonService.deleteLesson(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Xóa bài giảng thành công!');
          this.searchLesson();
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
    this.lessonModel = {
      PageNumber: 1,
      PageSize: 10,
      TotalItems: 0,
      TotalCreating: 0,
      TotalRequest: 0,
      TotalApproval: 0,
      TotalNotApproval: 0,

      Name: '',
      CategoryId: '',
      Status: null,
      Type: null
    }
    this.searchLesson();
  }

  showCreateUpdate(id: string, statusApproval: number) {
    // let activeModal = this.modalService.open(LessonCreateComponent, { container: 'body', windowClass: 'lesson-create-model', backdrop: 'static' })
    // activeModal.componentInstance.id = id;
    // activeModal.componentInstance.statusApproval = statusApproval;
    // activeModal.result.then((result: any) => {
    //   if (result) {
    //     this.searchLesson();
    //   }
    // });
    if (id) {
      this.route.navigate(['/dao-tao/quan-ly-bai-giang/chinh-sua/' + id]);
    } else {
      this.route.navigate(['/dao-tao/quan-ly-bai-giang/them-moi']);
    }
  }

  previewLesson(id: string, type: number) {
    let activeModal = this.modalService.open(ViewLessonComponent, { container: 'body', windowClass: 'view-lesson-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.type = type;
    activeModal.result.then((result) => {
      if (result) {
        this.searchLesson();
      }
    });
  }

  lessonHistory(id: string) {
    let activeModal = this.modalService.open(LessonHistoryComponent, { container: 'body', windowClass: 'lesson-history-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchLesson();
      }
    });
  }
}
