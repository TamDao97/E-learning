import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LearnerCreateComponent } from './learner-create.component';

describe('LearnerCreateComponent', () => {
  let component: LearnerCreateComponent;
  let fixture: ComponentFixture<LearnerCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LearnerCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LearnerCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
