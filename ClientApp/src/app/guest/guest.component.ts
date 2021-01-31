import { Component, Inject, Input, OnInit } from '@angular/core';
import { Guest } from './guest';
import { faEdit, faTrash, faEnvelope } from '@fortawesome/free-solid-svg-icons';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastService } from '../toast.service';

@Component({
  selector: 'app-guest',
  templateUrl: './guest.component.html',
  styleUrls: ['./guest.component.css']
})
export class GuestComponent implements OnInit {
  eventId: string;
  event: any = {};
  guests: Guest[] = [];
  faEdit = faEdit;
  faTrash = faTrash;
  faEnvelope = faEnvelope;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    private route: ActivatedRoute,
    public toastService: ToastService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.eventId = params.get('eventId');

      this.http.get<any>(this.baseUrl + 'api/events/base-data/' + this.eventId).subscribe(result => {
        this.event = result;

        this.http.get<Guest[]>(this.baseUrl + 'api/guests/event-guests/' + this.eventId).subscribe(result => {
          this.guests = result;
        }, error => console.error(error));

      }, error => console.error(error));
    });
  }

  deleteGuest(index: number, guest: Guest) {
    this.http.delete(this.baseUrl + 'api/guests/' + guest.id).subscribe(() => {
      this.guests.splice(index, 1);
    }, error => console.error(error));
  }

  sendInvitations(resend: boolean, guestId: string) {
    this.http.get(this.baseUrl + 'api/guests/send-invitations/' + this.eventId + `?resend=${resend}&guestId=${guestId}`).subscribe(() => {
      this.toastService.show(`Invitations sent successfuly`, { classname: 'bg-success text-light' });
    }, error => {
      console.error(error);
      this.toastService.show(error.error, { classname: 'bg-danger text-light' })
    });
  }
}
