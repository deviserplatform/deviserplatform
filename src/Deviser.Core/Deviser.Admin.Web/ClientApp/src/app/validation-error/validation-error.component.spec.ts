import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationErrorComponent } from './validation-error.component';

describe('ValidationErrorComponent', () => {
  let component: ValidationErrorComponent;
  let fixture: ComponentFixture<ValidationErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationErrorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
