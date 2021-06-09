import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SildeBarCreateComponent } from './silde-bar-create.component';

describe('SildeBarCreateComponent', () => {
  let component: SildeBarCreateComponent;
  let fixture: ComponentFixture<SildeBarCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SildeBarCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SildeBarCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
