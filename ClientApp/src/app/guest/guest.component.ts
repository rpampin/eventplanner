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

  public options: Object = {
    placeholderText: 'Edit Your Content Here!',
    heightMin: 250,
    events: {
      "image.beforeUpload": function (files) {
        var editor = this;
        if (files.length) {
          // Create a File Reader.
          var reader = new FileReader();
          // Set the reader to insert images when they are loaded.
          reader.onload = function (e) {
            var result = e.target.result;
            editor.image.insert(result, null, null, editor.image.get());
          };
          // Read image as base64.
          reader.readAsDataURL(files[0]);
        }
        editor.popups.hideAll();
        // Stop default upload chain.
        return false;
      }
    }
  }

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
        });

      });
    });
  }

  deleteGuest(index: number, guest: Guest) {
    this.http.delete(this.baseUrl + 'api/guests/' + guest.id).subscribe(() => {
      this.guests.splice(index, 1);
    });
  }

  updateTemplate() {
    this.http.post(this.baseUrl + `api/events/${this.eventId}/program`, this.event).subscribe(() => {});
  }

  sendInvitations(resend: boolean, guestId: string) {
    this.http.get(this.baseUrl + 'api/guests/send-invitations/' + this.eventId + `?resend=${resend}&guestId=${guestId}`).subscribe(() => {
      this.toastService.show(`Invitations sent successfuly`, { classname: 'bg-success text-light' });
    });
  }
}
