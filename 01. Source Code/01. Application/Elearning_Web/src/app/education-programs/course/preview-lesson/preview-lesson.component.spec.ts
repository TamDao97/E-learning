import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviewLessonComponent } from './preview-lesson.component';

describe('PreviewLessonComponent', () => {
  let component: PreviewLessonComponent;
  let fixture: ComponentFixture<PreviewLessonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PreviewLessonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PreviewLessonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
