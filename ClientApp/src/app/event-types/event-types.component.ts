import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { NgForm } from '@angular/forms';
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
    });
  }

  submitted = false;
  newType() {
    this.type = new EventType();
  }

  onSubmit(eventTypeForm: NgForm) {
    if (!this.type.id) {
      this.http.post<EventType>(this.baseUrl + 'api/eventtypes', { name: this.type.name }).subscribe(result => {
        this.types.push(result);
        this.types.sort(function (a, b) {
          if (a.name < b.name) { return -1; }
          if (a.name > b.name) { return 1; }
          return 0;
        });
        eventTypeForm.reset();
      });
    } else {
      this.http.put<EventType>(this.baseUrl + 'api/eventtypes/' + this.type.id, this.type).subscribe(() => {
        const edited = this.types.filter(t => t.id === this.type.id)[0];
        edited.name = this.type.name;
        this.types.sort(function (a, b) {
          if (a.name < b.name) { return -1; }
          if (a.name > b.name) { return 1; }
          return 0;
        });
        eventTypeForm.reset();
      });
    }

    this.submitted = true;
  }

  public edit(typeId: string) {
    window.scroll(0,0);
    const toEdit = this.types.filter(t => t.id == typeId)[0];
    this.type = new EventType();
    this.type.id = toEdit.id;
    this.type.name = toEdit.name;
  }

  public delete(typeId: string) {
    this.http.delete(this.baseUrl + 'api/eventtypes/' + typeId).subscribe(() => {
      this.types = this.types.filter(function (t) {
        return t.id !== typeId;
      });
    });
  }
}