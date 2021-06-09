import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Configuration } from '../share/config/configuration';
import { HomeServiceService } from './home-service.service';

@Component({
  selector: 'app-home-service',
  templateUrl: './home-service.component.html',
  styleUrls: ['./home-service.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HomeServiceComponent implements OnInit {

  constructor(
    private homeServiceService: HomeServiceService,
    public confg: Configuration,
  ) { }

  listData = [];
  number: number;

  ngOnInit(): void {
    this.searchHomeService();
  }

  searchHomeService() {
    this.homeServiceService.searchSildeBar().subscribe(
      (data: any) => {
        this.listData = data.data.dataResults;
        this.number = this.listData.length;
        // this.listImage = this.listData.splice(0, 1);
        // this.listData = this.listData.shift();
      })
  }

}
