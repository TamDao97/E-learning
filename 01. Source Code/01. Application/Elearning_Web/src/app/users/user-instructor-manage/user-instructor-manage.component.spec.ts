import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserInstructorManageComponent } from './user-instructor-manage.component';

describe('UserInstructorManageComponent', () => {
  let component: UserInstructorManageComponent;
  let fixture: ComponentFixture<UserInstructorManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserInstructorManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserInstructorManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
