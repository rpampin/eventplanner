import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Event } from './event';
import { EventType } from '../event-types/event-type.model';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { faUser, faUserTie, faClipboard } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {
  faUser = faUser;
  faUserTie = faUserTie;
  faClipboard = faClipboard;
  id: string;
  event: Event = new Event();
  weddingEventTyperId: string;
  eventTypes: EventType[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.id = params.get('eventId');
    });
    this.http.get<EventType[]>(this.baseUrl + 'api/eventtypes').subscribe(result => {
      this.eventTypes = result;

      this.http.get<string>(this.baseUrl + 'api/eventtypes/wedding-type-id').subscribe(result => {
        this.weddingEventTyperId = result;

        if (!!this.id) {
          this.http.get<Event>(this.baseUrl + 'api/events/' + this.id).subscribe(result => {
            this.event = result;
          }, error => console.error(error));
        }

      }, error => console.error(error));

    }, error => console.error(error));

  }

  onSubmit(eventForm: NgForm) {
    if (eventForm.valid) {
      let url = 'api/events';
      if (this.event.type.id == this.weddingEventTyperId) {
        url += '/wedding';
        this.event.celebrant = this.event.brideName + ' | ' + this.event.groomName;
      }

      if (!this.event.id) {
        this.http.post<Event>(this.baseUrl + url, this.event).subscribe(result => {
          this.router.navigate(['/'])
        }, error => console.error(error));
      } else {
        this.http.put<Event>(this.baseUrl + url + '/' + this.event.id, this.event).subscribe(result => {
          this.router.navigate(['/'])
        }, error => console.error(error));
      }
    } else {
      Object.keys(eventForm.controls).forEach(key => {
        eventForm.controls[key].markAsDirty();
      });
    }
  }

  compareFn = (a, b) => this._compareFn(a, b);
  _compareFn(a, b) {
    // Handle compare logic (eg check if unique ids are the same)
    if (!!a && !!b)
      return a.id === b.id;
    return false;
  }

  print(report: any) {
    // ??
  }

  getBudgetForm() {
    this.http.get<any>(this.baseUrl + 'api/reports/budget-form/' + this.event.id).subscribe(result => {
      this.print(result);
    }, error => console.error(error));
  }

  getChecklist() {
    this.http.get<string>(this.baseUrl + 'api/reports/checklist/' + this.event.id).subscribe(result => {
      this.print(result);
    }, error => console.error(error));
  }

  getProgram() {
    this.http.get<string>(this.baseUrl + 'api/reports/program/' + this.event.id).subscribe(result => {
      this.print(result);
    }, error => console.error(error));
  }
}
