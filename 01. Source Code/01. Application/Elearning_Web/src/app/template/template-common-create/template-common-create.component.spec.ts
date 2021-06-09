import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateCommonCreateComponent } from './template-common-create.component';

describe('TemplateCommonCreateComponent', () => {
  let component: TemplateCommonCreateComponent;
  let fixture: ComponentFixture<TemplateCommonCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TemplateCommonCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TemplateCommonCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
