import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { EvenListView } from './even-list-view';
import { faEdit, faTrash, faUser, faUserTie } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';

@Component({
  selector: 'app-upcoming-events',
  templateUrl: './upcoming-events.component.html',
  styleUrls: ['./upcoming-events.component.css']
})
export class UpcomingEventsComponent implements OnInit {
  faEdit = faEdit;
  faTrash = faTrash;
  faUser = faUser;
  faUserTie = faUserTie;
  events: EvenListView[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router) { }

  ngOnInit() {
    this.http.get<EvenListView[]>(this.baseUrl + 'api/events').subscribe(result => {
      this.events = result;
    }, error => console.error(error));
  }

  delete(eventId: string) {
    this.http.delete(this.baseUrl + 'api/events/' + eventId).subscribe(() => {
      this.events = this.events.filter(function (e) {
        return e.id !== eventId;
      });
    }, error => console.error(error));
  }

  editForm(eventId: string) {
    let url = 'event';
    if (!!eventId)
      url += `/${eventId}`;
    this.router.navigate([url]);
  }
}
