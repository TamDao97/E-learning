import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Constants, DateUtils, MessageService } from 'src/app/shared';
import { UserService } from '../service/user.service';

@Component({
  selector: 'app-show-user-student',
  templateUrl: './show-user-student.component.html',
  styleUrls: ['./show-user-student.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowUserStudentComponent implements OnInit {

  constructor(
    private router: Router,
    private routeA: ActivatedRoute,
    public constant: Constants,
    private userService: UserService,
    private messageService: MessageService,
    private dateUntil: DateUtils

  ) { }

  id: string;
  model: any;
  filedata: string;
  dateOfBirth: any;
  listData = [];
  ngOnInit(): void {
    this.id = this.routeA.snapshot.paramMap.get('id');
    this.getUserStudentById();
  }

  getUserStudentById(){
    this.userService.getUserStudentByid(this.id).subscribe(
      data => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.model = data.data;
          if(this.model.dateOfBirthday != null){
            this.dateOfBirth = this.dateUntil.convertDateToObject(this.model.dateOfBirthday)
          }
          this.listData = data.data.listCourse;
        }
        else {
          this.messageService.showListMessage(data.Message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  
  closeModal(isOK: boolean) {
      this.router.navigate(['/nguoi-dung/nguoi-dung']);
  }

}
