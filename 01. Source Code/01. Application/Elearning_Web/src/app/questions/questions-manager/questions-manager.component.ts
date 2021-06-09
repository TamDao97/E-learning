import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ShowQuestionComponent } from 'src/app/education-programs/lesson/show-question/show-question.component';
import {
  AppSetting,
  Configuration,
  Constants,
  FileProcess,
  MessageService,
} from 'src/app/shared';
import { QuestionHistoryComponent } from '../question-history/question-history.component';
import { QuestionsService } from '../service/questions.service';

@Component({
  selector: 'app-questions-manager',
  templateUrl: './questions-manager.component.html',
  styleUrls: ['./questions-manager.component.scss'],
})
export class QuestionsManagerComponent implements OnInit {
  constructor(
    public route: Router,
    public appSetting: AppSetting,
    public constant: Constants,
    public messageService: MessageService,
    public fileProcess: FileProcess,
    public questionsService: QuestionsService,
    public config: Configuration,
    private modalService: NgbModal
  ) { }

  startIndex = 1;
  listTopic: any[] = [];
  listData: any[] = [];
  listPageSize = this.constant.ListPageSize;
  totalItem = 0;
  model: any = {
    PageNumber: 1,
    PageSize: 10,
    TotalItems: 0,
    TotalCreating: 0,
    TotalRequest: 0,
    TotalApproval: 0,
    TotalNotApproval: 0,
    TotalNotBrowse: 0,
    Content: '',
    Type: null,
    Status: null,
    TopicId: null,
    ApprovalStatus: null
  };

  ngOnInit(): void {
    this.getTopicFull();
    this.search();
    this.appSetting.PageTitle = 'Quản lý ngân hàng câu hỏi';
  }

  searchOptions: any = {
    FieldContentName: '',
    Placeholder: 'Chọn thông tin tìm kiếm',
    Items: [
      {
        Name: 'Chủ đề',
        FieldName: 'TopicId',
        Placeholder: 'Chọn chủ đề',
        Type: 'treeSelect',
        Data: this.listTopic,
        DisplayName: 'title',
        ValueName: 'key',
      },
      {
        Name: 'Tình trạng',
        FieldName: 'ApprovalStatus',
        Placeholder: '',
        Type: 'select',
        Data: this.constant.approvalStatus,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'Loại câu hỏi',
        FieldName: 'Type',
        Placeholder: 'Chọn loại câu hỏi',
        Type: 'ngselect',
        Data: this.constant.QuestionType,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
    ],
  };

  getTopicFull() {
    this.questionsService.getTopicFull().subscribe(
      (data: any) => {
        this.listTopic = data.data;
        this.searchOptions.Items[0].Data = this.listTopic;
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  searchStatus(status: number) {
    this.model = {
      PageNumber: 1,
      PageSize: 10,
      TotalItems: 0,
      TotalCreating: 0,
      TotalRequest: 0,
      TotalApproval: 0,
      TotalNotApproval: 0,
      Content: '',
      Type: null,
      Status: null,
      TopicId: null,
      ApprovalStatus: status
    };
    this.search();
  }

  search() {
    this.questionsService.search(this.model).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.startIndex = (this.model.PageNumber - 1) * this.model.PageSize + 1;
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
        } else {
          this.messageService.showListMessage(data.message);
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  navigateCreate() {
    this.route.navigate(['/questions/questions-create']);
  }

  navigateEdit(id: string) {
    this.route.navigate(['/questions/questions-edit/' + id]);
  }

  showConfirmDelete(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá câu hỏi này không?')
      .then((data) => {
        this.fileProcess;
        this.delete(id);
      });
  }

  delete(id: string) {
    this.questionsService.delete(id).subscribe(
      (data) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Xóa câu hỏi thành công!');
          this.search();
        } else {
          this.messageService.showListMessage(data.message);
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  showQuestion(id: string) {
    let activeModal = this.modalService.open(ShowQuestionComponent, {
      container: 'body',
      windowClass: 'show-question-model',
      backdrop: 'static',
    });
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.type = 2;
    activeModal.result.then((result: any) => {
      if (result) {
        this.search();
      }
    });
  }

  UpdateStatus(id) {
    this.questionsService.updateStatusQuestion(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.search();
          this.messageService.showSuccess('Cập nhập câu hỏi thành công!');
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
      Content: '',
      Type: null,
      TopicId: null,
      ApprovalStatus: null,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      TotalCreating: 0,
      TotalRequest: 0,
      TotalApproval: 0,
      TotalNotApproval: 0,
      TotalNotBrowse: 0
    };
    this.search();
  }

  questionHistory(id: string) {
    let activeModal = this.modalService.open(QuestionHistoryComponent, { container: 'body', windowClass: 'question-history-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    });
  }
}
