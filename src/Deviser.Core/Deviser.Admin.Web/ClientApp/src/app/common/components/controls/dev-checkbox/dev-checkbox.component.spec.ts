import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DevCheckboxComponent } from './dev-checkbox.component';

describe('DevCheckboxComponent', () => {
  let component: DevCheckboxComponent;
  let fixture: ComponentFixture<DevCheckboxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DevCheckboxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DevCheckboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
