import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScreenWaitComponent } from './screen-wait.component';

describe('ScreenWaitComponent', () => {
  let component: ScreenWaitComponent;
  let fixture: ComponentFixture<ScreenWaitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScreenWaitComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScreenWaitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
