import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeLinkCreateComponent } from './home-link-create.component';

describe('HomeLinkCreateComponent', () => {
  let component: HomeLinkCreateComponent;
  let fixture: ComponentFixture<HomeLinkCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomeLinkCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeLinkCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
