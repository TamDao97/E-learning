import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NzFormatEmitEvent } from 'ng-zorro-antd/tree';
import { AppSetting, MessageService, FileProcess, Constants, DateUtils, Configuration } from 'src/app/shared';
import { ComboboxService } from 'src/app/shared/services/combobox.service';
import { ImageService } from 'src/app/shared/services/image.service';
import { CategoryCreateComponent } from '../../category/category-create/category-create.component';
import { CourseStatusComponent } from '../../course/course-status/course-status.component';
import { CategoryService } from '../../services/category.service';
import { LessonService } from '../../services/lesson.service';
import { ChooseQuestionRandomComponent } from '../choose-question-random/choose-question-random.component';
import { ChooseQuestionComponent } from '../choose-question/choose-question.component';
import { LessonFrameCreateComponent } from '../lesson-frame-create/lesson-frame-create.component';
import { LessonFrameTypeComponent } from '../lesson-frame-type/lesson-frame-type.component';
import { ShowQuestionComponent } from '../show-question/show-question.component';

declare var tinymce: any;

@Component({
  selector: 'app-lesson-create',
  templateUrl: './lesson-create.component.html',
  styleUrls: ['./lesson-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LessonCreateComponent implements OnInit {

  constructor(
    private route: Router,
    public appSetting: AppSetting,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constant: Constants,
    public config: Configuration,
    private modalService: NgbModal,
    public fileProcessImage: FileProcess,
    public fileProcessDataSheet: FileProcess,
    private serviceImg: ImageService,
    private lessonService: LessonService,
    private categoryService: CategoryService,
    private comboboxService: ComboboxService,
    private activatedRoute: ActivatedRoute,
  ) { }

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
    content_style: "body {font-size: 12pt;font-family: Arial;}",
    aligncenter: {
      selector: 'media', classes: 'center',
      styles: { display: 'block', margin: '0px auto', textAlign: 'center' }
    },
    file_browser_callback: function RoxyFileBrowser(field_name, url, type, win) {
      //var roxyFileman = '/fileman/index.html';
      var roxyFileman = "https://nhantinsoft.vn:9508/fileServer/fileman/index.html";
      if (roxyFileman.indexOf("?") < 0) {
        roxyFileman += "?type=" + type;
      }
      else {
        roxyFileman += "&type=" + type;
      }
      roxyFileman += '&input=' + field_name + '&value=' + win.document.getElementById(field_name).value;
      if (tinymce.activeEditor.settings.language) {
        roxyFileman += '&langCode=' + tinymce.activeEditor.settings.language;
      }
      tinymce.activeEditor.windowManager.open({
        file: roxyFileman,
        title: 'Roxy Fileman',
        width: 850,
        height: 650,
        resizable: "yes",
        plugins: "media",
        inline: "yes",
        close_previous: "no"
      }, {
        window: win,
        input: field_name
      });
      return false;
    },
    //setup: TinymceUserConfig.setup,
    // content_css: '/assets/css/custom_editor.css',
    images_upload_handler: (blobInfo, success, failure) => {
      this.serviceImg.uploadFile(blobInfo.blob(), 'E_Learning/Lesson').subscribe(
        result => {
          success(this.config.ServerApi + result.data.fileUrl);
        },
        error => {
          return;
        }
      );
    },
  };

  id: string;
  statusApproval: number;
  statusTinymce: boolean = false;
  isAction: boolean = false;
  listCategory: any[] = [];
  listSearch: any[] = [];
  expandKeys: any[] = [];
  listLessonFrame: any[] = [];
  filedata: any;
  fileImage: any;
  fileOld: any;
  modalInfo = {
    Title: 'Thêm mới bài giảng',
  };

  statusModel: any = {
    status: null,
    content: ''
  }

  model: any = {
    id: '',
    categoryId: null,
    name: '',
    description: '',
    content: '',
    imagePath: '',
    status: true,
    approvalStatus: null,
    type: null,
    isExam: false,
    examTime: null,
    totalRequestCorrect: 0,
    totalQuestion: 0,
    listQuestion: [],
    listLessonFrame: []
  }

  lessonFrameModel: any = {
    id: '',
    lessonId: '',
    name: '',
    content: '',
    type: '',
    estimatedTime: '',
    testTime: '',
    displayIndex: null,
    listQuestion: []
  }

  questionRandomModel: any = {
    NumberQuestion: 0,
    ListId: [],
    ListTopic: []
  }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.paramMap.get('id');
    this.getCategory();
    if (this.id) {
      //this.modalInfo.Title = 'Cập nhật bài giảng';
      this.appSetting.PageTitle = "Cập nhật bài giảng";
      this.getbyid();
    } else {
      //this.modalInfo.Title = 'Thêm mới bài giảng';
      this.appSetting.PageTitle = "Thêm mới bài giảng";
      this.statusTinymce = true;
    }
  }

  isExam() {
    if (this.model.type == this.constant.Lesson_Type.Lesson_Type_Exam) {
      this.model.isExam == true;
    }
    else {
      this.model.isExam == false;
    }
  }

  // onFileChange($event: any) {
  //   this.fileProcess.readDataFileIOnUploadSize($event).subscribe(
  //     data => {
  //       this.fileImage = data;
  //     }
  //   );
  // }

  getCategory() {
    this.comboboxService.getCategoryParent().subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listCategory = data.data;
          this.listSearch = this.listCategory;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getbyid() {
    this.lessonService.getLessonInfo(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          // if (data.data.imagePath) {
          //   this.filedata = this.config.ServerApi + data.data.imagePath;
          // }
          this.statusApproval = this.model.approvalStatus;
          if (this.statusApproval === 2) {
            this.discoveryConfig.readonly = 1;
          }
          this.statusTinymce = true;
          // if (this.model.type == this.constant.Lesson_Type.Lesson_Type_Theory && this.model.listLessonFrame.length > 0) {
          //   this.loadValue(this.model.listLessonFrame[0], 0);
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

  saveAndContinue() {
    this.save(true);
  }

  create(isContinue: any) {
    this.lessonService.createLesson(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thêm mới bài giảng thành công!');
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
    if (this.model.type == this.constant.Lesson_Type.Lesson_Type_Study && this.model.listQuestion.length > this.model.totalRequestCorrect) {
      this.messageService.showMessage("Số câu trả lời cần đạt không được lớn hơn số câu hỏi!");
    }

    if (!this.file) {
      if (this.id) {
        this.update();
      } else {
        this.create(isContinue);
      }
    }
    else {
      this.serviceImg.uploadFileZip(this.file.File, 'E_Learning/Lesson/Scorm').subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.model.imagePath = data.data.fileUrl;
            if (this.id) {
              this.update();
            } else {
              this.create(isContinue);
            }
          }
          else {
            this.messageService.showListMessage(data.message);
          }
        },
        error => {
          this.messageService.showError(error);
        });
    }

  }

  update() {
    this.lessonService.updateLesson(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

          this.messageService.showSuccess('Cập nhập bài giảng thành công!');
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
    //this.activeModal.close(isOK ? isOK : this.isAction);
    this.route.navigate(['dao-tao/quan-ly-bai-giang']);
  }

  clear() {
    this.model = {
      id: '',
      categoryId: this.model.categoryId,
      name: '',
      description: '',
      content: '',
      imagePath: '',
      status: true,
      type: this.model.type,
      isExam: false,
      examTime: null,
      totalRequestCorrect: 0,
      totalQuestion: 0,
      listQuestion: []
    }
    this.fileImage = null;
    this.filedata = null;
  }

  showConfirmDelete(index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá câu hỏi này không?").then(
      data => {
        this.model.listQuestion.splice(index, 1);
      }
    );
  }

  showConfirmDeleteLessonFrameQuestion(index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá câu hỏi này không?").then(
      data => {
        this.listQuestionLF.splice(index, 1);
      }
    );
  }

  showConfirmDeleteLessonFrame(index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá bài giảng chi tiết này không?").then(
      data => {
        this.model.listLessonFrame.splice(index, 1);
      }
    );
  }

  getQuestionRandom() {
    this.lessonService.getQuestionRandom(this.questionRandomModel).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          if (data.data.length > 0) {
            if (this.model.type == this.constant.Lesson_Type.Lesson_Type_Study) {
              data.data.forEach((element: any) => {
                this.model.listQuestion.push(element);
              });
            } else if (this.typeLF == this.constant.Lesson_Type.Lesson_Type_Study && this.model.type == this.constant.Lesson_Type.Lesson_Type_Theory) {
              data.data.forEach((element: any) => {
                this.listQuestionLF.push(element);
              });
            }
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

  onChange(event: NzFormatEmitEvent) {
    if (event) {
      this.listCategory = this.listSearch.filter(o => o.title.toUpperCase().includes(event));
    }
    else {
      this.listCategory = this.listSearch;
    }
  }

  showChooseQuestion() {
    var listQuestId = [];
    if (this.model.type == this.constant.Lesson_Type.Lesson_Type_Study) {
      this.model.listQuestion.forEach((element: { id: any; }) => {
        listQuestId.push(element.id);
      });
    } else if (this.typeLF == this.constant.Lesson_Type.Lesson_Type_Study && this.model.type == this.constant.Lesson_Type.Lesson_Type_Theory) {
      this.listQuestionLF.forEach((element: { id: any; }) => {
        listQuestId.push(element.id);
      });
    }
    let activeModal = this.modalService.open(ChooseQuestionComponent, { container: 'body', windowClass: 'choose-question-model', backdrop: 'static' })
    activeModal.componentInstance.listQuestId = listQuestId;
    activeModal.result.then((result: any) => {
      if (result.length > 0) {
        if (this.model.type == this.constant.Lesson_Type.Lesson_Type_Study) {
          result.forEach((element: any) => {
            this.model.listQuestion.push(element);
          });
        } else if (this.typeLF == this.constant.Lesson_Type.Lesson_Type_Study && this.model.type == this.constant.Lesson_Type.Lesson_Type_Theory) {
          result.forEach((element: any) => {
            this.listQuestionLF.push(element);
          });
        }
      }
    });
  }

  showChooseQuestionRandom() {
    var listQuestId = [];
    if (this.model.type == this.constant.Lesson_Type.Lesson_Type_Study) {
      this.model.listQuestion.forEach((element: { id: any; }) => {
        listQuestId.push(element.id);
      });
    } else if (this.typeLF == this.constant.Lesson_Type.Lesson_Type_Study && this.model.type == this.constant.Lesson_Type.Lesson_Type_Theory) {
      this.listQuestionLF.forEach((element: { id: any; }) => {
        listQuestId.push(element.id);
      });
    }

    let activeModal = this.modalService.open(ChooseQuestionRandomComponent, { container: 'body', windowClass: 'choose-question-random-model', backdrop: 'static' })
    activeModal.result.then((result: any) => {
      if (result) {
        this.questionRandomModel = {
          NumberQuestion: result.number,
          ListTopic: result.listData,
          ListId: listQuestId,
        }
        this.getQuestionRandom();
      }
    });
  }

  showCreateUpdate(id: string, parentId: string) {
    let activeModal = this.modalService.open(CategoryCreateComponent, { container: 'body', windowClass: 'category-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.parentId = parentId;
    activeModal.result.then((result: any) => {
      if (result) {
        this.getCategory();
      }
    });
  }

  showQuestion(id: string) {
    let activeModal = this.modalService.open(ShowQuestionComponent, { container: 'body', windowClass: 'show-question-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result: any) => {
      if (result) {

      }
    });
  }

  showConfirmDeleteCategory(row: any) {
    if (row.children && row.children.length == 0) {
      this.messageService.showConfirm("Bạn có chắc muốn xoá danh mục này không?").then(
        data => {
          this.deleteCategory(row.key);
        }
      );
    } else {
      this.messageService.showConfirm("Bạn có chắc muốn xoá danh mục này không. Xóa danh mục này sẽ xóa tất cả danh mục con?").then(
        data => {
          this.deleteCategory(row.key);
        }
      );
    }
  }

  deleteCategory(id: string) {
    this.categoryService.deleteCategory(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Xóa danh mục thành công!');
          this.getCategory();
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
    if (this.model.type == this.constant.Lesson_Type.Lesson_Type_Study && this.model.listQuestion.length > this.model.totalRequestCorrect) {
      this.messageService.showMessage("Số câu trả lời cần đạt không được lớn hơn số câu hỏi!");
    }

    if (this.fileImage == null) {
      if (this.id) {
        this.updateRequest();
      }
    }
    else {
      this.serviceImg.uploadFile(this.fileImage.File, 'E_Learning/Lesson').subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.model.imagePath = data.data.fileUrl;
            if (this.id) {
              this.updateRequest();
            }
          }
          else {
            this.messageService.showListMessage(data.message);
          }
        },
        error => {
          this.messageService.showError(error);
        });
    }
  }

  updateRequest() {
    this.lessonService.updateLesson(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Cập nhập bài giảng thành công!');
          this.lessonService.requestLesson(this.id, this.statusModel).subscribe(
            data => {
              if (data.statusCode == this.constant.StatusCode.Success) {
                this.messageService.showSuccess('Yêu cầu duyệt bài giảng thành công!');
                this.closeModal(true);
              }
              else {
                this.messageService.showListMessage(data.message);
              }
            }, error => {
              this.messageService.showError(error);
            });
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

  requestLesson() {
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

  approvalLesson() {
    this.messageService.showConfirm("Bạn có chắc muốn duyệt bài giảng này không?").then(
      data => {
        this.statusModel.status = 2;
        this.lessonService.approvalLesson(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Duyệt bài giảng thành công!');
              this.closeModal(true);
            }
            else {
              this.messageService.showListMessage(data.message);
            }
          }, error => {
            this.messageService.showError(error);
          });
      });
  }

  notApprovalLesson() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.statusModel.status = 4;
        if (result != true) {
          this.statusModel.content = result;
        }
        this.lessonService.approvalLesson(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Không duyệt bài giảng thành công!');
              this.closeModal(true);
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

  canceLesson() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.statusModel.status = 3;
        if (result != true) {
          this.statusModel.content = result;
        }
        this.lessonService.approvalLesson(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Hủy duyệt bài giảng thành công!');
              this.closeModal(true);
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

  typeLF: 1;
  contentLF: string;
  estimatedTime = '0000';
  testTime: number = 0;
  totalRequestCorrect: number = 0;
  lessonFrameName: '';
  listQuestionLF: any[] = [];
  selectIndex = -1;

  // loadValue(param: any, index: number) {
  //   //this.updateLessonFrame();

  //   //param.check = true;

  //   // if (this.selectIndex != index && this.selectIndex) {
  //   //   this.lessonFrameModel.check = false;
  //   // }

  //   this.lessonFrameModel = param;
  //   this.selectIndex = index;
  //   this.typeLF = param.type;
  //   this.contentLF = param.content;
  //   this.estimatedTime = param.estimatedTime;
  //   this.testTime = param.testTime;
  //   this.totalRequestCorrect = param.totalRequestCorrect;
  //   //this.lessonFrameName = param.name;
  //   this.listQuestionLF = param.listQuestion;

  // }

  addRowLessonFrameName() {
    if (this.model.type == this.constant.Lesson_Type.Lesson_Type_Theory && this.typeLF == this.constant.Lesson_Type.Lesson_Type_Study && this.listQuestionLF.length > this.totalRequestCorrect) {
      this.messageService.showMessage("Số câu trả lời cần đạt không được lớn hơn số câu hỏi!");
    }

    if (this.lessonFrameName == '') {
      this.messageService.showMessage("Bạn không được để trống tên danh mục");
    } else if (!this.typeLF) {
      this.messageService.showMessage("Bạn không được để trống loại bài giảng");
    }
    else {
      var paramM = Object.assign({}, this.lessonFrameModel);
      paramM.name = this.lessonFrameName;
      paramM.content = this.contentLF;
      paramM.type = this.typeLF;
      paramM.estimatedTime = this.estimatedTime;
      paramM.testTime = this.testTime;
      paramM.totalRequestCorrect = this.totalRequestCorrect;
      paramM.listQuestion = this.listQuestionLF;
      this.model.listLessonFrame.push(paramM);
    }

    this.typeLF = 1;
    this.contentLF = '';
    this.estimatedTime = '0000';
    this.testTime = 0;
    this.totalRequestCorrect = 0;
    this.lessonFrameName = '';
    this.listQuestionLF = [];
  }

  // Cập nhật thông tin danh mục
  updateLessonFrame() {
    if (this.selectIndex > -1) {
      //this.model.listLessonFrame[this.selectIndex].name = this.lessonFrameName;
      this.model.listLessonFrame[this.selectIndex].content = this.contentLF;
      this.model.listLessonFrame[this.selectIndex].type = this.typeLF;
      this.model.listLessonFrame[this.selectIndex].estimatedTime = this.estimatedTime;
      this.model.listLessonFrame[this.selectIndex].testTime = this.testTime;
      this.model.listLessonFrame[this.selectIndex].totalRequestCorrect = this.totalRequestCorrect;
      this.model.listLessonFrame[this.selectIndex].listQuestion = this.listQuestionLF;
      this.messageService.showSuccess('Cập nhập danh mục bài giảng thành công!');
    }

    // this.typeLF = 1;
    // this.contentLF = '';
    // this.estimatedTime = 0;
    // this.testTime = 0;
    // this.totalRequestCorrect = 0;
    // this.lessonFrameName = '';
    // this.listQuestionLF = [];
    // this.selectIndex = -1;
  }

  clearLessonFrame() {
    if (this.selectIndex > -1) {
      //this.updateLessonFrame();
      this.typeLF = 1;
      this.contentLF = '';
      this.estimatedTime = '0000';
      this.testTime = 0;
      this.totalRequestCorrect = 0;
      this.lessonFrameName = '';
      this.listQuestionLF = [];
      this.selectIndex = -1;
      //this.lessonFrameModel.check = false;
    }
  }

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
    }
  }

  chooseLessonType() {
    if (this.statusApproval !== 2) {
      let activeModal = this.modalService.open(LessonFrameTypeComponent, { container: 'body', windowClass: 'lesson-frame-type-model', backdrop: 'static' })
      activeModal.result.then((result) => {
        if (result) {
          this.createLessonFrame(result);
        }
      });
    }
  }

  createLessonFrame(result: any) {
    let activeModal = this.modalService.open(LessonFrameCreateComponent, { container: 'body', windowClass: 'lesson-frame-create-model', backdrop: 'static' })
    activeModal.componentInstance.type = result;
    activeModal.result.then((result) => {
      if (result) {
        var paramM = Object.assign({}, this.lessonFrameModel);
        paramM.name = result.name;
        paramM.content = result.content;
        paramM.type = result.type;
        paramM.estimatedTime = result.estimatedTime;
        paramM.testTime = result.testTime;
        paramM.totalRequestCorrect = result.totalRequestCorrect;
        paramM.listQuestion = result.listQuestion;
        this.model.listLessonFrame.push(paramM);
      }
    });
  }

  updateLessonFrames(row: any, index: number) {
    let activeModal = this.modalService.open(LessonFrameCreateComponent, { container: 'body', windowClass: 'lesson-frame-create-model', backdrop: 'static' })
    activeModal.componentInstance.type = row.type;
    activeModal.componentInstance.data = row;
    activeModal.result.then((result) => {
      if (result) {
        this.model.listLessonFrame[index].name = result.name;
        this.model.listLessonFrame[index].content = result.content;
        this.model.listLessonFrame[index].type = result.type;
        this.model.listLessonFrame[index].estimatedTime = result.estimatedTime;
        this.model.listLessonFrame[index].testTime = result.testTime;
        this.model.listLessonFrame[index].totalRequestCorrect = result.totalRequestCorrect;
        this.model.listLessonFrame[index].listQuestion = result.listQuestion;
      }
    });
  }

  file: any;
  onFileChange($event: any) {
    this.fileProcess.readDataFileIOnUploadSize($event).subscribe(
      data => {
        this.file = data;
        this.model.content = data.Name;
      }
    );
  }
}
