import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeSettingCreateComponent } from './home-setting-create.component';

describe('HomeSettingCreateComponent', () => {
  let component: HomeSettingCreateComponent;
  let fixture: ComponentFixture<HomeSettingCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomeSettingCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeSettingCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
