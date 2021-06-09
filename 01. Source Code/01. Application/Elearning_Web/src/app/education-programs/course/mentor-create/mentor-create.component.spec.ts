import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MentorCreateComponent } from './mentor-create.component';

describe('MentorCreateComponent', () => {
  let component: MentorCreateComponent;
  let fixture: ComponentFixture<MentorCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MentorCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MentorCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
