import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentFontEndComponent } from './comment-font-end.component';

describe('CommentFontEndComponent', () => {
  let component: CommentFontEndComponent;
  let fixture: ComponentFixture<CommentFontEndComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommentFontEndComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentFontEndComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
