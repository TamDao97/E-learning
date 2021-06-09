import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateCetificateManagerComponent } from './template-cetificate-manager.component';

describe('TemplateComponent', () => {
  let component: TemplateCetificateManagerComponent;
  let fixture: ComponentFixture<TemplateCetificateManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TemplateCetificateManagerComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TemplateCetificateManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
