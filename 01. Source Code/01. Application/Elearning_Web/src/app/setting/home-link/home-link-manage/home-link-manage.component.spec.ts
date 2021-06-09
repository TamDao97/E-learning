import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeLinkManageComponent } from './home-link-manage.component';

describe('HomeLinkManageComponent', () => {
  let component: HomeLinkManageComponent;
  let fixture: ComponentFixture<HomeLinkManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomeLinkManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeLinkManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
