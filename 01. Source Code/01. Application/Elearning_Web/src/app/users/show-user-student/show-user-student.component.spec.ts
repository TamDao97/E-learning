import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowUserStudentComponent } from './show-user-student.component';

describe('ShowUserStudentComponent', () => {
  let component: ShowUserStudentComponent;
  let fixture: ComponentFixture<ShowUserStudentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowUserStudentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowUserStudentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
