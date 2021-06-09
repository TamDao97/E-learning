import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DetailsResultComponent } from '../details-result/details-result.component';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';

@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestResultComponent implements OnInit {

  constructor(
    public constant: Constants,
    public config: Configuration,
    public appSetting: AppSetting,
    private activeModal: NgbActiveModal,
    private modalService: NgbModal,
  ) { }

  data = [];
  startIndex = 1;
  ngOnInit(): void {

  }

  closeModal(){
    this.activeModal.close(true);
  }
  
  detailResult(testId) {
    let activeModal = this.modalService.open(DetailsResultComponent, { container: 'body', windowClass: 'detail-result-model', backdrop: 'static' })
    activeModal.componentInstance.testId = testId;
    activeModal.result.then((result) => {
    }, (reason) => {
    });
  }
}
