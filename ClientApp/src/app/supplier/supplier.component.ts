import { Component, Inject, Input, OnInit } from '@angular/core';
import { Supplier } from './supplier';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { SupplierType } from '../supplier-types/supplier-type.model';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.css']
})
export class SupplierComponent implements OnInit {
  @Input() suppliers: Supplier[];
  types: SupplierType[];
  supplier: Supplier = new Supplier();
  supplierIndex: number = -1;
  faEdit = faEdit;
  faTrash = faTrash;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
    this.http.get<SupplierType[]>(this.baseUrl + 'api/suppliertypes').subscribe(result => {
      this.types = result;
    }, error => console.error(error));
  }

  compareFn = (a, b) => this._compareFn(a, b);
  _compareFn(a, b) {
    // Handle compare logic (eg check if unique ids are the same)
    if (!!a && !!b)
      return a.id === b.id;
    return false;
  }

  clearSupplier() {
    this.supplier = new Supplier();
    this.supplierIndex = -1;
  }

  submitSupplier() {
    if (this.supplierIndex > -1)
      this.suppliers[this.supplierIndex] = this.supplier;
    else
      this.suppliers.push(this.supplier);
    this.clearSupplier();
  }

  editSupplier(index: number, supplier: Supplier) {
    this.supplier = { ...supplier };
    this.supplierIndex = index;
  }

  deleteSupplier(index: number) {
    this.suppliers.splice(index, 1);
  }

}
