import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SildeBarManageComponent } from './silde-bar-manage.component';

describe('SildeBarManageComponent', () => {
  let component: SildeBarManageComponent;
  let fixture: ComponentFixture<SildeBarManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SildeBarManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SildeBarManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
