import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageUnitManageComponent } from './manage-unit-manage.component';

describe('ManageUnitManageComponent', () => {
  let component: ManageUnitManageComponent;
  let fixture: ComponentFixture<ManageUnitManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageUnitManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageUnitManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
