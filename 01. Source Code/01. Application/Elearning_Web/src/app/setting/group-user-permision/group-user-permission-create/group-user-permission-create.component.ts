import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants } from 'src/app/shared';
import { ComboboxService } from 'src/app/shared/services/combobox.service';
import { GroupUserService } from '../../services/group-user.service';

@Component({
  selector: 'app-group-user-permission-create',
  templateUrl: './group-user-permission-create.component.html',
  styleUrls: ['./group-user-permission-create.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class GroupUserPermissionCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: GroupUserService,
    private combobox: ComboboxService,
    public constant: Constants
  ) { }

  modalInfo = {
    Title: 'Thêm mới nhóm quyền',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  checkeds = false;

  model: any = {

    id: '',
    name: '',
    status: true,
    description: '',
    listPermission: []
  }

  ngOnInit(): void {
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa nhóm quyền';
      this.modalInfo.SaveText = 'Lưu';
    }
    else {
      this.modalInfo.Title = "Thêm mới nhóm quyền";
    }
    this.getGroupUserInfo();
  }

  getGroupUserInfo() {
    this.service.getGroupUserInfo(this.Id).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.model = data.data;
      }
      else {
        this.messageService.showError(data.message);
      }
    }, error => {
    });
  }

  updateGroupUser() {
    this.service.updateGroupUser(this.Id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật nhóm quyền thành công!');
        }
        else {
          this.messageService.showError(data.message);
        }
      },
      error => {
      }
    );
  }

  save(isContinue: boolean) {
      this.updateGroupUser();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  selectAllFunction() {
    if (this.checkeds) {
      this.model.listPermission.forEach(element => {
        element.checked = true;
      });
    } else {
      this.model.listPermission.forEach(element => {
        element.checked = false;
      });
    }
  }

  checkParent(groupFunctionId: any, checked: any, index: any, rowindex: any) {
    if (groupFunctionId == "") {
      if (checked) {
        for (let i = index + 1; i < this.model.listPermission.length; i++) {
          if (this.model.listPermission[i].index == "") {
            this.model.listPermission[i].checked = true;
          } else break;
        }
      } else {
        for (let i = index + 1; i < this.model.listPermission.length; i++) {
          if (this.model.listPermission[i].index == "") {
            this.model.listPermission[i].checked = false;
          } else break;
        }
      }
    } else {
      var group = this.model.listPermission.filter(t => t.groupFunctionId == rowindex);
      let grouped = group.filter(t => t.checked == false);
      let check = this.model.listPermission.filter(t => t.index == rowindex);
      if (checked) {
        if (grouped.length == 0) {
          //this.model.listPermission.index[rowindex].checked = true;
          this.model.listPermission.forEach(element => {
            if(element.index == check[0].index){
              element.checked = true;
            }
          });
        }
      }
      else {
        //this.model.listPermission.index[rowindex].checked = false;
        this.model.listPermission.forEach(element => {
          if(element.index == check[0].index){
            element.checked = false;
          }
        });
      }
    }
  }

}
