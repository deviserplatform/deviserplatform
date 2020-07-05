import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditPermissionComponent } from './edit-permission.component';

describe('EditPermissionComponent', () => {
  let component: EditPermissionComponent;
  let fixture: ComponentFixture<EditPermissionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditPermissionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditPermissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
