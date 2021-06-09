import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class SearchValueService {
  private searchValue = new BehaviorSubject("");
  public currentSearchValue = this.searchValue.asObservable();
  constructor() { }
  changeValue(value: string) {
    this.searchValue.next(value);
  }
}
