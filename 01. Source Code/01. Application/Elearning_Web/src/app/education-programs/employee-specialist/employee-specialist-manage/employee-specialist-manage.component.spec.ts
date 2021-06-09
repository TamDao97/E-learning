import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeSpecialistManageComponent } from './employee-specialist-manage.component';

describe('EmployeeSpecialistManageComponent', () => {
  let component: EmployeeSpecialistManageComponent;
  let fixture: ComponentFixture<EmployeeSpecialistManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeeSpecialistManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeSpecialistManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
