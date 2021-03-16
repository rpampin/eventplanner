import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { EvenListView } from './even-list-view';
import { faEdit, faTrash, faUser, faUserTie, faClipboard } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';

@Component({
  selector: 'app-upcoming-events',
  templateUrl: './upcoming-events.component.html',
  styleUrls: ['./upcoming-events.component.css']
})
export class UpcomingEventsComponent implements OnInit {
  pastChk: boolean = false;
  faEdit = faEdit;
  faTrash = faTrash;
  faUser = faUser;
  faUserTie = faUserTie;
  faClipboard = faClipboard;
  events: EvenListView[];
  pageOfEvents: Array<any>;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router) { }

  ngOnInit() {
    this.getEvents();
  }

  onChangePage(pageOfItems: Array<any>) {
    // update current page of items
    this.pageOfEvents = pageOfItems;
  }

  getEvents() {
    this.http.get<EvenListView[]>(this.baseUrl + `api/events?pastEvents=${this.pastChk}`).subscribe(result => {
      this.events = result;
    });
  }

  chkChanged() {
    this.getEvents();
  }

  delete(eventId: string) {
    this.http.delete(this.baseUrl + 'api/events/' + eventId).subscribe(() => {
      this.events = this.events.filter(function (e) {
        return e.id !== eventId;
      });
    });
  }

  editForm(eventId: string) {
    let url = 'event';
    if (!!eventId)
      url += `/${eventId}`;
    this.router.navigate([url]);
  }
}
