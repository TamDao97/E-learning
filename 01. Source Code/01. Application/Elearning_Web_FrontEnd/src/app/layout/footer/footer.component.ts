import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { TopbarService } from '../service/topbar.service';
import { ProgramService } from '../service/program.service';
import { Configuration } from 'src/app/share/config/configuration';
@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FooterComponent implements OnInit {

  constructor(
    public topbarService: TopbarService,
    public programService: ProgramService,
    public config: Configuration
  ) { }

  model: any = {
    id: '',
    logo: '',
    address: '',
    phone: '',
    gmail: '',
    linkFacebook: '',
    linkGoogle: '',
    linkYoutube: '',
    copyright: '',
  }
  imagePath: string;
  id: string;
  modelCourse: any = {
    imagePath: '',
    id: '',
    name: '',
    startDate: '',
    listEmployeeName: [],
    numberOfComment: '',
    description: ''
  };

  ngOnInit() {

    this.getHomeSetting();
    this.getTop2Course();
  }
  getHomeSetting() {
    this.topbarService.getHomeSetting().subscribe(
      (data: any) => {
        this.model = data.data;
      }
    );
  }
  getTop2Course() {
    this.programService.getTop2Course().subscribe(
      (data: any) => {
        if (data.data != null) {
          this.modelCourse = data.data;
        }
      }
    );
  }
}
