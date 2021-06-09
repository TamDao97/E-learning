import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SearchService } from '../search.service';
@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss'],
})
export class SearchComponent implements OnInit {

  constructor(
    private router: Router,
    public searchService: SearchService,
  ) {

  }
  listData:any[];
  searchValue = "";
  ngOnInit(): void {

    this.getAllHomeLink();
  }
  
  getAllHomeLink(){
    this.searchService.getAllHomeLink().subscribe(
      (data: any) => {
        this.listData = data.data;
      }
    );
  }
  search() {
      this.router.navigate(['/tim-kiem',{a:this.searchValue}]);
  }
}
