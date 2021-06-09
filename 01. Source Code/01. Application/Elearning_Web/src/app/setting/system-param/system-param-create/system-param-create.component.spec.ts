import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemParamCreateComponent } from './system-param-create.component';

describe('SystemParamCreateComponent', () => {
  let component: SystemParamCreateComponent;
  let fixture: ComponentFixture<SystemParamCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SystemParamCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SystemParamCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
