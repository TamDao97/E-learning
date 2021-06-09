import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, Constants, MessageService } from 'src/app/shared';
import { ChooseEmployeeComponent } from '../choose-employee/choose-employee.component';
import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService } from 'src/app/shared/services/combobox.service';
import { UploadService } from 'src/app/shared/services/upload.service';

@Component({
  selector: 'app-employee-specialist-create',
  templateUrl: './employee-specialist-create.component.html',
  styleUrls: ['./employee-specialist-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EmployeeSpecialistCreateComponent implements OnInit {

  constructor(
    public constant: Constants,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private routeA: ActivatedRoute,
    public config: Configuration,
  ) { }

  listData = [];

  isAction: boolean = false;
  id: string;
  modalInfo: any = {
    Title: 'Cấu hình chuyên gia'
  }

  model: any = {
    Title: 'Chuyên gia'
  }

  ngOnInit(): void {
    this.id = this.routeA.snapshot.paramMap.get('id');
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  selectLesson() {
    let activeModal = this.modalService.open(ChooseEmployeeComponent, { container: 'body', windowClass: 'choose-employee', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];

    this.listData.forEach(element => {
      ListIdSelect.push(element.id);
    });

    activeModal.componentInstance.listIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listData.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDelete(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá bài giảng này này không?").then(
      data => {
        this.removeLesson(row);
      }
    );
  }

  removeLesson(row) {
    var index = this.listData.indexOf(row);
    if (index > -1) {
      this.listData.splice(index, 1);
    }
  }


  save(isContinue: boolean) {
    if (this.id) {
      this.update();
    } else {
      this.create(isContinue);
    }
  }

  update() {

  }

  create(isContinue) {

  }


}
