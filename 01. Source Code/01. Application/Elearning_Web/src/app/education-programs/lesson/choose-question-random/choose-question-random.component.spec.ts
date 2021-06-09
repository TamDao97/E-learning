import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseQuestionRandomComponent } from './choose-question-random.component';

describe('ChooseQuestionRandomComponent', () => {
  let component: ChooseQuestionRandomComponent;
  let fixture: ComponentFixture<ChooseQuestionRandomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseQuestionRandomComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseQuestionRandomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
