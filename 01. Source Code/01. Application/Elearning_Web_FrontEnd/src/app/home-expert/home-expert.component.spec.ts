import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeExpertComponent } from './home-expert.component';

describe('HomeExpertComponent', () => {
  let component: HomeExpertComponent;
  let fixture: ComponentFixture<HomeExpertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomeExpertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeExpertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
