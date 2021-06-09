import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/share/common/Constants';
import { DateUtils } from 'src/app/share/common/date-utils';
import { FileProcess } from 'src/app/share/common/file-process';
import { Configuration } from 'src/app/share/config/configuration';
import { MessageService } from 'src/app/share/service/message.service';
import { UploadService } from 'src/app/share/service/upload.service';
import { TopbarService } from '../service/topbar.service';

@Component({
  selector: 'app-update-user',
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UpdateUserComponent implements OnInit {

  constructor(
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private topbarService: TopbarService,
    private messageService: MessageService,
    private dateUntil: DateUtils,
    public fileProcess: FileProcess,
    private uploadFileservice: UploadService,
    public config: Configuration

  ) { }

  email: number;
  filedata: string;
  listProvince = [];
  listDistrict = [];
  listWard = [];
  listNation = [];
  dateOfbirday = null;
  minDateNotificationV = this.dateUntil.getNgbDateStructStartYear(1950);

  maxDateNotufucationV = this.dateUntil.getNgbDateStructMaxYear();
  id: string;
  isAction: boolean = false;
  modalInfo = {
    Title: 'Cập nhật thông tin tài khoản',
  };
  model: any = {
    name: '',
    gender: true,
    dateOfBirthday: '',
    phoneNumber: '',
    email: '',
    provinceId: null,
    districtId: null,
    wardId: null,
    nationId: null,
    address: ''
  }

  ngOnInit(): void {
    this.getUserById();
    this.getListProvince();
    this.getListNation();
  }

  save() {
    var validEmail = true;
    var regex = this.constant.validEmailRegEx;
    if (this.model.email != '' && this.model.email != null) {
      if (!regex.test(this.model.email)) {
        validEmail = false;
      }
    }

    if (validEmail) {
      // this.update();
      if (this.fileProcess.FileDataBase == null) {
        this.update();

      }
      else {
        this.uploadFileservice.uploadFile(this.fileProcess.FileDataBase, 'UserFontend').subscribe(
          data => {
            if (data.statusCode == this.constant.StatusCode.Success) {
              this.model.avatar = data.data.fileUrl;
              this.update();
            }
            else {
              this.messageService.showMessage(data.message);
            }
          },
          error => {
            this.messageService.showError(error);
          });
      }
    }
    else {
      this.messageService.showMessage('Email không hợp lệ!');
    }
  }

  getUserById() {
    this.topbarService.getUserByid(this.id).subscribe(data => {
      this.model = data.data;

      if (this.model.dateOfBirthday != null) {
        this.dateOfbirday = this.dateUntil.convertDateToObject(this.model.dateOfBirthday);
      }
      if(data.data.avatar != null){
        this.filedata = this.config.ServerApi + data.data.avatar;
      } 
      else{
        this.filedata = '/assets/img/noavatar.png';
      }

      if (this.model.provinceId != null) {
        this.getListDistrictByProvinceId(this.model.provinceId);
        if (this.model.districtId != null) {
          this.getListWardByDistrictId(this.model.districtId);
        }
      }

    })
  }

  getListProvince() {
    this.topbarService.getListProvince().subscribe((data: any) => {
      this.listProvince = data.data.dataResults;
    });
  }

  getListNation() {
    this.topbarService.getListNation().subscribe((data: any) => {
      this.listNation = data.data.dataResults;
    });
  }

  getListDistrictByProvinceId(provinceId) {
    this.topbarService.getListDistrictByProvinceId(provinceId).subscribe((data: any) => {
      this.listDistrict = data.data.dataResults;
    });
  }

  getListWardByDistrictId(districtId) {
    this.topbarService.getListWardByDistrictId(districtId).subscribe((data: any) => {
      this.listWard = data.data.dataResults;
    });
  }

  update() {
    let dateNow = new Date();
    if (this.dateOfbirday != null) {
      let date = new Date(this.dateUntil.convertObjectToDate(this.dateOfbirday));
      if (date > dateNow) {
        this.messageService.showMessage("Ngày sinh không được lớn hơn ngày hiện tại!");
        return;
      }
      this.model.dateOfBirthday = this.dateUntil.convertObjectToDate(this.dateOfbirday);
    }

    this.topbarService.updateUser(this.id, this.model).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.messageService.showSuccess('Cập nhập tài khoản thành công!');
          localStorage.removeItem('learner');
          localStorage.setItem('learner', JSON.stringify(this.model));
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

  onFileChange($event) {
    this.fileProcess.onAFileChange($event);
  }


}
