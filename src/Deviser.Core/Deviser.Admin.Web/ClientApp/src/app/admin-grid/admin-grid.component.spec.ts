import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminGridComponent } from './admin-grid.component';

describe('AdminGridComponent', () => {
  let component: AdminGridComponent;
  let fixture: ComponentFixture<AdminGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
