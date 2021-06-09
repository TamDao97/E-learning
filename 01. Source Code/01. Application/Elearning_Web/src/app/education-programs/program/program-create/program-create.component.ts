import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, FileProcess, Constants, DateUtils, Configuration } from 'src/app/shared';
import { ImageService } from 'src/app/shared/services/image.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { EducationProgramService } from '../../services/education-program.service';
import { UploadService } from 'src/app/shared/services/upload.service';

declare var tinymce: any;

@Component({
  selector: 'app-program-create',
  templateUrl: './program-create.component.html',
  styleUrls: ['./program-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProgramCreateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constant: Constants,
    public fileProcessImage: FileProcess,
    public fileProcessDataSheet: FileProcess,
    public dateUtils: DateUtils,
    private activeModal: NgbActiveModal,
    private serviceImg: ImageService,
    private config: Configuration,
    private programService: EducationProgramService,
    private uploadFileservice: UploadService,

  ) { }
  listStatus = this.constant.StatusProgram;
  filedata: any;
  fileImage: any;
  modalInfo = {
    Title: 'Thêm mới chương trình đào tạo',
  };
  modelDelteFile: any = {
    avatar: '',
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
    content_style: "body {font-size: 12pt;font-family: Arial;}",
    file_browser_callback: function RoxyFileBrowser(field_name, url, type, win) {
      //var roxyFileman = '/fileman/index.html';
      var roxyFileman = "https://nhantinsoft.vn:956/fileServer/fileman/index.html";
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
      this.serviceImg.uploadFile(blobInfo.blob(), 'E_Learning/Program').subscribe(
        result => {
          success(this.config.ServerApi + result.data.fileUrl);
        },
        error => {
          return;
        }
      );
    }
  };

  isAction: boolean = false;
  model: any = {
    id: '',
    name: '',
    code: '',
    status: false,
    description: '',
    content: '',
    imagePath: '',
  }
  id: string;

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật chương trình đào tạo';
      this.getbyid();
    } else {
      this.modalInfo.Title = 'Thêm mới chương trình đào tạo';
    }
  }

  onFileChange($event: any) {
    this.fileProcess.readDataFileIOnUploadSize($event).subscribe(
      data => {
        this.fileImage = data;
      }
    );
  }

  getbyid() {
    this.programService.getProgramInfo(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;

          if (data.data.imagePath != null && data.data.imagePath != '') {
            this.filedata = this.config.ServerApi + data.data.imagePath;
          }
          else {
            this.filedata = null;
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

  saveAndContinue() {
    this.save(true);
  }

  create(isContinue) {
    this.programService.createProgram(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thêm mới chương trình đào tạo thành công!');
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

  save(isContinue: boolean) {
    if (this.fileImage == null) {
      if (this.id) {
        this.update();
      } else {
        this.create(isContinue);
      }
    }
    else {
      this.serviceImg.uploadFile(this.fileImage.File, 'E_Learning/Program').subscribe(
        data => {
          this.model.imagePath = data.data.fileUrl;
          if (this.id) {
            this.update();
          } else {
            this.create(isContinue);
          }
        },
        error => {
          this.messageService.showError(error);
        });
    }

  }

  update() {
    this.programService.updateProgram(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

          this.messageService.showSuccess('Cập nhập chương trình đào tạo thành công!');
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

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  clear() {
    this.fileImage = null;
    this.model = {
      id: '',
      name: '',
      code: '',
      status: false,
      description: '',
      content: '',
      imagePath: '',
    }
  }
}
