import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAlertComponent } from './admin-alert.component';

describe('AdminAlertComponent', () => {
  let component: AdminAlertComponent;
  let fixture: ComponentFixture<AdminAlertComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminAlertComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAlertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
