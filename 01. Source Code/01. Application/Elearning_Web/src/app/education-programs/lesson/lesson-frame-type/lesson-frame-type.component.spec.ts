import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LessonFrameTypeComponent } from './lesson-frame-type.component';

describe('LessonFrameTypeComponent', () => {
  let component: LessonFrameTypeComponent;
  let fixture: ComponentFixture<LessonFrameTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LessonFrameTypeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LessonFrameTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
