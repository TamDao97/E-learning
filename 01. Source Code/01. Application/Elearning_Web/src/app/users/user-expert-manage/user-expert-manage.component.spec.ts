import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserExpertManageComponent } from './user-expert-manage.component';

describe('UserExpertManageComponent', () => {
  let component: UserExpertManageComponent;
  let fixture: ComponentFixture<UserExpertManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserExpertManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserExpertManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
