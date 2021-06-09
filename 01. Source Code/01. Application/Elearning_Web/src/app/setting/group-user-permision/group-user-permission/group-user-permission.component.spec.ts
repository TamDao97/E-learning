import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupUserPermissionComponent } from './group-user-permission.component';

describe('GroupUserPermissionComponent', () => {
  let component: GroupUserPermissionComponent;
  let fixture: ComponentFixture<GroupUserPermissionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupUserPermissionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupUserPermissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
