import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemParamManageComponent } from './system-param-manage.component';

describe('SystemParamManageComponent', () => {
  let component: SystemParamManageComponent;
  let fixture: ComponentFixture<SystemParamManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SystemParamManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SystemParamManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
