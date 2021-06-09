import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';

import { MessageService, AppSetting, Constants, Configuration } from 'src/app/shared';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CourseService } from '../services/course.service';
import { ComboboxService } from '../../shared/services/combobox.service';
import { CourseCreateComponent } from '../course-create/course-create.component';
import { NtsSearchService } from 'src/app/shared/services/nts-search.service';
import { NTSSearchBarComponent } from 'src/app/shared/component/nts-search-bar/nts-search-bar.component';
import { PreviewCourseComponent } from '../course/preview-course/preview-course.component';
@Component({
  selector: 'app-course-manage',
  templateUrl: './course-manage.component.html',
  styleUrls: ['./course-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CourseManageComponent implements OnInit {

  @ViewChild('search') searchComponent: NTSSearchBarComponent;

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private modalService: NgbModal,
    private messageService: MessageService,
    public config: Configuration,
    private courseService: CourseService,
  ) { }

  startIndex = 1;
  listData: any[] = [];
  listPageSize = this.constant.ListPageSize;
  totalItem: number = 0;
  model: any = {
    Name: '',
    ApprovalStatus: null,
    ProgramId: null,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    TotalCreating: 0,
    TotalRequest: 0,
    TotalApproval: 0,
    TotalNotApproval: 0,
    ManageUnitId: null,
    TotalNotBrowse: 0
  }
  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
      {
        Name: 'Chương trình đào tạo',
        FieldName: 'ProgramId',
        Placeholder: 'Chọn',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.Program,
        DisplayName: 'name',
        ValueName: 'id'
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

  showCreateUpdate(id: string) {
    let activeModal = this.modalService.open(CourseCreateComponent, { container: 'body', windowClass: 'course-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    });
  }

  ngOnInit(): void {
    this.search();
    this.appSetting.PageTitle = "Quản lý khóa học";
  }

  search() {
    // this.searchComponent.search();
    this.courseService.searchCourse(this.model).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
          this.listData = data.data.dataResults;
          this.model.TotalItems = data.data.totalItems;
          this.model.TotalCreating = data.data.totalCreating;
          this.model.TotalRequest = data.data.totalRequest;
          this.model.TotalApproval = data.data.totalApproval;
          this.model.TotalNotApproval = data.data.totalNotApproval;
          this.model.TotalNotBrowse = data.data.totalNotBrowse;
          if (this.model.ApprovalStatus == null) {
            this.totalItem = 0;
            this.totalItem = data.data.totalItems;
          } else if (this.model.ApprovalStatus == this.constant.approval_status.Course_Approval_Creating) {
            this.totalItem = 0;
            this.totalItem = data.data.totalCreating;
          } else if (this.model.ApprovalStatus == this.constant.approval_status.Course_Approval_Request) {
            this.totalItem = 0;
            this.totalItem = data.data.totalRequest;
          } else if (this.model.ApprovalStatus == this.constant.approval_status.Course_Approval_Approved) {
            this.totalItem = 0;
            this.totalItem = data.data.totalApproval;
          } else if (this.model.ApprovalStatus == this.constant.approval_status.Course_Approval_NotApproved) {
            this.totalItem = 0;
            this.totalItem = data.data.totalNotApproval;
          } else if (this.model.ApprovalStatus == this.constant.approval_status.Course_Approval_NotBrowse) {
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

  searchStatus(status: number) {
    this.model = {
      Name: '',
      ApprovalStatus: status,
      ProgramId: null,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      TotalCreating: 0,
      TotalRequest: 0,
      TotalApproval: 0,
      TotalNotApproval: 0,
      TotalNotBrowse: 0
    }
    this.search();
  }

  UpdateStatus(id) {
    this.courseService.updateStatusCourse(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.search();
          this.messageService.showSuccess('Cập nhập khóa học thành công!');
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
      ProgramId: null,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
    }
    this.search();
  }

  showConfirmDelete(id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá khóa học này không?").then(
      data => {
        this.deleteCourse(id);
      });
  }

  deleteCourse(id) {
    this.courseService.deleteCourse(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.search();
          this.messageService.showSuccess('Xóa khóa học thành công!');
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  previewCourse(id: string) {
    let activeModal = this.modalService.open(PreviewCourseComponent, { container: 'body', windowClass: 'preview-course-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    });
  }
}
