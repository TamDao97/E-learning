import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoryManageComponent } from './history-manage.component';

describe('HistoryManageComponent', () => {
  let component: HistoryManageComponent;
  let fixture: ComponentFixture<HistoryManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HistoryManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HistoryManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
