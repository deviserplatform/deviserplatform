import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTreeComponent } from './admin-tree.component';

describe('AdminTreeComponent', () => {
  let component: AdminTreeComponent;
  let fixture: ComponentFixture<AdminTreeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminTreeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
