import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DevAlertComponent } from './dev-alert.component';

describe('AlertComponent', () => {
  let component: DevAlertComponent;
  let fixture: ComponentFixture<DevAlertComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DevAlertComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DevAlertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
