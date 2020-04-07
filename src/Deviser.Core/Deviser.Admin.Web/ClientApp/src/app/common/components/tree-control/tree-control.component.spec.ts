import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TreeControlComponent } from './tree-control.component';

describe('TreeControlComponent', () => {
  let component: TreeControlComponent;
  let fixture: ComponentFixture<TreeControlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TreeControlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TreeControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
