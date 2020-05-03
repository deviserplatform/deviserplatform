import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SortFilterComponent } from './sort-filter.component';

describe('SortFilterComponent', () => {
  let component: SortFilterComponent;
  let fixture: ComponentFixture<SortFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SortFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SortFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
