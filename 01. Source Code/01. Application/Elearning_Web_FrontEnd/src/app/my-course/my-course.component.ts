import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { from } from 'rxjs';
import { Configuration } from 'src/app/share/config/configuration';
import { MyCourseService } from './my-course.service';

@Component({
  selector: 'app-my-course',
  templateUrl: './my-course.component.html',
  styleUrls: ['./my-course.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class MyCourseComponent implements OnInit {

  constructor(
    public myCourseService: MyCourseService,
    public config: Configuration
  ) { }

  learnerId = localStorage.getItem('user');
  listData: any[] = [];
  ngOnInit(): void {
    window.scrollTo(0, 0)
    this.GetMyCourse();
  }

  GetMyCourse() {
    this.myCourseService.getMyCourse(this.learnerId).subscribe(
      (data: any) => {
        this.listData = data.data;
      }
    );
  }
}
