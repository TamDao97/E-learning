import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeSettingManageComponent } from './home-setting-manage.component';

describe('HomeSettingManageComponent', () => {
  let component: HomeSettingManageComponent;
  let fixture: ComponentFixture<HomeSettingManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomeSettingManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeSettingManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
