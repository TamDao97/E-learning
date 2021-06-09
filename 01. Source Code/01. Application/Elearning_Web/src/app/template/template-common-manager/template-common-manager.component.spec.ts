import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateCommonManagerComponent } from './template-common-manager.component';

describe('TemplateCommonManagerComponent', () => {
  let component: TemplateCommonManagerComponent;
  let fixture: ComponentFixture<TemplateCommonManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TemplateCommonManagerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TemplateCommonManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
