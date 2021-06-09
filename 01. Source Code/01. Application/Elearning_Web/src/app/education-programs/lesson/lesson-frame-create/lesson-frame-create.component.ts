import { Component, ElementRef, HostListener, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, Configuration, FileProcess } from 'src/app/shared';
import { ImageService } from 'src/app/shared/services/image.service';
import { LessonService } from '../../services/lesson.service';
import { ChooseQuestionRandomComponent } from '../choose-question-random/choose-question-random.component';
import { ChooseQuestionComponent } from '../choose-question/choose-question.component';
import { ShowQuestionComponent } from '../show-question/show-question.component';

declare var tinymce: any;

@Component({
  selector: 'app-lesson-frame-create',
  templateUrl: './lesson-frame-create.component.html',
  styleUrls: ['./lesson-frame-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LessonFrameCreateComponent implements OnInit {

  @ViewChild('fondovalor') fondovalor: ElementRef;
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private lessonService: LessonService,
    private modalService: NgbModal,
    public constant: Constants,
    public fileProcess: FileProcess,
    public config: Configuration,
    public fileProcessDataSheet: FileProcess,
    private serviceImg: ImageService,
    private elRef: ElementRef,
    private _sanitizer: DomSanitizer
  ) {
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

  data: any;
  type: number;
  modalInfo = {
    Title: 'Thêm mới nội dung bài giảng',
  };

  lessonFrameModel: any = {
    id: '',
    lessonId: '',
    name: '',
    content: '',
    type: '',
    estimatedTime: '0000',
    testTime: 0,
    totalRequestCorrect: 0,
    displayIndex: null,
    listQuestion: []
  }

  questionRandomModel: any = {
    NumberQuestion: 0,
    ListId: [],
    ListTopic: []
  }

  ngOnInit(): void {
    if (this.data && this.type) {
      this.modalInfo.Title = 'Cập nhật nội dung bài giảng';
      this.lessonFrameModel = this.data;
      if (this.lessonFrameModel.type == 3) {
        this.linkVideo(this.lessonFrameModel.content);
      }
    } else {
      this.lessonFrameModel.type = this.type;
    }
  }

  save() {
    if (!this.lessonFrameModel.content && this.lessonFrameModel.type == 3 && this.fondovalor) {
      this.lessonFrameModel.content = this.fondovalor.nativeElement.value;
    }
    if (this.type == 2 && this.lessonFrameModel.totalRequestCorrect > this.lessonFrameModel.listQuestion.length) {
      return this.messageService.showMessage("Số câu trả lời cần đạt không được lớn hơn số câu hỏi!");
    }
    this.activeModal.close(this.lessonFrameModel);
  }

  showConfirmDeleteLessonFrameQuestion(index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nội dung bài giảng này không?").then(
      data => {
        this.lessonFrameModel.listQuestion.splice(index, 1);
      }
    );
  }

  showChooseQuestion() {
    var listQuestId = [];
    this.lessonFrameModel.listQuestion.forEach((element: { id: any; }) => {
      listQuestId.push(element.id);
    });
    let activeModal = this.modalService.open(ChooseQuestionComponent, { container: 'body', windowClass: 'choose-question-model', backdrop: 'static' })
    activeModal.componentInstance.listQuestId = listQuestId;
    activeModal.result.then((result: any) => {
      if (result.length > 0) {
        result.forEach((element: any) => {
          this.lessonFrameModel.listQuestion.push(element);
        });
      }
    });
  }

  showChooseQuestionRandom() {
    var listQuestId = [];
    this.lessonFrameModel.listQuestion.forEach((element: { id: any; }) => {
      listQuestId.push(element.id);
    });
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

  getQuestionRandom() {
    this.lessonService.getQuestionRandom(this.questionRandomModel).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          if (data.data.length > 0) {
            data.data.forEach((element: any) => {
              this.lessonFrameModel.listQuestion.push(element);
            });
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

  showQuestion(id: string) {
    let activeModal = this.modalService.open(ShowQuestionComponent, { container: 'body', windowClass: 'show-question-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result: any) => {
      if (result) {

      }
    });
  }

  closeModal() {
    this.activeModal.close(false);
  }

  link: any;
  linkVideo(url: string) {
    if (!url) {
      return;
    }

    if (url.indexOf("www.youtube.com") !== -1) {
      const videoId = this.getVideoId(url);
      this.link = 'https://www.youtube.com/embed/' + videoId;
      this.link = this._sanitizer.bypassSecurityTrustResourceUrl(this.link);
    } else {
      this.link = this._sanitizer.bypassSecurityTrustResourceUrl(url);
    }

    const player = this.elRef.nativeElement.querySelector('video');
    if (player) {
      player.load();
    }
  }

  getVideoId(url: any) {
    const regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#&?]*).*/;
    const match = url?.match(regExp);

    return (match && match[2].length === 11)
      ? match[2]
      : null;
  }

  linkServer: string;
  RoxyFileBrowser(field_name, url, type, win) {
    //var roxyFileman = '/fileman/index.html';
    var roxyFileman = "https://nhantinsoft.vn:9508/fileServer/fileman/index.html?integration=custom&txtFieldId=txtSelectedFile";
    if (roxyFileman.indexOf("?") < 0) {
      roxyFileman += "?type=media";
    }
    else {
      roxyFileman += "&type=media";
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
  }

  openForm() {
    //var roxyFileman = '/fileman/index.html';
    var roxyFileman = "https://nhantinsoft.vn:9508/fileServer/filemanVideo/index.html?integration=custom&txtFieldId=txtSelectedFile";
    if (roxyFileman.indexOf("?") < 0) {
      roxyFileman += "?type=media";
    }
    else {
      roxyFileman += "&type=media";
    }
    window.open(roxyFileman, "txtSelectedFile", "width=850, height=650");
  }

  changeLink() {
    if (this.linkServer) {
      this.linkVideo(this.linkServer);
    }
  }

  onChange() {
    var x = this.fondovalor.nativeElement.value;
    var link = "https://nhantinsoft.vn:9508/fileServer" + x;
    console.log(link);
  }
}
