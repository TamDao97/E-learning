import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, AppSetting, MessageService } from 'src/app/shared';
import { CommentService } from '../../services/comment.service';

@Component({
  selector: 'app-comment-create',
  templateUrl: './comment-create.component.html',
  styleUrls: ['./comment-create.component.scss']
})
export class CommentCreateComponent implements OnInit {

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private commentService: CommentService,
  ) { }

  id: string;
  parentCommentId: number;
  courseId: string;
  lessonId: string;
  isAction: boolean = false;
  listCategory: any[] = [];
  modalInfo = {
    Title: 'Trả lời bình luận',
  };

  model: any = {
    id: '',
    parentCommentId: null,
    courseId: null,
    lessonId: null,
    content: '',
    status: 1
  }

  ngOnInit(): void {
    this.model.parentCommentId = this.parentCommentId;
    this.model.courseId = this.courseId;
    this.model.lessonId = this.lessonId;
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật bình luận';
      this.getbyid();
    } else {
      this.modalInfo.Title = 'Trả lời bình luận';
    }
  }

  getbyid() {
    this.commentService.getCommentInfo(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  saveAndContinue() {
    this.save(true);
  }

  create(isContinue) {
    this.commentService.createComment(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Trả lời bình luận thành công!');
          if (isContinue) {
            this.isAction = true;
            this.clear();
          } else {
            this.closeModal(true);
          }
        } else {
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.update();
    } else {
      this.create(isContinue);
    }
  }

  update() {
    this.commentService.updateComment(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

          this.messageService.showSuccess('Cập nhập bình luận thành công!');
          this.closeModal(true);
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

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  clear() {
    this.model = {
      id: '',
      parentCommentId: null,
      courseId: null,
      lessonId: null,
      content: '',
      status: 1
    }
  }

}
