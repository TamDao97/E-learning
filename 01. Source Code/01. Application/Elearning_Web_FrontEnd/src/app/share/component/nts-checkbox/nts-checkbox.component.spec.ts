import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NtsCheckboxComponent } from './nts-checkbox.component';

describe('NtsCheckboxComponent', () => {
  let component: NtsCheckboxComponent;
  let fixture: ComponentFixture<NtsCheckboxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NtsCheckboxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NtsCheckboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
