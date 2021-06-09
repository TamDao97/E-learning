import { Component, OnInit, VERSION, ViewEncapsulation } from '@angular/core';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
import { Configuration } from '../../share/config/configuration';
import { HomeSliderService } from '../service/home-slider.service';

@Component({
  selector: 'app-home-silder',
  templateUrl: './home-silder.component.html',
  styleUrls: ['./home-silder.component.scss'],
  providers: [NgbCarouselConfig],
  encapsulation: ViewEncapsulation.None
})
export class HomeSilderComponent implements OnInit {

  name = 'Angular ' + VERSION.major;
  constructor(
    public config: NgbCarouselConfig,
    private sildeBarService: HomeSliderService,
    public confg: Configuration,
  ) {
    config.interval = 2000;
    config.keyboard = true;
    config.pauseOnHover = true;
  }

  model: any = {
    imagePath: '',
  }

  imagePath: string;

  listData = [];
  listImage = [];

  ngOnInit(): void {
    this.searchSildeBar();
  }


  searchSildeBar() {
    this.sildeBarService.searchSildeBar().subscribe(
      (data: any) => {
        this.listData = data.data.dataResults;
        if (this.listData.length > 0) {
          this.imagePath = this.listData[0].imagePath;
          this.model = this.listData[0];

        }
        this.listData.forEach(a => {
          if (this.model.id != a.id) {
            this.listImage.push(a);
          }
        });
        // this.listImage = this.listData.splice(0, 1);
        // this.listData = this.listData.shift();
      })
  }
}
