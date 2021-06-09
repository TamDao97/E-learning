import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeSilderComponent } from './home-silder.component';

describe('HomeSilderComponent', () => {
  let component: HomeSilderComponent;
  let fixture: ComponentFixture<HomeSilderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomeSilderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeSilderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
