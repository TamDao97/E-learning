import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, Configuration, AppSetting, MessageService } from 'src/app/shared';
import { SildeBarService } from '../service/silde-bar.service';
import { SildeBarCreateComponent } from '../silde-bar-create/silde-bar-create.component';

@Component({
  selector: 'app-silde-bar-manage',
  templateUrl: './silde-bar-manage.component.html',
  styleUrls: ['./silde-bar-manage.component.scss']
})
export class SildeBarManageComponent implements OnInit {

  constructor(    
    public constant: Constants,
    public config: Configuration,
    public appSetting: AppSetting,
    private modalService: NgbModal,
    private messageService: MessageService,
    private sildeBarService: SildeBarService,
    ) { }


    startIndex = 1;
    listData: any[] = [];
  
    model: any = {
      PageNumber: 1,
      PageSize: 10,
      TotalItems: 0,
  
      Title: '',
    } 

    modelList: any = {
      ListDisplayIndex: [],
    }
  
    searchOptions: any = {
      FieldContentName: 'Title',
      Placeholder: 'Tiêu đề',
      Items: [
      ]
    };
  
    ngOnInit(): void {
      this.appSetting.PageTitle = "Quản lý silde bar";
      this.searchSildeBar();
    }

    updateIndex() {
      this.modelList.ListDisplayIndex = this.listData;
      this.sildeBarService.updateIndex(this.modelList).subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {

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

    drop(event: CdkDragDrop<string[]>) {
      this.listData = [];
      if (event.previousContainer === event.container) {
        moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
      } else {
        transferArrayItem(event.previousContainer.data,
          event.container.data,
          event.previousIndex,
          event.currentIndex);
      }
  
      this.listData = event.container.data;
      var i = 1;
      this.listData.forEach(element => {
        element.displayIndex = i++;
      })

      this.updateIndex();
    }
  
    searchSildeBar() {
      this.sildeBarService.searchSildeBar(this.model).subscribe(
        (data: any) => {
          if (data.statusCode == this.constant.StatusCode.Success) {
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
  
    showConfirmDelete(id: number) {
      this.messageService.showConfirm("Bạn có chắc muốn xoá silde bar này không?").then(
        data => {
          this.deleteSildeBar(id);
        }
      );
    }
  
    deleteSildeBar(id: number) {
      this.sildeBarService.deleteSildeBar(id).subscribe(
        data => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.messageService.showSuccess('Xóa silde bar thành công!');
            this.searchSildeBar();
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
    
        Title: '',
      }
      this.searchSildeBar();
    }
  
    showCreateUpdate(id: string) {
      let activeModal = this.modalService.open(SildeBarCreateComponent, { container: 'body', windowClass: 'silde-bar-model', backdrop: 'static' })
      activeModal.componentInstance.id = id;
      activeModal.result.then((result: any) => {
        if (result) {
          this.searchSildeBar();
        }
      });
    }

}
