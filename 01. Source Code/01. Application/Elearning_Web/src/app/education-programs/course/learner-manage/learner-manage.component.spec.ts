import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LearnerManageComponent } from './learner-manage.component';

describe('LearnerManageComponent', () => {
  let component: LearnerManageComponent;
  let fixture: ComponentFixture<LearnerManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LearnerManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LearnerManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
