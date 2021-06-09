import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopicManageComponent } from './topic-manage.component';

describe('TopicManageComponent', () => {
  let component: TopicManageComponent;
  let fixture: ComponentFixture<TopicManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TopicManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TopicManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
