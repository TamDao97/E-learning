import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailsResultComponent } from './details-result.component';

describe('DetailsResultComponent', () => {
  let component: DetailsResultComponent;
  let fixture: ComponentFixture<DetailsResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DetailsResultComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailsResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
