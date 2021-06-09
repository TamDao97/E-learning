import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, Configuration, AppSetting, MessageService } from 'src/app/shared';
import { CategoryService } from '../../services/category.service';
import { CategoryCreateComponent } from '../category-create/category-create.component';

@Component({
  selector: 'app-category-manage',
  templateUrl: './category-manage.component.html',
  styleUrls: ['./category-manage.component.scss']
})
export class CategoryManageComponent implements OnInit {

  constructor(
    public constant: Constants,
    public config: Configuration,
    public appSetting: AppSetting,
    private modalService: NgbModal,
    private messageService: MessageService,
    private categoryService: CategoryService,
  ) { }

  startIndex = 1;
  listData: any[] = [];

  model: any = {
    PageNumber: 1,
    PageSize: 10,
    TotalItems: 0,

    Name: '',
    ParentCategoryId: '',
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tên danh mục',
    Items: [
      {
        Name: 'Danh mục cha',
        FieldName: 'ParentCategoryId',
        Placeholder: 'Chọn',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.Category,
        DisplayName: 'name',
        ValueName: 'id'
      }
    ]
  };

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý danh mục";
    this.searchCategory();
  }

  searchCategory() {
    this.categoryService.searchCategory(this.model).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
          this.listData = data.data.dataResults;
          this.model.TotalItems = data.data.totalItems;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDelete(id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá danh mục này không?").then(
      data => {
        this.deleteCategory(id);
      }
    );
  }

  deleteCategory(id: string) {
    this.categoryService.deleteCategory(id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Xóa danh mục thành công!');
          this.searchCategory();
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
      PageNumber: 1,
      PageSize: 10,
      TotalItems: 0,

      Name: '',
      ParentCategoryId: '',
    }
    this.searchCategory();
  }

  showCreateUpdate(id: string) {
    let activeModal = this.modalService.open(CategoryCreateComponent, { container: 'body', windowClass: 'category-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result: any) => {
      if (result) {
        this.searchCategory();
      }
    });
  }

}
