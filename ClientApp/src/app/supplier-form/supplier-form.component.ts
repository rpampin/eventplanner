import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Supplier } from '../supplier/supplier';
import { Event } from '../event/event';
import { SupplierType } from '../supplier-types/supplier-type.model';
import { Attachment } from './attachment';
import { faTrash, faDownload } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-supplier-form',
  templateUrl: './supplier-form.component.html',
  styleUrls: ['./supplier-form.component.css']
})
export class SupplierFormComponent implements OnInit {
  faTrash = faTrash;
  faDownload = faDownload;
  eventId: string;
  types: SupplierType[];
  supplierId: string;
  supplier: Supplier = new Supplier();
  attachment: Attachment = new Attachment();
  attachments: Attachment[] = [];
  attachmentInput: any;
  allowUpload: boolean = false;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.eventId = params.get('eventId');
      this.supplierId = params.get('supplierId');

      this.http.get<SupplierType[]>(this.baseUrl + 'api/suppliertypes').subscribe(result => {
        this.types = result;

        if (this.supplierId != "0") {
          this.http.get<Supplier>(this.baseUrl + 'api/suppliers/' + this.supplierId).subscribe(result => {
            this.supplier = result;
            this.supplierId = result.id;
            this.attachments = this.supplier.attachments;
          }, error => console.error(error));
        } else {
          this.http.get<Event>(this.baseUrl + 'api/events/' + this.eventId).subscribe(result => {
            this.supplier.event = result;
          }, error => console.error(error));
        }

      }, error => console.error(error));
    });
  }

  compareFn = (a, b) => this._compareFn(a, b);
  _compareFn(a, b) {
    // Handle compare logic (eg check if unique ids are the same)
    if (!!a && !!b)
      return a.id === b.id;
    return false;
  }

  submitSupplier() {
    if (!this.supplier.id) {
      this.http.post<Supplier>(this.baseUrl + 'api/suppliers', this.supplier).subscribe(result => {
        this.router.navigate(['../'], { relativeTo: this.route });
      }, error => console.error(error));
    } else {
      this.http.put<Supplier>(this.baseUrl + 'api/suppliers/' + this.supplier.id, this.supplier).subscribe(result => {
        this.router.navigate(['../'], { relativeTo: this.route });
      }, error => console.error(error));
    }
  }

  handleUpload(event) {
    this.attachmentInput = event.target;
    const file = event.target.files[0];
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      this.allowUpload = true;
      this.attachment = new Attachment();
      this.attachment.name = file.name;
      this.attachment.base64 = reader.result.toString();
    };
  }

  postAttachment() {
    this.http.post<Attachment>(this.baseUrl + 'api/attachments', { attachment: this.attachment, supplierId: this.supplierId }).subscribe(result => {
      this.attachments.push(result);
      this.attachmentInput.value = null;
      this.attachment = new Attachment();
      this.allowUpload = false;
    }, error => console.error(error));
  }

  downloadAttachment(attachmentId: string) {
    this.http.get<Attachment>(this.baseUrl + 'api/attachments/' + attachmentId).subscribe(file => {
      var a = document.createElement("a"); //Create <a>
      a.href = file.base64; //Image Base64 Goes here
      a.download = file.name; //File name Here
      a.click(); //Downloaded file
    }, error => console.error(error));
  }

  deleteAttachment(index: number, attachmentId: string) {
    this.http.delete(this.baseUrl + 'api/attachments/' + attachmentId).subscribe(() => {
      this.attachments.splice(index, 1);
    }, error => console.error(error));
  }
}
