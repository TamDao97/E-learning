import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeServiceManageComponent } from './home-service-manage.component';

describe('HomeServiceManageComponent', () => {
  let component: HomeServiceManageComponent;
  let fixture: ComponentFixture<HomeServiceManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomeServiceManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeServiceManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
