import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageUnitCreateComponent } from './manage-unit-create.component';

describe('ManageUnitCreateComponent', () => {
  let component: ManageUnitCreateComponent;
  let fixture: ComponentFixture<ManageUnitCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageUnitCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageUnitCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
