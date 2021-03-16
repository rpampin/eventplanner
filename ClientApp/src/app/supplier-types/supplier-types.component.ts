import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { NgForm } from '@angular/forms';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { SupplierType } from './supplier-type.model';

@Component({
  selector: 'app-supplier-types',
  templateUrl: './supplier-types.component.html',
  styleUrls: ['./supplier-types.component.css']
})
export class SupplierTypesComponent {
  faEdit = faEdit;
  faTrash = faTrash;
  types: SupplierType[];
  type: SupplierType = new SupplierType();

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    http.get<SupplierType[]>(baseUrl + 'api/suppliertypes').subscribe(result => {
      this.types = result;
    });
  }

  submitted = false;
  newType() {
    this.type = new SupplierType();
  }

  onSubmit(supplierTypeForm: NgForm) {
    if (!this.type.id) {
      this.http.post<SupplierType>(this.baseUrl + 'api/suppliertypes', { name: this.type.name }).subscribe(result => {
        this.types.push(result);
        this.types.sort(function (a, b) {
          if (a.name < b.name) { return -1; }
          if (a.name > b.name) { return 1; }
          return 0;
        });
        supplierTypeForm.reset();
      });
    } else {
      this.http.put<SupplierType>(this.baseUrl + 'api/suppliertypes/' + this.type.id, this.type).subscribe(() => {
        const edited = this.types.filter(t => t.id === this.type.id)[0];
        edited.name = this.type.name;
        this.types.sort(function (a, b) {
          if (a.name < b.name) { return -1; }
          if (a.name > b.name) { return 1; }
          return 0;
        });
        supplierTypeForm.reset();
      });
    }

    this.submitted = true;
  }

  public edit(typeId: string) {
    const toEdit = this.types.filter(t => t.id == typeId)[0];
    this.type = new SupplierType();
    this.type.id = toEdit.id;
    this.type.name = toEdit.name;
  }

  public delete(typeId: string) {
    this.http.delete(this.baseUrl + 'api/suppliertypes/' + typeId).subscribe(() => {
      this.types = this.types.filter(function (t) {
        return t.id !== typeId;
      });
    });
  }
}
