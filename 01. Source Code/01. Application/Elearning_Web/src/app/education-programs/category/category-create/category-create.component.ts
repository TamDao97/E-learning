import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { ComboboxService } from 'src/app/shared/services/combobox.service';
import { CategoryService } from '../../services/category.service';

@Component({
  selector: 'app-category-create',
  templateUrl: './category-create.component.html',
  styleUrls: ['./category-create.component.scss']
})
export class CategoryCreateComponent implements OnInit {

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private categoryService: CategoryService,
    private comboboxService: ComboboxService,
  ) { }

  discoveryConfig = {
    plugins: ['image code', 'visualblocks', 'preview', 'table', 'preview', 'directionality', 'link', 'media', 'codesample', 'table', 'charmap', 'hr', 'pagebreak', 'nonbreaking', 'anchor', 'toc', 'insertdatetime', 'advlist', 'lists', 'textcolor', 'wordcount', 'imagetools', 'contextmenu', 'textpattern'],
    language: 'vi_VN',
    // file_picker_types: 'file image media',
    automatic_uploads: true,
    toolbar: 'undo redo | fontselect | fontsizeselect | bold italic backcolor |alignleft aligncenter alignright alignjustify alignnone | numlist | table | link image| link | outdent indent',
    convert_urls: false,
    height: 300,
    // file_browser_callback: TinymceUserConfig.FilemanUserConfig,
    auto_focus: false,
    //setup: TinymceUserConfig.setup,
    // content_css: '/assets/css/custom_editor.css',
  };

  id: string;
  parentId: string;
  isAction: boolean = false;
  listCategory: any[] = [];
  modalInfo = {
    Title: 'Thêm mới danh mục',
  };

  model: any = {
    id: '',
    parentCategoryId: null,
    name: '',
  }

  ngOnInit(): void {
    this.getCategory();
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật danh mục';
      this.getbyid();
    } else {
      this.modalInfo.Title = 'Thêm mới danh mục';
      this.model.parentCategoryId = this.parentId;
    }
  }

  getCategory() {
    this.comboboxService.getCategoryParent().subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listCategory = data.data;
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
    this.categoryService.getCategoryInfo(this.id).subscribe(
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
    this.categoryService.createCategory(this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Thêm mới danh mục thành công!');
          if (isContinue) {
            this.isAction = true;
            this.clear();
            this.getCategory();
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
    this.categoryService.updateCategory(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {

          this.messageService.showSuccess('Cập nhập danh mục thành công!');
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
      parentCategoryId: null,
      name: '',
    }
  }

}
