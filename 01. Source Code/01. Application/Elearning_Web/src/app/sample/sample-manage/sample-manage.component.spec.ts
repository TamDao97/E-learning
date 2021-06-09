import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SampleManageComponent } from './sample-manage.component';

describe('SampleManageComponent', () => {
  let component: SampleManageComponent;
  let fixture: ComponentFixture<SampleManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SampleManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SampleManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
