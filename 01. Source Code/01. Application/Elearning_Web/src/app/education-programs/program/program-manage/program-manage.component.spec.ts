import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramManageComponent } from './program-manage.component';

describe('ProgramManageComponent', () => {
  let component: ProgramManageComponent;
  let fixture: ComponentFixture<ProgramManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProgramManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
