import { Component, Inject, Input, OnInit } from '@angular/core';
import { Supplier } from './supplier';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { SupplierType } from '../supplier-types/supplier-type.model';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.css']
})
export class SupplierComponent implements OnInit {
  eventId: string;
  event: any = {};
  suppliers: Supplier[] = [];
  faEdit = faEdit;
  faTrash = faTrash;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.eventId = params.get('eventId');

      this.http.get<any>(this.baseUrl + 'api/events/base-data/' + this.eventId).subscribe(result => {
        this.event = result;

        this.http.get<Supplier[]>(this.baseUrl + 'api/suppliers/event-suppliers/' + this.eventId).subscribe(result => {
          this.suppliers = result;
        });

      });
    });
  }

  deleteSupplier(index: number, supplier: Supplier) {
    this.http.delete(this.baseUrl + 'api/suppliers/' + supplier.id).subscribe(() => {
      this.suppliers.splice(index, 1);
    });
  }
}
