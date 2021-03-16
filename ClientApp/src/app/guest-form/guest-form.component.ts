import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Event } from '../event/event';
import { Guest } from '../guest/guest';

@Component({
  selector: 'app-guest-form',
  templateUrl: './guest-form.component.html',
  styleUrls: ['./guest-form.component.css']
})
export class GuestFormComponent implements OnInit {
  eventId: string;
  guestId: string;
  guest: Guest = new Guest();

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.eventId = params.get('eventId');
      this.guestId = params.get('guestId');

      if (this.guestId != "0") {
        this.http.get<Guest>(this.baseUrl + 'api/guests/' + this.guestId).subscribe(result => {
          this.guest = result;
          this.guestId = result.id;
        });
      } else {
        this.http.get<Event>(this.baseUrl + 'api/events/' + this.eventId).subscribe(result => {
          this.guest.event = result;
        });
      }
    });
  }

  onWillAttendChange(entry): void {
    this.guest.willAttend = entry;
  }

  submitGuest() {
    if (!this.guest.id) {
      this.http.post<Guest>(this.baseUrl + 'api/guests', this.guest).subscribe(result => {
        this.router.navigate(['../'], { relativeTo: this.route });
      });
    } else {
      this.http.put<Guest>(this.baseUrl + 'api/guests/' + this.guest.id, this.guest).subscribe(result => {
        this.router.navigate(['../'], { relativeTo: this.route });
      });
    }
  }
}
