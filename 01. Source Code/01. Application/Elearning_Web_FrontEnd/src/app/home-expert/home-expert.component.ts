import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { from } from 'rxjs';
import {Configuration } from 'src/app/share/config/configuration';
import{ExpertService} from '../layout/service/expert.service';
@Component({
  selector: 'app-home-expert',
  templateUrl: './home-expert.component.html',
  styleUrls: ['./home-expert.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HomeExpertComponent implements OnInit {

  constructor(
    public expertService:ExpertService,
    public config:Configuration
  ) { }

  listData:any[]=[];
  ngOnInit(): void {
    this.GetExpert();
  }
  
  GetExpert(){
    this.expertService.searchExpert().subscribe(
      (data: any) => {
       
          this.listData = data.data;
      }
    );
  }

}
