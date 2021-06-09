import { Component, OnInit } from '@angular/core';

import { MessageService, AppSetting, Constants, Configuration } from 'src/app/shared';
import { HomeSettingCreateComponent } from '../home-setting-create/home-setting-create.component';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HomeSettingService } from '../../services/home-setting.service'
@Component({
  selector: 'app-home-setting-manage',
  templateUrl: './home-setting-manage.component.html',
  styleUrls: ['./home-setting-manage.component.scss']
})
export class HomeSettingManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private modalService: NgbModal,
    private messageService: MessageService,
    private homeSettingService: HomeSettingService,
    public config: Configuration,
  ) { }

  btn = {
    Name: 'Thêm mới',
  };
  model: any = {
    id:'',
    logo: '',
    address: '',
    phone: '',
    gmail: '',
    linkFacebook: '',
    linkGoogle: '',
    linkYoutube: '',
    copyright: '',
  }

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý thiết lập trang chủ";
    this.getHomeSetting();

  }

  getHomeSetting(){
    this.homeSettingService.getHomeSetting().subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          if(data.data!=null)
          {
            this.btn.Name="Thêm mới";
          }
          else{
            this.btn.Name="Chỉnh sửa";
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

  showCreateUpdate(id: string) {
    let activeModal = this.modalService.open(HomeSettingCreateComponent, { container: 'body', windowClass: 'home-setting-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.getHomeSetting();
      }
    });
  }

}
