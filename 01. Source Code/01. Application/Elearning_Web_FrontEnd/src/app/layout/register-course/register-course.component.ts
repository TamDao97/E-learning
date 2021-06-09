import { Component, OnInit } from '@angular/core';
import { ProgramService } from '../service/program.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/share/common/Constants';
import { MessageService } from 'src/app/share/service/message.service';
@Component({
  selector: 'app-register-course',
  templateUrl: './register-course.component.html',
  styleUrls: ['./register-course.component.scss']
})
export class RegisterCourseComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public programService: ProgramService,
    public constant: Constants,
    private messageService: MessageService,
  ) { }

  model: any={
    courseId:'',
    learnerId:'',
  };
  id: string;
  learnerId = localStorage.getItem('user');
  ngOnInit(): void {
    this.model={
      courseId:this.id,
      learnerId:this.learnerId,
    }
  }

  Creat() {
    this.programService.createLearnerCourse(this.model).subscribe(
      (data: any) => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            this.messageService.showSuccess('Đăng ký khóa học thành công!');
            this.closeModal();
          }
          else {
            this.messageService.showListMessage(data.message);
            this.closeModal();
          }
        },
        error => {
          this.messageService.showError(error);
        }
    );
  }

  closeModal() {
    this.activeModal.close();
  }
}
