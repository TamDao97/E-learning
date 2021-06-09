import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService, Configuration, DateUtils } from 'src/app/shared';
import { SystemParamService } from '../../services/system-param.service';
import { SystemParamCreateComponent } from '../system-param-create/system-param-create.component';

@Component({
  selector: 'app-system-param-manage',
  templateUrl: './system-param-manage.component.html',
  styleUrls: ['./system-param-manage.component.scss']
})
export class SystemParamManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private modalService: NgbModal,
    private messageService: MessageService,
    private systemParamService: SystemParamService,
    public config: Configuration,
    private dateUtils: DateUtils
  ) { }

  listData: any[] = []
  ngOnInit(): void {
    this.appSetting.PageTitle = "Cấu hình thông số";
    this.getListSystemParam();

  }

  getListSystemParam() {
    this.systemParamService.getListSystemParam().subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listData = data.data;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.listData.forEach(item => {
      if (item.controlType == 5 && item.paramValue) {
        item.paramValue = this.dateUtils.convertObjectToDate(item.paramValue);
      }
    });
    this.systemParamService.updateSystemParam(this.listData).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess("Cập nhật cấu hình thành công!");
        }
        else {
          this.messageService.showMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate() {
    let activeModal = this.modalService.open(SystemParamCreateComponent, { container: 'body', windowClass: 'system-param-create-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.getListSystemParam();
      }
    });
  }

}
