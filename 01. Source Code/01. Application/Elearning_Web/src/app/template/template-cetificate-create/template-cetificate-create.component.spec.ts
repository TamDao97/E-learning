import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateCetificateCreateComponent } from './template-cetificate-create.component';

describe('TemplateCetificateCreateComponent', () => {
  let component: TemplateCetificateCreateComponent;
  let fixture: ComponentFixture<TemplateCetificateCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TemplateCetificateCreateComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TemplateCetificateCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
