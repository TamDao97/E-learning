import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, Configuration, AppSetting, MessageService } from 'src/app/shared';
import { CommentService } from '../../services/comment.service';
import { CommentCreateComponent } from '../comment-create/comment-create.component';

@Component({
  selector: 'app-comment-manage',
  templateUrl: './comment-manage.component.html',
  styleUrls: ['./comment-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CommentManageComponent implements OnInit {

  constructor(
    public constant: Constants,
    public config: Configuration,
    public appSetting: AppSetting,
    private modalService: NgbModal,
    private messageService: MessageService,
    private commentService: CommentService,
  ) { }

  startIndex = 1;
  listData: any[] = [];
  listDataAll: any[] = [];
  listComments: any[] = [];
  selectAll: boolean = false;
  selectAllPage: boolean = false;

  totalItem: 0;
  model: any = {
    PageNumber: 1,
    PageSize: 10,
    TotalItems: 0,
    Pending: 0,
    Approved: 0,
    UnApproved: 0,

    Content: '',
    Status: null,
  }

  searchOptions: any = {
    FieldContentName: 'Content',
    Placeholder: 'Bình luận',
    Items: [
      {
        Name: 'Tình trạng',
        FieldName: 'Status',
        Placeholder: 'Chọn tình trạng',
        Type: 'select',
        Data: this.constant.CommentStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  listItem = [];
  listItemAll = [];
  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý bình luận";
    this.searchComment();
  }

  search(status: number) {
    this.model.Status = status;
    this.searchComment();
  }

  searchComment() {
    this.selectAll = false;
    this.listItem = [];
    this.commentService.searchComment(this.model).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
          this.listData = data.data.dataResults;
          this.listDataAll = data.data.dataResultsAll;
          this.model.TotalItems = data.data.totalItems;
          if (this.model.Status == null) {
            this.totalItem = 0;
            this.totalItem = data.data.totalItems;

          }
          if (this.model.Status == 0) {
            this.totalItem = 0;
            this.totalItem = data.data.pending;

          }
          if (this.model.Status == 1) {
            this.totalItem = 0;
            this.totalItem = data.data.approved;

          }
          if (this.model.Status == 2) {
            this.totalItem = 0;
            this.totalItem = data.data.delete;

          }
          this.model.Pending = data.data.pending;
          this.model.Approved = data.data.approved;
          this.model.UnApproved = data.data.delete;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDelete(id: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá bình luận này không?").then(
      data => {
        this.deleteComment(id);
      }
    );
  }

  deleteComment(id: number) {
    this.commentService.deleteComment(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Xóa bình luận thành công!');
          this.searchComment();
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  approve(id: number) {
    this.listComments.push(id);
    this.approved();
  }

  approvedComment() {
    this.listData.forEach(element => {
      if (element.Checked) {
        this.listComments.push(element.id);
      }
    });
    this.approved();

  }

  approved() {
    this.commentService.approvedComment(this.listComments).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Phê duyệt bình luận thành công!');
          this.listComments = [];
          this.selectAll = false;
          this.searchComment();
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

  unapprove(id: number) {
    this.listComments.push(id);
    this.unapproved();
  }

  unapprovedComment() {
    this.listData.forEach(element => {
      if (element.Checked) {
        this.listComments.push(element.id);
      }
    });
    this.unapproved();
  }

  unapproved() {
    this.commentService.unapprovedComment(this.listComments).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Ẩn bình luận thành công!');
          this.listComments = [];
          this.selectAll = false;
          this.searchComment();
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

  reply(row: any) {
    let activeModal = this.modalService.open(CommentCreateComponent, { container: 'body', windowClass: 'comment-create-model', backdrop: 'static' })
    activeModal.componentInstance.parentCommentId = row.id;
    activeModal.componentInstance.courseId = row.courseId;
    activeModal.componentInstance.lessonId = row.lessonId;
    activeModal.result.then((result: any) => {
      if (result) {
        this.searchComment();
      }
    });
  }

  clear() {
    this.listItem = [];
    this.selectAll = false;
    this.model = {
      PageNumber: 1,
      PageSize: 10,
      TotalItems: 0,

      Content: '',
      Status: null,
    }
    this.searchComment();
  }

  selectAlls() {
    this.listItem = [];
    this.listData.forEach(element => {
      if (this.selectAll) {
        element.Checked = true;
        this.listItem.push(element)
      } else {
        element.Checked = false;
        this.listItem = [];
      }
    });
  }

  selectAllPages() {
    this.selectAllPage = true;
    this.listItemAll = [];
    this.listDataAll.forEach(element => {
      if (this.selectAllPage) {
        element.Checked = true;
        this.listItemAll.push(element)
      } else {
        element.Checked = false;
        this.listItemAll = [];
      }
    });

    this.listData.forEach(element => {
      if (this.selectAllPage) {
        element.Checked = true;
        this.listItem.push(element)
      } else {
        element.Checked = false;
        this.listItem = [];
      }
    });
  }

  checkItems(row) {
    if (row.Checked == true) {
      this.listItem.push(row);
    }
    else {
      var index = -1;
      var dem = 0;
      this.listItem.forEach(element => {
        dem = dem + 1;
        if (element.id == row.id) {
          index = dem - 1;
        }
      });

      if (index > -1) {
        this.listItem.splice(index, 1);
      }
    }
    if (this.listItem.length == this.listData.length) {
      this.selectAll = true;
    }
    else {
      this.selectAll = false;

    }
  }

  showCreateUpdate(id: string) {
    let activeModal = this.modalService.open(CommentCreateComponent, { container: 'body', windowClass: 'comment-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result: any) => {
      if (result) {
        this.searchComment();
      }
    });
  }

}
