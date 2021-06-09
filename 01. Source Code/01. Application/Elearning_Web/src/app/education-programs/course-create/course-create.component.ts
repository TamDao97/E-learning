import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, Constants, DateUtils, FileProcess, Configuration } from 'src/app/shared';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ImageService } from 'src/app/shared/services/image.service';
import { UploadService } from 'src/app/shared/services/upload.service';
import { CourseService } from '../services/course.service';
import { ComboboxService } from '../../shared/services/combobox.service';
import { CourseStatusComponent } from '../course/course-status/course-status.component';

declare var tinymce: any;

@Component({
  selector: 'app-course-create',
  templateUrl: './course-create.component.html',
  styleUrls: ['./course-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CourseCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public fileProcess: FileProcess,
    private messageService: MessageService,
    private serviceImg: ImageService,
    public constant: Constants,
    public config: Configuration,
    public courseService: CourseService,
    private uploadFileservice: UploadService,
    public dateUtils: DateUtils,
    public comboboxService: ComboboxService,
    private modalService: NgbModal,
  ) { }

  statusTinymce: boolean = false;
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
      this.serviceImg.uploadFile(blobInfo.blob(), 'E_Learning/Course').subscribe(
        result => {
          success(this.config.ServerApi + result.data.fileUrl);
        },
        error => {
          return;
        }
      );
    }
  };
  listId = [];
  listHistory: any[] = [];
  modelDelteFile: any = {
    avatar: '',
  }
  id: string;
  isAction: boolean = false;
  listProgram: any[] = [];
  startDate: any = null;
  finishDate: any = null;
  listStatus = this.constant.StatusCourse;
  filedata: any;
  fileImage: any;
  modalInfo = {
    Title: 'Thêm khóa học',
  };
  model: any = {
    id: '',
    name: '',
    status: false,
    description: '',
    content: '',
    imagePath: '',
    startDate: null,
    finishDate: null,
    programId: null,
    employeeCourses: [],
    learnerCourses: [],
    lessonCourses: [],
    isDelete: false,
    displayIndex: null,
    certificateTemplateId: null
  };

  modelLesson = {
    Listid: [],
    isDelete: false,
  };

  statusModel: any = {
    status: null,
    content: ''
  }

  listLesson: any[] = [];
  listMentorId: any[] = [];
  listLeanerId: any[] = [];
  listOrder: any[] = [];
  listMentor = [];
  listLeaner = [];
  listTemplate: any = [];
  course: any;

  @ViewChild('scrollHeaderHistory') scrollHeaderHistory: ElementRef;

  ngOnInit(): void {
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderHistory.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.listId = [];
    this.getListOrder();
    this.getAllProgram();
    this.getFileTemplates();
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật khóa học';
      this.getInfo();
      this.getMentorById();
      this.getLearnerById();
      this.approvalHistiry();
    } else {
      this.statusTinymce = true;
      this.modalInfo.Title = 'Thêm mới khóa học';
    }
  }

  onFileChange($event: any) {
    this.fileProcess.readDataFileIOnUploadSize($event).subscribe(
      data => {
        this.fileImage = data;
      }
    );
  }

  getAllProgram() {
    this.comboboxService.getProgram().subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listProgram = data.data
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

  getInfo() {
    this.courseService.getCourseInfo(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          if (this.model.startDate != null) {
            this.startDate = this.dateUtils.convertObjectToDateView(this.model.startDate);
          }
          if (this.model.finishDate != null) {
            this.finishDate = this.dateUtils.convertObjectToDateView(this.model.finishDate);
          }
          if (this.model.approvalStatus === 2) {
            this.discoveryConfig.readonly = 1;
          }
          this.statusTinymce = true;
          if (data.data.imagePath) {
            this.filedata = this.config.ServerApi + data.data.imagePath;
          }
          this.course = { courseId: this.model.id, courseName: this.model.name };
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

  getListOrder() {
    this.courseService.getListOrder().subscribe((data: any) => {
      this.listOrder = data.data;
      if (this.id == null || this.id == '') {
        this.model.displayIndex = data.data[data.data.length - 1].order;
      } else {
        this.listOrder.splice(this.listOrder.length - 1, 1);
      }

    });
  }

  getMentorById() {
    this.courseService.searchMentor(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listMentor = data.data.dataResults;

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

  getLearnerById() {
    this.courseService.searchLearner(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listLeaner = data.data.dataResults;

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

  deleteFileError() {
    this.modelDelteFile.avatar = this.model.avatar;
    this.uploadFileservice.deleteFile(this.modelDelteFile).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
        }
        else {
          this.messageService.showMessage(data.message);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateCourse() {
    this.courseService.updateCourse(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

          this.messageService.showSuccess('Cập nhập khóa học thành công!');
          this.closeModal(true);
        }
        else {
          this.deleteFileError();
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.deleteFileError();
        this.messageService.showError(error);
      }
    );
  }

  createCourse(isContinue) {
    this.courseService.createCourse(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thêm mới khóa học thành công!');
          if (isContinue) {
            this.isAction = true;
            this.clear();
          } else {
            this.closeModal(true);
          }
        } else {
          this.deleteFileError();
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.deleteFileError();
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.listId = [];
    this.listLesson = [];
    this.listMentorId = [];
    this.listLeanerId = [];
    this.startDate = null;
    this.finishDate = null;
    this.fileImage = null;
    this.modelLesson = {
      Listid: [],
      isDelete: false,
    };
    this.listMentor = [];
    this.listLeaner = [];
    this.model = {
      id: '',
      name: '',
      status: false,
      description: '',
      content: '',
      imagePath: '',
      startDate: null,
      finishDate: null,
      programId: null,
      employeeCourses: [],
      learnerCourses: [],
      lessonCourses: [],
      isDelete: false,
      displayIndex: null,
      certificateTemplateId: null
    };
    this.getListOrder();
  }



  save(isContinue) {
    this.listLesson = [];
    this.listMentorId = [];
    this.listLeanerId = []
    if (this.finishDate == '' || this.finishDate == null) {
      this.model.finishDate = null;
    }
    else {
      this.model.finishDate = this.dateUtils.convertObjectToDate(this.finishDate);
    }
    if (this.startDate == '' || this.startDate == null) {
      this.model.startDate = null;
    }
    else {
      this.model.startDate = this.dateUtils.convertObjectToDate(this.startDate);
    }
    this.modelLesson;
    var i = 1;
    this.modelLesson.Listid.forEach(data => {
      data.displayIndex = i++;
    });

    this.modelLesson.Listid.forEach((element: any) => {
      this.listLesson.push({ id: element.id, lessonId: element.lessonId, courseId: "", displayIndex: element.displayIndex, name: element.name, imagePath: element.imagePath, description: element.description });
    });
    this.model.lessonCourses = this.listLesson;
    this.model.isDelete = this.modelLesson.isDelete;

    this.listMentor.forEach((element: any) => {
      this.listMentorId.push(element.id);
    });
    this.model.employeeCourses = this.listMentorId;

    this.listLeaner.forEach((element: any) => {
      this.listLeanerId.push(element.id);
    });
    this.model.learnerCourses = this.listLeanerId;

    if (this.fileImage == null) {
      if (this.id) {
        this.updateCourse();
      } else {
        this.createCourse(isContinue);
      }
    }
    else {
      this.serviceImg.uploadFile(this.fileImage.File, 'E_Learning/Course').subscribe(
        data => {
          this.model.imagePath = data.data.fileUrl;
          if (this.id) {
            this.updateCourse();
          } else {
            this.createCourse(isContinue);
          }
        },
        error => {
          this.messageService.showError(error);
        });
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  getFileTemplates() {
    this.courseService.getFileTemplates().subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listTemplate = data.data;
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
    this.listLesson = [];
    this.listMentorId = [];
    this.listLeanerId = []
    if (this.finishDate == '' || this.finishDate == null) {
      this.model.finishDate = null;
    }
    else {
      this.model.finishDate = this.dateUtils.convertObjectToDate(this.finishDate);
    }
    if (this.startDate == '' || this.startDate == null) {
      this.model.startDate = null;
    }
    else {
      this.model.startDate = this.dateUtils.convertObjectToDate(this.startDate);
    }
    this.modelLesson;
    var i = 1;
    this.modelLesson.Listid.forEach(data => {
      data.displayIndex = i++;
    });

    this.modelLesson.Listid.forEach((element: any) => {
      this.listLesson.push({ id: element.id, lessonId: element.lessonId, courseId: "", displayIndex: element.displayIndex, name: element.name, imagePath: element.imagePath, description: element.description });
    });
    this.model.lessonCourses = this.listLesson;
    this.model.isDelete = this.modelLesson.isDelete;

    this.listMentor.forEach((element: any) => {
      this.listMentorId.push(element.id);
    });
    this.model.employeeCourses = this.listMentorId;

    this.listLeaner.forEach((element: any) => {
      this.listLeanerId.push(element.id);
    });
    this.model.learnerCourses = this.listLeanerId;

    if (this.fileImage == null) {
      if (this.id) {
        this.updateRequestCourse();
      }
    }
    else {
      this.serviceImg.uploadFile(this.fileImage.File, 'E_Learning/Course').subscribe(
        data => {
          this.model.imagePath = data.data.fileUrl;
          if (this.id) {
            this.updateRequestCourse();
          }
        },
        error => {
          this.messageService.showError(error);
        });
    }
  }

  updateRequestCourse() {
    this.courseService.updateCourse(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Cập nhập khóa học thành công!');
          this.courseService.requestCourse(this.id, this.statusModel).subscribe(
            data => {
              if (data.statusCode == this.constant.StatusCode.Success) {
                this.messageService.showSuccess('Yêu cầu duyệt khóa học thành công!');
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
          this.deleteFileError();
          this.messageService.showListMessage(data.message);
        }
      },
      error => {
        this.deleteFileError();
        this.messageService.showError(error);
      }
    );
  }

  requestCourse() {
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

  approvalCourse() {
    this.messageService.showConfirm("Bạn có chắc muốn duyệt khóa học này không?").then(
      data => {
        this.statusModel.status = 2;
        this.courseService.approvalCourse(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Duyệt khóa học thành công!');
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

  notApprovalCourse() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.statusModel.status = this.constant.approval_status.Course_Approval_NotBrowse;
        if (result != true) {
          this.statusModel.content = result;
        }
        this.courseService.approvalCourse(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Không duyệt khóa học thành công!');
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

  canceCourse() {
    let activeModal = this.modalService.open(CourseStatusComponent, { container: 'body', windowClass: 'course-status-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.statusModel.status = this.constant.approval_status.Course_Approval_NotApproved;
        if (result != true) {
          this.statusModel.content = result;
        }
        this.courseService.approvalCourse(this.id, this.statusModel).subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.messageService.showSuccess('Hủy duyệt khóa học thành công!');
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

  approvalHistiry() {
    this.courseService.approvalHistiry(this.id).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listHistory = data.data;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
}
