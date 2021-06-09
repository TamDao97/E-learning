import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowErroUserComponent } from './show-erro-user.component';

describe('ShowErroUserComponent', () => {
  let component: ShowErroUserComponent;
  let fixture: ComponentFixture<ShowErroUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowErroUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowErroUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
