import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SupplierTypesComponent } from './supplier-types.component';

describe('SupplierTypesComponent', () => {
  let component: SupplierTypesComponent;
  let fixture: ComponentFixture<SupplierTypesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SupplierTypesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SupplierTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
