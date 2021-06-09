import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeServiceCreateComponent } from './home-service-create.component';

describe('HomeServiceCreateComponent', () => {
  let component: HomeServiceCreateComponent;
  let fixture: ComponentFixture<HomeServiceCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomeServiceCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeServiceCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
