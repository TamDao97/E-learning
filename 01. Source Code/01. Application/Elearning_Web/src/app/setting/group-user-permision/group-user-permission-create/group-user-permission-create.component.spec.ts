import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupUserPermissionCreateComponent } from './group-user-permission-create.component';

describe('GroupUserPermissionCreateComponent', () => {
  let component: GroupUserPermissionCreateComponent;
  let fixture: ComponentFixture<GroupUserPermissionCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupUserPermissionCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupUserPermissionCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
