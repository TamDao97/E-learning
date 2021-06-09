import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseTabContentComponent } from './choose-tab-content.component';

describe('ChooseTabContentComponent', () => {
  let component: ChooseTabContentComponent;
  let fixture: ComponentFixture<ChooseTabContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseTabContentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseTabContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
