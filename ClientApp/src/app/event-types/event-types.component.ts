import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { EventType } from './event-type.model';

@Component({
  selector: 'app-event-types',
  templateUrl: './event-types.component.html',
  styleUrls: ['./event-types.component.css']
})
export class EventTypesComponent {
  faEdit = faEdit;
  faTrash = faTrash;
  types: EventType[];
  type: EventType = new EventType();

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    http.get<EventType[]>(baseUrl + 'api/eventtypes').subscribe(result => {
      this.types = result;
    }, error => console.error(error));
  }

  submitted = false;
  newType() {
    this.type = new EventType();
  }
  
  onSubmit() {
    this.submitted = true;
  }

  public update(event: EventType) {
    // this.http.put<EventType>(this.baseUrl + '/api/eventtypes/' + event.id, null).subscribe(result => {

    // }, error => console.error(error));
  }

  public edit(typeId: string) {

  }

  public delete(typeId: string) {
    this.http.delete(this.baseUrl + 'api/eventTypes/' + typeId).subscribe(result => {
      this.types = this.types.filter(function (t) {
        return t.id !== typeId;
      });
    }, error => console.error(error));
  }
}