import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoryFontendComponent } from './history-fontend.component';

describe('HistoryFontendComponent', () => {
  let component: HistoryFontendComponent;
  let fixture: ComponentFixture<HistoryFontendComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HistoryFontendComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HistoryFontendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
