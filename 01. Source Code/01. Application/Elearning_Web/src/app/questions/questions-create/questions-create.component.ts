import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ItemsList } from '@ng-select/ng-select/lib/items-list';
import { strict } from 'assert';
import { stringify } from 'querystring';
import { AppSetting, Configuration, Constants, FileProcess, MessageService } from 'src/app/shared';
import { ImageService } from 'src/app/shared/services/image.service';
import { QuestionsService } from '../service/questions.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TopicService } from 'src/app/education-programs/services/topic.service';
import { TopicCreateComponent } from '../topic-create/topic-create.component';
import { CourseStatusComponent } from 'src/app/education-programs/course/course-status/course-status.component';

declare var tinymce: any;

@Component({
  selector: 'app-questions-create',
  templateUrl: './questions-create.component.html',
  styleUrls: ['./questions-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class QuestionsCreateComponent implements OnInit {

  type: any = {
    Question_Type_OneOption: 1,
    Question_Type_MultiOption: 2,
    Question_Type_Boolean: 3,
    Question_Type_FillWords: 4,
    Question_Type_OrderWords: 5
  };

  isDisable: boolean = true;
  isValidateCheckbox: boolean = true;
  isValidateTopic = true;
  statusApp: boolean = false;
  characters: any = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
  topics: any = [];
  id: string;
  model = {
    id: '',
    name: '',
    topicId: null,
    type: 1,
    status: true,
    content: '',
    approvalStatus: null,
    listAnswer: []
  }

  statusModel: any = {
    status: null,
    content: ''
  }

  discoveryConfig = {
    plugins: ['image code', 'visualblocks', 'print preview', 'table', 'directionality', 'link', 'media', 'codesample', 'table', 'charmap', 'hr', 'pagebreak', 'nonbreaking', 'anchor', 'toc', 'insertdatetime', 'advlist', 'lists', 'textcolor', 'wordcount', 'imagetools', 'contextmenu', 'textpattern', 'searchreplace visualblocks code fullscreen',],
    language: 'vi_VN',
    // file_picker_types: 'file image media',
    automatic_uploads: true,
    toolbar: 'undo redo | fontselect | fontsizeselect | bold italic forecolor backcolor |alignleft aligncenter alignright alignjustify alignnone | numlist | table | link | outdent indent',
    convert_urls: false,
    height: 300,
    auto_focus: false,
    plugin_preview_width: 1000,
    plugin_preview_height: 650,
    readonly: 0,
    images_upload_handler: (blobInfo, success, failure) => {
      this.serviceImg.uploadFile(blobInfo.blob(), 'question').subscribe(
        result => {
          success(this.config.ServerApi + result.data.fileUrl);
        },
        error => {
          return;
        }
      );
    }
  };
  constructor(
    private config: Configuration,
    public constant: Constants,
    public appSetting: AppSetting,
    private route: Router,
    private activatedRoute: ActivatedRoute,
    private serviceImg: ImageService,
    private messageService: MessageService,
    private modalService: NgbModal,
    private questionService: QuestionsService,
    public fileProcess: FileProcess,
    private topicService: TopicService
  ) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.paramMap.get('id');
    this.getTopicFull();
    this.inItModel();
  }

  getTopicFull() {
    this.questionService.getTopicFull().subscribe(data => {
      this.topics = data.data;
    });
  }

  getQuestionById(id) {
    this.questionService.getQuestionById(id).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.model = data.data;
        if (this.model.approvalStatus == 2) {
          this.discoveryConfig.readonly = 1;
        }
        this.statusApp = true;
        this.validate();
      }
      else {
        this.messageService.showListMessage(data.Message);
      }
    },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  inItModel() {
    let id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.appSetting.PageTitle = "Cập nhập câu hỏi";
      this.getQuestionById(id);
    } else {
      this.statusApp = true;
      this.appSetting.PageTitle = "Thêm mới câu hỏi";
      this.model = {
        id: '',
        name: '',
        topicId: null,
        type: 1,
        status: true,
        content: '',
        approvalStatus: null,
        listAnswer: []
      }
      this.setListAnswer();
    }
  }

  setListAnswer() {
    if (this.model.type == 4 || this.model.type == 5) {
      this.isValidateCheckbox = false;
    } else {
      this.isValidateCheckbox = true;
    }

    if (this.model.type == 3)
      this.model.listAnswer = [{ answerLabel: "Đúng", answerContent: 'Đúng', isCorrect: false }, { answerLabel: "Sai", answerContent: 'Sai', isCorrect: false }]
    else if (this.model.type == 4)
      this.model.listAnswer = [{ answerContent: '', answerLabel: "", isCorrect: true }]
    else
      this.model.listAnswer = [
        {
          answerContent: '',
          answerLabel: "A",
          isCorrect: false,
        },
        {
          answerContent: '',
          answerLabel: "B",
          isCorrect: false,
        },
        {
          answerContent: '',
          answerLabel: "C",
          isCorrect: false,
        },
        {
          answerContent: '',
          answerLabel: "D",
          isCorrect: false,
        }];
  }

  refreshAnswer() {
    if (this.model.type == 4) {
      this.model.listAnswer.forEach((item, index) => {
        item.answerLabel = index + 1;
      });
    }
    else if (this.model.type == 3) {
      return;
    } else {
      this.model.listAnswer.forEach((item, index) => {
        item.answerLabel = this.characters[index];
      });
    }
  }

  addElemt() {
    if (this.model.listAnswer.length == this.characters.length) {
      return;
    }
    this.model.listAnswer.push(
      {
        id: '',
        answerContent: '',
        answerLabel: "",
        isCorrect: false,
        displayIndex: this.model.listAnswer.length
      });
    this.refreshAnswer();
    this.validate();
  }

  removeElemt(index: any) {
    this.model.listAnswer.splice(index, 1);
    this.refreshAnswer();
    this.validate();
  }

  changeType() {
    this.setListAnswer();
    this.refreshAnswer();
    this.validate();
  }

  changeStatus(item: any) {
    if (this.model.type == this.type.Question_Type_OneOption || this.model.type == this.type.Question_Type_Boolean) {
      this.model.listAnswer.forEach(elemt => {
        elemt.isCorrect = false;
      });

      item.isCorrect = true;
    } else if (this.model.type == this.type.Question_Type_MultiOption) {
      item.isCorrect = item.isCorrect == true ? false : true;
    }
    this.validate();
  }

  save(isContinue: boolean) {
    this.model.name = tinymce.activeEditor.getContent({ format: "text" });
    if (this.model.type == this.type.Question_Type_FillWords) {
      this.model.listAnswer.forEach(item => {
        item.displayIndex = item.answerLabel;
      });
    }

    if (this.model.id) {
      this.update();
    } else {
      this.create(isContinue);
    }
  }

  create(isContinue) {
    this.questionService.create(this.model).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.messageService.showSuccess('Tạo mới câu hỏi thành công!');
        if (!isContinue) {
          this.route.navigate(['/questions/questions-manager']);
        } else {
          this.clear();
        }
      } else {
        this.messageService.showListMessage(data.Message);
      }
    },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.questionService.update(this.model).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.messageService.showSuccess('Cập nhập câu hỏi thành công!');
        this.inItModel();
      }
      else {
        this.messageService.showListMessage(data.Message);
      }
    },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.inItModel();
  }

  back() {
    this.route.navigate(['/questions/questions-manager']);
  }

  validate() {
    if (this.model.type == 4 || this.model.type == 5) {
      this.isValidateCheckbox = false;
    } else {
      this.isValidateCheckbox = true;
    }
    if (this.model.type == 1 || this.model.type == 2 || this.model.type == 3) {
      this.validateCheckSelected();
    }
    if (this.model.topicId) {
      this.isValidateTopic = false;
    } else
      this.isValidateTopic = true;

    this.validateInputAnswer();
  }

  validateCheckSelected() {
    this.isValidateCheckbox = true;
    this.model.listAnswer.forEach(item => {
      if (item.isCorrect == true) {
        return this.isValidateCheckbox = false;
      }
    });
  }

  validateInputAnswer() {
    this.isDisable = false;
    if (this.model.type != 3) {
      this.model.listAnswer.forEach(item => {
        if (item.answerContent == null || item.answerContent == "") {
          this.isDisable = true;
          return;
        } else {
          this.isDisable = false;
          this.model.listAnswer.forEach(elemt => {
            if (elemt.answerContent == null || elemt.answerContent == "") {
              this.isDisable = true;
              return;
            }
          });
        }
      });
    }
  }

  onChange($event: string): void {
    console.log($event);
    this.validate()
  }

  nodeClicked(node: any) {
    if (!node.isLeaf) {
      node.isSelectable = false;
      node.isExpanded = !node.isExpanded;
    }
  }

  showModalCreateTopic(id: string) {
    let activeModal = this.modalService.open(TopicCreateComponent, { container: 'body', windowClass: 'topic-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (id != null && id != '') {
        this.model.topicId = null;
      }
      if (result) {
        this.getTopicFull();
      }
    });
  }

  showConfirmDelete(row: any) {
    if (row.children && row.children.length == 0) {
      this.messageService.showConfirm("Bạn có chắc muốn xoá chủ đề này không?").then(
        data => {
          this.deletetopic(row.key);
        });
    } else {
      this.messageService.showConfirm("Bạn có chắc muốn xoá chủ đề này không. Xóa chủ đề này sẽ xóa tất cả chủ đề con?").then(
        data => {
          this.deletetopic(row.key);
        }
      );
    }
  }

  deletetopic(id) {
    this.topicService.deletetopic(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Xóa chủ đề thành công!');
          this.getTopicFull();
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  request() {
    this.model.name = tinymce.activeEditor.getContent({ format: "text" });
    if (this.model.type == this.type.Question_Type_FillWords) {
      this.model.listAnswer.forEach(item => {
        item.displayIndex = item.answerLabel;
      });
    }

    if (this.model.id) {
      this.updateRequest();
    }
  }

  updateRequest() {
    this.questionService.update(this.model).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.messageService.showSuccess('Cập nhập câu hỏi thành công!');
        this.questionService.requestQuestion(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Yêu cầu duyệt câu hỏi thành công!');
              this.route.navigate(['/questions/questions-manager']);
            }
            else {
              this.messageService.showListMessage(data.message);
            }
          }, error => {
            this.messageService.showError(error);
          });
      }
      else {
        this.messageService.showListMessage(data.Message);
      }
    },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  requestQuestion() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        if (result != true) {
          this.statusModel.content = result;
        }
        this.statusModel.status = 1;
        this.request();
      }
    });
  }

  approvalQuestion() {
    this.messageService.showConfirm("Bạn có chắc muốn duyệt câu hỏi này không?").then(
      data => {
        this.statusModel.status = 2;
        this.questionService.approvalQuestion(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Duyệt câu hỏi thành công!');
              this.route.navigate(['/questions/questions-manager']);
            }
            else {
              this.messageService.showListMessage(data.message);
            }
          }, error => {
            this.messageService.showError(error);
          });
      });
  }

  notApprovalQuestion() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.statusModel.status = 4;
        if (result != true) {
          this.statusModel.content = result;
        }
        this.questionService.approvalQuestion(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Không duyệt câu hỏi thành công!');
              this.route.navigate(['/questions/questions-manager']);
            }
            else {
              this.messageService.showListMessage(data.message);
            }
          }, error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  canceQuestion() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.statusModel.status = 3;
        if (result != true) {
          this.statusModel.content = result;
        }
        this.questionService.approvalQuestion(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Hủy duyệt câu hỏi thành công!');
              this.route.navigate(['/questions/questions-manager']);
            }
            else {
              this.messageService.showListMessage(data.message);
            }
          }, error => {
            this.messageService.showError(error);
          });
      }
    });
  }
}