import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class DataService {
  public userLogin = JSON.parse(localStorage.getItem('learner'));
  private learnerId = new BehaviorSubject("");
  public currentLearnerId = this.learnerId.asObservable();
  private dataLogin = new BehaviorSubject("");
  public dataUserLogin = this.learnerId.asObservable();
  private lessonId = new BehaviorSubject("");
  public currentLessonId = this.lessonId.asObservable();
  constructor() { }

  changeId(id: string) {
    this.learnerId.next(id);
  }

  changeLessonIdId(id: string) {
    this.lessonId.next(id);
  }

  changeUserLogin(data: any){
    this.dataLogin.next(data);
  }
}
