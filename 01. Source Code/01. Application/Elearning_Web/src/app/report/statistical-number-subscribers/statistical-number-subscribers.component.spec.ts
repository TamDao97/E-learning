import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatisticalNumberSubscribersComponent } from './statistical-number-subscribers.component';

describe('StatisticalNumberSubscribersComponent', () => {
  let component: StatisticalNumberSubscribersComponent;
  let fixture: ComponentFixture<StatisticalNumberSubscribersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatisticalNumberSubscribersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StatisticalNumberSubscribersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
