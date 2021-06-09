import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-lesson-frame-type',
  templateUrl: './lesson-frame-type.component.html',
  styleUrls: ['./lesson-frame-type.component.scss']
})
export class LessonFrameTypeComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
  ) { }

  ngOnInit(): void {
  }

  clickType(type: number) {
    this.activeModal.close(type);
  }

  closeModal() {
    this.activeModal.close(false);
  }
}
