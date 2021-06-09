import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LessonFrameCreateComponent } from './lesson-frame-create.component';

describe('LessonFrameCreateComponent', () => {
  let component: LessonFrameCreateComponent;
  let fixture: ComponentFixture<LessonFrameCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LessonFrameCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LessonFrameCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
