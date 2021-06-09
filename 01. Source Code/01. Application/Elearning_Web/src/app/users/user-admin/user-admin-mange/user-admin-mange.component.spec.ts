import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAdminMangeComponent } from './user-admin-mange.component';

describe('UserAdminMangeComponent', () => {
  let component: UserAdminMangeComponent;
  let fixture: ComponentFixture<UserAdminMangeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserAdminMangeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAdminMangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
