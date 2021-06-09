import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserStudentManagerComponent } from './user-student-manager.component';

describe('UserStudentManagerComponent', () => {
  let component: UserStudentManagerComponent;
  let fixture: ComponentFixture<UserStudentManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserStudentManagerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserStudentManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
