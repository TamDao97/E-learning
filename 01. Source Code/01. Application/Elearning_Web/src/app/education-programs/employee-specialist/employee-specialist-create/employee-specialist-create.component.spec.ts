import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeSpecialistCreateComponent } from './employee-specialist-create.component';

describe('EmployeeSpecialistCreateComponent', () => {
  let component: EmployeeSpecialistCreateComponent;
  let fixture: ComponentFixture<EmployeeSpecialistCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeeSpecialistCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeSpecialistCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
